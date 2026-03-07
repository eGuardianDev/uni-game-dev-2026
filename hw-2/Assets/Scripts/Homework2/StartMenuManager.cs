using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMainGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ViewOptions()
    {
        
    }

    public void ExitGame()
    {

        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
