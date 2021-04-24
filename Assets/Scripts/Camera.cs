using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private Vector3 _offset;
    private Vector3 _startOffset;
    private Vector3 _rotation;
    private float _intensity;
    private bool _shake;
    private bool _end=false;

    void Start()
    {
        _offset = transform.position - _player.transform.position;
        _startOffset = _offset;
        _rotation = transform.rotation.eulerAngles;
        _player.GetComponent<Player>().ChangeOxy += EndOfOxy;
        _player.GetComponent<Player>().Floor += ChangeOffset;
        _player.GetComponent<Player>().DeathFish += Death;
    }

    private void ChangeOffset(bool dir)
    {
        
        if (dir)
        {
            Debug.Log("cam");
            _offset = _startOffset + new Vector3(0, 10, 0);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,new Vector3(20, 0, 0), Time.deltaTime));
        }
        else
        {
            _offset = _startOffset;
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,_rotation, Time.deltaTime));
        }
    }

    private void Death()
    {
        GetComponent<UnityEngine.Camera>().backgroundColor = new Color(0.377f, 0.031f, 0.031f, 1);
    }
    
    private void EndOfOxy(float oxy)
    {
        if (oxy <= 0) _end = true;
        if (oxy < 20)
        {
            _shake = true;
            _intensity = -(20 - oxy) / 150;
        }
        else _shake = false;
        
    }
    


    private void LateUpdate()
    {
        if (_end) return;
        transform.position = Vector3.Lerp(transform.position, _player.transform.position+_offset, 3f * Time.deltaTime);
        if (_shake)  transform.position+=new Vector3(Random.Range(-_intensity, _intensity), Random.Range(-_intensity, _intensity), Random.Range(-_intensity, _intensity));
    }
}
