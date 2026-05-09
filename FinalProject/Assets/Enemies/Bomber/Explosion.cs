using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;

    private bool hasDealtDamage_ = false;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player") && !hasDealtDamage_)
        {
            other.GetComponent<PlayerScript>().GetDamage(enemy.Damage);
            hasDealtDamage_ = true;
        }
    }
}
