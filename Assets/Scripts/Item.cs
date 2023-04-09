using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = ("ScriptableObject/Item"), order = 3)]
public class Item : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public List<StatModifier> modifiers { get; private set; }
}