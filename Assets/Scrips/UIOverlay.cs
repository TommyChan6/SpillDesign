using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOverlay : MonoBehaviour
{
    public PauseMenu pauseMenu;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (pauseMenu != null) {
                pauseMenu.PauseGame();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu != null) {
                pauseMenu.PauseGame();
            }
        }
    }
}
