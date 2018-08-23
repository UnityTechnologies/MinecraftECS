using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Minecraft
{
    [Serializable]
    public struct SpriteInstanceRenderer : ISharedComponentData
    {
        public Texture2D sprite;
        public int pixelsPerUnit;
        public float2 pivot;

        public SpriteInstanceRenderer(Texture2D sprite, int pixelsPerUnit, float2 pivot)
        {
            this.sprite = sprite;
            this.pixelsPerUnit = pixelsPerUnit;
            this.pivot = pivot;
        }
    }

    public class SpriteInstanceRendererComponent : SharedComponentDataWrapper<SpriteInstanceRenderer> { }
}