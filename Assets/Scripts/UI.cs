using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GM _gm;
    [SerializeField] private Player _player;
    [Header("GUI")]
    [SerializeField] private Image _oxygen;
    [SerializeField] private Image _light;
    [SerializeField] private Text _instruments;
    [Header("Menu")]
    [SerializeField] private GameObject _menu;
    [SerializeField] private Text _status;

    private bool _isPause = false;
    void Start()
    {
        _player.ChangeOxy += ChangeOxyText;
        _gm.Instruments += ChangeInstruments;
    }

    private void ChangeOxyText(float oxy, float charge)
    {
        _oxygen.fillAmount = oxy / 100;
       _light.fillAmount = charge / 100;
       if (oxy<=0) Invoke(nameof(LooseLevel), 3f);
    }

    private void ChangeInstruments(int instruments)
    {
        _instruments.text = "Instruments: " + instruments.ToString();
        if (instruments==0) Invoke(nameof(WinLevel), 3f);
    }

    public void Pause()
    {
        if (_isPause) return;
        _isPause = true;
        _menu.SetActive(true);
        Time.timeScale = 0;
        _status.text = "Pause";
    }

    public void Resume()
    {
        if (!_isPause) return;
        _isPause = false;
        _menu.SetActive(false);
        Time.timeScale = 1;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private void LooseLevel()
    {
        if (_isPause) return;
        _isPause = true;
        _menu.SetActive(true);
        _status.text = "Loose";
        Time.timeScale = 0;
    }
    private void WinLevel()
    {
        if (_isPause) return;
        _isPause = true;
        _menu.SetActive(true);
        _status.text = "Win";
        Time.timeScale = 0;
    }
}
