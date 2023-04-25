using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBehaviour : MonoBehaviour
{
    public void OnEnded()
    {
        Debug.Log("animPlayEnd");
        AnimationHandler.count++;
    }
}
