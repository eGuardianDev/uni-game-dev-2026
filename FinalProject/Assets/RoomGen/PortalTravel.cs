using UnityEngine;

public class PortalTravel : MonoBehaviour
{
    private GameData gd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gd = GameObject.Find("GameData").GetComponent<GameData>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            gd.LoadNextScene();
        }
    }
    
}
