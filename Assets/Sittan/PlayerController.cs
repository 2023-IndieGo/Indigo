using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public class KeyCommand
    {
        public KeyCode KeyCode;
        public Command Command;
    }

    List<KeyCommand> keyCommands = new List<KeyCommand>();


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (var key in keyCommands)
        {
            if (Input.GetKeyDown(key.KeyCode))
            {
                key.Command.Do();

            }
        }
    }
}