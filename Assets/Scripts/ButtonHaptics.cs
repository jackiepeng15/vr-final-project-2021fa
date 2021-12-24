using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHaptics : MonoBehaviour
{
    private bool hapticRunning = false;
    private float frames = 0;
    public float pulseLength = 15f;

    void Start()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
   
    // Update is called once per frame
    void Update()
    {
        if (hapticRunning)
        {
            HapticPulse();
            frames += 1;
        }
    }
    
    private void HapticPulse()
    {
        
        if (frames == 0)
        {
            OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
            
        }
        if (frames >= pulseLength)
        {
            OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            frames = -1;
            hapticRunning = false;
        }
        
    }

    public void StartPulse()
    {
        hapticRunning = true;
    }

}
