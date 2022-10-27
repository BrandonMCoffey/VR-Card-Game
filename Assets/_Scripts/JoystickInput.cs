using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : MonoBehaviour
{
    private void Update()
    {
        // Left Joystick
        Debug.Log(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        
        // Right Joystick
        Debug.Log(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));
    }
}
