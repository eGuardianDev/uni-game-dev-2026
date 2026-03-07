using UnityEngine;

public class Inventory : MonoBehaviour
{

    public int key_counter =0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
