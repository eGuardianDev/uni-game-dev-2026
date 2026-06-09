using UnityEngine;

public class AbilityHolder : MonoBehaviour
{

    private SpriteRenderer icon;

    public GameObject ab;



    [Header("SFX")]
    [SerializeField] [Range(0f, 1f)] public float sfxVolume = 1f;
    [SerializeField] private AudioClip ExchangingItemSound;


    // public Ability ab;
    void Start()
    {
        if (ab != null)
        {
            GameObject clone = Instantiate(ab, transform);
            ab = clone;
            icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
            icon.sprite = ab.GetComponent<Ability>().Icon;
        }
    }


    public void ExchangeItem()
    {
        // Debug.Log("Exchanged");




        GameObject player = GameObject.Find("Player");
        
        AbilityManager abm = player.GetComponent<AbilityManager>();

        Ability ab1 = ab.GetComponent<Ability>();

        int key = (int)ab1.AssignedTo;

        if(abm.abilities[key] == null)
        {
            ab.transform.SetParent(player.transform);
            abm.abilities[key] = ab1;
            this.gameObject.SetActive(false);
        }
        else
        {   
            abm.abilities[key].gameObject.transform.SetParent(transform);
    
            Ability old_ab = abm.abilities[key];
            
            ab.transform.SetParent(player.transform);
            abm.abilities[key] = ab1;
            
            ab = old_ab.gameObject;
        }

        if (ab != null)
        {
            icon = transform.GetChild(0).GetComponent<SpriteRenderer>();
            icon.sprite = ab.GetComponent<Ability>().Icon;
        }

        if (ExchangingItemSound)
        {
            AudioSource.PlayClipAtPoint(ExchangingItemSound, transform.position,sfxVolume * 0.5f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
