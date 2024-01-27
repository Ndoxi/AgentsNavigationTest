using System;
using UnityEngine;

namespace AgentsTest.Core.Input
{
    public class InputChanel
    {
        public event Action<Vector2> OnClick;

        public Vector2 CameraMoveInput { get; private set; }

        public void SetCameraInput(Vector2 input)
        {
            CameraMoveInput = input;
        }

        public void InvokeClick(Vector2 screenPosition)
        {
            OnClick?.Invoke(screenPosition);
        }
    }
}