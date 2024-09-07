using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public float moveSpeed = 10.0f; // �̵� �ӵ�
    public bool leftRightChange = false;    // �¿� ���� ����
    public RopeManager ropeManager; // ���� ���� ��
    Animator animator;  // ��� �ִϸ��̼�
    Rigidbody2D rigidbody2D;
    //public NavMeshAgent navMeshAgent;

    private Vector3 remotePosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        /*
        if (collision.gameObject.tag == "Water" && navMeshAgent != null)
        {
            navMeshAgent.speed *= 0.6f;
        }
        */

        if (collision.gameObject.tag == "Water")
        {
            moveSpeed = 6.0f;
            animator.SetBool("InWater", true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!this.gameObject.GetPhotonView().IsMine)
            return;

        /*
        if (collision.gameObject.tag == "Water" && navMeshAgent != null)
        {
            navMeshAgent.speed /= 0.6f;
        }
        */

        if (collision.gameObject.tag == "Water")
        {
            moveSpeed = 10.0f;
            animator.SetBool("InWater", false);
        }

    }



    void EquippedItemPos()   // ���� ������ ��ġ
    {
        if (this.transform.childCount > 2)  // 
        {
            for (int i = 2; i < this.transform.childCount; i++)
            {
                /*
                // ���߿��� �����ۺ��� �� �־ ��ġ �����ϱ�
                if (this.transform.GetChild(i).gameObject.GetComponent<ItemInfo>().GetItemIndexNumber() == 18)  // 18�� �������� ����, ���� �̰͸� �ȴ°� ���� ��ġ ���� ��
                {
                    if (animator.GetBool("Walk"))   // �ȱ� �����϶��� �ƴҶ� �������� ��ġ �ٸ��� ����
                    {
                        this.transform.GetChild(i).localPosition = new Vector3(-1.25f, -3.3f, 0.0f);
                        this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, -40.0f);
                    }
                    else
                    {
                        this.transform.GetChild(i).localPosition = new Vector3(1.25f, -3.3f, 0.0f);
                        this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                    }
                    break;
                }
                */


                switch (this.transform.GetChild(i).gameObject.GetComponent<ItemInfo>().GetItemIndexNumber())
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                    case 6:
                        break;
                    case 7:     // �̵��ӵ� ������Ű�� �̳� �޸� ����
                        if (animator.GetBool("Walk"))   // �ȱ� �����϶��� �ƴҶ� �������� ��ġ �ٸ��� ����
                        {
                            this.transform.GetChild(i).localPosition = new Vector3(0.8f, 5.0f, 0.0f);
                        }
                        else
                        {
                            this.transform.GetChild(i).localPosition = new Vector3(1.3f, 5.0f, 0.0f);
                        }
                        break;
                    case 8:
                        break;
                    case 9:
                        break;
                    case 10:
                        break;
                    case 11:
                        break;
                    case 12:
                        break;
                    case 13:
                        break;
                    case 14:
                        break;
                    case 15:
                        break;
                    case 16:
                        break;
                    case 17:
                        break;
                    case 18:    // ����
                        if (animator.GetBool("Walk"))   // �ȱ� �����϶��� �ƴҶ� �������� ��ġ �ٸ��� ����
                        {
                            this.transform.GetChild(i).localPosition = new Vector3(-1.25f, -3.3f, 0.0f);
                            this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, -40.0f);
                        }
                        else
                        {
                            this.transform.GetChild(i).localPosition = new Vector3(1.25f, -3.3f, 0.0f);
                            this.transform.GetChild(i).localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                        }
                        break;
                    case 19:
                        break;
                    case 20:
                        break;
                    case 21:
                        break;
                    case 22:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void Start()
    {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ropeManager = this.transform.GetChild(0).GetComponent<RopeManager>();
        rigidbody2D = this.GetComponent<Rigidbody2D>();

        //navMeshAgent.updatePosition = false;
        //navMeshAgent.updateRotation = false;


    }

    void Update()
    {

        if (this.GetComponent<PhotonView>().IsMine)
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime; // x�� �̵�
            float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;   // y�� �̵�

            if (moveX != 0.0f || moveY != 0.0f) // �ִϸ����� ����
            {
                animator.SetBool("Walk", true);
            }
            else
            {
                animator.SetBool("Walk", false);
            }
            EquippedItemPos();  // ��� ��ġ ����

            if (moveX < 0.0f && !leftRightChange)   // �¿� ���� ����
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

            //transform.Translate(new Vector3(moveX, moveY, 0.0f));   // �̵�

            rigidbody2D.velocity = new Vector3(moveX, moveY, 0.0f).normalized * moveSpeed;

            /*
            Vector3 movement = new Vector3(moveX, moveY, 0.0f);

            // Rigidbody2D�� ����Ͽ� ����Ű�� �̵��մϴ�.
            //rb.velocity = movement * moveSpeed;

            // NavMeshAgent���� �̵� �������� �����մϴ�.
            if (movement != Vector3.zero)
            {
                
                Vector3 moveDestination = transform.position + new Vector3(moveX, moveY, 0.0f) * 5.0f;
                navMeshAgent.SetDestination(moveDestination);
                
                
                rigidbody2D.velocity = new Vector3(moveX, moveY, 0.0f).normalized * moveSpeed;

            }
            else
            {
                navMeshAgent.SetDestination(transform.position);
            }
            */
        }

    }
}
