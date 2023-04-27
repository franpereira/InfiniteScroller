using UnityEngine;

namespace Kumi.UI
{
    /// <summary>
    /// For avoiding the screen notch area on mobile platforms.
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class UISafeArea : MonoBehaviour {
        RectTransform safeAreaRect;
        Canvas canvas;
        Rect lastSafeArea;

        void Start() {
            safeAreaRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            OnRectTransformDimensionsChange();
        }

        void OnRectTransformDimensionsChange() {

            if (Screen.safeArea != lastSafeArea && canvas != null) {
                lastSafeArea = Screen.safeArea;
                UpdateSizeToSafeArea();
            }
        }

        void UpdateSizeToSafeArea() {

            Rect safeArea = Screen.safeArea;
            Vector2 inverseSize = new Vector2(1f, 1f) / canvas.pixelRect.size; 
            Vector2 newAnchorMin = Vector2.Scale(safeArea.position, inverseSize);
            Vector2 newAnchorMax = Vector2.Scale(safeArea.position + safeArea.size, inverseSize);

            safeAreaRect.anchorMin = newAnchorMin;
            safeAreaRect.anchorMax = newAnchorMax;

            safeAreaRect.offsetMin = Vector2.zero;
            safeAreaRect.offsetMax = Vector2.zero;
        }
    }
}