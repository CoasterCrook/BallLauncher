using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] float delayDuration;
    [SerializeField] float respawnDelay;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivot;
    
    Rigidbody2D currentBallRigidBody;
    SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging = false;

    private void Start() 
    {
        mainCamera = Camera.main;
        SpawnNewBall();
    }
    
    void Update()
    {
        if (currentBallRigidBody == null) {return;}
        
        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }
            
            isDragging =  false;
            return;
        }
        
        isDragging = true;
        currentBallRigidBody.isKinematic = true;
        Vector2 touchPosition =  Touchscreen.current.primaryTouch.position.ReadValue();
        
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidBody.position = worldPosition;
    }

     void LaunchBall()
    {
        currentBallRigidBody.isKinematic = false;
        currentBallRigidBody = null;
        
        Invoke(nameof(DetachBall), delayDuration);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }

    void SpawnNewBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        currentBallRigidBody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot;
    }
}
