using UnityEngine;

public class RoomCameraMapping : MonoBehaviour
{

    [SerializeField] private Transform playerPosition_;
    [SerializeField] private Transform roomCenter_;
    
    [SerializeField] private CameraFollow cameraScript_;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // playerPosition_ = GameObject.Find("Player").GetComponent<Transform>();
        playerPosition_ = GameObject.FindWithTag("Player").transform;
        cameraScript_ = Camera.main.GetComponent<CameraFollow>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        Debug.Log("Player entered new room");
        cameraScript_.LockedOnRoom_ = this.transform;
    }
    
}
