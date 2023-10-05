using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeveling : MonoBehaviour
{
    [SerializeField] private int experience = 0;
    [SerializeField] private int experienceToNextLevel = 0;
    [SerializeField] private int level = 0;
    [Space]
    [SerializeField] private InGameUI inGameUI = default;
    [SerializeField] private Inventory inventory = default;
    [Space]
    [SerializeField] private List<Item> LevelUpgrades = default;

    private void Update()
    {
        AttractExperience();
    }

    public void AttractExperience()
    {
        Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(transform.position, 5f);
        foreach (Collider2D collider in collidersInRange)
        {
            if (collider.TryGetComponent(out ExperienceBehaviour expBehaviour))
            {
                expBehaviour.PickUp();
            }
        }
    }

    public void PickUpExperience(int amount)
    {
        experience += amount;
        experienceToNextLevel += amount;
        inGameUI.UpdateExpBar(ExperienceNeededToNextLevel(), experienceToNextLevel);

        CheckForLevelUp();
    }

    public int ExperienceNeededToNextLevel()
    {
        return 3 + level;
    }

    public void CheckForLevelUp() 
    {
        if (experienceToNextLevel < ExperienceNeededToNextLevel()) return;

        level++;
        experienceToNextLevel = 0;
        LevelUpEvent();
    }

    void LevelUpEvent() 
    {
        GameManager.Instance.gamePlayManager.LevelUpEvent();
    }

    public void LevelUpgrade(int type) 
    {
        inventory.PickUpItem(LevelUpgrades[type]);
    }

    public int GetCurrentLevel() 
    {
        return level;
    }

    public int GetCurrentExperience() 
    {
        return experience;
    }
}
