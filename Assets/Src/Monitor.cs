using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CursedDimension
{
    public class Monitor : MonoBehaviour
    {
        [SerializeField] private MeshRenderer screenRenderer;
        [SerializeField] private Material cameraDisplay, TVnoise;
        [SerializeField] private List<SecurityCamera> cameraList;
        [SerializeField] private SecurityCamera currentCamera;
        private int currentIndex = 0;
        
        
        private void ShowTVNoise()
        {
            ChangeMaterial(TVnoise);
            CallAfterDelay.Create(0.2f, () => ChangeMaterial(cameraDisplay) );
        }
        private void ChangeMaterial(Material m) { screenRenderer.material = m; }
        public void SwitchToNextCamera()
        {
            currentIndex = (currentIndex + 1) % cameraList.Count;
            foreach (var camera in cameraList) 
            { 
                camera.gameObject.SetActive(false);
            }
            currentCamera = cameraList[currentIndex];
            currentCamera.gameObject.SetActive(true);
            ShowTVNoise();
        }
        public void SwitchToPreviousCamera()
        {
            currentIndex = (currentIndex - 1 + cameraList.Count) % cameraList.Count;
            foreach (var camera in cameraList)
            {
                camera.gameObject.SetActive(false);
            }
            currentCamera = cameraList[currentIndex];
            currentCamera.gameObject.SetActive(true);
            ShowTVNoise();
        }
    }
}
