using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject menu;

    [SerializeField] private GameObject player;

    [SerializeField] private Camera cam;
    
    [SerializeField] private GameObject loco;
    [SerializeField] private GameObject pointer;

    [SerializeField] private GameObject zombiePrefab;
    
    [SerializeField] private GameObject endGameMenu;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject loseText;

    public float dist = 7.0f;
    public float rise = 2.5f;

    private bool paused = false;

    private bool endgame = false;

    private bool hapticRunning = false;
    private float frames = 0;
    public float pulseLength = 15f;
    
    [SerializeField] private GameObject narrator;
    [SerializeField] private Vector3 narratorPos1;
    [SerializeField] private Vector3 narratorPos2;
    [SerializeField] private Vector3 narratorPos3;
    [SerializeField] private Vector3 narratorPos4;

    private Vector3 narratorTargetPos;

    [SerializeField] private GameObject zombie1;
    [SerializeField] private GameObject zombie2;
    [SerializeField] private GameObject zombie3;
    [SerializeField] private GameObject zombie4;

    
    private int phase = 0;

    [SerializeField] private AudioSource intro;
    [SerializeField] private AudioSource elevator;
    [SerializeField] private AudioSource rotationAudio;
    [SerializeField] private AudioSource headDown;
    [SerializeField] private AudioSource healthBars;
    [SerializeField] private AudioSource weapons;
    [SerializeField] private AudioSource crossbowAudio;
    [SerializeField] private AudioSource grabSword;
    [SerializeField] private AudioSource swingSword;
    [SerializeField] private AudioSource grenadeAudio;
    [SerializeField] private AudioSource shieldAudio;
    [SerializeField] private AudioSource end;
    
    [SerializeField] private GameObject target1;
    [SerializeField] private GameObject target2;
    
    [SerializeField] private GameObject crossbow;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject grenade;
    [SerializeField] private GameObject shield;

    private bool shieldBeenGrabbed = false;
    
    [SerializeField] private AudioSource zombieDeathSound;

    // Start is called before the first frame update
    void Start()
    {
        narratorTargetPos = narratorPos1;
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.Start))
        {
            TogglePause(true);
        }
        //Pause Haptic
        if (hapticRunning)
        {
            HapticPulse(pulseLength, OVRInput.Controller.LTouch);
            frames += 1;
        }

        
        moveNarrator();
        
        

        if (CheckMoveOn())
        {
            NextPhase();
        }

        if (!shieldBeenGrabbed && shield.activeSelf && shield.GetComponent<OVRGrabbable>().isGrabbed)
        {
            shieldBeenGrabbed = true;
        }

    }

    private void moveNarrator()
    {
        
        if (Vector3.Distance(narrator.transform.position, narratorTargetPos) > .1)
        {
            narrator.transform.position = Vector3.MoveTowards(narrator.transform.position, narratorTargetPos, 0.1f);
            narrator.transform.LookAt(narratorTargetPos, Vector3.up);
        }
        else
        {
            narrator.transform.LookAt(player.transform.position, Vector3.up);
        }
    }
    
    private bool CheckMoveOn()
    {
        if (phase == 0)
        {
            if (intro.isPlaying == false)
            {
                return true;
            }
        }
        else if (phase == 1)
        {
            if (Vector3.Distance(player.transform.position, target1.transform.position) <= 2)
            {
                return true;
            }
        }
        else if (phase == 2)
        {
            if (elevator.isPlaying == false && player.transform.position.y >= 9)
            {
                return true;
            }
        }
        else if (phase == 3)
        {
            if (rotationAudio.isPlaying == false)
            {
                return true;
            }
        }
        else if (phase == 4)
        {
            if (headDown.isPlaying == false && Vector3.Distance(player.transform.position, target2.transform.position) <= 2)
            {
                return true;
            }
        }
        else if (phase == 5)
        {
            if (healthBars.isPlaying == false)
            {
                return true;
            }
        }
        else if (phase == 6)
        {
            if (weapons.isPlaying == false && crossbow.GetComponent<OVRGrabbable>().isGrabbed)
            {
                return true;
            }
        }
        else if (phase == 7)
        {
            if (crossbowAudio.isPlaying == false && !zombie1.activeSelf)
            {
                return true;
            }
        }
        else if (phase == 8)
        {
            if (grabSword.isPlaying == false && sword.GetComponent<OVRGrabbable>().isGrabbed)
            {
                return true;
            }
        }
        else if (phase == 9)
        {
            if (swingSword.isPlaying == false && !zombie2.activeSelf)
            {
                return true;
            }
        }
        else if (phase == 10)
        {
            if (grenadeAudio.isPlaying == false && !zombie3.activeSelf)
            {
                return true;
            }
        }
        else if (phase == 11)
        {
            if (shieldAudio.isPlaying == false && shieldBeenGrabbed && !zombie4.activeSelf)
            {
                return true;
            }
        }
        else if (phase == 12)
        {
            if (end.isPlaying == false)
            {
                return true;
            }
        }

        return false;
    }

    private void NextPhase()
    {
        phase += 1;
        if (phase == 1)
        {
            loco.SetActive(true);
            target1.SetActive(true);
            narratorTargetPos = narratorPos2;
        }

        else if (phase == 2)
        {
            elevator.Play();
            target1.SetActive(false);
            narratorTargetPos = narratorPos3;
        }
        else if (phase == 3)
        {
            rotationAudio.Play();
        }
        else if (phase == 4)
        {
            target2.SetActive(true);
            headDown.Play();
            narratorTargetPos = narratorPos4;
        }
        else if (phase == 5)
        {
            target2.SetActive(false);
            healthBars.Play();
        }
        else if (phase == 6)
        {
            weapons.Play();
            crossbow.SetActive(true);
        }
        else if (phase == 7)
        {
            crossbowAudio.Play();
            zombie1.SetActive(true);
        }
        else if (phase == 8)
        {
            grabSword.Play();
            sword.SetActive(true);
        }
        else if (phase == 9)
        {
            zombie2.SetActive(true);
            swingSword.Play();
        }
        else if (phase == 10)
        {
            grenadeAudio.Play();
            zombie3.SetActive(true);
            grenade.SetActive(true);
        }
        else if (phase == 11)
        {
            shieldAudio.Play();
            shield.SetActive(true);
            zombie4.SetActive(true);
        }
        else if (phase == 12)
        {
            zombie4.SetActive(false);
            end.Play();
            narrator.SetActive(false);
        }
        else if (phase == 13)
        {
            paused = true;
            LoadScene("final_project");
        }

    }

    public void TogglePause(bool triggerHaptic = false)
    {
        if (paused)
        {
            Time.timeScale = 1;
            menu.SetActive(false);
            loco.SetActive(true);
            pointer.SetActive(false);
            
        }
        else
        {
            Time.timeScale = 0;
            //Update Menu Pose
            Vector3 posn = player.transform.position;
            Quaternion rot = player.transform.rotation;
            rot.x = 0;
            rot.z = 0;
            GameObject tempPose = new GameObject();
            tempPose.transform.position = posn;
            tempPose.transform.rotation = rot;
            Vector3 menuPosn = posn + tempPose.transform.forward * dist;
            menuPosn.y = menuPosn.y + rise;
            Destroy(tempPose);
            menu.transform.position = menuPosn;
            Quaternion orientation = cam.transform.rotation;
            orientation.x = 0;
            orientation.z = 0;
            menu.transform.rotation = orientation;
            
            //Toggle Elements
            menu.SetActive(true);
            loco.SetActive(false);
            pointer.SetActive(true);

        }

        paused = !paused;
        if (triggerHaptic)
        {
            hapticRunning = true;
        }
        
        



    }

    public void Restart()
    {
        TogglePause(false);
        Scene scene = SceneManager.GetActiveScene();
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        SceneManager.LoadScene(scene.name);
    }
    
    public void LoadScene(string name)
    {
        TogglePause(false);
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
        SceneManager.LoadScene(name);
    }

    void HapticPulse(float length, OVRInput.Controller cont)
    {
        
        
        if (frames == 0)
        {
            OVRInput.SetControllerVibration(1, 1, cont);
            
        }
        if (frames >= length)
        {
            OVRInput.SetControllerVibration(0, 0, cont);
            frames = -1;
            hapticRunning = false;
        }
        
    }
    
    public void PlayZombieDeathSound()
    {
        zombieDeathSound.Play();
    }
    
}

