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
            public readonly int Length;
            [ReadOnly] public EntityArray entity;
            [ReadOnly] public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<BlockTag> tags;
        }

        struct DestoryBlockGroup
        {
            public readonly int Length;
            [ReadOnly] public EntityArray entity;
            [ReadOnly]public ComponentDataArray<Position> positions;
            [ReadOnly] public ComponentDataArray<DestroyTag> tags;
        }
        [Inject] BlockGroup targetBlocks;
        [Inject] DestoryBlockGroup sourceBlock;

        protected override void OnUpdate()
        {
            for (int i = 0; i < sourceBlock.Length; i++)
            {
                for (int j = 0; j < targetBlocks.Length; j++)
                {
                    Vector3 tpos = targetBlocks.positions[j].Value;
                    Vector3 spos = sourceBlock.positions[i].Value;
                    if (tpos == spos)
                   {
                        PostUpdateCommands.DestroyEntity(sourceBlock.entity[i]);
                        PostUpdateCommands.DestroyEntity(targetBlocks.entity[j]);
                    }
                }
            }
        }
    }
}
