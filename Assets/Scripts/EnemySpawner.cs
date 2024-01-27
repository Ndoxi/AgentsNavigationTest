using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace AgentsTest.Core
{
    public class EnemySpawner : MonoBehaviour
    {
        public Observable<int> AliveUnitsAmount = new Observable<int>();
        public Observable<int> DeadUnitsAmount = new Observable<int>();

        public List<Entity> SpawnedEntities => _enemies;

        [SerializeField] private AllySpawner _allySpawner;
        [SerializeField] private EnemyPool _pool;
        [Header("Settings")]
        [SerializeField] private int _initialEnemyCount;
        [SerializeField] private float _spawnInterval = 10;
        [SerializeField] private int _spawnAmount = 10;
        [SerializeField] private Vector3 _spawnZoneSize = new Vector3(4, 0, 4);

        private float _lastSpawnTime;
        private readonly List<Entity> _enemies = new List<Entity>();
        private readonly Random _random = new Random(1);

        private void Start()
        {
            SpawnEnemies(_initialEnemyCount);
        }

        private void Update()
        {
            if (Time.time - _lastSpawnTime > _spawnInterval)
            {
                SpawnEnemies(_spawnAmount);
            }
        }

        private void SpawnEnemies(int amount)
        {
            _lastSpawnTime = Time.time;
            for (int i = 0; i < amount; i++)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            float3 offset = _random.NextFloat3(-_spawnZoneSize, _spawnZoneSize);
            float3 position = (float3)transform.position + offset;
            Entity newEnemy = _pool.Get();
            newEnemy.Initialize(_allySpawner.SpawnedEntities);
            newEnemy.transform.SetPositionAndRotation(position, Quaternion.identity);

            _enemies.Add(newEnemy);
            AliveUnitsAmount.Value++;

            newEnemy.OnDeath += OnDeathEvent;

            void OnDeathEvent()
            {
                _enemies.Remove(newEnemy);
                _pool.Return(newEnemy);
                AliveUnitsAmount.Value--;
                DeadUnitsAmount.Value++;
                newEnemy.OnDeath -= OnDeathEvent;
            }
        }
    }
}