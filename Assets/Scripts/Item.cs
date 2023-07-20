using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemTypes
{
    none, dash, doubleJump, rotate, fly
}

public class Item : MonoBehaviour
{
    public ItemTypes itemType;
    public Vector3 rotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
