using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace AgentsTest.Core.UI
{
    public class EntitiesMonitor : MonoBehaviour
    {
        [SerializeField] private EntitiesMonitorRecord _record1;
        [SerializeField] private EntitiesMonitorRecord _record2;
        [SerializeField] private EntitiesMonitorRecord _record3;
        [SerializeField] private EntitiesMonitorRecord _record4;

        private AllySpawner _allySpawner;
        private EnemySpawner _enemySpawner;

        [Inject]
        private void Construct(AllySpawner allySpawner, EnemySpawner enemySpawner)
        {
            _allySpawner = allySpawner;
            _enemySpawner = enemySpawner;
        }

        private void OnEnable()
        {
            _record1.SetNewValue(_allySpawner.AliveUnitsAmount.Value);
            _record2.SetNewValue(_allySpawner.DeadUnitsAmount.Value);
            _record3.SetNewValue(_enemySpawner.AliveUnitsAmount.Value);
            _record4.SetNewValue(_enemySpawner.DeadUnitsAmount.Value);

            _allySpawner.AliveUnitsAmount.OnValueChanged += _record1.SetNewValue;
            _allySpawner.DeadUnitsAmount.OnValueChanged += _record2.SetNewValue;
            _enemySpawner.AliveUnitsAmount.OnValueChanged += _record3.SetNewValue;
            _enemySpawner.DeadUnitsAmount.OnValueChanged += _record4.SetNewValue;
        }

        private void OnDisable()
        {
            _allySpawner.AliveUnitsAmount.OnValueChanged -= _record1.SetNewValue;
            _allySpawner.DeadUnitsAmount.OnValueChanged -= _record2.SetNewValue;
            _enemySpawner.AliveUnitsAmount.OnValueChanged -= _record3.SetNewValue;
            _enemySpawner.DeadUnitsAmount.OnValueChanged -= _record4.SetNewValue;
        }
    }
}