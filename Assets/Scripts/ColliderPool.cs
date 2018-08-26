using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minecraft
{
    public class ColliderPool : MonoBehaviour
    {
    //idealy I want to make a pool system for reuse these colliders but still don't find a good way to do it.
        public static ColliderPool CP;
        public GameObject boxCollider;

        void Awake()
        {
            if (CP != null && CP != this)
                Destroy(gameObject);
            else
                CP = this;
        }

        public void AddCollider(Vector3 entitypos)
        {
            GameObject obj = (GameObject)Instantiate(boxCollider);
            obj.transform.position = entitypos;
            obj.transform.parent = transform;
            obj.layer = 9;
            //obj.GetComponent<BoxCollider>().enabled = false;
            //obj.gameObject.SetActive(false);
        }
    }
}
