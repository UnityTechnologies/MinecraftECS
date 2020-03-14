using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;

namespace Minecraft
{
    public class PickaxeController : MonoBehaviour
    {
        //Vector2 mouseLook;

        public LayerMask blockLayer;
        //private static Minecraft.GameSettings MatRef;

        GameObject player;
        //public GameObject playerEntity;
        public static Transform BlockToDestroy;

        Material BlockToPlace;

        public static int blockID = 1;


        public AudioClip grass_audio;
        public AudioClip stone_audio;
        public AudioClip dirt_audio;
        public AudioClip wood_audio;

        AudioSource AS;

        public ParticleSystem digEffect;

        //bool stepAudioIsPlaying = false;

        EntityManager manager;

        void Start()
        {
            AS = transform.GetComponent<AudioSource>();
            player = this.transform.parent.gameObject;
            Cursor.lockState = CursorLockMode.Locked;

            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        void Update()
        {
            /*
            if (Input.GetButtonUp("Q") && Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (Input.GetButtonDown("Q") && Cursor.lockState == CursorLockMode.Locked)
            {
                Cursor.lockState = CursorLockMode.None;
            }*/

            if (blockID > 7)
            {
                blockID = 1;
            }
            if (blockID < 1)
            {
                blockID = 7;
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                blockID++;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                blockID--;
            }


            #region // blocklist
            if (blockID == 1)
            {
                //stone
                BlockToPlace = Minecraft.GameSettings.GM.stoneMaterial;
            }
            else if (blockID == 2)
            {
                //plank
                BlockToPlace = Minecraft.GameSettings.GM.plankMaterial;
            }
            else if (blockID == 3)
            {
                //glass
                BlockToPlace = Minecraft.GameSettings.GM.glassMaterial;
            }
            else if (blockID == 4)
            {
                //wood
                BlockToPlace = Minecraft.GameSettings.GM.woodMaterial;
            }
            else if (blockID == 5)
            {
                //cobble
                BlockToPlace = Minecraft.GameSettings.GM.cobbleMaterial;
            }
            else if (blockID == 6)
            {
                //TNT
                BlockToPlace = Minecraft.GameSettings.GM.tntMaterial;
            }
            else if (blockID == 7)
            {
                //brick
                BlockToPlace = Minecraft.GameSettings.GM.brickMaterial;
            }
            #endregion

            //right click to place block
            if (Input.GetMouseButtonDown(1))
            {
                PlaceBlock(BlockToPlace);
            }

            //left click to dig
            if (Input.GetMouseButtonDown(0))
            {
                DestroyBlock();
            }

        }

        /*
        IEnumerator PlayStep(AudioClip audioClip)
        {
            stepAudioIsPlaying = true;
            AS.PlayOneShot(audioClip, 0.1f);
            yield return new WaitForSecondsRealtime(0.5f);
            stepAudioIsPlaying = false;
        }*/

        void PlaceBlock(Material Block)
        {
            RaycastHit hitInfo;
            Physics.Raycast(transform.position, transform.forward, out hitInfo, 7, blockLayer);
            if (hitInfo.transform != null)
            {

                if (blockID == 1 || blockID == 3 || blockID == 5 || blockID == 7)
                {
                    AS.PlayOneShot(stone_audio);
                }
                else if (blockID == 2 || blockID == 4)
                {
                    AS.PlayOneShot(wood_audio);
                }

                Entity entities = manager.CreateEntity(Minecraft.GameSettings.BlockArchetype);
                manager.SetComponentData(entities, new Translation { Value = hitInfo.transform.position + hitInfo.normal });
                manager.AddComponentData(entities, new BlockTag { });
                manager.AddSharedComponentData(entities, new RenderMesh
                {
                    mesh = Minecraft.GameSettings.GM.blockMesh,
                    material = Block
                });

                //add a box collider at the same location of the entity
                Translation posTemp = new Translation { Value = (hitInfo.transform.position + hitInfo.normal) };
                Minecraft.ColliderPool.CP.AddCollider(posTemp.Value);
            }
        }

        void DestroyBlock()
        {
            RaycastHit hitInfo;
            Physics.Raycast(transform.position, transform.forward, out hitInfo, 7, blockLayer);
            if (hitInfo.transform != null)
            {
                Entity entities = manager.CreateEntity(Minecraft.GameSettings.BlockArchetype);
                manager.SetComponentData(entities, new Translation { Value = hitInfo.transform.position });
                manager.AddComponentData(entities, new DestroyTag {});

                //move the dig effect to the position and play
                if (digEffect && !digEffect.isPlaying)
                {
                    digEffect.transform.position = hitInfo.transform.position;
                    digEffect.Play();
                }

                AS.PlayOneShot(dirt_audio);
                Destroy(hitInfo.transform.gameObject);
            }
        }
    }
}