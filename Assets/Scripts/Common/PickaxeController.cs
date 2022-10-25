using System;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

namespace Unity.Physics.Extensions
{
    public class PickaxeController : MonoBehaviour
    {
        public float Distance = 10.0f;
        public Vector3 Direction = new Vector3(0, 0, 1);

        protected RaycastInput RaycastInput;
        protected NativeList<RaycastHit> RaycastHits;

        public static int m_blockID = 1;
        public AudioClip grass_audio;
        public AudioClip stone_audio;
        public AudioClip dirt_audio;
        public AudioClip wood_audio;
        AudioSource AS;

        public ParticleSystem digEffect;
        // Contrarily to ISystem, SystemBase systems are classes.
        // They are not Burst compiled, and can use managed code.
        void Start()
        {
            AS = this.GetComponent<AudioSource>();
            Cursor.lockState = CursorLockMode.Locked;
            RaycastHits = new NativeList<RaycastHit>(Allocator.Persistent);
        }

        void Update()
        {
            //scroll blocks
            float m_mat = 0f;
            int m_block = 1;

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                m_blockID++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                m_blockID--;
            }

            if (m_blockID > 7)
            {
                m_blockID = 1;
            }
            if (m_blockID < 1)
            {
                m_blockID = 7;
            }

            //m_block list
            //0 = sixSided
            //1 = default
            //2 = alpha
            //3 = plant

            #region // blocklist
            if (m_blockID == 1)
            {
                //stone
                m_block = 1;
                m_mat = 1;
            }
            else if (m_blockID == 2)
            {
                //plank
                m_block = 1;
                m_mat = 4;
            }
            else if (m_blockID == 3)
            {
                //glass
                m_block = 2;
                m_mat = 49;
            }
            else if (m_blockID == 4)
            {
                //wood
                m_block = 0;
                m_mat = 0.67f;
            }
            else if (m_blockID == 5)
            {
                //cobble
                m_block = 1;
                m_mat = 16;
            }
            else if (m_blockID == 6)
            {
                //TNT
                m_block = 0;
                m_mat = 0.33f;
            }
            else if (m_blockID == 7)
            {
                //brick
                m_block = 1;
                m_mat = 7;
            }
            #endregion
            
            if(Input.GetButtonDown("Fire1"))
            {
                //Left click to place block
                PlaceNRemoveBlock(0, 0,false);
            }
            if(Input.GetButtonDown("Fire2"))
            {
                //Right click to place block
                PlaceNRemoveBlock(m_block, m_mat, true);
            }
            
        }

        void OnDestroy()
        {
            if (RaycastHits.IsCreated)
            {
                RaycastHits.Dispose();
            }
        }

        void PlaceNRemoveBlock(int m_block, float m_mat, bool b_place)
        {
            
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp)
                .WithAll<PhysicsWorldSingleton>();
            EntityQuery singletonQuery = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntityQuery(builder);
            PhysicsWorld phyworld = singletonQuery.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;
            float3 origin = transform.position;
            float3 direction = (transform.rotation * Direction) * Distance;

            RaycastHits.Clear();
            singletonQuery.Dispose();


            if (math.any(new float3(Direction) != float3.zero))
            {
            

                RaycastInput = new RaycastInput
                {
                    Start = origin,
                    End = origin + direction,
                    Filter = CollisionFilter.Default
                };

                if(phyworld.CastRay(RaycastInput, out RaycastHit hit))
                {
                    var world = World.DefaultGameObjectInjectionWorld;
                    var entityManager = world.EntityManager;

                    if(b_place)
                    {
                        //play place sound effect
                        if (m_blockID == 1 || m_blockID == 3 || m_blockID == 5 || m_blockID == 7)
                        {
                            AS.PlayOneShot(stone_audio);
                        }
                        else if (m_blockID == 2 || m_blockID == 4 || m_blockID == 6)
                        {
                            AS.PlayOneShot(wood_audio);
                        }

                        //Make sure this is a Block
                        if(entityManager.HasComponent<BlockID>(hit.Entity))
                        {
                            //add a entity
                            //EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
                            var newBlock = entityManager.CreateEntity();

                            var blockPos = entityManager.GetComponentData<LocalToWorld>(hit.Entity);

                            var newposition = hit.SurfaceNormal + blockPos.Position;
                            entityManager.AddComponentData(newBlock, new AddBlock {spawnPos = newposition, spawnType = m_block, spawnMat= m_mat});
                        }

                        //ecb.Dispose();
                    }
                    //Remove a block
                    else
                    {
                        if(entityManager.HasComponent<BlockID>(hit.Entity))
                        {
                            if (digEffect && !digEffect.isPlaying)
                            {
                                digEffect.transform.position = hit.Position;
                                digEffect.Play();
                            }

                            AS.PlayOneShot(dirt_audio);
                            //add a remove tag on this block
                            entityManager.AddComponentData(hit.Entity, new RemoveBlock());
                        }
                    }
                }
            }
        }
    }
}
