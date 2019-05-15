using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wokarol.MessageSystem;

public class ExampleListenerA : MonoBehaviour
{
    private void Start()
    {
        // That function add listener to event of type MessageA
        Messenger.Default.AddListener<MessageA>(OnMessageA);
    }

    private void OnMessageA(MessageA e)
    {
        Debug.Log($"I want to eat some {e.Value}");
    }
}
