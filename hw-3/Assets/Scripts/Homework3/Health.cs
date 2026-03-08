using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using TMPro;
public class Health : MonoBehaviour
{
    Animator animator;
    public int currentHealth = 0;
    public int MaxAttackTime = 5;

    private bool can_be_hit = true;

    public Volume volume;
    private Vignette vignette;
    private DepthOfField dof;

    public RawImage[] Healths;
    public Texture FullHearth;
    public Texture EmptyHearth;
  
    public TextMeshProUGUI ImmuneText;

    public bool CanDamage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        
        animator = GetComponent<Animator>();
        currentHealth = Healths.Length;
        volume.profile.TryGet(out vignette);
        volume.profile.TryGet(out dof);
        // OnPlayerHit();
        dof.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0 ;i<Healths.Length; ++i)
        {
            if(i < currentHealth)
                Healths[i].texture = FullHearth;
            else
                Healths[i].texture = EmptyHearth;
        }   
        if (currentHealth <= 1)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0.45f, Time.deltaTime * 5f);
        }
        else
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, Time.deltaTime * 5f);
        }
    }

    public void GetHit()
    {
        if (can_be_hit)
        {
            currentHealth--;
            animator.SetTrigger("Damaged");
            StartCoroutine(HitBlur());
        }
    }
    IEnumerator HitBlur()
    {
        can_be_hit = false;
        dof.active = true;
        yield return new WaitForSeconds(1f);
        dof.active = false;
        can_be_hit = true;
    }


    void powerUP()
    {
        StartCoroutine(immune());

    }

    int waiting = 0;
    IEnumerator immune()
    {
        can_be_hit = false;
        ImmuneText.gameObject.SetActive(true);
        waiting = MaxAttackTime;
        CanDamage = true;
        while (true)
        {
            ImmuneText.text = "Immune for " + waiting-- + " s";
            yield return new WaitForSeconds(1f);
            if(waiting == 0) break;
        }
        CanDamage = false;
        can_be_hit = true;
        ImmuneText.gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("ImmuneObject"))
        {   
            powerUP();
            Destroy(collision.gameObject);
        }

        
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if(CanDamage == true
         && collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(collision.gameObject);
        } 
    }
}
