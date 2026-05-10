using UnityEngine;

public class BombAbility : Ability
{
    public GameObject bombPrefab;
    protected override bool isCastable()
    {
        return true; 
    }

    protected override bool OnCast()
    {
        GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);

        Explosion_particles_friendly part = bomb.GetComponentInChildren<Explosion_particles_friendly>();
        part.damage = this.Damage;
        return true;
    }
}
