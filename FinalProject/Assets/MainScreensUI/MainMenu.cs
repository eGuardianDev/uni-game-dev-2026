using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu_;
    [SerializeField] private GameObject PlayerSelect_;
    [SerializeField] private string MainGameSceneName_ = "MainGame";
    public void Open_MainMenu()
    {
        MainMenu_.SetActive(true);
        PlayerSelect_.SetActive(false);
    }
    public void Open_PlayerSelect()
    {
        MainMenu_.SetActive(false);
        PlayerSelect_.SetActive(true);
    }
    public void Start_Game()
    {
        SceneManager.LoadScene(MainGameSceneName_);
    }
}
