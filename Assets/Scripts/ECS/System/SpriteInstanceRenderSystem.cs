using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Experimental.PlayerLoop;
using Unity.Transforms2D;

namespace Minecraft.Rendering2D
{
    /// <summary>
    /// Renders all Entities containing both SpriteInstanceRenderer & TransformMatrix components.
    /// </summary>
    [UpdateAfter(typeof(PreLateUpdate.ParticleSystemBeginUpdateAll))]
    [ExecuteInEditMode]
    public class SpriteInstanceRendererSystem : ComponentSystem
    {
        private readonly Dictionary<SpriteInstanceRenderer, Material> materialCahce = new Dictionary<SpriteInstanceRenderer, Material>();
        private readonly Dictionary<SpriteInstanceRenderer, Mesh> meshCache = new Dictionary<SpriteInstanceRenderer, Mesh>();

        // Instance renderer takes only batches of 1023
        private readonly Matrix4x4[] matricesArray = new Matrix4x4[1023];
        private readonly List<SpriteInstanceRenderer> cacheduniqueRendererTypes = new List<SpriteInstanceRenderer>(10);
        private ComponentGroup instanceRendererGroup;

        // This is the ugly bit, necessary until Graphics.DrawMeshInstanced supports NativeArrays pulling the data in from a job.
        private static unsafe void CopyMatrices(ComponentDataArray<TransformMatrix> transforms, int beginIndex, int length, Matrix4x4[] outMatrices)
        {
            // @TODO: This is using unsafe code because the Unity DrawInstances API takes a Matrix4x4[] instead of NativeArray.
            // We want to use the ComponentDataArray.CopyTo method
            // because internally it uses memcpy to copy the data,
            // if the nativeslice layout matches the layout of the component data. It's very fast...
            fixed (Matrix4x4* matricesPtr = outMatrices)
            {
                Assert.AreEqual(sizeof(Matrix4x4), sizeof(TransformMatrix));
                var matricesSlice = NativeSliceUnsafeUtility.ConvertExistingDataToNativeSlice<TransformMatrix>(matricesPtr, sizeof(Matrix4x4), length);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                NativeSliceUnsafeUtility.SetAtomicSafetyHandle(ref matricesSlice, AtomicSafetyHandle.GetTempUnsafePtrSliceHandle());
#endif

                transforms.CopyTo(matricesSlice, beginIndex);
            }
        }

        protected override void OnCreateManager(int capacity)
        {
            // We want to find all MeshInstanceRenderer & TransformMatrix combinations and render them
            instanceRendererGroup = GetComponentGroup(typeof(SpriteInstanceRenderer), typeof(TransformMatrix));
        }

        protected override void OnUpdate()
        {
            // We want to iterate over all unique MeshInstanceRenderer shared component data,
            // that are attached to any entities in the world
            EntityManager.GetAllUniqueSharedComponentDatas(cacheduniqueRendererTypes);
            for (int i = 0; i != cacheduniqueRendererTypes.Count; i++)
            {
                // For each unique MeshInstanceRenderer data, we want to get all entities with a TransformMatrix
                // SharedComponentData gurantees that all those entities are packed togehter in a chunk with linear memory layout.
                // As a result the copy of the matrices out is internally done via memcpy.
                var renderer = cacheduniqueRendererTypes[i];
                if (renderer.sprite == null)
                    continue;
                instanceRendererGroup.SetFilter(renderer);
                var transforms = instanceRendererGroup.GetComponentDataArray<TransformMatrix>();

                Mesh mesh;
                Material material;
                var size = math.max(renderer.sprite.width, renderer.sprite.height) / (float)renderer.pixelsPerUnit;
                float2 meshPivot = renderer.pivot * size;
                if (!meshCache.TryGetValue(renderer, out mesh))
                {
                    mesh = MeshUtils.GenerateQuad(size, meshPivot);
                    meshCache.Add(renderer, mesh);
                }

                if (!materialCahce.TryGetValue(renderer, out material))
                {
                    material = new Material(Shader.Find("Sprites/Instanced"))
                    {
                        enableInstancing = true,
                        mainTexture = renderer.sprite
                    };
                    materialCahce.Add(renderer, material);
                }

                // Graphics.DrawMeshInstanced has a set of limitations that are not optimal for working with ECS.
                // Specifically:
                // * No way to push the matrices from a job
                // * no NativeArray API, currently uses Matrix4x4[]
                // As a result this code is not yet jobified.
                // We are planning to adjust this API to make it more efficient for this use case.

                // For now, we have to copy our data into Matrix4x4[] with a specific upper limit of how many instances we can render in one batch.
                // So we just have a for loop here, representing each Graphics.DrawMeshInstanced batch
                int beginIndex = 0;
                while (beginIndex < transforms.Length)
                {
                    int length = math.min(matricesArray.Length, transforms.Length - beginIndex);
                    CopyMatrices(transforms, beginIndex, length, matricesArray);
                    Graphics.DrawMeshInstanced(mesh, 0, material, matricesArray, length);

                    beginIndex += length;
                }
            }
            cacheduniqueRendererTypes.Clear();
        }
    }
}