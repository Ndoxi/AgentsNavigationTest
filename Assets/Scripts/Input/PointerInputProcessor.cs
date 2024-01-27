using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AgentsTest.Core.Input
{
    public class PointerInputProcessor : MonoBehaviour
    {
        public event System.Action<Vector3> OnTerrainClick;

        [SerializeField] private Camera _camera;
        [SerializeField] private LayerMask _terrainLayer;

        private InputChanel _inputChanel;

        [Inject]
        private void Construct(InputChanel inputChanel)
        {
            _inputChanel = inputChanel;
        }

        private void OnEnable()
        {
            _inputChanel.OnClick += OnPointerClick;
        }

        private void OnDisable()
        {
            _inputChanel.OnClick -= OnPointerClick;
        }

        private void OnPointerClick(Vector2 screenPos)
        {
            Ray ray = _camera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _terrainLayer.value))
            {
                OnTerrainClick?.Invoke(hit.point);
            }
        }
    }
}