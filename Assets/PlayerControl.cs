using System;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace CursedDimension
{
    public class PlayerControl : MonoBehaviour
    {
        private const int LOW_BOUND = -90, HIGH_BOUND = 90;
        [Header("References")]
        [SerializeField] private Rigidbody rigidBody;
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform feet;
        [Space]
        [SerializeField] private Transform frontGroundCheck;
        [SerializeField] private Transform centerGroundCheck;
        [SerializeField] private Transform backGroundCheck;
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
        [SerializeField] private float bobAmountX;
        private Vector3 direction;
        private Vector2 moveData, lookData;
        private Vector3 groundNormal;
        private bool isOnGround = true;

        private float speedMultiplier = 1f;
        private float xRot, yRot;
        private float timer;
        private float cameratDefaultLocalY, cameratDefaultLocalX;

        public float gravityRotationSpeed = 9.81f;
        private float turnSpeed = 2f;

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            cameratDefaultLocalY = playerCamera.transform.localPosition.y;
            cameratDefaultLocalX = playerCamera.transform.localPosition.x;
            groundNormal = transform.up;
        }


        private void Update()
        {
            GroundCheck();
            RotateCamera();
            TakeInput();
            HeadBob();
        }


        private void HeadBob()
        {
            if (!isOnGround) return;

            if (rigidBody.velocity.magnitude > 0.1)
            {
                timer += Time.deltaTime * bobbingSpeed;
                playerCamera.transform.localPosition = new Vector3
                (
                    cameratDefaultLocalX + Mathf.Cos(timer)/2 * bobAmountX,
                    cameratDefaultLocalY + Mathf.Sin(timer) * bobAmountY,
                    playerCamera.transform.localPosition.z
                );;
            }
        }

        private void FixedUpdate()
        {
            /*Vector3 targetGroundNormal = GetGroundNormal();
            groundNormal = Vector3.Lerp(groundNormal, targetGroundNormal, Time.fixedDeltaTime * gravityRotationSpeed);
            RotateSelf(targetGroundNormal);
            RotateMesh(direction);*/
            ApplyForceToReachVelocity(direction * moveSpeed * speedMultiplier, force);
        }
        private void RotateSelf(Vector3 targetNormal)
        {
            Vector3 lerpDir = Vector3.Lerp(transform.up, targetNormal, Time.fixedDeltaTime * gravityRotationSpeed);
            transform.rotation = Quaternion.FromToRotation(transform.up, lerpDir) * transform.rotation;
        }
        void RotateMesh(Vector3 dir)
        {
            if (direction == Vector3.zero) dir = transform.forward;
            Quaternion SlerpRot = Quaternion.LookRotation(dir, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, SlerpRot, turnSpeed * Time.fixedDeltaTime);
        }
        private Vector3 GetGroundNormal()
        {
            RaycastHit frontHit, centerHit, backHit;
            Physics.Raycast(frontGroundCheck.position, -frontGroundCheck.up, out frontHit, 10f, walkableLayer);
            Physics.Raycast(centerGroundCheck.position, -centerGroundCheck.up, out centerHit, 10f, walkableLayer);
            Physics.Raycast(backGroundCheck.position, -backGroundCheck.up, out backHit, 10f, walkableLayer);

            Vector3 currentNormal = transform.up;
            if (frontHit.transform != null) currentNormal += frontHit.normal;
            if (centerHit.transform != null) currentNormal += centerHit.normal;
            if (backHit.transform != null) currentNormal += backHit.normal;

            return currentNormal.normalized;
        }
        private void TakeInput()
        {
            direction = transform.forward * moveData.y + transform.right * moveData.x;
        }
        private void RotateCamera()
        {
            xRot += lookData.x * cameraSensitivity;
            yRot += lookData.y * cameraSensitivity;
            yRot = Mathf.Clamp(yRot, LOW_BOUND, HIGH_BOUND);
            transform.localRotation = Quaternion.AngleAxis(xRot, Vector3.up);
            playerCamera.transform.localRotation = Quaternion.AngleAxis(yRot, Vector3.left);

        }
        private void GroundCheck()
        {
            isOnGround = Physics.CheckSphere(feet.position, 0.05f, walkableLayer);
            speedMultiplier = isOnGround? 1.0f : 0.5f;
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
