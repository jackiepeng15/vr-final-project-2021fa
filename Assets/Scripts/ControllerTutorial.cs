using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerTutorial : MonoBehaviour
{
    [SerializeField] private Animator controllerAnimator;
    bool leftPressed;
    float animDuration = 1; 
    float animTime;
    // Start is called before the first frame update
    void Start()
    {
        if (controllerAnimator == null) {
            controllerAnimator = GetComponentInChildren<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9 && !leftPressed) {
        //     // Play animation
        //     controllerAnimator.Play("Teleport");

        //     // Crossbow state
        //     leftPressed = true;
        // }

        // if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) <= 0.9) {
        //     leftPressed = false;
        // }

        // if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9 && !leftPressed) {
        //     // Play animation
            controllerAnimator.Play("Shoot");

        //     // Crossbow state
        //     leftPressed = true;
        // }

        // if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) <= 0.9) {
        //     leftPressed = false;
        // }


    }
}