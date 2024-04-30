using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PrisonManager : MonoBehaviour
{
    private float prisonTimer = 20.0f;
    private bool inPrison = false;
    private int caughtCount = 0;
    public float inPrisonTime = 30.0f;
    public RectTransform mapImage;
    public bool escapePosSelect = false;

    public void CaughtByRope()
    {
        caughtCount++;
        prisonTimer = inPrisonTime + (caughtCount - 1) * 10.0f;
        inPrison = true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (inPrison)
        {
            prisonTimer -= Time.deltaTime;

            if (prisonTimer <= 0.0f)
            {
                mapImage.gameObject.SetActive(true);
                inPrison = false;
            }
        }

    }
}
