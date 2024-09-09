using UnityEngine;
using UnityEngine.InputSystem;

namespace CursedDimension
{
    public class PlayerControl : MonoBehaviour
    {
        private const int LOW_BOUND = -90, HIGH_BOUND = 90;
        [Header("References")]
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Animator animator;
        [SerializeField] private Transform feet;
        [Header("Walkable Layers")]
        [SerializeField] private LayerMask walkableLayer;
        [Header("Control Settings")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float force;
        [SerializeField] private float jumpForce;
        [SerializeField] private float cameraSensitivity;
        [Header("Bobbing Settings")]
        [SerializeField] private float bobbingSpeed;
        [SerializeField] private float bobAmountY;
        [Header("Interaction")]
        [SerializeField] private Flashlight handObject;

        private Vector3 direction;
        private Vector2 moveData, lookData;
        private bool isOnGround = true;
        private bool isSprinting = false;

        #region Speed Variables
        private float speedMultiplier = 1f;
        private float sprintScale = 1.35f;
        private float normalMoveScale = 1f;
        private float slowScale = 0.5f;

        private float cameraRotationSpeedLimit = 5;
        private float currentBobbingSpeed;
        #endregion

        private float xRotation, yRotation;
        private float headBobbingTimer;
        private float cameratDefaultLocalY;

        public Camera PlayerCamera => playerCamera;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cameratDefaultLocalY = playerCamera.transform.localPosition.y;
            currentBobbingSpeed = bobbingSpeed;
        }


        private void Update()
        {
            GroundCheck();
            SprintCheck();
            AnimationCheck();
            RotateCamera();
            RotateHandObject();
            CalculateMoveInput();
            HeadBob();
        }
      

        private void HeadBob()
        {
            if (!isOnGround) return;

            if (rigidBody.velocity.magnitude > 0.1)
            {
                headBobbingTimer += Time.deltaTime * currentBobbingSpeed;
                playerCamera.transform.localPosition = new Vector3
                (
                    playerCamera.transform.localPosition.x,
                    cameratDefaultLocalY + Mathf.Sin(headBobbingTimer) * bobAmountY,
                    playerCamera.transform.localPosition.z
                );;
            }
        }

        private void FixedUpdate()
        {
            ApplyForceToReachVelocity(direction * moveSpeed * speedMultiplier, force);
        }

       
        private void CalculateMoveInput()
        {
            direction = transform.forward * moveData.y + transform.right * moveData.x;
        }
        private void RotateHandObject()
        {
            Quaternion rot = Quaternion.Euler
                (
                    -yRotation,
                    handObject.transform.localEulerAngles.y,
                    handObject.transform.localEulerAngles.z
                );
            handObject.transform.localRotation = Quaternion.Slerp
                (
                    handObject.transform.localRotation,
                    rot, 
                    Time.deltaTime * 10
                );
        }
        private void RotateCamera()
        {
            float xRotChange = Mathf.Clamp(lookData.x * cameraSensitivity, -cameraRotationSpeedLimit, cameraRotationSpeedLimit);
            float yRotChange = Mathf.Clamp(lookData.y * cameraSensitivity, -cameraRotationSpeedLimit, cameraRotationSpeedLimit);
            xRotation += xRotChange;
            yRotation += yRotChange;

            yRotation = Mathf.Clamp(yRotation, LOW_BOUND, HIGH_BOUND);
            
            Quaternion camRot =
                Quaternion.Euler(-yRotation, playerCamera.transform.localEulerAngles.y, playerCamera.transform.localEulerAngles.z);
            Quaternion bodyRot = 
                Quaternion.Euler(rigidBody.rotation.eulerAngles.x, xRotation, rigidBody.rotation.eulerAngles.z);

            rigidBody.rotation = Quaternion.Slerp(rigidBody.rotation,bodyRot, Time.deltaTime * 10);
            playerCamera.transform.localRotation = Quaternion.Slerp(playerCamera.transform.localRotation,camRot, Time.deltaTime * 10);

        }
        private void AnimationCheck()
        {
            bool isMoving = moveData.magnitude > 0;
            animator.SetBool("isWalking", isMoving && !isSprinting);
            animator.SetBool("isRunning", isMoving && isSprinting);
        }
        private void GroundCheck()
        {
            isOnGround = Physics.CheckSphere(feet.position, 0.1f, walkableLayer);
            
            if (!isOnGround ) 
            { 
                speedMultiplier = slowScale;
            }
        }
        private void SprintCheck()
        {
            if (isOnGround && isSprinting)
            {
                speedMultiplier = sprintScale;
                currentBobbingSpeed = bobbingSpeed * sprintScale;
            }
            else if(isOnGround)
            {
                speedMultiplier =normalMoveScale;
                currentBobbingSpeed = bobbingSpeed;
            }
       

        }
        public void OnMove(InputAction.CallbackContext context)
        {
            moveData = context.ReadValue<Vector2>();
        }
        public void OnLook(InputAction.CallbackContext context) 
        {
            lookData = context.ReadValue<Vector2>();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if(isOnGround && context.performed)
            {
                rigidBody.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }
        public void OnFlashlightTriggered(InputAction.CallbackContext context)
        {
            handObject.TriggerFlashlight();
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            if(isOnGround)
            {
                isSprinting = context.started || context.performed;

            }
        }
        public void ApplyForceToReachVelocity(Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
        {
            if (force == 0 || velocity.magnitude == 0)
                return;

            velocity += 0.2f * rigidBody.drag * velocity.normalized;

            force = Mathf.Clamp(force, -rigidBody.mass / Time.fixedDeltaTime, rigidBody.mass / Time.fixedDeltaTime);

            if (rigidBody.velocity.magnitude == 0)
            {
                rigidBody.AddForce(velocity * force, mode);
            }
            else
            {
                var velocityProjectedToTarget = (velocity.normalized * Vector3.Dot(velocity, rigidBody.velocity) / velocity.magnitude);
                rigidBody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
            }
        }
    }
}
