using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieBehavior : MonoBehaviour
{
    private GameObject player;
	private float frames = 0;
	private float attackCooldown = 40;
	[SerializeField] private GameObject enemyBullet;
	private float enemyBulletSpeed = 3f;
	
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
		if (Time.timeScale != 0){
        	Vector3 playerPosition = player.transform.position;
        	playerPosition.y = this.transform.position.y;
        	transform.LookAt(playerPosition, Vector3.up);
			if (Vector3.Distance(playerPosition, transform.position) > 0.5)
			{
        		transform.position = Vector3.MoveTowards(transform.position, playerPosition, 0.005f);
    		}
			else 
			{
        		transform.position = Vector3.MoveTowards(transform.position, playerPosition, -0.005f);
    		}
		}

		shootAtPlayer();
	}

	void shootAtPlayer() {
		if (frames >= attackCooldown) {
			GameObject bullet = Instantiate(
				enemyBullet, 
				transform.position + new Vector3(0, 1.5f, 0), 
				transform.rotation
			);
			Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
			bulletRb.freezeRotation = true;
			bullet.transform.LookAt(player.transform.position);
			bulletRb.AddForce(bullet.transform.forward * enemyBulletSpeed, ForceMode.Impulse);
			Destroy(bullet, 500f);

			frames = 0;
		} else {
			frames++;
		}
	}

}
