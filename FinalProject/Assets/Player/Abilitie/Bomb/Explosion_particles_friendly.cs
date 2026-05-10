using UnityEngine;

public class Explosion_particles_friendly : MonoBehaviour
{

    private bool hasDealtDamage_ = false;
    public int damage = 0;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy") && !hasDealtDamage_)
        {
            other.GetComponent<Enemy>().GetDamage(damage);
            // hasDealtDamage_ = true;
        }
    }
}
