using UnityEngine;

public class PortalTravel : MonoBehaviour
{
    private GameData gd;

    private UI_Manager ui;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui = GameObject.Find("GameManager").GetComponent<UI_Manager>();
        gd = GameObject.Find("GameData").GetComponent<GameData>();

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            // gd.LoadNextScene();
            gd.LoadNextSceneTransition();
        }
        else
        {
            if(gd.level >= 3)
            {
                ui.Final_Screen.SetActive(true);
            }
        }
    }
    
}
