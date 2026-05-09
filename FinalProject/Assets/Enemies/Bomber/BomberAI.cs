using UnityEngine;
using System.Collections;

public class BomberAI : Enemy
{
    [SerializeField] private float detonationTimer = 3f;
    [SerializeField] protected ParticleSystem explode_particles_;

    private bool detonating =  false;
    
    override protected void MoveTowardsPlayer()
    {

        if(detonating) return;

        transform.position = Vector3.MoveTowards(
            transform.position,
            player_.position,
            movement_speed_ * Time.deltaTime
        );
       
    }
    override protected void AttackPlayer()
    {
        // attackTimer_ -= Time.deltaTime;
        if (attackTimer_ <= 0f)
        {
            detonating = true;
            StartCoroutine(Detonate());
            attackTimer_ = 1f / attack_speed_;
        }
    }
    private IEnumerator Detonate()
    {
        this.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(detonationTimer);
        Explode(); 
    }

    private void Explode()
    {
        explode_particles_.transform.SetParent(null); 
        explode_particles_.Emit(25);
        Destroy(explode_particles_.gameObject, 2f); 
        Destroy(gameObject);
    }
}
