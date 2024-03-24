using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetResourceManager : MonoBehaviour
{
    public ItemIndex itemIndex;
    private int getResourceNum = 0;
    private Transform resourceItemPos;

    public void GetResourceActive(int resourceNum, Transform resourceDropPos)
    {
        getResourceNum = resourceNum;
        resourceItemPos = resourceDropPos;
        switch (resourceNum)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }

    }

    public void OnClickButtonInGetResource()
    {
        GameObject newResource = Instantiate(itemIndex.items[getResourceNum].gameObject);
        newResource.transform.position = resourceItemPos.position;

        this.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
