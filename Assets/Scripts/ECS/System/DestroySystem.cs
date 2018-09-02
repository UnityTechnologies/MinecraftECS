using Unity.Collections;
using Unity.Entities;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class DestroySystem : ComponentSystem
    {
        struct BlockGroup
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray entity;
            [ReadOnly] public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<BlockTag> tags;
        }

        struct DestoryBlockGroup
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray entity;
            [ReadOnly]public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<DestroyTag> tags;
        }

        struct SurfacePlantGroup
        {
            [ReadOnly] public readonly int Length;
            [ReadOnly] public EntityArray entity;
            [ReadOnly] public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<SurfacePlantTag> tags;
        }
        [Inject] BlockGroup targetBlocks;
        [Inject] DestoryBlockGroup sourceBlock;
        [Inject] SurfacePlantGroup surfaceplants;

        protected override void OnUpdate()
        {
            for (int i = 0; i < sourceBlock.Length; i++)
            {
                for (int j = 0; j < targetBlocks.Length; j++)
                {
                    Vector3 offset = targetBlocks.positions[j].Value- sourceBlock.positions[i].Value;
                    float sqrLen = offset.sqrMagnitude;

                    //find the block to destroy
                    if (sqrLen == 0)
                   {
                        //remove the plant from the surface;
                        for (int k = 0; k < surfaceplants.Length;k++)
                        {
                            float3 tmpPos = new float3(surfaceplants.positions[k].Value.x, surfaceplants.positions[k].Value.y+Vector3.down.y, surfaceplants.positions[k].Value.z);
                            offset = targetBlocks.positions[j].Value - tmpPos;
                            sqrLen = offset.sqrMagnitude;

                            if (sqrLen == 0)
                            {
                                PostUpdateCommands.DestroyEntity(surfaceplants.entity[k]);
                            }
                        }

                        //remove blocks
                        PostUpdateCommands.DestroyEntity(sourceBlock.entity[i]);
                        PostUpdateCommands.DestroyEntity(targetBlocks.entity[j]);
                    }
                }
            }
        }
    }
}
