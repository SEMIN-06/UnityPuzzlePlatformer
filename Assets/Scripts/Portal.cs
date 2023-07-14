using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool triggered = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player") && !triggered)
        {
            G.gameManager.OnChangeStage(G.gameManager.nowStage, G.gameManager.nowStage + 1);
            triggered = true;
        }
    }
}
