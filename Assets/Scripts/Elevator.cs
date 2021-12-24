using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
	[SerializeField] private  float bottomHeight;
	[SerializeField] private  float topHeight;
	[SerializeField] private  float speed = 0.5f;

	
	private bool moving; 
	private bool up;
	
	private float target;
	private float start;
	private float t;
	

    // Start is called before the first frame update
    void Start()
    {
    	moving = false;
		up = false;
		t= 0.0f;    
    }

    // Update is called once per frame
    void Update()
    {
		if(moving)
		{
			OVRInput.SetControllerVibration(.3f, .5f, OVRInput.Controller.LTouch);
			OVRInput.SetControllerVibration(.3f, .5f, OVRInput.Controller.RTouch);
			transform.position = new Vector3(transform.position.x, Mathf.Lerp(start, target, t), transform.position.z);
			t+= speed * Time.deltaTime;
			if(t >=1.0f)
			{
				moving = false;
				up = !up;
				OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.LTouch);
				OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
			}
		}

        
    }

	public void BeginMove()
	{
		if(!moving)
		{
			if(up)
			{
				start = topHeight;
				target = bottomHeight;
			}
			else
			{
				start = bottomHeight;
				target = topHeight;
			}	
			moving = true;
			t = 0.0f; 
			OVRInput.SetControllerVibration(.3f, .5f, OVRInput.Controller.LTouch);
			OVRInput.SetControllerVibration(.3f, .5f, OVRInput.Controller.RTouch);
		}
	}
}
