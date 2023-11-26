using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopEye : MonoBehaviour
{
    public Transform eye;

    void Pop()
    {
        Vector3 pos = eye.position;
        pos.y = -1000;
        eye.position = pos;
    }
}
