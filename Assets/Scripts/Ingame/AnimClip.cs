using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimClip
{
    public AnimationType animType;
    [SerializeField, LabelText("종료시점 커스터마이징")]
    private bool isEndOverTimeCustomic;
    [SerializeField, ShowIf("isEndOverTimeCustomic")]
    private float animEndedTime;

    public AnimationClip animation;
    public ParticleSystem particle;
    public GameObject animTarget;
    private Del_NoRet_NoParams OnPlaying_Callback;

    public virtual void Init()
    {
        switch (animType)
        {
            case AnimationType.Animation:
                {
                    if (animation != null)
                    {
                        float animTime = animation.length;
                        var custom = new AnimationEvent();
                        custom.time = isEndOverTimeCustomic ? animEndedTime : animTime;
                        custom.functionName = "OnEnded";
                        animation.legacy = true;
                        animation.AddEvent(custom);
                    }
                    break;
                }
            case AnimationType.Particle:
                {
                    if (particle != null)
                    {

                    }
                    break;
                }
            case AnimationType.Both:
                break;
            case AnimationType.None:
                break;
        }
    }

    public void Play()
    {
        switch (animType)
        {
            case AnimationType.Animation:
                {
            if (animation != null && animTarget != null)
            {
                if (animTarget.TryGetComponent<Animation>(out var comp))
                {
                    comp.clip = animation;
                    comp.Play();
                }
            }
            break;
        }
            case AnimationType.Particle:
                break;
            case AnimationType.Both:
                break;
            case AnimationType.None:
                break;
    }
}


private void OnEnded()
{
    AnimationHandler.count++;
}
}
