using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : MonoBehaviour
{
    public Transform anchor;
    
    public Outline Outline;
    public Collider Collider;
    public Rigidbody Rigidbody;

    private bool _outlined;
    private bool _grabbed;

    private void Update()
    {
        _outlined = false;
    }

    private void LateUpdate()
    {
        if (_outlined || _grabbed)
        {
            Outline.enabled = true;
        }
        else
        {
            Outline.enabled = false;
        }
    }
    
    public void OnGrabStart()
    {
        Rigidbody.useGravity = false;
        Rigidbody.angularVelocity = Vector3.zero;
        Rigidbody.velocity = Vector3.zero;
        Collider.enabled = false;
        _grabbed = true;
    }

    public void OnGrabEnd()
    {
        Rigidbody.useGravity = true;
        Collider.enabled = true;
        _grabbed = false;
    }

    public void OnAbleToGrab()
    {
        _outlined = true;
    }
}
