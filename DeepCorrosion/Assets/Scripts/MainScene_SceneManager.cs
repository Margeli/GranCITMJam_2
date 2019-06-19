using UnityEngine.SceneManagement;
using UnityEngine;

public class MainScene_SceneManager : MonoBehaviour
{
    
    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
