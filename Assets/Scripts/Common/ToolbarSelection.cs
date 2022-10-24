using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using Unity.Transforms;

public class ToolbarSelection : MonoBehaviour {

    int blockNum = 0;
    //public static Transform[] BlockUI = new Transform[7];
    public static Transform[] BlockUI;

    public static Transform BlockUIChild;
    
    void Start()
    {
        BlockUI = new Transform[10];
        for(int i=0;i<7;i++)
        {
            BlockUI[i] = this.transform.GetChild(i);
        }

    }

    void Update () {

        blockNum = Unity.Physics.Extensions.PickaxeController.m_blockID - 1;
        //print("blockNum=" +blockNum);
        
        for(int i=0; i < 7; i++)
        {
            BlockUIChild = BlockUI[i].transform.GetChild(0);

            if (i == blockNum)
            {
                
                BlockUIChild.GetComponent<RawImage>().enabled = true;
            }
            else
            {
                BlockUIChild.GetComponent<RawImage>().enabled = false;
            }
        }
    }
}
