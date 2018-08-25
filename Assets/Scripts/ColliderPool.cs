using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minecraft
{
    public class ColliderPool : MonoBehaviour
    {

        //private Vector3 playerPrevPosition;
        public static ColliderPool CP;
        public GameObject boxCollider;
        //public int pooledAmount = 100;
        //List<GameObject> boxColliders;

        // Use this for initialization
        //void Start()
        //{

        //boxColliders = new List<GameObject>();
        //for (int i = 0; i < pooledAmount; i++)
        //{
        // GameObject obj = (GameObject)Instantiate(boxCollider);
        // obj.SetActive(false);
        //boxColliders.Add(obj);
        //}
        //}
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
            //obj.SetActive(false);

            /*
            for (int i = 0; i < boxColliders.Count;i++)
            {
                if(!boxColliders[i].activeInHierarchy)
                {
                    boxColliders[i].transform.position = new Vector3 (xPos, yPos, zPos);
                    boxColliders[i].SetActive(true);
                    break;
                }

            }*/
        }
    }
}
