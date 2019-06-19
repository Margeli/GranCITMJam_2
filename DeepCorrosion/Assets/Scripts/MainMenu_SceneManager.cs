using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu_SceneManager : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadSceneAsync("Main_Scene", LoadSceneMode.Single);
    }

    public void Quit()
    {
        Application.Quit();
    }
    
}
