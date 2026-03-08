using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float speed;
    public Transform pointA;
    public Transform pointB;
    private Transform currentPoint;
    public float checkDistance = 1f;
    public LayerMask groundLayer;

    public Rigidbody2D rb;

    bool left, right;
    bool move = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentPoint = pointA;
    }

    // Update is called once per frame
    void Update()
    { 
        RaycastHit2D hit1 = Physics2D.Raycast(pointA.position, Vector2.down, checkDistance, groundLayer);
        RaycastHit2D hit2 = Physics2D.Raycast(pointB.position, Vector2.down, checkDistance, groundLayer);
        if (hit1.collider != null)
        {
            left = true;
            Debug.Log("Left platform");   
        }else left = false;
        if (hit2.collider != null)
        {
            right = true;
            Debug.Log("Right platform");   
        }else right = false;
    }
    int position = 1;
    void FixedUpdate()
    {
        if(right && !left)
        {
            position = -1;
        }else if (!right && left)
        {
            position = 1;
        }
        if (move)
        {
            // rb.linearVelocity = new Vector2(position * speed, rb.linearVelocity.y);
            transform.position += position * Vector3.right * speed * Time.deltaTime;
        }
        else
        {
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Health>().GetHit();
        }
        // Debug.Log("Touched: " + collision.gameObject.name);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            move = true;
        }
    }  
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            move = false;
        }
    }
}
