using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


    class BlockIDAuthoring : MonoBehaviour
    {
        public float blockID;
    }

    class BlockIDBaker : Baker<BlockIDAuthoring>
    {
        public override void Bake(BlockIDAuthoring authoring)
        {
            AddComponent(new BlockID
            {
                blockID = authoring.blockID
            });
        }
    }
    

