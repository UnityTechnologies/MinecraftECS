using Unity.Collections;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;


namespace Minecraft
{
    public class DestroySystem : ComponentSystem
    {

        private EntityQuery targetBlocks, sourceBlock, surfaceplants;

        protected override void OnCreate(){
            targetBlocks = GetEntityQuery(ComponentType.ReadOnly<BlockTag>(), ComponentType.ReadOnly<Translation>());
            sourceBlock = GetEntityQuery(ComponentType.ReadOnly<DestroyTag>(), ComponentType.ReadOnly<Translation>());
            surfaceplants = GetEntityQuery(ComponentType.ReadOnly<SurfacePlantTag>(), ComponentType.ReadOnly<Translation>());
        }

        protected override void OnUpdate()
        {
            var targetPos = targetBlocks.ToComponentDataArray<Translation>(Allocator.TempJob);
            var sourcePos = sourceBlock.ToComponentDataArray<Translation>(Allocator.TempJob);
            var plantPos = surfaceplants.ToComponentDataArray<Translation>(Allocator.TempJob);

            var targetEntity = targetBlocks.ToEntityArray(Allocator.TempJob);
            var sourceEntity = sourceBlock.ToEntityArray(Allocator.TempJob);
            var plantEntity = surfaceplants.ToEntityArray(Allocator.TempJob);

            for(int i=0; i<sourcePos.Length; i++){
                for(int j=0; j< targetPos.Length; j++){
                    Vector3 offset = targetPos[j].Value- sourcePos[i].Value;
                    float sqrLen = offset.sqrMagnitude;

                    //find the block to destroy
                    if (sqrLen == 0)
                   {
                       
                        //remove the plant from the surface;
                        for (int k = 0; k < plantPos.Length;k++)
                        {
                            float3 tmpPos = new float3(plantPos[k].Value.x, plantPos[k].Value.y+Vector3.down.y, plantPos[k].Value.z);
                            offset = targetPos[j].Value - tmpPos;
                            sqrLen = offset.sqrMagnitude;

                            if (sqrLen == 0)
                            {
                                PostUpdateCommands.DestroyEntity(plantEntity[k]);
                            }
                        }

                        //remove blocks
                        PostUpdateCommands.DestroyEntity(sourceEntity[i]);
                        PostUpdateCommands.DestroyEntity(targetEntity[j]);
                    }
                }
            }

            targetPos.Dispose();
            sourcePos.Dispose();
            plantPos.Dispose();

            targetEntity.Dispose();
            sourceEntity.Dispose();
            plantEntity.Dispose();

        }

    }
}
