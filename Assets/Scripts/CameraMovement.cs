using AgentsTest.Core.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AgentsTest.Core
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 15f;

        private IInputChanel _inputChanel;

        [Inject]
        private void Construct(IInputChanel inputChanel) 
        {
            _inputChanel = inputChanel;
        }

        private void LateUpdate()
        {
            Vector3 forwardDir = transform.forward;
            forwardDir.y = 0f;
            forwardDir.Normalize();
            Vector3 rightDir = transform.right;
            rightDir.y = 0f;
            rightDir.Normalize();

            Vector2 moveInput = _inputChanel.GetCameraMoveInput();
            Vector3 moveDir = forwardDir * moveInput.y + rightDir * moveInput.x;
            transform.Translate(_speed * Time.deltaTime * moveDir, Space.World);
        }
    }
}