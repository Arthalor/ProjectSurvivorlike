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
    }
}
