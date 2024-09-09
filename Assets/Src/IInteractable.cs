using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CursedDimension
{
    public interface IInteractable 
    {
        public void Interact(Interactor interactor);
    }
}
