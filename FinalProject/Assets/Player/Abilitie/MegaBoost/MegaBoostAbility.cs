using UnityEngine;

public class MegaBoostAbility : Ability
{

    public GameObject healEffect;
    protected override bool isCastable()
    {
        return true; 
    }

    protected override bool OnCast()
    {
        this.player_script_.gameObject.GetComponent<EffectSystem>().AddEffect(healEffect);
        return true;
    }
}