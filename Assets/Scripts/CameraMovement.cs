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

        private InputChanel _inputChanel;

        [Inject]
        private void Construct(InputChanel inputChanel) 
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

            Vector3 moveDir = forwardDir * _inputChanel.CameraMoveInput.y + rightDir * _inputChanel.CameraMoveInput.x;
            transform.Translate(_speed * Time.deltaTime * moveDir, Space.World);
        }
    }
}