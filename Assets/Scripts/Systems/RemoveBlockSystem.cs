using System;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
//using UnityEditor;
using UnityEngine;
using Unity.Burst;
using Unity.Transforms;
using Unity.Jobs;

    [UpdateAfter(typeof(AddBlockSystem))]
    partial class RemoveBlockSystem : SystemBase
    {
        private BeginSimulationEntityCommandBufferSystem m_BeginSimECBSystem;

        protected override void OnCreate()
        {
            m_BeginSimECBSystem = World.GetExistingSystemManaged<BeginSimulationEntityCommandBufferSystem>();
        }

        protected override void OnUpdate()
        {
            var ecb = m_BeginSimECBSystem.CreateCommandBuffer();
            var PosTemp = new float3(0,0,0);
            
            Entities
            .WithAll<RemoveBlock>()
            .ForEach((Entity entity, in TransformAspect blockPos) =>
            {
                //Offset position y+1
                PosTemp = new float3(blockPos.LocalTransform.Position.x, blockPos.LocalTransform.Position.y+1, blockPos.LocalTransform.Position.z);
                ecb.DestroyEntity(entity);
            })
            .WithoutBurst()
            .Run();
            //.Schedule();
    
            if(!PosTemp.Equals(new float3(0,0,0)))
            {

                //Check if needs to remove plant
                Entities
                .WithAll<PlantBlock>()
                .ForEach((Entity entity, in TransformAspect blockPos) =>
                {
                    if(PosTemp.Equals(blockPos.LocalTransform.Position))
                    {
                    //Destroy Plant block
                        ecb.DestroyEntity(entity);
                    }

                })
                .WithoutBurst()
                .Run();

            }
            //ecb.Dispose();
            m_BeginSimECBSystem.AddJobHandleForProducer(Dependency);
        }
    }