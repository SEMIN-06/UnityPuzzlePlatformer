using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockTypes
{
    general, temp, jump, fake, straight
}

public class Block : MonoBehaviour
{
    public BlockTypes blockType;
}
