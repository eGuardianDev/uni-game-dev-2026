using System.Collections;
using UnityEngine;

public class TurretAI : Enemy
{   
    [Header("Lazer attack")]
    [SerializeField] private LineRenderer lineRenderer_;
    [SerializeField] private float warningDuration_ = 1f;
    [SerializeField] private float laserWidth_ = 0.1f;
    [SerializeField] private Animator animator;
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


    override protected void AfterStart()
    {
        animator = GetComponent<Animator>();
        
    }
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
        animator.SetBool("Attacking", true);

        Vector2 origin = spawnPoint_.position;
        Vector2 dir = ((Vector2)player_script_.transform.position - origin).normalized;
        Vector2 endPoint = origin + dir * 50f;

        lineRenderer_.enabled = true;
        lineRenderer_.startWidth = laserWidth_;
        lineRenderer_.endWidth = laserWidth_;
        lineRenderer_.SetPosition(0, origin);
        lineRenderer_.SetPosition(1, endPoint);
        lineRenderer_.startColor = new Color(1f, 0f, 0f, 0.3f);
        lineRenderer_.endColor   = new Color(1f, 0f, 0f, 0.3f);

        yield return new WaitForSeconds(warningDuration_);

        lineRenderer_.startColor = Color.white;
        lineRenderer_.endColor   = Color.white;

        float activeDuration = 0.2f;
        float elapsed = 0f;
        float tickRate = 0.1f;
        float tickTimer = 0f;

        bool flag = false;
        while (elapsed < activeDuration)
        {
            elapsed += Time.deltaTime;
            tickTimer += Time.deltaTime;

            RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, new Vector2(laserWidth_, laserWidth_), 0f, dir, 50f);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject == gameObject) continue;
                if (hit.collider.CompareTag("Player") && tickTimer >= tickRate)
                {
                    player_script_.GetDamage(damage_);
                    tickTimer = 0f;
                    flag = true;
                    break;
                }
            }
            if(flag) break;

            yield return null;
        }

        animator.SetBool("Attacking", false);
        lineRenderer_.enabled = false;
    }
    
}
