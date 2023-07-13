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
                G.gameManager.playerJumpTime = Time.time + 0.01f;
                float jumpPower = 10f;

                switch (block.blockType)
                {
                    case BlockTypes.general:
                        Debug.Log("aa");
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
                    default:
                        break;
                }

                G.gameManager.playerRigid.velocity = Vector3.zero;
                G.gameManager.playerRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            } else
            {
                Debug.Log("bb");
            }
        }
    }
}
