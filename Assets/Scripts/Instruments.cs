using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruments : MonoBehaviour
{
    private Player _player;
    private Renderer _rend;
    private Color _startColor1;
    private Color _startColor2;
    
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>().GetComponent<Player>();
        _player.TakeInsruments += PlayerTakeInstruments;
        _rend = GetComponent<Renderer>();
        _startColor1 = _rend.materials[1].color;
        _startColor2 = _rend.materials[0].color;
        StartCoroutine(nameof(ChangeColor1));
    }
    private void PlayerTakeInstruments(GameObject gameObject)
    {
        if (gameObject == this.gameObject)
        {
            Destroy(this.gameObject);
            _player.TakeInsruments -= PlayerTakeInstruments;
        }
    }

   IEnumerator ChangeColor1()
    {
        for (int i = 0; i < 100; i++)
        {
            _rend.materials[1].color = Color.Lerp(_rend.materials[1].color, Color.yellow, Time.deltaTime);
            _rend.materials[0].color = Color.Lerp(_rend.materials[0].color, Color.yellow, Time.deltaTime);
            yield return null;
        }
        StartCoroutine(nameof(ChangeColor2));
    }
   
   IEnumerator ChangeColor2()
   {
       for (int i = 0; i < 100; i++)
       {
           _rend.materials[1].color = Color.Lerp(_rend.materials[1].color, _startColor1, Time.deltaTime);
           _rend.materials[0].color = Color.Lerp(_rend.materials[0].color, _startColor2, Time.deltaTime);
           yield return null;
       }
       StartCoroutine(nameof(ChangeColor1));
   }
}
