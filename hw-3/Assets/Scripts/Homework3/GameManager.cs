using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public GameObject WinScreen;
    public GameObject FallenScreen;
    public GameObject HurtScreen;
    public bool playing;
    public Health player_health;
    public Inventory player_inventory;
    public Rigidbody2D player_rigidbody; 
    public Animator anim; 

    void Start()
    {
        WinScreen.SetActive(false);
        FallenScreen.SetActive(false);
        HurtScreen.SetActive(false);
        playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playing)
            player_rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        else player_rigidbody.constraints =  RigidbodyConstraints2D.FreezeRotation;
        if (player_inventory.key_counter == 3)
        {
            anim.enabled = false;
            playing = false;
            WinScreen.SetActive(true);
        }
        if(player_health.currentHealth == 0)
        {
            anim.enabled = false;
            playing = false;
            HurtScreen.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }
  
    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
