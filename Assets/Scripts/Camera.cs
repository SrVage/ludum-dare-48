using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Camera : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private Light _sun;

    private Vector3 _offset;
    private Vector3 _startOffset;
    private Vector3 _rotation;
    private float _intensity;
    private bool _shake;
    private bool _end=false;
    private bool _dir;
    private bool _isChange=false;

    void Start()
    {
        _offset = transform.position - _player.transform.position;
        _startOffset = _offset;
        _rotation = transform.rotation.eulerAngles;
        _player.GetComponent<Player>().ChangeOxy += EndOfOxy;
        _player.GetComponent<Player>().Floor += ChangeOffset;
        _player.GetComponent<Player>().DeathFish += Death;
        _player.GetComponent<Player>().PlayerPosition += ChangeWide;
    }

    private void ChangeWide(float pos)
    {
        GetComponent<UnityEngine.Camera>().fieldOfView = 60 + pos / 50;
        _sun.intensity = 0.3f - pos / 500;
    }

    private void ChangeOffset(bool dir)
    {
        _dir = dir;
        _isChange = true;
        Invoke(nameof(CancelChange), 2f);
        /*if (dir)
        {
            _offset = _startOffset + new Vector3(0, 10, 0);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,new Vector3(20, 0, 0), Time.deltaTime));
        }
        else
        {
            _offset = _startOffset;
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,_rotation, Time.deltaTime));
        }*/
    }

    private void CancelChange()
    {
        _isChange = false;
    }

    private void Death()
    {
        GetComponent<UnityEngine.Camera>().backgroundColor = new Color(0.377f, 0.031f, 0.031f, 1);
    }
    
    private void EndOfOxy(float oxy, float charge)
    {
        if (oxy <= 0) _end = true;
        if (oxy < 20)
        {
            _shake = true;
            _intensity = -(20 - oxy) / 150;
        }
        else _shake = false;
        
    }

    private void Update()
    {
        if (!_isChange) return;
        if (_dir)
        {
            _offset = Vector3.Lerp(_offset, _startOffset + new Vector3(0, 10, 0), 3*Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,new Vector3(20, 0, 0), Time.deltaTime));
        }
        else
        {
            _offset = Vector3.Lerp(_offset,_startOffset, 3*Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,_rotation, Time.deltaTime));
        }
    }

    private void LateUpdate()
    {
        if (_end) return;
        transform.position = Vector3.Lerp(transform.position, _player.transform.position+_offset, 3f * Time.deltaTime);
        if (_shake)  transform.position+=new Vector3(Random.Range(-_intensity, _intensity), Random.Range(-_intensity, _intensity), Random.Range(-_intensity, _intensity));
    }
}
