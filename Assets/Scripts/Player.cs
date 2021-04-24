using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Player : MonoBehaviour
{
    public event Action<float> ChangeOxy;
    public event Action<GameObject> TakeOxy;
    public event Action<bool> Floor;
    public event Action DeathFish;

    private float _speed = 0.1f;
    private float _oxygen = 30f;
    private float _chargeLight = 100f;
    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Rigidbody> _partsOfBody = new List<Rigidbody>();
    [SerializeField] private Transform _raycast;
    [SerializeField] private ParticleSystem _buble;
    private Rigidbody _rb;
    
    void Start()
    {
        ChangeOxy += Death;
        _rb = GetComponent<Rigidbody>();
        _partsOfBody.AddRange((_ragdoll.GetComponentsInChildren<Rigidbody>()));
        KinematicOn();
        InvokeRepeating(nameof(Buble), 1f, 7f);
    }

    private void Buble()
    {
        _buble.Play();
        _oxygen -= 4f;
        ChangeOxy?.Invoke(_oxygen);
    }

    private void KinematicOn()
    {
        for (int i=0; i<_partsOfBody.Count; i++)
        {
            _partsOfBody[i].isKinematic = true;
        }
    }

    private void Death(float ox)
    {
        if (ox <= 0)
        {
            _animator.enabled = false;
            _ragdoll.GetComponent<CapsuleCollider>().enabled = false;
            for (int i=0; i<_partsOfBody.Count; i++)
            {
                _partsOfBody[i].isKinematic = false;
            } 
            DeathFish?.Invoke();
        }
    }
    
    void Update()
    {
        if (_oxygen<=0) return;
        _currentSpeed = _rb.velocity.magnitude;
        RaycastHit hit;
        if (Physics.Raycast((_raycast.position), _raycast.position-transform.position, out hit,20f))
        {
            if (hit.collider.CompareTag("floor")) Floor?.Invoke(true);
            else Floor?.Invoke(false);
        }
        else Floor?.Invoke(false);
        if (Input.GetAxis("Horizontal")<0)
        {
            StopCoroutine(nameof(Rotator));
            StartCoroutine(nameof(Rotator), 1);
        }
        
        if (Input.GetAxis("Horizontal")>0)
        {
            StopCoroutine(nameof(Rotator));
            StartCoroutine(nameof(Rotator), 2);
        }
                
        if (Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0) _animator.SetBool("move", true);
        else _animator.SetBool("move", false);
    }

    void FixedUpdate()
    {
        if (_oxygen<=0) return;
        if (_currentSpeed > 10) _rb.velocity = _rb.velocity.normalized * 10;
        _rb.AddForce(0, Input.GetAxis("Vertical")*_speed, 0, ForceMode.VelocityChange);
        _rb.AddForce(Input.GetAxis("Horizontal")*_speed, 0, 0, ForceMode.VelocityChange);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_currentSpeed > 5)
        {
            if (_oxygen >= (_currentSpeed - 5)) _oxygen -= (_currentSpeed - 5);
            else _oxygen = 0;
        }
        ChangeOxy?.Invoke(_oxygen);
        if (other.collider.gameObject.CompareTag("Enemy"))
        {
            _oxygen = 0;
            Death(0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Oxygen"))
        {
            _oxygen += 30;
            TakeOxy?.Invoke(other.gameObject);
        }
    }


    IEnumerator Rotator(int direction)
    {
        if (direction == 1)
        {
            while (transform.rotation.eulerAngles != new Vector3(57f, 270f, 0))
            {
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,
                    new Vector3(57f, 270f, 0), Time.deltaTime*2));
                yield return null;
            }
        }
        if (direction == 2)
        {
            while (transform.rotation.eulerAngles != new Vector3(57f, 90f, 0))
            {
                transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles,
                    new Vector3(57f, 90f, 0), Time.deltaTime*2));
                yield return null;
            }
        }
    }
    
    
}
