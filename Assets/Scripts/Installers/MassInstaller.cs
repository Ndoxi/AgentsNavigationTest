using AgentsTest.Core.Input;
using UnityEngine;
using Zenject;

namespace AgentsTest.Core.Installers
{
    public class MassInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<InputChanel>().AsSingle();
        }
    }
}