using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using UnityEngine;

public class GrenadeBehavior : MonoBehaviour
{
    OVRGrabbable m_GrabState;
    private Rigidbody rb; // crossbow rigidbody
    private Vector3 offsetPosition;
    private Quaternion offsetRotation;
    private bool alreadyGrabbed;
    private AudioSource audioSource;
    [SerializeField] private AudioClip boomSound;
    [SerializeField] private AudioClip grabSound;
    
    [SerializeField]
    private GameObject player;
    [SerializeField] private GameObject explosion;
    private float frames = 0;
    private float duration = 100;
    private float radius = 3f;
    private float force = 700f;
    private bool isExplode = false;

    // Vibration variables
    private float pulseLength = 15f;
    private float vibrationFrames = 0;
    private bool hapticRunning = false;
    
    private bool grabbedByRight;
    private bool grabbedByLeft;
    [SerializeField] private GameObject rightHand;
    private OVRGrabber rightGrabber;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_GrabState = gameObject.GetComponent<OVRGrabbable>();
        rb.isKinematic = false;
        offsetPosition = transform.localPosition;
        offsetRotation = transform.localRotation;
        alreadyGrabbed = false;
        audioSource = GetComponent<AudioSource>();
        rightGrabber = rightHand.GetComponent<DistanceGrabber>();
    }
    
    // Dock the weapon
    void ReParent()
    {
        rb.isKinematic = false; 
        alreadyGrabbed = false;
        transform.parent = player.transform;
        transform.localPosition = offsetPosition;
        transform.localRotation = offsetRotation;
        frames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_GrabState.isGrabbed)
        {
            if (m_GrabState.grabbedBy == rightGrabber)
            {
                grabbedByRight = true;
                grabbedByLeft = false;
            }
            else
            {
                grabbedByRight = false;
                grabbedByLeft = true;
            }
        }
        else
        {
            grabbedByRight = false;
            grabbedByLeft = false;
        }

        if (m_GrabState.allowOffhandGrab && 
            (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9f && grabbedByLeft || 
             OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9f && grabbedByRight))
        {
            if (alreadyGrabbed) return;

            transform.parent = null;
            rb.isKinematic = true;

            if (!m_GrabState.isGrabbed) // did not successfully grab grenade
            {
                ReParent();
            } else {
                if (audioSource) {
                    audioSource.clip = grabSound;
                    audioSource.Play();
                }

                alreadyGrabbed = true;
                isExplode = false;
            }
        } else if (!m_GrabState.isGrabbed) {
            if (alreadyGrabbed) {
                rb.isKinematic = false;

                if (frames >= duration && !isExplode) { // Boom
                    // Play boom audio
                    if (audioSource) {
                        audioSource.clip = boomSound;
                        audioSource.Play();
                    }

                    // Boom effects
                    GameObject ps = Instantiate(explosion, transform.position, transform.rotation);
                    Destroy(ps, 2);

                    // Boom to zombie
                    Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

                    foreach (Collider nearbyObject in colliders) {
                        if (nearbyObject.gameObject.tag != "Enemy") continue;

                        ZombieDie dest = nearbyObject.GetComponent<ZombieDie>();
                        if (dest != null) {
                            dest.Destroy();
                        }
                    }

                    // Boom to zombie debris, launching them away.
                    Collider[] destColliders = Physics.OverlapSphere(transform.position, radius);

                    foreach (Collider nearbyObject in destColliders) {
                        if (nearbyObject.gameObject.tag != "Debris") continue;

                        Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();

                        if (rb != null) {
                            rb.AddExplosionForce(force, transform.position, radius);
                        }
                        Destroy(nearbyObject, 5);
                    }

                    // Boom vibration
                    hapticRunning = true;

                    // Spawn the grenade back at belt.
                    ReParent();
                } else {
                    frames += 1;
                }

            } else {
                ReParent();
            }
        }

        // Boom vibration
        if (hapticRunning) {
            HapticPulse(pulseLength);
            vibrationFrames += 1;
        } else if (vibrationFrames != 0) {
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
            vibrationFrames = 0;
            hapticRunning = false;
        }
    }

    void HapticPulse(float length)
    {
        if (vibrationFrames == 0)
        {
            OVRInput.SetControllerVibration(0.8f, 0.8f, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0.8f, 0.8f, OVRInput.Controller.RTouch);
        }
        if (vibrationFrames >= length)
        {
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
            hapticRunning = false;
            vibrationFrames = 0;
        }
    }
}
