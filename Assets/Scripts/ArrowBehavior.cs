using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    private Quaternion q;
    private Vector3 v3;

    private bool hasHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate() {
        transform.forward = Vector3.Slerp(transform.forward, GetComponent<Rigidbody>().velocity.normalized, Time.deltaTime);    
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Z_rightLeg" || col.gameObject.tag == "Z_rightArm" ||
            col.gameObject.tag == "Z_leftLeg" || col.gameObject.tag == "Z_leftArm") {
            Destroy(this);
            return;
        }
        if (col.gameObject.tag != "Environment") return;

        Rigidbody m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.isKinematic = true;
        hasHit = true;
        m_rigidbody.velocity = Vector3.zero;
        m_rigidbody.angularVelocity = Vector3.zero;
    }

    // Update is called once per frame
    // void LateUpdate()
    // {
    //     if (hasHit) {
    //         transform.position = v3;
    //         transform.rotation = q;
    //         hasHit = false;
    //     } else {
    //         v3 = transform.position;
    //         q = transform.rotation;
    //     }
    // }
}
