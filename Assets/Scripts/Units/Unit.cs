using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

namespace AgentsTest.Core
{
    public struct UnitData : IComponentData
    {
        public int FractionId;
        public int SpawnerId;
        public float ContactDistance;
    }

    public class Unit : MonoBehaviour
    {
        public event System.Action OnDeath;

        [SerializeField] private float _minimalDistance = 1;
        private AgentAuthoring _agent;
        private Entity _entity;
        private World _world;

        private void Awake()
        {
            _agent = GetComponent<AgentAuthoring>();
            _entity = _agent.GetOrCreateEntity();
            _world = World.DefaultGameObjectInjectionWorld;
        }

        public void Initialize(int fractionId, int spawderId)
        {
            _world.EntityManager.AddComponentObject(_entity, this);
            _world.EntityManager.AddComponentData(_entity, new UnitData
            {
                FractionId = fractionId,
                SpawnerId = spawderId,
                ContactDistance = _minimalDistance
            });
        }

        public void SetDead()
        {
            if (_world != null)
            {
                _world.EntityManager.RemoveComponent<Unit>(_entity);
                _world.EntityManager.RemoveComponent<UnitDead>(_entity);
                _world.EntityManager.RemoveComponent<UnitData>(_entity);
            }
            OnDeath?.Invoke();
        }
    }
}