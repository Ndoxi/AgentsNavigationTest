using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AgentsTest.Core
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private Unit _enemyPrefab;
        [SerializeField] private Transform _container;
        [SerializeField] private int _initialSize;
        [SerializeField, HideInInspector] private List<Unit> _enemies;

        public Unit Get() 
        {
            if (_enemies.Count == 0)
            {
                return Instantiate(_enemyPrefab);
            }
            else
            {
                Unit enemy = _enemies[^1];
                enemy.transform.SetParent(null);
                _enemies.RemoveAt(_enemies.Count - 1);
                return enemy;
            }
        }

        public void Return(Unit enemy)
        {
            _enemies.Add(enemy);
            enemy.transform.SetParent(_container);
        }
    }
}