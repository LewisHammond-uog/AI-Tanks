using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    private float turretRotationSpeed;
    public float TurretRotationSpeed {
        get => turretRotationSpeed;
        set => turretRotationSpeed = Mathf.Max(value, 0f);
    }

    private bool useTurretLookPoint; //If we should be using the last set look point 
    private Vector3 turretLookPoint; //Point for the turret to look at
    public Vector3 TurretLookAtPoint
    {
        set
        {
            //Sanitize input to only use Y vector component
            useTurretLookPoint = true;
            turretLookPoint = new Vector3(value.x, 0, value.z);
        }
    }

    private void Start()
    {
        turretLookPoint = transform.forward;
    }
    
    private void Update()
    {
        if (useTurretLookPoint)
        {
            RotateTurretToTargetPoint(Time.deltaTime);
        }
    }

    //Rotate the turret towards the turrget look at point
    private void RotateTurretToTargetPoint(float deltaTime)
    {
        float stepThisFrame = turretRotationSpeed * deltaTime;
        //Calculate new direction this frame
        Vector3 newDirection =
            Vector3.RotateTowards(transform.forward, turretLookPoint, stepThisFrame, 0f);
        //Change rotation of turret
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
