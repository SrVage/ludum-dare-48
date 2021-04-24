using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour, IDisposable
{
    private Player _player;
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>().GetComponent<Player>();
        _player.TakeOxy += PlayerTakeOxy;
    }

    private void PlayerTakeOxy(GameObject gameObject)
    {
        if (gameObject == this.gameObject)
        {
            Destroy(this.gameObject);
            _player.TakeOxy -= PlayerTakeOxy;
        }
    }
    
    void Update()
    {
        
    }
    
    public void Dispose()
    {
        
    }
}
