using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void level1()
    {
        SceneManager.LoadScene("Pacman");
    }

    public void level2()
    {
        SceneManager.LoadScene("Pacman 1");
    }

    public void level3()
    {
        SceneManager.LoadScene("Pacman 2");
    }

    public void levelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
