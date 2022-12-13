using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class InGameMenu : MonoBehaviour
{
    private PlayerInputAction playerInputAction;
    private bool isMenuVisible;
    private GameObject menuPanel;

    private void Awake()
    {
        playerInputAction = new PlayerInputAction();
        menuPanel = transform.GetChild(0).gameObject;
    }

    private void OnEnable()
    {
        playerInputAction.Player.PlayerMenu.started += PlayerMenu;
        playerInputAction.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputAction.Player.PlayerMenu.started -= PlayerMenu;
        playerInputAction.Player.Disable();
    }

    private void PlayerMenu(InputAction.CallbackContext obj)
    {
        if (isMenuVisible)
            menuPanel.SetActive(false);

        else
            menuPanel.SetActive(true);

        isMenuVisible = !isMenuVisible;
    }

    public void OnButtonMainMenuClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnButtonBackClick()
    {
        isMenuVisible = false;
    }
}
