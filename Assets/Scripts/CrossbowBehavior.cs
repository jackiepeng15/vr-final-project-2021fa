using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossbowBehavior : MonoBehaviour
{
    OVRGrabbable m_GrabState;
    private Rigidbody rb; // crossbow rigidbody
    private Vector3 offsetPosition;
    private Quaternion offsetRotation;
    private bool alreadyGrabbed;
    private AudioSource grabAudio;
    
    [SerializeField]
    private GameObject player;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_GrabState = gameObject.GetComponent<OVRGrabbable>();
        rb.isKinematic = false;
        offsetPosition = transform.localPosition;
        offsetRotation = transform.localRotation;
        alreadyGrabbed = false;
        grabAudio = transform.GetChild(7).gameObject.GetComponent<AudioSource>();
    }
    
    void ReParent()
    {
        transform.parent = player.transform;
        transform.localPosition = offsetPosition;
        transform.localRotation = offsetRotation;
        rb.isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GrabState.allowOffhandGrab &&
            (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9f ||
             OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9f))
            // crossbow in range and at least one grab is pressed
        {
            transform.parent = null; // detach from parent so distance grab can work
            rb.isKinematic = true;
            if (!m_GrabState.isGrabbed) // did not successfully grab crossbow
            {
                ReParent();
            }

            if (!alreadyGrabbed && m_GrabState.isGrabbed)
            {
                grabAudio.Play(); // play grab crossbow sound effect
                alreadyGrabbed = true;
            }
        }
        
        if (!m_GrabState.isGrabbed) // user let go of grip
        {
            ReParent();
            alreadyGrabbed = false;
        }
    }
}

