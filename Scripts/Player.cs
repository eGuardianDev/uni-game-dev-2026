using System.Numerics;
using UnityEngine;

public class Player : MonoBehaviour
{


    public Rigidbody2D rb;
    public float speed = 10f;
    
    private Vector3 input;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        input = new Vector3(Input.GetAxis("Horiozntal"),0, Input.GetAxis("Vertical"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddRelativeForce(input *speed * Time.deltaTime);
    }
}
