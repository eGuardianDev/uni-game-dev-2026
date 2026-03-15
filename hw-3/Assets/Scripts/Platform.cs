using UnityEngine;

public class Platform : MonoBehaviour
{
    public Rigidbody2D rb;
    public bool falling;

    public bool moving;
    public float moving_speed = 5f;

    public Transform one_point;
    public Transform second_point;
    private Transform move_to_target;
    private Vector3 velocity = Vector3.zero;

    public SpriteRenderer platformSprite;

    public Transform DecorationSpawn1;
    public Transform DecorationSpawn2;
    public GameObject decorationPrefab;   
    public Transform Spawner_pickups;
    public Transform Spawner_jumping_powerup;

    bool fallen = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        move_to_target = one_point;

        if (falling)
        {
            Color c = platformSprite.color;
            c.a = 0.75f; 
            platformSprite.color = c;
        }
      
        GameObject obj = Instantiate(
            decorationPrefab,
            DecorationSpawn1.position,
            DecorationSpawn1.rotation
        );
        obj.transform.SetParent(this.transform);

        obj = Instantiate(
            decorationPrefab,
            DecorationSpawn2.position,
            Quaternion.identity
        );
        obj.transform.SetParent(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (moving && !fallen)
        {
            transform.position = Vector3.SmoothDamp(
                transform.position,
                move_to_target.position,
                ref velocity,
                moving_speed
            );
        }
    }

      void OnTriggerEnter2D(Collider2D collision)
    {
        if( !fallen && moving
        && collision.gameObject.layer == LayerMask.NameToLayer("Point")
        && (collision.transform == one_point || collision.transform == second_point))
        {
            if(move_to_target == one_point)
                move_to_target = second_point;
            else move_to_target = one_point;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!fallen &&  falling && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            // Debug.Log("Touched ground!");
            rb.linearVelocity = Vector2.zero;
            velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints2D.None;
            fallen = true;
        }
    }
}
