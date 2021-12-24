using System;
using System.Collections;
using System.Collections.Generic;
// using UnityEditor.SearchService;
using UnityEngine;


public class ZombieDie : MonoBehaviour
{
    
    private int zBodyHits;
    [SerializeField] public GameObject destroyedVersion;

    [SerializeField] private AudioSource zombieHitAudio;

    private SceneController sceneController;

    private TutorialController tutorialController;

    // [SerializeField] private AudioSource zombieDeathAudio;
    // Start is called before the first frame update
    void Start()
    {
        zBodyHits = 0;
        sceneController = GameObject.Find("SceneController").GetComponent<SceneController>(); // null if tutorial
        tutorialController = GameObject.Find("SceneController").GetComponent<TutorialController>(); // null if not tutorial
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 0) // zombie fell off the edge
        {
            PlayZombieDeathSound();
            gameObject.SetActive(false);
        }
        
    }
    
   /*private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "MeleeWeapon" || col.gameObject.tag == "Bullet")
        {
            this.gameObject.SetActive(false);
        
            
        }
    }*/

    public void Destroy() {
        PlayZombieDeathSound();
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision col)
    {
        //private List<GameObject> createdZombies = new List<GameObject>();
        //ContactPoint[] contact = new ContactPoint[col.contactCount];
        if (col.gameObject.tag == "MeleeWeapon" || col.gameObject.tag == "Bullet")
        {
            ContactPoint contact = col.GetContact(0);
            if (contact.thisCollider.gameObject.tag == "Enemy" || contact.thisCollider.gameObject.tag == "Enemy")
            {
                this.gameObject.SetActive(false);
            }
            else if (contact.thisCollider.gameObject.tag == "Z_rightLeg" || contact.thisCollider.gameObject.tag == "Z_rightArm" ||
                contact.thisCollider.gameObject.tag == "Z_leftLeg" || contact.thisCollider.gameObject.tag == "Z_leftArm" || 
                contact.otherCollider.gameObject.tag == "Z_rightLeg" || contact.otherCollider.gameObject.tag == "Z_rightArm" ||
                contact.otherCollider.gameObject.tag == "Z_leftLeg" || contact.otherCollider.gameObject.tag == "Z_leftArm")
            {
                zBodyHits += 1;
                zombieHitAudio.Play();

                if (zBodyHits > 2)
                {
                    PlayZombieDeathSound();
                    this.gameObject.SetActive(false);
                }
                else
                {
                    for (var a = 0; a < transform.childCount; a++)
                    {
                        if (transform.GetChild(a).gameObject.tag == contact.otherCollider.gameObject.tag ||
                            transform.GetChild(a).gameObject.tag == contact.thisCollider.gameObject.tag)
                        {
                            transform.GetChild(a).gameObject.SetActive(false);
                        }
                    }
                }

            }
        }

        if (col.gameObject.tag == "Bullet") {
            Destroy(col.gameObject);
        }
    }

    void PlayZombieDeathSound()
    {
        if (tutorialController == null)
        {
            sceneController.PlayZombieDeathSound();
        }
        else
        {
            tutorialController.PlayZombieDeathSound();
        }
    }
}
