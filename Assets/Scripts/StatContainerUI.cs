using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatContainerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statNameText = default;
    [SerializeField] private TextMeshProUGUI statValueText = default;

    public void UpdateStatInfo(string name, string value) 
    {
        statNameText.text = name;
        statValueText.text = value;
    }
}