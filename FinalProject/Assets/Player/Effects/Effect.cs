using UnityEngine;
using System.Collections.Generic;
public enum StatType
{
    Health,
    AttackSpeed,
    Mana,
    MovementSpeed
}

public enum ModifierType
{
    Flat,
    Percent
}
[System.Serializable]
public struct StatModifier
{
    public StatType stat;
    public ModifierType modifierType;
    public int amount;
}
public class Effect : MonoBehaviour
{

    public Sprite icon;
    public string name;
    public string description;
    public float cooldown;
    public List<StatModifier> modifiers;

}
