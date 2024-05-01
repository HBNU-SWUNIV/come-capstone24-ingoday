using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TMP_Text prisonTimerText;


    public void ShowPrisonTimer()
    {
        prisonTimerText.text = Mathf.FloorToInt(prisonTimer / 60.0f).ToString() + " : ";
        if (prisonTimer % 60.0f < 10)
            prisonTimerText.text += "0";
        prisonTimerText.text += Mathf.FloorToInt(prisonTimer % 60.0f).ToString();
    }

    public void CaughtByRope()
    {
        prisonTimerText.gameObject.SetActive(true);
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
            ShowPrisonTimer();

            if (prisonTimer <= 0.0f)
            {
                mapImage.gameObject.SetActive(true);
                prisonTimerText.gameObject.SetActive(false);
                inPrison = false;
            }
        }

    }
}
