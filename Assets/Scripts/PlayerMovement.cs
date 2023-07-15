using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float dashPower = 3f;
    [SerializeField] private float doubleJumpPower = 10f;
    [SerializeField] private float straightSpeed = 5f;
    [SerializeField] private ItemTypes enabledItem = ItemTypes.none;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance;
    private float[] clickTimes = new float[4];
    private float hAxis = 0;
    private float itemTime;
    private bool straight = false;
    private Transform straightBlockTrans;
    private Coroutine cameraRotateCoroutine;
    private Rigidbody playerRigid;
    private MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        playerRigid = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        bool isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        bool[] keyDowns = new bool[4] { Input.GetKeyDown(KeyCode.D), Input.GetKeyDown(KeyCode.A), Input.GetKeyDown(KeyCode.LeftArrow), Input.GetKeyDown(KeyCode.RightArrow) };

        for (int i = 0; i < 4; i++)
        {
            if (keyDowns[i])
            {
                if (!isGround)
                {
                    if (clickTimes[i] > Time.time && enabledItem != ItemTypes.none)
                    {
                        meshRenderer.material = (Material)Resources.Load("Material/Material_Player_General");

                        switch (enabledItem)
                        {
                            case ItemTypes.dash:
                                Vector3 dir = Vector3.zero; 
                                if (keyDowns[0] || keyDowns[3])
                                {
                                    dir = transform.right;
                                }
                                else if (keyDowns[1] || keyDowns[2])
                                {
                                    dir = -transform.right;
                                }
                                //playerRigid.velocity = Vector3.zero;
                                //playerRigid.AddForce((dir * dashPower) + (Vector3.up * 4), ForceMode.Impulse);
                                G.gameManager.AddForcePlayer((dir * dashPower) + (Vector3.up * 4));
                                G.gameManager.PlaySound("jump");
                                break;
                            case ItemTypes.doubleJump:
                                //playerRigid.velocity = Vector3.zero;
                                //playerRigid.AddForce(Vector3.up * doubleJumpPower, ForceMode.Impulse);
                                G.gameManager.AddForcePlayer(Vector3.up * doubleJumpPower);
                                G.gameManager.PlaySound("jump");
                                break;
                            default:
                                break;
                        }
                        enabledItem = ItemTypes.none;
                    }
                }
                clickTimes[i] = Time.time + 0.3f;

                if (straight)
                {
                    straight = false;
                    playerRigid.isKinematic = false;
                    playerRigid.velocity = Vector3.zero;
                }
            }
        }

        if (transform.position.y < G.gameManager.stageInfos[G.gameManager.nowStage].minY)
        {
            G.gameManager.Die();
        }

        hAxis = Input.GetAxisRaw("Horizontal");

      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (straight)
        {
            transform.Translate(straightSpeed * Time.deltaTime * straightBlockTrans.forward, Space.World);
            bool blockMove = Physics.Raycast(transform.position, straightBlockTrans.forward, 0.5f, LayerMask.GetMask("Block"));
            if (blockMove)
            {
                straight = false;
                playerRigid.isKinematic = false;
                playerRigid.velocity = Vector3.zero;
            }
        }
        else
        {
            bool blockMoveR = Physics.Raycast(transform.position, transform.right, 0.3f, LayerMask.GetMask("Block"));
            bool blockMoveL = Physics.Raycast(transform.position, -transform.right, 0.3f, LayerMask.GetMask("Block"));
            if (blockMoveR) hAxis = Mathf.Clamp(hAxis, -1, 0);
            if (blockMoveL) hAxis = Mathf.Clamp(hAxis, 0, 1);
            Vector3 newPos = new Vector3(hAxis, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(newPos);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            if (itemTime < Time.time)
            {
                Item item = other.GetComponent<Item>();
                switch (item.itemType)
                {
                    case ItemTypes.rotate:
                        G.gameManager.player.position = new Vector3(other.transform.position.x, G.gameManager.player.position.y, other.transform.position.z);
                        G.gameManager.player.localEulerAngles = item.rotate;
                        if (cameraRotateCoroutine != null) StopCoroutine(cameraRotateCoroutine);
                        cameraRotateCoroutine = StartCoroutine(StartCameraRotate(item.rotate));
                        break;
                    default:
                        enabledItem = item.itemType;
                        break;
                }

                Material itemMaterial = (Material)Resources.Load(string.Format("Material/Material_Player_{0}", item.itemType.ToString()));
                if (itemMaterial == null) itemMaterial = (Material)Resources.Load("Material/Material_Player_General");
                meshRenderer.material = itemMaterial;

                Destroy(other.gameObject);
                itemTime = Time.time + 0.5f;
            }
        }
        else if (other.CompareTag("EnemyHitPoint"))
        {
            if (playerRigid.velocity.y < 0) // 플레이어가 위에서 아래로 내려올때만 가능 (위에서 아래로가면 -)
            {
                G.gameManager.AddForcePlayer(Vector3.up * 10);
                G.gameManager.DeleteEnemy(other.transform.parent.gameObject);
                G.gameManager.PlaySound("pang");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            G.gameManager.Die();
        }
    }

    IEnumerator StartCameraRotate(Vector3 rotate)
    {
        float maxTime = 0.5f;
        float nowTime = 0;
        Quaternion rot = G.gameManager.cameraParent.rotation;
        while (nowTime < maxTime)
        {
            nowTime += Time.deltaTime;
            G.gameManager.cameraParent.rotation = Quaternion.Lerp(rot, Quaternion.Euler(rotate), nowTime / maxTime);
            yield return null;
        }
    }

    public void StartStraight(Transform blockTrans)
    {
        straight = true;
        transform.position = blockTrans.position + blockTrans.forward;
        straightBlockTrans = blockTrans;
        playerRigid.isKinematic = true;
        playerRigid.velocity = Vector3.zero;
    }
}
