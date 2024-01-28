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
            Entities.ForEach((Entity entity, Unit unit, in UnitData unitData) =>
            {
                if (unitData.Killed)
                {
                    unit.SetDead();
                }
            }).WithStructuralChanges().WithoutBurst().Run();
        }
    }
}