using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AgentsTest.Core.UI
{
    public class EntitiesMonitorRecord : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textMeshValue;

        public void SetNewValue(int newValue)
        {
            _textMeshValue.text = newValue.ToString();
        }
    }
}