using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockTypes
{
    general, temp, jump, fake, straight, descent, move
}

public class Block : MonoBehaviour
{
    public BlockTypes blockType;
    public bool isMoveBlockMoving;

    public IEnumerator StartMoveBlock()
    {
        isMoveBlockMoving = true;
        yield return new WaitForSeconds(0.2f);
        
        if (!Physics.Raycast(transform.position + transform.up, transform.up, 1f, LayerMask.GetMask("Block")))
        {
            transform.position += transform.up;
        }
        isMoveBlockMoving = false;
    }
}
