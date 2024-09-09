using UnityEngine;

namespace CursedDimension
{
    public abstract class PowerDependant: MonoBehaviour
    {

        [SerializeField] protected float energyCost;
        [SerializeField] protected PowerSystem powerSystem;
        protected bool isPowerOn = true;
        public abstract void OnPowerOutage();
    }
}
