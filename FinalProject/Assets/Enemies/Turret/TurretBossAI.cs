using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretBossAI : Enemy
{    
    [Header("Laser Attack")]
    [SerializeField] private LineRenderer lineRenderer_;
    [SerializeField] private float warningDuration_ = 1f;
    [SerializeField] private float laserWidth_ = 0.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform spawnPoint_;

    [Header("Boss Phases")]
    [SerializeField] private int phase_ = 1;
    [SerializeField] private int phase2_health_threshold_ = 66;
    [SerializeField] private int phase3_health_threshold_ = 33;

    [Header("Phase 2 - Rotating Laser")]
    [SerializeField] private float rotation_speed_ = 90f;       
    [SerializeField] private float phase2_laser_duration_ = 3f; 

    [Header("Phase 3 - Tracking Laser")]
    [SerializeField] private float phase3_laser_duration_ = 2f;
    [SerializeField] private float tracking_speed_ = 5f; 

    [Header("Boss UI")]
    [SerializeField] private Slider health_slider_;
    [SerializeField] private GameObject health_bar_;

    private int maxHealth_;
    private bool transitioningPhase_ = false;
    private bool isAttacking_ = false;

    override protected void AfterStart()
    {
        animator = GetComponent<Animator>();
        maxHealth_ = health_;
        health_bar_ = GameObject.Find("Bossbar");
        health_slider_ = health_bar_.transform.GetChild(0).GetComponent<Slider>();
        health_bar_.SetActive(false);
    }

    override public void Engage()
    {
        StartCoroutine(EngageAfterDelay());
        health_bar_.SetActive(true);
    }

    override protected void Update()
    {
        if (gm_Behavior_.Is_Paused) return;
        if (!Player_in_room) return;
        if (transitioningPhase_) return;

        attackTimer_ -= Time.deltaTime;
        damageTimer_ -= Time.deltaTime;

        health_slider_.value = health_;
        health_slider_.maxValue = maxHealth_;

        CheckPhaseTransition();

        if (player_ == null) return;
        if (!isAttacking_)
            AttackPlayer();
    }

    override public void GetDamage(int amount)
    {
        if (transitioningPhase_) return;
        if (!Player_in_room) return;
        base.GetDamage(amount);
    }

    private void CheckPhaseTransition()
    {
        float healthPercent = (float)health_ / maxHealth_ * 100f;

        if (phase_ == 1 && healthPercent <= phase2_health_threshold_)
            StartCoroutine(TransitionToPhase(2));
        else if (phase_ == 2 && healthPercent <= phase3_health_threshold_)
            StartCoroutine(TransitionToPhase(3));
    }

    private IEnumerator TransitionToPhase(int newPhase)
    {
        phase_ = newPhase;
        transitioningPhase_ = true;
        isAttacking_ = false;
        lineRenderer_.enabled = false;

        for (int i = 0; i < 5; i++)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            yield return new WaitForSeconds(0.15f);
            GetComponent<SpriteRenderer>().color = Color.white;
            yield return new WaitForSeconds(0.15f);
        }

        if (newPhase == 3)
            GetComponent<SpriteRenderer>().color = new Color(1f, 0.4f, 0.4f);

        transitioningPhase_ = false;
    }

    override protected void AttackPlayer()
    {
        if (attackTimer_ <= 0f)
        {
            if (phase_ == 1)
                StartCoroutine(Phase1Attack());
            else if (phase_ == 2)
                StartCoroutine(Phase2RotatingLaser());
            else if (phase_ == 3)
                StartCoroutine(Phase3TrackingLaser());

            attackTimer_ = 1f / attack_speed_;
        }
    }

    private IEnumerator Phase1Attack()
    {
        isAttacking_ = true;
        animator?.SetBool("Attacking", true);

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

        // Lock and fire
        lineRenderer_.startColor = Color.white;
        lineRenderer_.endColor   = Color.white;

        float elapsed = 0f;
        float activeDuration = 0.2f;
        float tickTimer = 0f;
        float tickRate = 0.1f;

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
                }
            }

            yield return null;
        }

        lineRenderer_.enabled = false;
        animator?.SetBool("Attacking", false);
        isAttacking_ = false;
    }

    private IEnumerator Phase2RotatingLaser()
    {
        isAttacking_ = true;
        animator?.SetBool("Attacking", true);

        Vector2 origin = spawnPoint_.position;
        float currentAngle = 0f;
        float elapsed = 0f;

        // Warning flash
        lineRenderer_.enabled = true;
        lineRenderer_.startWidth = laserWidth_;
        lineRenderer_.endWidth = laserWidth_;
        lineRenderer_.startColor = new Color(1f, 0f, 0f, 0.3f);
        lineRenderer_.endColor   = new Color(1f, 0f, 0f, 0.3f);

        yield return new WaitForSeconds(warningDuration_);

        lineRenderer_.startColor = Color.white;
        lineRenderer_.endColor   = Color.white;

        float tickTimer = 0f;
        float tickRate = 0.1f;

        while (elapsed < phase2_laser_duration_)
        {
            elapsed += Time.deltaTime;
            tickTimer += Time.deltaTime;
            currentAngle += rotation_speed_ * Time.deltaTime;

            Vector2 dir = new Vector2(
                Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                Mathf.Sin(currentAngle * Mathf.Deg2Rad)
            );

            Vector2 endPoint = origin + dir * 50f;
            lineRenderer_.SetPosition(0, origin);
            lineRenderer_.SetPosition(1, endPoint);

            RaycastHit2D[] hits = Physics2D.BoxCastAll(origin, new Vector2(laserWidth_, laserWidth_), 0f, dir, 50f);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.gameObject == gameObject) continue;
                if (hit.collider.CompareTag("Player") && tickTimer >= tickRate)
                {
                    player_script_.GetDamage(damage_);
                    tickTimer = 0f;
                }
            }

            yield return null;
        }

        lineRenderer_.enabled = false;
        animator?.SetBool("Attacking", false);
        isAttacking_ = false;
    }
    private IEnumerator Phase3TrackingLaser()
    {
        isAttacking_ = true;

        yield return StartCoroutine(Phase1Attack());
        yield return StartCoroutine(Phase2RotatingLaser());
        yield return StartCoroutine(Phase1Attack());
        yield return StartCoroutine(Phase2RotatingLaser());

        isAttacking_ = false;
    }

    override protected void MoveTowardsPlayer() { } 

    protected override void onDeath()
    {
        health_bar_.SetActive(false);
        lineRenderer_.enabled = false;
        gm_Room_gen_.Spawn_end_portal();
    }
}
