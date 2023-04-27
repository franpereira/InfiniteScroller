using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

namespace Kumi.UI
{
    /// <summary>
    /// Based on built-in On-Screen Stick.
    /// </summary>
    [AddComponentMenu("Input/On-Screen Axis")]
    public class OnScreenAxis : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] float axisCenter;
        [SerializeField] float mMovementRange = 50;

        [InputControl(layout = "Vector2")]
        [SerializeField] string mControlPath;

        Vector2 _axisCenterPos;
        public float MovementRange
        {
            get => mMovementRange;
            set => mMovementRange = value;
        }

        protected override string controlPathInternal
        {
            get => mControlPath;
            set => mControlPath = value;
        }

        private void Start() => _axisCenterPos = new Vector3(axisCenter, 0f);

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null) throw new System.ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out _);
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null) throw new System.ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
            Vector2 delta = position - _axisCenterPos;

            delta = Vector2.ClampMagnitude(delta, MovementRange);

            var newPos = new Vector2(delta.x / MovementRange, delta.y / MovementRange);
            SendValueToControl(newPos);
        }

        public void OnPointerUp(PointerEventData eventData) => SendValueToControl(Vector2.zero);
    }
}
