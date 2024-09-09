using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace CursedDimension
{
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private Light lightSource;
        [SerializeField] private float flashLightPower;
        private bool isOn = false;
        public void TriggerFlashlight()
        {
            if (isOn)
                lightSource.intensity = 0;
            else
                lightSource.intensity = flashLightPower;
            isOn = !isOn;
        }
    }
}
