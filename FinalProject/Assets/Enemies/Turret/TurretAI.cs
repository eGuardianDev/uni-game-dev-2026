using System.Collections;
using UnityEngine;

public class TurretAI : Enemy
{   
    [Header("Lazer attack")]
    [SerializeField] private LineRenderer lineRenderer_;
    [SerializeField] private float warningDuration_ = 1f;
    [SerializeField] private float laserWidth_ = 0.1f;
    [SerializeField] private Transform spawnPoint_;

    // override protected void AttackPlayer()
    // {
    //     attackTimer_ -= Time.deltaTime;
    //     if (attackTimer_ <= 0f)
    //     {
    //         isDashing_ = false;
    //         player_script_.GetDamage(damage_);
    //         attackTimer_ = 1f / attack_speed_;
    //     }
    // }

    override protected void MoveTowardsPlayer()
    {
       
    }

    override protected void AttackPlayer()
    {
        // attackTimer_ -= Time.deltaTime;
        if (attackTimer_ <= 0f)
        {
            StartCoroutine(LaserSequence());
            attackTimer_ = 1f / attack_speed_;
        }
    }
    private IEnumerator LaserSequence()
    {
        Vector2 origin = spawnPoint_.position;
        Vector2 dir = ((Vector2)player_script_.transform.position - origin).normalized;

        Vector2 endPoint = origin + dir * 50f;

        lineRenderer_.enabled = true;

        lineRenderer_.startWidth = laserWidth_;
        lineRenderer_.endWidth = laserWidth_;
        lineRenderer_.startColor = new Color(1f, 0f, 0f, 0.3f);
        lineRenderer_.endColor   = new Color(1f, 0f, 0f, 0.3f);
        lineRenderer_.SetPosition(0, origin);
        lineRenderer_.SetPosition(1, endPoint);

        yield return new WaitForSeconds(warningDuration_);

        lineRenderer_.startColor = Color.white;
        lineRenderer_.endColor   = Color.white;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, dir, 50f);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.CompareTag("Player"))
            {
                player_script_.GetDamage(damage_);
            }
        }

        yield return new WaitForSeconds(0.2f);
        lineRenderer_.enabled = false;
    }
    
}
