using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Image expBar;
    [SerializeField] private TextMeshProUGUI experienceText = default;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider reloadSlider = default;
    [SerializeField] private GameObject perfectReloadUI = default;
    [Space(10)]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private GameObject statContainerUI = default;
    [SerializeField] private GameObject statContainerLayout = default;
    [SerializeField] private Dictionary<Stat, StatContainerUI> statUIPairs = new();
    [Space(10)]
    [SerializeField] private List<Button> skillTreeButtons = new();

    private void Start()
    {
        InstantiateStatScreen();
    }

    public void UpdateHealthBar(float baseAmount, float currentAmount) 
    {
        healthBar.fillAmount = currentAmount / baseAmount;
    }

    public void UpdateExpBar(float baseAmount, float currentAmount)
    {
        expBar.fillAmount = currentAmount / baseAmount;
    }

    public void UpdateLevelUpUI()
    {
        levelText.text = "Level " + playerStats.GetComponent<PlayerLeveling>().GetCurrentLevel();
    }

    public void UpdateSkillTreeUI()
    {
        foreach (Button button in skillTreeButtons) 
        {
            button.interactable = !playerStats.GetComponent<PlayerLeveling>().IsSkillUnlocked(button.gameObject.GetComponent<SkillEditorComponent>().skill);
        }
    }

    public void UpdateReloadUI(bool setAcitve,float _value, bool perfectReloadPossible, float perfectReloadStart) 
    {
        reloadSlider.gameObject.SetActive(setAcitve);
        reloadSlider.value = _value;
        perfectReloadUI.SetActive(perfectReloadPossible);
        perfectReloadUI.GetComponent<RectTransform>().localPosition = new Vector2((160 * perfectReloadStart) - 80, 0);
    }

    public void InstantiateStatScreen() 
    {
        int statCount = playerStats.statList.Count;
        for (int i = 0; i < statCount; i++) 
        {
            GameObject statContainerUIobject = Instantiate(statContainerUI, statContainerLayout.transform);
            statUIPairs.Add(playerStats.statList[i], statContainerUIobject.GetComponent<StatContainerUI>());
        }
        UpdateStatUI();
    }

    public void UpdateStatUI() 
    {
        foreach (var item in statUIPairs)
        {
            item.Value.UpdateStatInfo(item.Key.Name, item.Key.CalculatedStat.ToString());
        }
    }

    public void UpdatePostGameUI() 
    {
        experienceText.text = "Experience Collected: " + playerStats.GetComponent<PlayerLeveling>().GetCurrentExperience();
    }
}