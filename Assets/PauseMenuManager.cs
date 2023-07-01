using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] CameraController cameraPlayer;

    public void ExitGame() => Application.Quit();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.activeSelf)
                CloseMenu();
            else OpenMenu();
        }
    }

    public void OpenMenu()
    {
        cameraPlayer.controlsEnabled = false;
        UnlockCursor();
        pauseMenu.SetActive(true);
    }
    public void CloseMenu()
    {
        cameraPlayer.controlsEnabled = true;
        LockCursor();
        pauseMenu.SetActive(false);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
