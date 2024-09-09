using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CursedDimension
{
    public class SecurityCamera : MonoBehaviour
    {
        [SerializeField] private Transform cameraItself;
        [SerializeField] private float rotationAngle;
        [SerializeField] private float duration;

        private Quaternion initialRotatin;
        private float timer;

        void Start()
        {
            timer = 0f;
        }

        void Update()
        {
            timer += Time.deltaTime;
            float phase = Mathf.PingPong(timer / duration, 1f);
            float angle = Mathf.Lerp(-rotationAngle, rotationAngle, phase);
            cameraItself.localRotation = Quaternion.Euler
                (
                    cameraItself.eulerAngles.x, 
                    angle,
                    cameraItself.eulerAngles.z
                );
        }

    }
}
