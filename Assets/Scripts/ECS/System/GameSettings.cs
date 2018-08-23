using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft
{
    public class GameSettings : MonoBehaviour
    {
        public static GameSettings GM;
        public static Texture2D Heightmap;

        public static EntityArchetype BlockArchetype;


        [Header("Block Info")]
        public GameObject blockPrefab;
        //public GameObject spritePrefab;
        [Header("Chunk Info")]
        public int ChunkCount = 1;
        //public int ChunkCountIncremement = 1;

        [Header("Material Info")]
        public Mesh blockMesh;
        public Mesh dirtMesh;
        //public Mesh tallgrassMesh;
        public Material stoneMaterial;
        public Material brickMaterial;
        public Material cobbleMaterial;
        public Material dirtMaterial;
        public Material glassMaterial;
        public Material grassMaterial;
        public Material grassTopMaterial;
        //public Material roseMaterial;
        //public Material tallGrassMaterial;

        public Material woodsideMaterial;
        public Material woodtopMaterial;
        public Material dirtwgrassMaterial;

        /*
        public Material no1Mat;
        public Material no2Mat;
        public Material no3Mat;
        public Material no4Mat;
        public Material no5Mat;
        public Material no6Mat;
        public Material no0Mat;
        public Material noQMat;
*/

        //int RanMat;
        Material Matemp;
        Mesh Meshtemp;

        void Awake()
        {
            if (GM != null && GM != this)
                Destroy(gameObject);
            else
                GM = this;
        }

        EntityManager manager;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            // This method creates archetypes for entities we will spawn frequently in this game.
            // Archetypes are optional but can speed up entity spawning substantially.

            EntityManager manager = World.Active.GetOrCreateManager<EntityManager>();

            // Create an archetype for basic blocks.
            BlockArchetype = manager.CreateArchetype(
                typeof(TransformMatrix),
                typeof(Position));
             //typeof(MeshInstanceRenderer));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        void Start()
        {
            manager = World.Active.GetOrCreateManager<EntityManager>();
            GenerateChunk(ChunkCount);
           //GenerateSprite(ChunkCount);
        }

        void GenerateChunk(int amount)
        {

            int totalamount = (amount * amount) * 1500;
            int ordernumber = 0;
            int hightlevel;
            bool airChecker;

            //NativeArray<Entity> entities = new NativeArray<Entity>(totalamount, Allocator.Temp);
            //manager.Instantiate(blockPrefab, entities);



            for (int i = 0; i < amount; i++)
            {

                if (ordernumber == totalamount)
                    return;

                //Block ordering from X*0,0,0 to 10,10,10( * ChunkCount)
                for (int yBlock = 0; yBlock < 15; yBlock++)
                {
                    for (int xBlock = 0; xBlock < 10 * amount; xBlock++)
                    {
                        for (int zBlock = 0; zBlock < 10 * amount; zBlock++)
                        {
                            hightlevel = (int)(Heightmap.GetPixel(xBlock, zBlock).r * 100) - yBlock;
                            airChecker = false;

                            switch (hightlevel)
                            {
                                case 0:
                                    //Meshtemp = tallgrassMesh;
                                    //Matemp = tallGrassMaterial;
                                    /*
                                    RanMat = Random.Range(1, 201);
                                    if (RanMat <= 20)
                                    {
                                        //Matemp = grassTopMaterial;

                                    }
                                    if (RanMat == 200)
                                    {
                                        //Matemp = roseMaterial;

                                    }
                                    if (RanMat == 199)
                                    {
                                        //Matemp = woodsideMaterial;

                                    }*/
                                    //break;
                                    //surface
                                    airChecker = true;
                                    //Meshtemp = blockMesh;
                                    //Matemp = no0Mat;
                                    break;
                                case 1:
                                    Meshtemp = dirtMesh;
                                    Matemp = dirtwgrassMaterial;
                                    break;
                                case 2:
                                case 3:
                                case 4:
                                    //Dirt
                                    Meshtemp = blockMesh;
                                    Matemp = dirtMaterial;
                                    break;
                                case 5:
                                case 6:
                                    //stone block
                                    Meshtemp = blockMesh;
                                    Matemp = stoneMaterial;
                                    break;
                                case 7:
                                case 8:
                                    Meshtemp = blockMesh;
                                    Matemp = cobbleMaterial;
                                    break;
                                default:
                                    //airBlock or any highlevel < 0
                                    airChecker = true;
                                
                                    break;

                            }

                            if (!airChecker)
                            {
                                Entity entities = manager.CreateEntity(BlockArchetype);
                                manager.SetComponentData(entities, new Position { Value = new float3(xBlock, yBlock, zBlock) });
                                //manager.SetSharedComponentData(entities[ordernumber], new MeshInstanceRenderer
                                //{
                                //  mesh = Meshtemp,
                                //material = Matemp
                                //});

                                manager.AddSharedComponentData(entities, new MeshInstanceRenderer
                                {
                                    mesh = Meshtemp,
                                    material = Matemp
                                });

                            }

                            ordernumber += 1;
                        }
                    }
                }
            }
            //entities.Dispose();
        }
    }
}
