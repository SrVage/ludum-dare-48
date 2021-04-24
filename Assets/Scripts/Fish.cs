using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour, IDisposable
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Light _light;
    private Player _player;

    void Start()
    {
        _player = FindObjectOfType<Player>().GetComponent<Player>();
        _player.DeathFish += ChangeLight;
    }

    private void ChangeLight()
    {
        _light.color = new Color(0.377f, 0.031f, 0.031f, 1);
    }
    
    void FixedUpdate()
    {
        transform.Translate(0, 0, 0.1f);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.LookAt(other.transform.position);
            transform.Translate(0, 0, 0.2f);
            _animator.SetBool("attack", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _animator.SetBool("attack", false);
    }

    public void Dispose()
    {
        _player.DeathFish -= ChangeLight;
    }
}
