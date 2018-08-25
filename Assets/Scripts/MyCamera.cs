using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Transforms;

public class MyCamera : MonoBehaviour {

    Vector2 mouseLook;
    //Vector2 smoothV;
    //Vector3 interactionPoint;
    //public float sensitivity = 5f;
    //public float smoothing = 2f;

    public LayerMask LmaskBlockPlacing;
    public LayerMask LmaskBlockDestruction;

    GameObject player;
    public static Transform BlockToDestroy;

    //blockplacing
    public Transform PlacingGraphics;
    Material BlockToPlace;

    /*
    //blocks
    public Material Stone_block;
    public Material Plank_block;
    public Material Glass_block;
    public Material Wood_block;
    public Material Cobble_block;
    public Material Brick_block;
    public Material TNT_block;
    */
    //UI
    public static int block_ = 1;


    public AudioClip grass_audio;
    public AudioClip stone_audio;
    public AudioClip dirt_audio;
    public AudioClip wood_audio;

    AudioSource AS;

    bool stepAudioIsPlaying = false;

    //
    //public static EntityArchetype BlockArchetype;
    EntityManager manager;

    void Start () 
    {
        AS = transform.GetComponent<AudioSource>();
        player = this.transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;

       
        manager = World.Active.GetOrCreateManager<EntityManager>();
    }
	
	void Update () {
   
        if (Input.GetButtonUp("Q") && Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        if (Input.GetButtonDown("Q") && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
        }

        if (block_ > 7)
        {
            block_ = 1;
        }
        if (block_ < 1)
        {
            block_ = 7;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            block_++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            block_--;
        }


        #region // blocklist
        if (block_ == 1)
        {
            //stone
            BlockToPlace = Minecraft.GameSettings.GM.stoneMaterial;
        }
        else if (block_ == 2)
        {
            //plank
            BlockToPlace = Minecraft.GameSettings.GM.plankMaterial;
        }
        else if (block_ == 3)
        {
            //glass
            BlockToPlace = Minecraft.GameSettings.GM.glassMaterial;
        }
        else if (block_ == 4)
        {
            //wood
            BlockToPlace = Minecraft.GameSettings.GM.woodMaterial;
        }
        else if (block_ == 5)
        {
            //cobble
            BlockToPlace = Minecraft.GameSettings.GM.cobbleMaterial;
        }
        else if (block_ == 6)
        {
            //TNT
            BlockToPlace = Minecraft.GameSettings.GM.tntMaterial;
        }
        else if (block_ == 7)
        {
            //brick
            BlockToPlace = Minecraft.GameSettings.GM.brickMaterial;
        }
        #endregion


        //PlaceBlockGraphics();
        if (Input.GetMouseButtonDown(1))
        {
            PlaceBlock(BlockToPlace);
        }

        if (Input.GetMouseButtonDown(0))
        {
            DestroyBlock();
        }

    }

    IEnumerator PlayStep(AudioClip audioClip)
    {
        stepAudioIsPlaying = true;
        AS.PlayOneShot(audioClip, 0.1f);
        yield return new WaitForSecondsRealtime(0.5f);
        stepAudioIsPlaying = false;
    }

    void PlaceBlock(Material Block)
    {
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 5, LmaskBlockPlacing);
        if (hitInfo.transform != null)
        {

            if (block_ == 1 || block_ == 3 || block_ == 5 || block_ == 7)
            {
                AS.PlayOneShot(stone_audio);
            }
            else if (block_ == 2 || block_ == 4)
            {
                AS.PlayOneShot(wood_audio);
            }

            Entity entities = manager.CreateEntity(Minecraft.GameSettings.BlockArchetype);
            manager.SetComponentData(entities, new Position { Value = hitInfo.transform.position + hitInfo.normal });
            manager.AddSharedComponentData(entities, new MeshInstanceRenderer
            {
                mesh = Minecraft.GameSettings.GM.blockMesh,
                material = Block
            });

            //add a box collider at the same location of the entity
            Position posTemp = new Position(hitInfo.transform.position + hitInfo.normal);
            Minecraft.ColliderPool.CP.AddCollider(posTemp.Value);
        }
    }

    void PlaceBlockGraphics()
    {
        /*
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 5, LmaskBlockPlacing);
        //Debug.DrawRay(transform.position, transform.forward, Color.yellow);

        if(hitInfo.transform == null)
        {
            PlacingGraphics.position = new Vector3(0,0,0);
        }*/
    }

    void DestroyBlock()
    {
        /*
        RaycastHit hitInfo;
        Physics.Raycast(transform.position, transform.forward, out hitInfo, 5, LmaskBlockDestruction);
        if (hitInfo.transform != null)
        {

            if (hitInfo.transform.tag == "Block" && hitInfo.transform.name != "TNT(Clone)")
            {
                if (hitInfo.transform.name == "Dirt_block")
                {
                    AS.PlayOneShot(dirt_audio);
                }
            }
        }*/
    }
}
