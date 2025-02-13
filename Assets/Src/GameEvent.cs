using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CursedDimension
{
    [CreateAssetMenu(fileName = "GameEvent", menuName ="Scriptables/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        
        [SerializeField] private List<GameEventListener> listeners = new List<GameEventListener>();
        public void Raise()
        {
            for (int i = listeners.Count - 1; i >= 0; i--) 
            {
                listeners[i].OnEventRaised();
            }
        }
        
        public void RegisterListener(GameEventListener listener)
        {
            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);
            }
        }
        public void UnregisterListener(GameEventListener listener)
        {
            if (listeners.Contains(listener))
            {
                listeners.Remove(listener);
            }
        }
    }
}
