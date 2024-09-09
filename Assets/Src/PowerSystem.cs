using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CursedDimension
{
    public class PowerSystem : MonoBehaviour
    {

        [SerializeField] private float charge, drain;
        [SerializeField] private GameEvent powerOutageEvent;

        private void Update()
        {
            charge -= drain* Time.deltaTime;
        
            if (charge <= 0)
            {
                charge = 0;
                powerOutageEvent.Raise();
                enabled = false;
            }
        }
        public void UpdateDrain(float cost)
        {
            drain += cost;
        }
        private void OnGUI()
        {
            var style = GUI.skin.GetStyle("label");
            style.fontSize = 40;
            GUILayout.Label(charge.ToString(), style);
        }
    }
}
