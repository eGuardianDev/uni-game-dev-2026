using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform target;
    public float speed = 5f;
    public bool follow_target = false;
    private Vector3 velocity = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 targetPos = new Vector3(
            target.position.x,target.position.y,
            transform.position.z  
        );

        if (follow_target)
        {
            
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPos,
                ref velocity,
                speed
            );
        }
    }
}
