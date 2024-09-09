using UnityEngine;
using UnityEngine.Events;

namespace CursedDimension
{
    public class PushButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private UnityEvent OnPressed;
        
        public void Interact(Interactor interactor)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.5f)
            {
                OnPressed?.Invoke();
            }
        }
    }
}
