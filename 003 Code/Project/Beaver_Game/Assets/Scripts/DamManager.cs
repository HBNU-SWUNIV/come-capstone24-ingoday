using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class DamManager : MonoBehaviour
{
    public int[] requiredResources = new int[4]; // 0: ³ª¹«, 1: ÁøÈë, 2: µ¹, 3: °­Ã¶
    public ResourceManager resourceManager;
    public InvnetorySlotGroup invnetorySlotGroup;


    public void DamCreate()
    {
        bool damCreateBool = true;
        invnetorySlotGroup.PlayerResourceCount();

        for (int i = 0; i < 4; i++)
        {
            if (requiredResources[i] > invnetorySlotGroup.playerResourceCountInts[i] + resourceManager.resourceCountInts[i])
            {
                damCreateBool = false;
                break;
            }
        }

        if (damCreateBool)
        {
            int[] remainNum = new int[4] {0, 0, 0, 0};

            for (int i = 0; i < 4; i++)
            {
                if (requiredResources[i] > invnetorySlotGroup.playerResourceCountInts[i])
                {
                    remainNum[i] = requiredResources[i] - invnetorySlotGroup.playerResourceCountInts[i];
                }

                invnetorySlotGroup.UseItem(i, -requiredResources[i] + remainNum[i]);
                requiredResources[i] = 0;
            }
            resourceManager.StorageResourceCountChange(-remainNum[0], -remainNum[1], -remainNum[2], -remainNum[3]);

            
            Color damColor =  gameObject.GetComponent<SpriteRenderer>().color;
            damColor.a = 150;
            gameObject.GetComponent<SpriteRenderer>().color = damColor;
            
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
