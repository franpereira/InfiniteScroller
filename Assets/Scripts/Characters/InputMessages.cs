using Kumi.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Kumi.Characters
{
    /// <summary>
    /// Recieves the input messages from the PlayerInput component.
    /// The "Behavior" attribute of the PlayerInput must be set on "Send Messages".  
    /// </summary>
    public class InputMessages : MonoBehaviour
    {
        Character character;
        PlayerInput playerInput;
        void Awake()
        {
            character = GetComponent<Character>();
            playerInput = GetComponent<PlayerInput>();
        }

        #region Input Actions
        void OnJump() => character.RequestJump();
        void OnAxis(InputValue value) => character.HorizontalAxis = value.Get<Vector2>().x;
        void OnPause() => TimePause.Switch();
        #endregion

        #region Events that enables/disables the input 
        void EnableInput() => playerInput.enabled = true;
        void DisableInput() => playerInput.enabled = false;

        void Start() => DisableInput();
        void OnEnable()
        {
            Events.Begin += EnableInput;
            Events.Resurrection += EnableInput;
            Events.End += DisableInput;
        }

        void OnDisable()
        {
            Events.Begin -= EnableInput;
            Events.Resurrection -= EnableInput;
            Events.End -= DisableInput;
        }
        #endregion
    }
}
