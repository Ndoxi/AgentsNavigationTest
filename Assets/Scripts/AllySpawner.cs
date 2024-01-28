using AgentsTest.Core.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace AgentsTest.Core
{
    public class AllySpawner : MonoBehaviour
    {
        public Observable<int> AliveUnitsAmount = new Observable<int>();
        public Observable<int> DeadUnitsAmount = new Observable<int>();
        public List<Unit> SpawnedEntities => _allies;

        [SerializeField] private Unit _allyPrefab;
        [SerializeField] private int _fractionId;
        [SerializeField] private PointerInputProcessor _pointerInputProcessor;

        private readonly List<Unit> _allies = new List<Unit>();
        private EnemySpawner _enemySpawner;
        private int _spawnerId;

        private void Awake()
        {
            _spawnerId = Guid.NewGuid().GetHashCode();
        }

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
            SpawnUnit(position);
        }

        private void SpawnUnit(Vector3 position)
        {
            Unit newAlly = Instantiate(_allyPrefab, position, Quaternion.identity);
            newAlly.Initialize(_fractionId, _spawnerId);
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