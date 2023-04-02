using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Moru;

public class Viewer<T> : MonoBehaviour
{
    public virtual void Init(T model)
    {

    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + new Vector3(0, 1, 0), 0.1f);
    }
}
