using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    [SerializeField] private Animator _animator;

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
        if (other.gameObject.CompareTag("Player"))
            _animator.SetBool("attack", false);
    }
}
    
