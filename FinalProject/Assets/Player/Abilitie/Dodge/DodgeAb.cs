using UnityEngine;

public class DodgeAb : Ability
{
    public GameObject dodgeEffect;
    protected override bool isCastable()
    {
        return true; 
    }

    protected override bool OnCast()
    {
        this.player_script_.gameObject.GetComponent<EffectSystem>().AddEffect(dodgeEffect);
        return true;
    }
}
