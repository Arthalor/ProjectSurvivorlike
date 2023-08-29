using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image expBar;

    public void UpdateHealthBar(float baseAmount, float currentAmount) 
    {
        healthBar.fillAmount = currentAmount / baseAmount;
    }

    public void UpdateExpBar(float baseAmount, float currentAmount)
    {
        expBar.fillAmount = currentAmount / baseAmount;
    }
}