using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace AgentsTest.Core.Systems
{
    public partial class UnitDeathSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((Entity entity, in Unit unit, in UnitDead unitDead) =>
            {
                unit.SetDead();
            }).WithStructuralChanges().WithoutBurst().Run();
        }
    }
}