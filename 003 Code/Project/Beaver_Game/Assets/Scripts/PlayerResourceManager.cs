using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerResourceManager : MonoBehaviour
{
    public ResourceManager resourceManager;
    public TMP_Text[] playerResourceCountTexts;
    public int[] playerResourceCountInts = new int[4] {0, 0, 0, 0};
    public InvnetorySlotGroup InvnetorySlotGroup;


    public void PlayerResourceCountChange(int resourceNum, int resourceVariation)
    {
        if (playerResourceCountInts[resourceNum] + resourceVariation >= 0)
        {
            playerResourceCountInts[resourceNum] += resourceVariation;
            playerResourceCountTexts[resourceNum].text = playerResourceCountInts[resourceNum].ToString();
        }
    }

    public void StoreResource()
    {
        resourceManager.StorageResourceCountChange(playerResourceCountInts[0], playerResourceCountInts[1], playerResourceCountInts[2], playerResourceCountInts[3]);
        
        for (int i = 0; i < 3; i++)
            PlayerResourceCountChange(i, -playerResourceCountInts[i]);


    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
