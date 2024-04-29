using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public bool leftRightChange = false;
    public RopeManager ropeManager;

    void Start()
    {
        
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        

        if (moveX < 0.0f && !leftRightChange)
        {
            leftRightChange = true;
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
            ropeManager.ThrowRopeLineLeftRightChange();
        }
        else if (moveX > 0.0f && leftRightChange)
        {
            leftRightChange = false;
            this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);
            ropeManager.ThrowRopeLineLeftRightChange();
        }

        transform.Translate(new Vector3(moveX, moveY, 0.0f));
    }
}
