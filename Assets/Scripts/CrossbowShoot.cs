using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using UnityEngine;

public class CrossbowShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject arrowPrefab;

    [Header("Animation References")]
    [SerializeField] private Animator crossbowAnimator;
    [SerializeField] private Transform dockLocation;
    [SerializeField] private GameObject arrowDummy;

    [Header("Settings")]
    [Tooltip("Arrow Speed")] [SerializeField] private float arrowSpeed;
    
    private AudioSource arrowSound;
    OVRGrabbable m_GrabState;
    bool buttonPressed;

    private bool hapticRunning = false;
    private float frames = 0;
    public float pulseLength = 3f;
    
    private bool grabbedByRight; // true iff crossbow grabbed by right hand
    [SerializeField] private GameObject rightHand;
    private OVRGrabber rightGrabber;

    private void Awake()
    {
        m_GrabState = this.GetComponent<OVRGrabbable>();
        buttonPressed = false;
        arrowDummy.SetActive(true);
        grabbedByRight = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        arrowSound = GetComponent<AudioSource>();

        if (crossbowAnimator == null) {
            crossbowAnimator = GetComponentInChildren<Animator>();
        }
        crossbowAnimator.speed = arrowSpeed;
        rightGrabber = rightHand.GetComponent<DistanceGrabber>();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (m_GrabState.isGrabbed) {
            if (m_GrabState.grabbedBy == rightGrabber)
            {
                grabbedByRight = true;
            }
            else
            {
                grabbedByRight = false;
            }
            // Shoot the arrow
            if ((OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.9 && grabbedByRight
                || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.9 && !grabbedByRight) && !buttonPressed) {
                // Play audio
                arrowSound.Play();

                // Create an arrow object
                GameObject arrowObj = Instantiate(arrowPrefab, dockLocation.position, dockLocation.rotation);
                arrowObj.GetComponent<Rigidbody>().AddForce(dockLocation.forward * arrowSpeed);
                Destroy(arrowObj, 5);

                // Crossbow animation
                crossbowAnimator.ResetTrigger("Fire");
                arrowDummy.SetActive(false);
                
                // Haptic
                hapticRunning = true;

                // Crossbow state
                buttonPressed = true;
            }

            if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) <= 0.9 && grabbedByRight
            || OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) <= 0.9 && !grabbedByRight) {
                buttonPressed = false;
                hapticRunning = false;
                crossbowAnimator.SetBool("Fire", true);
                arrowDummy.SetActive(true);
                if (grabbedByRight)
                {
                    OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
                }
                else
                {
                    OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
                }
                
                
            }
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

    void HapticPulse(float length, OVRInput.Controller cont)
    {
        if (frames == 0)
        {
            OVRInput.SetControllerVibration(0.2f, 0.2f, cont);
            
        }
        if (frames >= length)
        {
            OVRInput.SetControllerVibration(0, 0, cont);
        }
    }
}
