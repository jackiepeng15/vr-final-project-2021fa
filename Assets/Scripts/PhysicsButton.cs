using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float deadZone = 0.025f;

    private bool m_isPressed;
    private Vector3 m_startPos;
    private ConfigurableJoint m_joint;

    public UnityEvent onPressed, onReleased;

    void Start()
    {
        m_startPos = transform.localPosition;
        m_joint = GetComponent<ConfigurableJoint>();
    }

    void Update()
    {
        if (!m_isPressed && GetValue() + threshold >= 1)
        {
            Pressed();
        }
        if (m_isPressed && GetValue() - threshold <= 0)
        {
            Released();
        }
    }

    private void Pressed()
    {
        m_isPressed = true;
        onPressed.Invoke();
    }
    
    private void Released()
    {
        m_isPressed = false;
        onReleased.Invoke();
    }

    private float GetValue()
    {
        var value = Vector3.Distance(m_startPos, transform.localPosition) / (m_joint.linearLimit.limit/30);
        if (Mathf.Abs(value) < deadZone)
        {
            value = 0;
        }

        return Mathf.Clamp(value, -1f, 1f);
    }


}