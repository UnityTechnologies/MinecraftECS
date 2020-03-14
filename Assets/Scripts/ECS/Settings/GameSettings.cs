using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

namespace Minecraft {
    public class GameSettings : MonoBehaviour {

        #region Fields
        public static GameSettings GM;
        public static Texture2D Heightmap;

        public static EntityArchetype BlockArchetype;

        [Header ("World = ChunkBase x ChunkBase")]
        public int ChunkBase;

        [Header ("Mesh Info")]
        public Mesh blockMesh;
        public Mesh surfaceMesh;
        public Mesh tallGrassMesh;

        [Header ("Nature Block Type")]
        public Material stoneMaterial;
        public Material woodMaterial;
        public Material leavesMaterial;
        public Material surfaceMaterial;
        public Material cobbleMaterial;
        public Material dirtMaterial;
        public Material tallGrassMaterial;
        public Material roseMaterial;
        public Material CloudMaterial;

        [Header ("Other Block Type")]
        public Material glassMaterial;
        public Material brickMaterial;
        public Material plankMaterial;
        public Material tntMaterial;
        [Header ("")]
        public Material pinkMaterial;

        [Header ("Collision Settings")]
        public bool createCollider;

        int ranDice;
        Material maTemp;
        Mesh meshTemp;

        public EntityManager manager;
        public Entity entities;

        #endregion

        #region Original Methods
        void Awake () {
            if (GM != null && GM != this)
                Destroy (gameObject);
            else
                GM = this;
        }

        [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize () {
            // This method creates archetypes for entities we will spawn frequently in this game.
            // Archetypes are optional but can speed up entity spawning substantially.

            EntityManager manager = World.DefaultGameObjectInjectionWorld.EntityManager;

            // Create an archetype for basic blocks.
            BlockArchetype = manager.CreateArchetype (
                typeof (LocalToWorld),
                typeof (Translation),
                typeof (RenderMesh),
                typeof (RenderBounds)
            );
        }

        [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.AfterSceneLoad)]
        void Start () {
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            //Generate the world
            ChunkGenerator (ChunkBase);
        }

        void ChunkGenerator (int amount) {

            int hightlevel;
            bool airChecker;

            //Block ordering from X*0,0,0 to 15,10,10( * Chunk x2)
            for (int yBlock = 0; yBlock < 15; yBlock++) {
                for (int xBlock = 0; xBlock < 10 * amount; xBlock++) {
                    for (int zBlock = 0; zBlock < 10 * amount; zBlock++) {
                        hightlevel = (int) (Heightmap.GetPixel (xBlock, zBlock).r * 100) - yBlock;
                        airChecker = false;
                        Vector3 posTemp = new Vector3 (xBlock, yBlock, zBlock);

                        switch (hightlevel) {
                            case 0:
                                Debug.Log ("height = " + hightlevel + ", plant or tree or flower or cloud");
                                //random surface block
                                ranDice = UnityEngine.Random.Range (1, 201);
                                if (ranDice <= 20) {
                                    //grass
                                    PlantGenerator (xBlock, yBlock, zBlock, 1);
                                }
                                if (ranDice == 198) {
                                    //clouds
                                    CloudGenerator (xBlock, yBlock, zBlock);
                                }
                                if (ranDice == 200) {
                                    //rose
                                    PlantGenerator (xBlock, yBlock, zBlock, 2);
                                }
                                if (ranDice == 199) {
                                    //tree
                                    TreeGenerator (xBlock, yBlock, zBlock);

                                }
                                airChecker = true;
                                break;
                            case 1:
                                Debug.Log ("height = " + hightlevel + ", surface");
                                meshTemp = surfaceMesh;
                                maTemp = surfaceMaterial;
                                break;
                            case 2:
                            case 3:
                            case 4:
                                //Dirt
                                Debug.Log ("height = " + hightlevel + ", dirt");
                                meshTemp = blockMesh;
                                maTemp = dirtMaterial;
                                break;
                            case 5:
                            case 6:
                                //stone block
                                Debug.Log ("height = " + hightlevel + ", stone");
                                meshTemp = blockMesh;
                                maTemp = stoneMaterial;
                                break;
                            case 7:
                            case 8:
                                Debug.Log ("height = " + hightlevel + ", cobble");
                                meshTemp = blockMesh;
                                maTemp = cobbleMaterial;
                                break;
                            default:
                                Debug.Log ("height = " + hightlevel + ", default");
                                //airBlock or anything higher level < 0
                                airChecker = true;
                                break;
                        }

                        if (!airChecker) {

                            if (!maTemp)
                                maTemp = pinkMaterial;

                            //Add a collider for all blocks
                            //maybe this is not a best way to do collision, so it's just a temporary solution until we can use ECS to handle it.
                            AddCollider (posTemp);

                            Entity entities = manager.CreateEntity (BlockArchetype);
                            manager.SetComponentData (entities, new Translation { Value = new int3 (xBlock, yBlock, zBlock) });
                            manager.AddComponentData (entities, new BlockTag { });

                            manager.AddSharedComponentData (entities, new RenderMesh {
                                mesh = meshTemp,
                                    material = maTemp
                            });
                        }
                    }
                }
            }
        }
        void TreeGenerator (int xPos, int yPos, int zPos) {
            //xpos,ypos,zpos is the root position of the tree that we are going to plant.
            //woods
            for (int i = yPos; i < yPos + 7; i++) {
                //top leaves
                if (i == yPos + 6) {
                    maTemp = leavesMaterial;
                } else {
                    maTemp = woodMaterial;
                }

                if (!maTemp)
                    maTemp = pinkMaterial;

                //these are temporary codes, until we have a collision system for ECS.
                Vector3 posTemp = new Vector3 (xPos, i, zPos);
                AddCollider (posTemp);

                Entity entities = manager.CreateEntity (BlockArchetype);
                manager.SetComponentData (entities, new Translation { Value = new int3 (xPos, i, zPos) });
                manager.AddComponentData (entities, new BlockTag { });
                manager.AddSharedComponentData (entities, new RenderMesh {
                    mesh = blockMesh,
                        material = maTemp
                });

                //leaves
                if (i >= yPos + 3 && i <= yPos + 6) {
                    for (int j = xPos - 1; j <= xPos + 1; j++) {
                        for (int k = zPos - 1; k <= zPos + 1; k++) {
                            if (k != zPos || j != xPos) {
                                //this is a temporary line.
                                posTemp = new Vector3 (j, i, k);
                                AddCollider (posTemp);

                                entities = manager.CreateEntity (BlockArchetype);
                                manager.SetComponentData (entities, new Translation { Value = new int3 (j, i, k) });
                                manager.AddComponentData (entities, new BlockTag { });
                                manager.AddSharedComponentData (entities, new RenderMesh {
                                    mesh = blockMesh,
                                        material = leavesMaterial
                                });
                            }
                        }
                    }
                }
            }
        }

        void PlantGenerator (int xPos, int yPos, int zPos, int plantType) {

            //xpos,ypos,zpos is the root position of the plant that we are going to build.
            //rose
            if (plantType == 1) {
                maTemp = tallGrassMaterial;
            } else {
                maTemp = roseMaterial;
            }

            if (!maTemp)
                maTemp = pinkMaterial;

            Quaternion rotation = Quaternion.Euler (0, 45, 0);
            Entity entities = manager.CreateEntity (BlockArchetype);
            manager.SetComponentData (entities, new Translation { Value = new int3 (xPos, yPos, zPos) });
            manager.AddComponentData (entities, new Rotation { Value = rotation });
            manager.AddComponentData (entities, new SurfacePlantTag { });
            manager.AddSharedComponentData (entities, new RenderMesh {
                mesh = tallGrassMesh,
                    material = maTemp
            });
        }

        void CloudGenerator (int xPos, int yPos, int zPos) {

            meshTemp = blockMesh;
            maTemp = CloudMaterial;

            if (!maTemp)
                maTemp = pinkMaterial;

            ranDice = UnityEngine.Random.Range (4, 7);

            for (int i = 0; i < ranDice; i++) {
                for (int j = 0; j < ranDice; j++) {
                    Entity entities = manager.CreateEntity (BlockArchetype);
                    manager.SetComponentData (entities, new Translation { Value = new int3 (xPos + i, yPos + 15, zPos + j) });
                    manager.AddSharedComponentData (entities, new RenderMesh {
                        mesh = meshTemp,
                            material = maTemp
                    });
                }
            }
        }

        void AddCollider (Vector3 posTemp) {
            if (createCollider)
                GM.GetComponent<ColliderPool> ().AddCollider (posTemp);
        }

        #endregion

    }
}