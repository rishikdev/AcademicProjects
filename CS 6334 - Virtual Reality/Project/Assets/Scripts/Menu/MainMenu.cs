using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button buttonPlay;

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.firstSelectedGameObject = buttonPlay.gameObject;
        EventSystem.current.SetSelectedGameObject(buttonPlay.gameObject);
        buttonPlay.OnSelect(null);
    }

    public void OnButtonPlayClick()
    {
        SceneManager.LoadScene(1);
    }

    public void OnButtonQuitClick()
    {
        Application.Quit();
    }
}
