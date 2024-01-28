using ProjectDawn.Navigation;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace AgentsTest.Core.Systems
{
    [BurstCompile]
    public partial class PathingSystem : SystemBase
    {
        [BurstCompile]
        protected override void OnUpdate()
        {
            EntityQuery entityQuery = GetEntityQuery(
                ComponentType.ReadOnly<LocalTransform>(),
                ComponentType.ReadWrite<AgentBody>(),
                ComponentType.ReadOnly<UnitData>());

            RequireForUpdate(entityQuery);

            var ecbs = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var buffer = ecbs.CreateCommandBuffer(World.Unmanaged);

            Dependency = new MoveJob
            {
                Entities = entityQuery.ToEntityArray(Allocator.Persistent),
                TransformLookup = GetComponentLookup<LocalTransform>(true),
                BodyLookup = GetComponentLookup<AgentBody>(),
                UnitDataLookup = GetComponentLookup<UnitData>(),
                ECB = buffer
            }.Schedule(Dependency);
        }

        [BurstCompile]
        private struct MoveJob : IJob
        {
            [ReadOnly]
            public NativeArray<Entity> Entities;
            [ReadOnly]
            public ComponentLookup<LocalTransform> TransformLookup;
            public ComponentLookup<AgentBody> BodyLookup;
            public ComponentLookup<UnitData> UnitDataLookup;
            public EntityCommandBuffer ECB;

            [BurstCompile]
            public void Execute()
            {
                foreach (Entity entity in Entities)
                {
                    if (!TransformLookup.TryGetComponent(entity, out LocalTransform transform))
                        continue;
                    if (!BodyLookup.TryGetComponent(entity, out AgentBody body))
                        continue;
                    if (!UnitDataLookup.TryGetComponent(entity, out UnitData unitData))
                        continue;

                    Entity target = Entity.Null;
                    float minDistance = 0;
                    foreach (Entity potentialTarget in Entities)
                    {
                        if (!TransformLookup.TryGetComponent(potentialTarget, out LocalTransform targetsTransform))
                            continue;
                        if (!UnitDataLookup.TryGetComponent(potentialTarget, out UnitData targetsUnitData))
                            continue;
                        if (unitData.FractionId == targetsUnitData.FractionId)
                            continue;

                        float distance = math.distance(transform.Position, targetsTransform.Position);
                        if (distance < minDistance || target == Entity.Null)
                        {
                            minDistance = distance;
                            target = potentialTarget;
                        }
                    }

                    if (target == Entity.Null)
                    {
                        body.IsStopped = true;
                    }
                    else if (minDistance > unitData.ContactDistance)
                    {
                        body.IsStopped = false;
                        body.Destination = TransformLookup[target].Position;
                    }
                    else
                    {
                        body.IsStopped = true;
                        AgentBody targetBody = BodyLookup[target];
                        targetBody.IsStopped = true;
                        BodyLookup[target] = targetBody;

                        ECB.AddComponent<UnitDead>(entity);
                        ECB.AddComponent<UnitDead>(target);
                    }
                    BodyLookup[entity] = body;
                }
            }
        }
    }
}