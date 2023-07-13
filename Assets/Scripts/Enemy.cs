using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyTypes
{
    walk, rush
}

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyTypes enemyType;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<Transform> walkEnemyWayPoints = new();
    [SerializeField] private float rushEnemyFindDistance = 3f;
    private int nowWayPointIdx = 0;
    private bool rushing = false;

    // Start is called before the first frame update
    void Start()
    {
        G.gameManager.spawnedEnemies.Add(gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemyTypes.walk:
                Vector3 moveDir = (walkEnemyWayPoints[nowWayPointIdx].position - transform.position).normalized;
                moveDir.y = 0;
                transform.Translate(moveSpeed * Time.deltaTime * moveDir);
                Vector3 pos = transform.position;
                pos.y = 0;
                Vector3 mpos = walkEnemyWayPoints[nowWayPointIdx].position;
                mpos.y = 0;
                if (Vector3.Distance(pos, mpos) <= 0.1f)
                {
                    nowWayPointIdx++;
                    if (nowWayPointIdx >= walkEnemyWayPoints.Count)
                        nowWayPointIdx = 0;
                }
                break;
            case EnemyTypes.rush:
                if (Vector3.Distance(transform.position, G.gameManager.player.position) < rushEnemyFindDistance)
                {
                    if (!rushing) {
                        rushing = true;
                        StartCoroutine(Rush());
                    }
                }
                break;
            default:
                break;
        }
    }

    IEnumerator Rush()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 targetPos = G.gameManager.player.position;
        targetPos.y = transform.position.y;
        while (true)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 3f * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) <= 0.1f)
            {
                break;
            }
            yield return null;
        }
        rushing = false;
    }
}
