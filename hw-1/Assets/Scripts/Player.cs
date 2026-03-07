using UnityEngine.SceneManagement;
using UnityEngine;

    public class Player : MonoBehaviour
    {


        public Rigidbody2D rb;
        public Transform tf;
        public float speed = 10f;
        public float jumpingForce = 10f;
        public LayerMask groundLayer;
        public Transform groundChecker;
        public float groundCheckerRadius;
        public SpriteRenderer playerSprite;


        public bool displayDebug = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }

        bool isGrounded = false;
        float input;
        void Update()
        {
            if (displayDebug)
            {
                Debug.Log(isGrounded);
            }
            
            
            isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckerRadius, groundLayer);
            input = Input.GetAxis("Horizontal");
            // Debug.Log(input.x);
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
                Jump();
            if(rb.linearVelocity.x != 0)
                playerSprite.flipX = (rb.linearVelocity.x < 0);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // tf.position += new Vector3(input * speed * Time.deltaTime, 0f, 0f);
            rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);
        }

        public void Jump()
        {
            rb.AddForce(Vector2.up * jumpingForce, ForceMode2D.Impulse);
        }
        void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.layer == LayerMask.NameToLayer("LevelBottom"))
            {
                Debug.Log("restart");
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
