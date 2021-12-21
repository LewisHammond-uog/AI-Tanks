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

    //Transform used when we don't have another transform target to look at (i.e a vector)
    [SerializeField] private Transform turretDummyTransform;
    
    private Transform turretLookPoint; //Point for the turret to look at

    private void Start()
    {
        turretLookPoint = transform;
    }
    
    private void Update()
    {
        if (turretLookPoint)
        {
            RotateTurretToTargetPoint(Time.deltaTime);
        }
    }

    /// <summary>
    /// Set the turret look at target
    /// </summary>
    /// <param name="targetTransform">Transform to look at</param>
    public void SetTurretLookTarget(Transform targetTransform)
    {
        turretLookPoint = targetTransform;
    }

    /// <summary>
    /// Set the turret look at target
    /// </summary>
    /// <param name="targetPosition">Position to look at</param>
    public void SetTurretLookTarget(Vector3 targetPosition)
    {
        //Set the dummy transform to our position and look at that
        turretDummyTransform.position = targetPosition;
        SetTurretLookTarget(turretDummyTransform);
    }

    /// <summary>
    /// Gets the difference in angle between the current turret look a point and the turret target
    /// </summary>
    /// <returns></returns>
    public float GetAngleToTurretTarget()
    {
        Transform turretTransform = transform;
        //Santisze target position so that we don't rotate the turret vertically
        Vector3 targetPosition =
            new Vector3(turretLookPoint.position.x, transform.position.y, turretLookPoint.position.z);
        Vector3 targetDirection = targetPosition - turretTransform.position;
        return Vector3.Angle(targetDirection, turretTransform.forward);
    }

    //Rotate the turret towards the turrget look at point
    private void RotateTurretToTargetPoint(float deltaTime)
    {
        //Santisze target position so that we don't rotate the turret vertically
        Vector3 targetPos = new Vector3(turretLookPoint.position.x, transform.position.y, turretLookPoint.position.z);
        Vector3 targetDirection = targetPos - transform.position;
        float stepThisFrame = turretRotationSpeed * deltaTime;
        //Calculate new direction this frame
        Vector3 newDirection =
            Vector3.RotateTowards(transform.forward, targetDirection, stepThisFrame, 0f).normalized;
        //Change rotation of turret
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
