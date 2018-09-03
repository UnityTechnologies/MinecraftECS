using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class SpawnOneBlock : MonoBehaviour
    {

        public static EntityArchetype BlockArchetype;

        [Header("Mesh Info")]
        public Mesh blockMesh;

        [Header("Nature Block Type")]
        public Material blockMaterial;

        public EntityManager manager;
        public Entity entities;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {

            EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();

            // Create an archetype for basic blocks.
            BlockArchetype = manager.CreateArchetype(
                typeof(Position)
            );
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        void Start()
        {
            manager = World.Active.GetOrCreateManager<EntityManager>();

            Entity entities = manager.CreateEntity(BlockArchetype);
            manager.SetComponentData(entities, new Position { Value = new int3(0, 0, 0) });
            manager.AddComponentData(entities, new BlockTag { });

            manager.AddSharedComponentData(entities, new MeshInstanceRenderer
            {
                mesh = blockMesh,
                material = blockMaterial
            });
        }
    }
}
