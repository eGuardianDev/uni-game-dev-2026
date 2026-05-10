using UnityEngine;

public class SpeedBoostAb : Ability
{
    public GameObject speedBoost;

    protected override bool isCastable()
    {
        return true; 
    }

    protected override bool OnCast()
    {
        this.player_script_.gameObject.GetComponent<EffectSystem>().AddEffect(speedBoost);

        return true;
    }
}
