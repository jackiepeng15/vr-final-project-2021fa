using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyBehavior : MonoBehaviour
{
    [SerializeField] GameObject player;
    public Quaternion orientation; // orientation of body
    
    // Start is called before the first frame update
    void Start()
    {
        orientation = player.transform.rotation;
        transform.rotation = orientation; // initial orientation is where the player is facing

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = orientation; // orientation variable will be changed by other scripts throughout
    }
}
