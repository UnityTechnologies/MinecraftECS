using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minecraft
{
    public class ColliderPool : MonoBehaviour
    {

        //private Vector3 playerPrevPosition;

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
        public void AddCollider(int xPos, int yPos, int zPos)
        {
            GameObject obj = (GameObject)Instantiate(boxCollider);
            obj.transform.position = new Vector3(xPos, yPos, zPos);
            obj.transform.parent = transform;

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
