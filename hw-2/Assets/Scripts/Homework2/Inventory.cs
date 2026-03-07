using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public int key_counter =0;

    public RawImage[] Keys;
    public Texture FullKey;
    // public Texture EmptyKey;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i =0 ;i<key_counter; ++i)
        {
            Keys[i].texture = FullKey;
        }   
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Pickable"))
        {
            key_counter++;
            Destroy(collision.gameObject);
            Debug.Log("Picked up: " + collision.gameObject.name );
        }
    }
}
