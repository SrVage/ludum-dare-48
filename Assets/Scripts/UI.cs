using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{

    [SerializeField] private Text _oxygen;
    [SerializeField] private Player _player;
    
    void Start()
    {
        _player.ChangeOxy += ChangeOxyText;
    }

    private void ChangeOxyText(float oxy)
    {
        _oxygen.text = "Oxygen" + oxy.ToString("#.#");
    }
    void Update()
    {
        
    }
}
