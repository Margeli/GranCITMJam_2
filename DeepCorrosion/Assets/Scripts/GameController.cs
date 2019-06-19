using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Player player;
    private FadeToBlack ftb;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        ftb = GameObject.Find("FadeToBlack").GetComponent<FadeToBlack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.health <= 0)
        {
            ftb.fade = true;
            if(ftb.finished)
                SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);

        }
        else if (player.totalBarrels == 0)
        {
            GameObject.Find("WinText").SetActive(true);
            Invoke("ReturnToMenu", 5.0f);
        }
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
    }
}
