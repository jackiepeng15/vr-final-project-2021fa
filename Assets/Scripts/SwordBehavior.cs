using System;
using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using UnityEngine;

public class SwordBehavior : MonoBehaviour
{
    //Accessing the script that takes care of sword's grabbing state
    OVRGrabbable m_GrabState;
    private AudioSource swingAudio;
    private AudioSource drawAudio;
    private AudioSource hitAudio;
    private Rigidbody swordRb;
    // private Rigidbody playerRb;
    private float minSpeed=15.0f; // minimum sword speed needed to trigger sound

    private Vector3 offsetPosition;
    private Quaternion offsetRotation;
    
    [SerializeField]
    private GameObject player;

    private bool alreadyGrabbed;
    
    private float frames = 0;
    private bool hapticRunning = false;
    private bool grabbedByRight;
    [SerializeField] private GameObject rightHand;
    private OVRGrabber rightGrabber;
    public float pulseLength = 3f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        swordRb = GetComponent<Rigidbody>();
        // playerRb = player.GetComponent<Rigidbody>();
        swingAudio = transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        drawAudio = transform.GetChild(1).gameObject.GetComponent<AudioSource>();
        hitAudio = transform.GetChild(2).gameObject.GetComponent<AudioSource>();
        m_GrabState = gameObject.GetComponent<OVRGrabbable>();
        swordRb.isKinematic = false;
        Transform swordTransform = transform;
        offsetPosition = swordTransform.localPosition;
        offsetRotation = swordTransform.localRotation;
        alreadyGrabbed = false;
        rightGrabber = rightHand.GetComponent<DistanceGrabber>();
    }
    
    void ReParent()
    {
        transform.parent=player.transform;
        transform.localPosition = offsetPosition;
        transform.localRotation = offsetRotation;
        swordRb.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GrabState.isGrabbed)
        {
            if (m_GrabState.grabbedBy == rightGrabber)
            {
                grabbedByRight = true;
            }
            else
            {
                grabbedByRight = false;
            }
        }
        
        Vector3 vel = swordRb.velocity;
        // Vector3 playerVel = playerRb.velocity;
        
        // sword audio
        if (vel.magnitude > minSpeed && m_GrabState.isGrabbed && !swingAudio.isPlaying)
        {
            swingAudio.Play();
        }
        
        if (m_GrabState.allowOffhandGrab &&
            (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9f ||
             OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9f))
            // sword in range and at least one grab is pressed
        {
            transform.parent = null; // detach from parent so distance grab can work
            swordRb.isKinematic = true;
            if (!m_GrabState.isGrabbed) // did not successfully grab sword
            {
                ReParent();
            }

            if (!alreadyGrabbed && m_GrabState.isGrabbed)
            {
                drawAudio.Play(); // play grab sword sound effect
                alreadyGrabbed = true;
            }
        }
        
        if (!m_GrabState.isGrabbed) // user let go of grip
        {
            ReParent();
            alreadyGrabbed = false;
        }
        
        if (hapticRunning) {
            if (grabbedByRight)
            {
                HapticPulse(pulseLength, OVRInput.Controller.RTouch);
            }
            else
            {
                HapticPulse(pulseLength, OVRInput.Controller.LTouch);
            }
            frames += 1;
        } else if (frames != 0) {
            if (grabbedByRight)
            {
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
            }
            else
            {
                OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
            }

            frames = 0;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            hitAudio.Play();
            hapticRunning = true;
        }
    }
    
    void HapticPulse(float length, OVRInput.Controller cont)
    {
        if (frames == 0)
        {
            OVRInput.SetControllerVibration(0.2f, 0.2f, cont);
            
        }
        if (frames >= length)
        {
            OVRInput.SetControllerVibration(0, 0, cont);
            hapticRunning = false;
        }
    }
}
