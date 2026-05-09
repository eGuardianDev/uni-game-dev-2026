using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class SpiderAI : Enemy
{

    [SerializeField] protected float wait_before_attacK_timer_ = 3f;
    private Vector2 _dashTarget;
    [SerializeField] private bool isDashing_ = false;
    float wait_before_attack_ = 0f;
    override protected void Update()
    {
        if(gm_Behavior_.Is_Paused) return;
        
        if(!Player_in_room) return;

        attackTimer_ -= Time.deltaTime;
        if (!isDashing_)
        {
            wait_before_attack_ -= Time.deltaTime;
        }
        if(wait_before_attack_ > 0) return;

        if (player_ == null) return;

        MoveTowardsPlayer();
    }
    // override protected void Update()
    // {
    //     if(gm_Behavior_.Is_Paused) return;
        
    //     if(!Player_in_room) return;

    //     attackTimer_ -= Time.deltaTime;
    //     if (!isDashing_)
    //     {
    //         wait_before_attack_ -= Time.deltaTime;
    //     }

    //     if(wait_before_attack_ > 0) return;
    //     if (player_ == null) return;

    //     if (engage_ && !IsInAttackRange())
    //     {
    //         MoveTowardsPlayer();
    //     }
    //     if (engage_ && IsInAttackRange() && !IsAttackOnCooldown)
    //     {
    //         isDashing_ = false;
    //         wait_before_attack_ = wait_before_attacK_timer_;
    //     }
    //     if (isDashing_ && !IsAttackOnCooldown)
    //     {
    //         MoveTowardsPlayer();
    //         return;
    //     }
    //     if (engage_ && IsInAttackRange())
    //     {
    //         isDashing_ = false;
    //         wait_before_attack_ = wait_before_attacK_timer_;
    //         AttackPlayer();
    //     }
        
    //     if (Detectplayer_IsClose())
    //     {
    //         engage_ = true;
    //     }
    // }
    override protected void AttackPlayer()
    {
        attackTimer_ -= Time.deltaTime;
        if (attackTimer_ <= 0f)
        {
            isDashing_ = false;
            player_script_.GetDamage(damage_);
            attackTimer_ = 1f / attack_speed_;
        }
    }

    override protected void MoveTowardsPlayer()
    {
        if (!isDashing_)
        {
            Vector2 dir = ((Vector2)player_.position - (Vector2)transform.position).normalized;
            float overshoot = Random.Range(1, 5);
            _dashTarget = (Vector2)player_.position + dir * overshoot;
            isDashing_ = true;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            _dashTarget,
            movement_speed_ * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, _dashTarget) < 0.1f)
        {   
            isDashing_ = false;
            wait_before_attack_ = wait_before_attacK_timer_;
        }

    }

}
