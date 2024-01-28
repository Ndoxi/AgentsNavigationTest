using AgentsTest.Core.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace AgentsTest.Core
{
    public class AllySpawner : MonoBehaviour
    {
        public Observable<int> AliveUnitsAmount = new Observable<int>();
        public Observable<int> DeadUnitsAmount = new Observable<int>();
        public List<Entity> SpawnedEntities => _allies;

        [SerializeField] private Entity _allyPrefab;
        [SerializeField] private PointerInputProcessor _pointerInputProcessor;

        private readonly List<Entity> _allies = new List<Entity>();
        private EnemySpawner _enemySpawner;

        [Inject]
        private void Construct(EnemySpawner enemySpawner)
        {
            _enemySpawner = enemySpawner;
        }

        private void OnEnable()
        {
            _pointerInputProcessor.OnTerrainClick += OnTerainClick;
        }

        private void OnDisable()
        {
            _pointerInputProcessor.OnTerrainClick -= OnTerainClick;
        }

        private void OnTerainClick(Vector3 position)
        {
            SpawnEntity(position);
        }

        private void SpawnEntity(Vector3 position)
        {
            Entity newAlly = Instantiate(_allyPrefab, position, Quaternion.identity);
            newAlly.Initialize(_enemySpawner.SpawnedEntities);
            _allies.Add(newAlly);
            AliveUnitsAmount.Value++;

            newAlly.OnDeath += OnDeathEvent;

            void OnDeathEvent()
            {
                _allies.Remove(newAlly);
                Destroy(newAlly.gameObject);
                AliveUnitsAmount.Value--; 
                DeadUnitsAmount.Value++;
                newAlly.OnDeath -= OnDeathEvent;
            }
        }
    }
}