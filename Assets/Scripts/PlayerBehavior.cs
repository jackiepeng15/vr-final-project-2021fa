using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private GameObject damageFlash;
    [SerializeField] private GameObject HealthBar1;
    [SerializeField] private GameObject HealthBar2;
    [SerializeField] private GameObject HealthBar3;
    [SerializeField] private GameObject HealthBar4;
    [SerializeField] private GameObject HealthBar5;
    private int frames = 0;
    private int flashDuration = 5;
    private bool isDamaged;
    private int hits;
    public bool playerLost;

    private List<GameObject> playerHealth = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        damageFlash.SetActive(false);
        isDamaged = false;
        hits = 0;
        playerHealth.Add(HealthBar1);
        playerHealth.Add(HealthBar2);
        playerHealth.Add(HealthBar3);
        playerHealth.Add(HealthBar4);
        playerHealth.Add(HealthBar5);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDamaged) {
            damageFlash.SetActive(true);
            if (frames < flashDuration) {
                frames += 1;
            } else {
                isDamaged = false;
                frames = 0;
            }
        } else {
            damageFlash.SetActive(false);
        }

        if (transform.position.y < 0) // player dies when falling off the edge
        {
            playerLost = true;
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "EnemyBullet")
        {
            isDamaged = true;
            hits += 1;
            Destroy(col.gameObject);
            if (hits == 5)
            {
                if (playerHealth.Count == 1)
                {
                    playerHealth[0].SetActive(false);
                    playerHealth.Remove(playerHealth[0]);
                    playerLost = true;
                }
                else
                {
                    playerHealth[playerHealth.Count - 1].SetActive(false);
                    playerHealth.Remove(playerHealth[playerHealth.Count - 1]);
                    hits = 0;
                }
            }
        }

    }
}
