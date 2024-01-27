using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace AgentsTest.Core
{
    public class Entity : MonoBehaviour
    {
        public event System.Action OnDeath;

        public int GroupId => _groupId;

        [SerializeField] private int _groupId;
        [SerializeField] private float _minimalDistance = 1;
        private List<Entity> _potentialEnemies;
        private AgentAuthoring _agent;
        private Entity _target;
        private bool _killed;

        private void Awake()
        {
            _agent = GetComponent<AgentAuthoring>();
        }

        public void Initialize(List<Entity> potentialEnemies)
        {
            _potentialEnemies = potentialEnemies;
            _killed = false;
        }

        private void Update()
        {
            if (_killed)
                return;

            _target = SelectNearest();
            if (_target)
                _agent.SetDestination(_target.transform.position);
            else
                _agent.Stop();
        }

        public void Kill()
        {
            OnDeath?.Invoke();
            _killed = true;
        }

        private Entity SelectNearest()
        {
            if (_potentialEnemies.Count == 0)
                return null;

            Entity nearest = _potentialEnemies[0];
            float minDistance = Vector3.Distance(transform.position, nearest.transform.position);

            if (minDistance <= _minimalDistance)
            {
                nearest.Kill();
                this.Kill();
            }

            foreach (var enemy in _potentialEnemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = enemy;
                }

                if (minDistance <= _minimalDistance)
                {
                    nearest.Kill();
                    this.Kill();
                    return null;
                }
            }

            return nearest;
        }
    }
}