using System;
using UnityEngine;

namespace AgentsTest.Core.Input
{
    public class InputChanel : IInputChanel
    {
        public event Action<Vector2> OnClick;
        private Vector2 _cameraMoveInput;

        public void SetCameraInput(Vector2 input)
        {
            _cameraMoveInput = input;
        }

        public void InvokeClick(Vector2 screenPosition)
        {
            OnClick?.Invoke(screenPosition);
        }

        public Vector2 GetCameraMoveInput()
        {
            return _cameraMoveInput;
        }
    }
}