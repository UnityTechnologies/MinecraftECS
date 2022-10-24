using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


    class PlantBlockAuthoring : MonoBehaviour
    {
    }

    class PlantBlockBaker : Baker<PlantBlockAuthoring>
    {
        public override void Bake(PlantBlockAuthoring authoring)
        {
            AddComponent(new PlantBlock{});
        }
    }
    

