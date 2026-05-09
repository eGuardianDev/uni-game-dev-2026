using System.Data;
using System.Collections.Generic;
using UnityEngine;
public class EffectSystem : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    [SerializeField] List<Effect> effects;


    void Start()
    {
        playerScript = GetComponent<PlayerScript>();
        RecomputeStats();
    }

    void RecomputeStats()
    {
        int health = playerScript.max_health_;
        int attackSpeed = playerScript.attack_speed_;
        int mana = playerScript.max_mana_;
        int movementSpeed = playerScript.movement_speed_;

        foreach (var effect in effects)
        {
            foreach (var mod in effect.modifiers)
            {
                ref int stat = ref GetStatRef(mod.stat, ref health, ref attackSpeed, ref mana, ref movementSpeed);
                if (mod.modifierType == ModifierType.Percent)
                    stat = (int)(stat * (1 + mod.amount / 100f));
                else
                    stat += mod.amount;
            }
        }

        playerScript.final_max_health_ = health;
        playerScript.GetHealth(0);
        playerScript.final_attack_speed_ = attackSpeed;
        playerScript.final_max_mana_ = mana;
        playerScript.FillMana(0);
        playerScript.final_movement_speed_ = movementSpeed;

        playerScript.update_HUD();
    }

    ref int GetStatRef(StatType type, ref int health, ref int attackSpeed, ref int mana, ref int movementSpeed)
    {
        switch (type)
        {
            case StatType.Health:        return ref health;
            case StatType.AttackSpeed:   return ref attackSpeed;
            case StatType.Mana:          return ref mana;
            case StatType.MovementSpeed: return ref movementSpeed;
            default: return ref health;
        }
    }
    void Update()
    {

        for (int i = effects.Count - 1; i >= 0; i--)
        {
            effects[i].cooldown -= Time.deltaTime;
            if (effects[i].cooldown <= 0)
            {
                Destroy(effects[i]);
                effects.RemoveAt(i);
            }
        }

        RecomputeStats();
    }
}
