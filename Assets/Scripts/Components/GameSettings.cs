using System;
using Unity.Entities;
using UnityEngine;

    public struct GameSettings : IComponentData
    {
        public int chunkSize;
    }