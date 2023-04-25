using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AnimationHandler : MonoBehaviour
{
    public static int count;
    [ShowInInspector]
    public Queue<List<AnimClip>> animationClip;
    private List<AnimClip> lastData;

    public void Init()
    {
        lastData = new List<AnimClip>();
        count = 0;
        animationClip = new Queue<List<AnimClip>>();

    }

    public void Play()
    {
        StartCoroutine(PlayQueue());
    }

    public void AddClipOnLastQueue(AnimClip clip)
    {
        if(animationClip.Count == 0)
        {
            var list = new List<AnimClip>();
            list.Add(clip);
            animationClip.Enqueue(list);
            lastData = list;
        }
        else
        {
            lastData.Add(clip);
        }
    }

    public void AddList(AnimClip clip = null)
    {
        var list = new List<AnimClip>();
        if(clip != null)
        {
            list.Add(clip);
        }
        lastData = list;
        animationClip.Enqueue(list);
    }


    /// <summary>
    /// ��ŸƮ �ڷ�ƾ���� �ִϸ��̼��� ȣ���մϴ�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator PlayQueue()
    {
        while (animationClip.Count != 0)
        {
            var list = animationClip.Dequeue();
            foreach (var clip in list)
            {
                clip.Play();
            }
            while (count < list.Count)
            {
                yield return null;
            }
            count = 0;
        }
    }

}
