using System.Collections;
using System.Collections.Generic;
using OculusSampleFramework;
using UnityEngine;

public class ShieldBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 offsetPosition; // position relative to player
    private Quaternion offsetRotation; // rotation relative to player
    OVRGrabbable m_GrabState;
    private Rigidbody rb;
    private AudioSource source; // grab audio
    private AudioSource blockAudio; // block audio
    private bool alreadyGrabbed; // whether the shield is currently already grabbed or not
    private Vector3 grabbedScale; // scale of shield when grabbed
    private Vector3 notGrabbedScale; // larger scale of shield when not grabbed (behind player)
    private MeshRenderer mesh; // mesh renderer of shield
    [SerializeField] private GameObject reflectedBullet;
    private List<GameObject> reflBulletsList;
    private int maxReflBullets = 20; // max reflected bullets in scene allowed (older are removed)
    
    private float frames = 0;
    private bool hapticRunning = false;
    private bool grabbedByRight; // true iff crossbow grabbed by right hand
    [SerializeField] private GameObject rightHand;
    private OVRGrabber rightGrabber;
    public float pulseLength = 3f;

    // Start is called before the first frame update
    void Start()
    {
        m_GrabState = gameObject.GetComponent<OVRGrabbable>();
        Transform shieldTransform = transform; 
        offsetPosition = shieldTransform.localPosition;
        offsetRotation = shieldTransform.localRotation;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        source = transform.GetChild(0).GetComponent<AudioSource>();
        blockAudio = transform.GetChild(1).GetComponent<AudioSource>();
        alreadyGrabbed = false;
        grabbedScale = new Vector3(0.1f, 0.1f, 0.1f);
        notGrabbedScale = new Vector3(0.4f, 0.4f, 0.4f);
        mesh = GetComponent<MeshRenderer>();
        mesh.enabled = false;
        reflBulletsList = new List<GameObject>();
        rightGrabber = rightHand.GetComponent<DistanceGrabber>();
    }

    void ReParent()
    {
        transform.parent=player.transform;
        transform.localPosition = offsetPosition;
        transform.localRotation = offsetRotation;
        rb.isKinematic = false;
        mesh.enabled = false;
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

        if ((OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9f ||
             OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9f))
            // at least one grab is pressed
        {
            transform.parent = null; // detach from parent so distance grab can work
            rb.isKinematic = true;
            if (!m_GrabState.isGrabbed) // did not successfully grab shield (wrong controller to grab, or did not detect)
            {
                ReParent();
            }
            else // is grabbed
            {
                transform.localScale = grabbedScale; // smaller shield in hand
                mesh.enabled = true;
            }

            if (!alreadyGrabbed && m_GrabState.isGrabbed)
            {
                source.Play(); // play grab shield sound effect
                alreadyGrabbed = true;
            }
        }
        
        if (!m_GrabState.isGrabbed) // user let go of grip
        {
            ReParent();
            alreadyGrabbed = false;
            transform.localScale = notGrabbedScale; // bigger shield size on back
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
    
    
    private void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.tag == "EnemyBullet")
        {
            Vector3 incomingVel = col.gameObject.GetComponent<Rigidbody>().velocity;
            float incomingSpeed = incomingVel.magnitude;
            Destroy(col.gameObject);
            Vector3 loc = col.transform.position + 0.3f * transform.forward;
            Quaternion rotation = transform.rotation;
            GameObject newBullet = Instantiate(reflectedBullet, loc, rotation);
            newBullet.GetComponent<Rigidbody>().velocity = incomingSpeed * transform.forward;
            // blockAudio.Play(); // play blocking sound effect
            reflBulletsList.Add(newBullet);
            hapticRunning = true;
            if (reflBulletsList.Count > maxReflBullets)
            {
                Destroy(reflBulletsList[0]);
                reflBulletsList.RemoveAt(0);
            }
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
