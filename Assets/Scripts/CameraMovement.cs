using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        G.gameManager.cameraParent = transform;
    }

    private void FixedUpdate()
    {
        
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, G.gameManager.player.position, 3f * Time.deltaTime);

    }
}
