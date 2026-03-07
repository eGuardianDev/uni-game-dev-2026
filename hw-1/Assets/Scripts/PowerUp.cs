using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float jumpingForce;
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            rb.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
        }
    }
}
