using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages the title screen.

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // starts the game.
    public void StartGame()
    {
        SceneHelper.ChangeScene("GameScene");
    }

    // quits the game.
    public void QuitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
