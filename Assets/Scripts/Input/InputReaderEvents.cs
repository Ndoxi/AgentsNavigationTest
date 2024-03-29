using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace AgentsTest.Core.Input
{
    public class InputReaderEvents : MonoBehaviour
    {
        private Vector2 _pointerPos;
        private IInputChanel _inputChanel;

        [Inject]
        private void Construct(IInputChanel inputChanel)
        {
            _inputChanel = inputChanel;
        }

        public void OnCameraMove(CallbackContext context)
        {
            _inputChanel.SetCameraInput(context.ReadValue<Vector2>());
        }

        public void OnClick(CallbackContext context)
        {
            if (context.canceled)
                _inputChanel.InvokeClick(_pointerPos);
        }

        public void OnPointerMove(CallbackContext context)
        {
            _pointerPos = context.ReadValue<Vector2>();
        }
    }
}