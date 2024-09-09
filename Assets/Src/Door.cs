    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    namespace CursedDimension
    {
        public class Door : PowerDependant
        {
            [SerializeField] private float distance, speed;
            [SerializeField] private Vector3 direction;
            private Vector3 startPosition;
            private bool isOpen = true;
            private float travelTime;
            private float time = 0;
            private void Awake()
            {
                startPosition = transform.position;
                travelTime = distance / speed;
            }
            public override void OnPowerOutage()
            {
                if (!isOpen)
                {
                    StartCoroutine(TriggerDoor());
                }
                isPowerOn = false;
            }

            public void UseDoor()
            {
                if (!isPowerOn || (time > 0 && time < 1)) return;

                StartCoroutine(TriggerDoor());
            }

            private IEnumerator TriggerDoor()
            {
                Vector3 targetPosition = isOpen ? (startPosition + direction * distance) : (startPosition);
                Vector3 currentPosition = transform.position;
        
                time = 0;
                isOpen = !isOpen;
                powerSystem.UpdateDrain(isOpen ? -energyCost : energyCost);
                while (time < 1)
                {
                    time += Time.deltaTime / travelTime;
                    if (time > 1) time = 1;
                    transform.position = Vector3.Lerp(currentPosition, targetPosition, time);
                    yield return null;
                }
            }

        }
    }
