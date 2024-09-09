using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CursedDimension
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private PlayerUI playerUI;
        [SerializeField] private float rayLength;
        private IInteractable currentInteractable;
        public PlayerUI PlayerUI => playerUI;

        private void Update()
        {
            Ray r = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            if (Physics.Raycast(r, out RaycastHit hit, rayLength))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactee))
                {
                    if (interactee != currentInteractable)
                    { 
                        currentInteractable = interactee;
                        playerUI.ShowPrompt(true); 
                    }
                }
                else
                {
                    if (currentInteractable != null)
                    {
                        currentInteractable = null;
                        playerUI.ShowPrompt(false);
                    }
                }
            }
            else
            {
                if (currentInteractable != null)
                {
                    currentInteractable = null;
                    playerUI.ShowPrompt(false);
                }
            }
        }
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (currentInteractable != null) 
            {
                currentInteractable.Interact(this);
            }
        }

    }
}
