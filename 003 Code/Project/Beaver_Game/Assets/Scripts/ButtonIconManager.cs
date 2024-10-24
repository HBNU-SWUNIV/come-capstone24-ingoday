using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonIconManager : MonoBehaviour
{
    public Sprite[] citizenButtonSprites;
    public Sprite[] spyButtonSprites;
    public Sprite[] useButtonSprites;
    // ��ư ���� 0: �⺻ �׼�, 1: ä��, 2: ����, 3: â��, 4: ����, 5: ����, 6: �� �Ǽ�, 7: �� �Ǽ� ����/����, 8: �� �ϰ�, 9:õ��ž ö��, 10: ����ž �Ǽ�(�����̸�), 11: ����ž ���(�����̸�)

    
    public Button actionButton;
    public Button demolishTowerButton;
    public Button buildTowerComunicationButton;
    public Button throwRopeButton;
    public Button escapePrisonButton;
    

    public void SetButtonIcons(bool isSpy)
    {
        
        if (isSpy)
        {
            actionButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[0];
            demolishTowerButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[9];
            buildTowerComunicationButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[10];
            throwRopeButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[4];
            escapePrisonButton.gameObject.GetComponent<Image>().sprite = spyButtonSprites[5];

            useButtonSprites = spyButtonSprites;
        }
        else
        {
            actionButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[0];
            demolishTowerButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[9];
            throwRopeButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[4];
            escapePrisonButton.gameObject.GetComponent<Image>().sprite = citizenButtonSprites[5];

            useButtonSprites = citizenButtonSprites;
        }
        
    }

    public void ChangeActionButtonIcon(int btnNum)
    {
        actionButton.gameObject.GetComponent<Image>().sprite = useButtonSprites[btnNum];
    }

    public void ChangeBuildTowerComunicationButton(bool doComunication)
    {
        if (doComunication)
        {
            buildTowerComunicationButton.gameObject.GetComponent<Image>().sprite = useButtonSprites[11];
        }
        else
        {
            buildTowerComunicationButton.gameObject.GetComponent<Image>().sprite = useButtonSprites[10];
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
