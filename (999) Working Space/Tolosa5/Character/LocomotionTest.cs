using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocomotionTest : MonoBehaviour
{
    public enum MovementMode
    {
        RelativeToCharacter,
        RelativeToCamera,
    };

    public enum OrientationMode
    {
        OrientateToCameraForward,
        OrientateToMovementForward,
        OrientateToTarget,
        DoNotOrientate,
    };

    #region Referencias
    CharacterController chC;
    #endregion


    [Header("Movement")]
    [SerializeField] MovementMode movementMode;
    private Vector3 dirMov;
    private Vector3 velocityToApply;
    [SerializeField] private float moveSpeed = 5f;
    
    
    [Header("Animation")]
    [SerializeField] float transitionVelocity = 1f;
    private Vector3 smoothedAnimationVelocity;


    [Header("Orientation Settings")]
    [SerializeField] float angularSpeed = 360f;
    [SerializeField] Transform orientationTarget;
    [SerializeField] OrientationMode orientationMode = OrientationMode.OrientateToMovementForward;


    [Header("Jump")]
    [SerializeField] float Gravity = -9.8f;
    [SerializeField] float jumpSpeed = 2f;
    private float verticalVelocity;

    [Header("IVisible Settings")]
    [SerializeField] string allegiance = "Player";


    [Header("Inputs")]
    //para evitar el componente playerActions, viene mejor hacer esto
    [SerializeField] InputActionReference playerMove;

    private void Awake() 
    {
        chC = GetComponent<CharacterController>();
    }

    private void OnEnable() 
    {
        playerMove.action.Enable();
    }

    private void Update() 
    {
        velocityToApply = Vector3.zero;
        HandlePlaneMovement();
        HandleVerticalMovement();
        chC.Move(velocityToApply * Time.deltaTime);
        HandleOrientation();
    }

    private void HandleOrientation()
    {
        if (orientationMode != OrientationMode.DoNotOrientate)
        {
            Vector3 desiredDirection = Vector3.zero;
            switch (orientationMode)
            {
                case OrientationMode.OrientateToCameraForward:
                desiredDirection = Camera.main.transform.forward;
                
                break;

                case OrientationMode.OrientateToMovementForward:
                if (velocityToApply.sqrMagnitude > 0f)
                    desiredDirection = velocityToApply;
                
                break;

                case OrientationMode.OrientateToTarget:
                desiredDirection = orientationTarget.transform.position - transform.position;
                
                break;
            }

            desiredDirection = Vector3.ProjectOnPlane(desiredDirection, Vector3.up);

            float angularDistance = Vector3.SignedAngle(transform.forward, desiredDirection, Vector3.up);
            float angleToApply = angularSpeed * Time.deltaTime;

            angleToApply = Mathf.Min(angleToApply, Mathf.Abs(angularDistance));
            angleToApply *= Mathf.Sign(angularDistance);

            Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, Vector3.up);
            transform.rotation *= rotationToApply;
        }
    }

    private void HandlePlaneMovement()
    {
        Vector2 rawMove = playerMove.action.ReadValue<Vector2>();
        dirMov = (Vector3.right * rawMove.x) + (Vector3.forward * rawMove.y);
        dirMov.Normalize();

        switch (movementMode)
        {
            
            case MovementMode.RelativeToCharacter:
            {
                Vector3 velocity = dirMov * moveSpeed;
                //chC.Move(velocity * Time.deltaTime);

                velocityToApply = velocity;

                break;
            }

            case MovementMode.RelativeToCamera:
            {
                Transform cameraTransform = Camera.main.transform;
                Vector3 cameraMoveDir = cameraTransform.TransformDirection(dirMov);

                float originalMagnitude = cameraMoveDir.magnitude;
                cameraMoveDir = Vector3.ProjectOnPlane(cameraMoveDir, Vector3.up).normalized * originalMagnitude;

                Vector3 velocity = cameraMoveDir * moveSpeed;
                //chC.Move(velocity * Time.deltaTime);
                velocityToApply = velocity;
            }

            break;
        }
    }

    private void HandleVerticalMovement()
    {
        if (chC.isGrounded)
        verticalVelocity = 0f;

        verticalVelocity += Gravity * Time.deltaTime;

        velocityToApply += verticalVelocity * Vector3.up;
    }
    
    private void OnDisable() 
    {
        playerMove.action.Disable();
    }
}
