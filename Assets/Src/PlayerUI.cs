using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CursedDimension
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _prompt;

        public void ShowPrompt(bool isVisible)
        {
            _prompt.gameObject.SetActive(isVisible);
        }
    }
}
