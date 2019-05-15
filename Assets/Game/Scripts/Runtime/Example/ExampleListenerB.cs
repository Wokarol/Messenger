using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wokarol.MessageSystem;

public class ExampleListenerB : MonoBehaviour
{
    private void Start()
    {
        // That function add listener to event of type MessageB
        Messenger.Default.AddListener<MessageB>(OnMessageB);
    }

    private void OnMessageB(MessageB e)
    {
        Debug.Log($"I have to buy new {e.Value}");
    }
}
