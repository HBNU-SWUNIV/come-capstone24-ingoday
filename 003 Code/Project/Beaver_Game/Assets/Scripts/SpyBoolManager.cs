using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpyBoolManager : MonoBehaviour
{
    [SerializeField]
    private bool is_Spy = false;

    public Button buildTowerButton;
    public Button RadioComunicationButton;
    SpyBeaverAction spyAction;


    public void OnClickSpyChangeButton()    // test
    {
        is_Spy = !is_Spy;
        SpyManager(is_Spy);
    }

    public void SpyManager(bool isSpy)
    {
        if (isSpy)
        {
            spyAction.enabled = true;
            buildTowerButton.gameObject.SetActive(true);
            RadioComunicationButton.gameObject.SetActive(true);
        }
        else
        {
            spyAction.enabled = false;
            buildTowerButton.gameObject.SetActive(false);
            RadioComunicationButton.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        spyAction = GetComponent<SpyBeaverAction>();
    }

    void Update()
    {
        
    }
}
