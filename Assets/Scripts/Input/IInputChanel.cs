using System;
using UnityEngine;

namespace AgentsTest.Core.Input
{
    public interface IInputChanel
    {
        public event Action<Vector2> OnClick;
        public Vector2 GetCameraMoveInput();
        public void SetCameraInput(Vector2 input);
        public void InvokeClick(Vector2 screenPosition);
    }
}