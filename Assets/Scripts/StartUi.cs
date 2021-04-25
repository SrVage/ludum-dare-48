using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUi : MonoBehaviour
{
    [SerializeField] private GameObject _instructions;
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Instructions()
    {
        if (!_instructions.activeSelf) _instructions.SetActive(true);
        else _instructions.SetActive(false);
    }
}
