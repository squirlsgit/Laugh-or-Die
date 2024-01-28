using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressSpaceRestart : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            SceneM.instance.Restart();
        }
    }
}
