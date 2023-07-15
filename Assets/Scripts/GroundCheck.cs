using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Block"))
        {
            Block block = collider.GetComponent<Block>();
            if (G.gameManager.playerJumpTime <= Time.time)
            {
                G.gameManager.playerJumpTime = Time.time + 0.1f;
                float jumpPower = 10f;

                switch (block.blockType)
                {
                    case BlockTypes.general:
                        G.gameManager.PlaySound("normalCollision");
                        break;
                    case BlockTypes.temp:
                        G.gameManager.PlaySound("normalCollision");
                        Destroy(collider.gameObject);
                        break;
                    case BlockTypes.jump:
                        G.gameManager.PlaySound("jump");
                        jumpPower = 15f;
                        break;
                    case BlockTypes.fake:
                        G.gameManager.PlaySound("normalCollision");
                        G.gameManager.Die();
                        break;
                    case BlockTypes.straight:
                        G.gameManager.PlaySound("jump");
                        GetComponentInParent<PlayerMovement>().StartStraight(collider.transform);
                        break;
                    case BlockTypes.descent:
                        G.gameManager.PlaySound("jump");
                        collider.GetComponent<Rigidbody>().isKinematic = false;
                        collider.GetComponentInChildren<MeshRenderer>().material = (Material)Resources.Load("Material/Material_BlockImgs");
                        break;
                    case BlockTypes.move:
                        G.gameManager.PlaySound("normalCollision");
                        if (!block.isMoveBlockMoving)
                        {
                            StartCoroutine(block.StartMoveBlock());
                        }
                        break;
                    default:
                        break;
                }

                //G.gameManager.playerRigid.velocity = Vector3.zero;
                //G.gameManager.playerRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                G.gameManager.AddForcePlayer(Vector3.up * jumpPower);
            }
        }
    }
}
