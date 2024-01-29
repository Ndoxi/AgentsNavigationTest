using AgentsTest.Core.Input;
using UnityEngine;
using Zenject;

namespace AgentsTest.Core.Installers
{
    public class MassInstaller : MonoInstaller
    {
        [SerializeField] private AllySpawner _allySpawner;
        [SerializeField] private EnemySpawner _enemySpawner;

        public override void InstallBindings()
        {
            Container.Bind<IInputChanel>().To<InputChanel>().AsSingle();
            Container.Bind<AllySpawner>().FromComponentOn(_allySpawner.gameObject).AsSingle();
            Container.Bind<EnemySpawner>().FromComponentOn(_enemySpawner.gameObject).AsSingle();
        }
    }
}