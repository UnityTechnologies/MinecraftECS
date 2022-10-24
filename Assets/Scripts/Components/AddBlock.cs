using System;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

    public struct AddBlock : IComponentData
    {
        public float3 spawnPos;
        public int spawnType;
        public float spawnMat;
    }