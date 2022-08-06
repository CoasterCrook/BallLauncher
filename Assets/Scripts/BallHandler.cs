using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] Rigidbody2D currentBallRigidBody;
    [SerializeField] SpringJoint2D currentBallSpringJoint;
    [SerializeField] float delayDuration = 0.5f;
    
    private Camera mainCamera;
    private bool isDragging = false;

    private void Start() 
    {
        mainCamera = Camera.main;
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
    }
}
