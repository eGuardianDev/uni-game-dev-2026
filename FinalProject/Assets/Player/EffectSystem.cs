using System.Data;
using System.Collections.Generic;
using UnityEngine;
public class EffectSystem : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    [SerializeField] public List<Effect> effects;
    [SerializeField] List<EffectDataDisplay> displayerImages;
    [SerializeField] GameObject displayerImagesHolder;
    [SerializeField] GameObject effectDisplayerPrefab;
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
        int invicibility = playerScript.Invicible_default;

        for(int i =0;i<effects.Count;++i)
        // foreach (Effect effect in effects)
        {
            Effect effect = effects[i];
            if (effect.icon_displayer == null)
            {
                GameObject instance = Instantiate(effectDisplayerPrefab, displayerImagesHolder.transform);
                effect.icon_displayer = instance.GetComponent<EffectDataDisplay>();
                effect.icon_displayer.effect_index_ = i;
                displayerImages.Add(effect.icon_displayer);
                effect.icon_displayer.effectObject = effect.gameObject;
            }
            foreach (var mod in effect.modifiers)
            {
                ref int stat = ref GetStatRef(mod.stat, ref health, ref attackSpeed, ref mana, ref movementSpeed, ref invicibility);
                if (mod.modifierType == ModifierType.Percent)
                    stat = (int)(stat * (1 + mod.amount / 100f));
                else
                    stat += mod.amount;
            }
        }

        for (int i = displayerImages.Count - 1; i >= 0; i--)
        {
            if (displayerImages[i].effectObject == null)
            {
                GameObject gm = displayerImages[i].gameObject;
                displayerImages.RemoveAt(i);
                Destroy(gm);
            }
        }

        playerScript.final_max_health_ = health;
        playerScript.GetHealth();
        playerScript.final_attack_speed_ = attackSpeed;
        playerScript.final_max_mana_ = mana;
        playerScript.GetMana();

        playerScript.final_movement_speed_ = movementSpeed;
        playerScript.Invicible = invicibility;

        playerScript.update_HUD();
    }


    public void AddEffect(GameObject effect)
    {
        GameObject ef = Instantiate(effect, transform.position, transform.rotation, transform);
        effects.Add(ef.GetComponent<Effect>());
    }
    ref int GetStatRef(StatType type, ref int health, ref int attackSpeed, ref int mana, ref int movementSpeed, ref int invicibility)
    {
        switch (type)
        {
            case StatType.Health:        return ref health;
            case StatType.AttackSpeed:   return ref attackSpeed;
            case StatType.Mana:          return ref mana;
            case StatType.MovementSpeed: return ref movementSpeed;
            case StatType.Invicible:     return ref invicibility;
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
                Destroy(effects[i].gameObject);
                effects.RemoveAt(i);
                
            }
        }

        for(int i = 0; i< displayerImages.Count; ++i)
        {
            displayerImages[i].effect_index_ = i;
        }


        RecomputeStats();
    }
}
