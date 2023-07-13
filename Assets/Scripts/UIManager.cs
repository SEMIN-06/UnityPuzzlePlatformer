using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyText;

    private void Awake()
    {
        G.uiManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyText.SetText(string.Format("ENEMY: {0}", G.gameManager.spawnedEnemies.Count));
    }
}
