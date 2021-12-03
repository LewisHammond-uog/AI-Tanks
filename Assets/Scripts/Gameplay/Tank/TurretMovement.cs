using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretMovement : MonoBehaviour
{
    [SerializeField] private float turretRotationSpeed = 1f;
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
        turretLookPoint = transform.right;
        useTurretLookPoint = true;
    }
    
    private void Update()
    {
        if (useTurretLookPoint)
        {
            RotateTurretToTargetPoint(Time.deltaTime);
        }
    }

    /// <summary>
    /// Gets the difference in angle between the current turret look a point and the turret target
    /// </summary>
    /// <returns></returns>
    public float GetAngleToTurretTarget()
    {
        Vector3 targetDirection = turretLookPoint - transform.position;
        return Vector3.Angle(targetDirection, transform.forward);
    }

    //Rotate the turret towards the turrget look at point
    private void RotateTurretToTargetPoint(float deltaTime)
    {
        Vector3 targetDirection = turretLookPoint - transform.position;
        float stepThisFrame = turretRotationSpeed * deltaTime;
        //Calculate new direction this frame
        Vector3 newDirection =
            Vector3.RotateTowards(transform.forward, targetDirection, stepThisFrame, 0f).normalized;
        //Change rotation of turret
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
