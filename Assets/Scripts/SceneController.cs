using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
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

    private bool hapticRunning = false;
    private float frames = 0;
    public float pulseLength = 15f;

    private GameObject zombie1;
    private GameObject zombie2;
    private GameObject zombie3;

    private static Vector3 location1 = new Vector3(-3.89f, 4.29f, 41.67f);
    private static Vector3 location2 = new Vector3(10.91543f, 4.29f, 51.87262f);
    private static Vector3 location3 = new Vector3(-2.597591f, 4.29f, 59.68f);
    private static Vector3 location4 = new Vector3(-7.406487f, 4.29f, 38.98684f);
    private static Vector3 location5 = new Vector3(0.9229925f, 4.29f, 46.23018f);
    private static Vector3 location6 = new Vector3(5.326944f, 4.29f, 54.92865f);
    private static Vector3 location7 = new Vector3(5.703934f, 4.29f, 46.0558f);
    private static Vector3 location8 = new Vector3(15.80111f, 4.29f, 48.49199f);
    private static Vector3 location9 = new Vector3(5.703934f, 4.29f, 46.0558f);
    private static Vector3 location10 = new Vector3(22.595f, 10.06f, 40.49f);
    
    private List<GameObject> createdZombies = new List<GameObject>();
    
    private List<Vector3> zombieLocations = new List<Vector3>{location1, location2, location3, location4, location5, location6, location7, location8, location9, location10};

    private int counter = 0;

	private bool ended = false;

	private AudioSource zombieDeathSound;
	
    // Start is called before the first frame update
    void Start()
    {
        //zombie1 = Instantiate(zombiePrefab, location1, Quaternion.identity);
        zombie2 = Instantiate(zombiePrefab, location2, Quaternion.identity);
        //zombie3 = Instantiate(zombiePrefab, location3, Quaternion.identity);
        //createdZombies.Add(zombie1);
        createdZombies.Add(zombie2);
        //createdZombies.Add(zombie3);
        //counter = 3;
        counter = 1;
        zombieDeathSound = GetComponentInChildren<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.Start) && !ended)
        {
            TogglePause(true);
        }
        //Pause Haptic
        if (hapticRunning)
        {
            HapticPulse(pulseLength, OVRInput.Controller.LTouch);
            frames += 1;
        }

        foreach (GameObject zombie in createdZombies)
        {
            if (!zombie.activeSelf)
            {
                createdZombies.Remove(zombie);
            }
        }
        
        if (createdZombies.Count < 2)
        {
            if (counter < 10)
            {
                int index = Random.Range (0, zombieLocations.Count);
                GameObject zombie = Instantiate(zombiePrefab, zombieLocations[index], Quaternion.identity);
                createdZombies.Add(zombie);
                counter += 1;
            }
           
        } 
		


		if(!ended)        
		{
        	if (createdZombies.Count == 0 )
        	{
            	//end of game screen 
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
            	endGameMenu.transform.position = menuPosn;
            	Quaternion orientation = cam.transform.rotation;
            	orientation.x = 0;
            	orientation.z = 0;
            	endGameMenu.transform.rotation = orientation;
            
            	//Toggle Elements
            	endGameMenu.SetActive(true);
            	winText.gameObject.SetActive(true);
            	loseText.gameObject.SetActive(false);
            	loco.SetActive(false);
            	pointer.SetActive(true);
            	paused = true;
				ended = true;
        	}

        	if (player.GetComponent<PlayerBehavior>().playerLost)
        	{
            	//end of game screen 
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
            	endGameMenu.transform.position = menuPosn;
            	Quaternion orientation = cam.transform.rotation;
            	orientation.x = 0;
            	orientation.z = 0;
            	endGameMenu.transform.rotation = orientation;
            
            	//Toggle Elements
            	endGameMenu.SetActive(true);
            	winText.gameObject.SetActive(false);
            	loseText.gameObject.SetActive(true);
            	loco.SetActive(false);
            	pointer.SetActive(true);
            	paused = true;
				ended = true;
        	}
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
