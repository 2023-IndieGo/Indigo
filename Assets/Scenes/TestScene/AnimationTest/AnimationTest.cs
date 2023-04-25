using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestCode.Moru
{

    public class AnimationTest : MonoBehaviour
    {
        public GameObject queueObj;
        public AnimationHandler animHandler;
        public AnimClip clip1;
        public AnimClip clip2;
        // Start is called before the first frame update
        void Start()
        {
            animHandler.Init();
            Enqueue();

        }

        public void Enqueue()
        {
            animHandler.AddClipOnLastQueue(clip1);
            animHandler.AddList(clip2);
        }

        public void Play()
        {
            animHandler.Play();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}