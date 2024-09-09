using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CursedDimension
{
    public class CallAfterDelay : MonoBehaviour
    {
        float delay;
        System.Action action;
        public static CallAfterDelay Create(float delay, System.Action action)
        {
            CallAfterDelay cad = new GameObject("CallAfterDelay").AddComponent<CallAfterDelay>();
            cad.delay = delay;
            cad.action = action;
            return cad;
        }
        float age;
        void Update()
        {
            if (age > delay)
            {
                action();
                Destroy(gameObject);
            }
        }
        void LateUpdate() { age += Time.deltaTime; }
    }
}
