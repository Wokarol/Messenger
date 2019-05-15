using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wokarol.MessageSystem;

public class ExampleSender : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            // This function calls message of given type (MessageA in this case)
            Messenger.Default.SendMessage(new MessageA("Bananas"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Messenger.Default.SendMessage(new MessageA("Apples"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            Messenger.Default.SendMessage(new MessageA("Lemos"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            Messenger.Default.SendMessage(new MessageB("Car"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            Messenger.Default.SendMessage(new MessageB("Plane"));
        }

        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            Messenger.Default.SendMessage(new MessageB("TV"));
        }
    }
}
