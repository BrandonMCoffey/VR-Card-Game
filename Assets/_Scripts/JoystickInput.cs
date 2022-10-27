using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickInput : MonoBehaviour
{
    private void Update()
    {
        // Left Joystick
        var leftStick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        
        // Right Joystick
        var rightStick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
    }
}
