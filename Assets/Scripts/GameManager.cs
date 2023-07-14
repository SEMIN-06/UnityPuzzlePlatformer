using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct StageInfo {
    public string name;
    public GameObject map;
    public Transform startBlock;
    public GameObject portalBlock;
    public float minY;
    public Quaternion cameraRot;
}

public class GameManager : MonoBehaviour
{
    [HideInInspector] public Transform player;
    [HideInInspector] public Rigidbody playerRigid;
    [HideInInspector] public Transform cameraParent;
    [HideInInspector] public float playerJumpTime = 0;
    [HideInInspector] public List<GameObject> spawnedEnemies = new();
    public List<StageInfo> stageInfos = new();
    public int nowStage = 0;

    private void Awake()
    {
        G.gameManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        OnChangeStage(-1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            OnChangeStage(nowStage, nowStage + 1);
        }
    }

    public void Die()
    {
        PlaySound("Die");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void DeleteEnemy(GameObject enemy)
    {
        spawnedEnemies.Remove(enemy);
        Destroy(enemy);

        if (spawnedEnemies.Count <= 0)
        {
            if (nowStage == (stageInfos.Count - 1)) // GAME CLEAR
            {
                Debug.Log("Game Clear");
            } else
            {
                stageInfos[nowStage].portalBlock.SetActive(true);
            }
        }
    }

    public void PlaySound(string sound)
    {
        Instantiate((GameObject)Resources.Load(string.Format("Sounds/{0}", sound)));
    }

    public void OnChangeStage(int prevStage, int toStage)
    {
        if (prevStage > -1) stageInfos[prevStage].map.SetActive(false);
        spawnedEnemies = new();
        stageInfos[toStage].map.SetActive(true);
        player.position = stageInfos[toStage].startBlock.position + new Vector3(0, 3f, 0);
        cameraParent.position = stageInfos[toStage].startBlock.position + new Vector3(0, 3f, 0);
        cameraParent.rotation = stageInfos[toStage].cameraRot;
        player.rotation = stageInfos[toStage].cameraRot;
        nowStage = toStage;
    }
}
