using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public event Action<float, float> ChangeOxy;
    public event Action<GameObject> TakeOxy;
    public event Action<bool> Floor;
    public event Action DeathFish;
    public event Action<float> PlayerPosition;
    public event Action<GameObject> TakeInsruments;

    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private float _currentSpeed;
    [SerializeField] private Animator _animator;
    [SerializeField] private List<Rigidbody> _partsOfBody = new List<Rigidbody>();
    [SerializeField] private Transform _raycast;
    [SerializeField] private ParticleSystem _buble;
    [SerializeField] private Light _light;
    [SerializeField] private AudioClip[] _clip;
    [SerializeField] private Transform _ps;
        
    private float _speed = 0.1f;
    private float _oxygen = 100f;
    private float _chargeLight = 100f;
    private float _startCharge = 2f;
    private Rigidbody _rb;
    private AudioSource _audio;
    private ParticleSystem _objP;
    
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        ChangeOxy += Death;
        _rb = GetComponent<Rigidbody>();
        _partsOfBody.AddRange((_ragdoll.GetComponentsInChildren<Rigidbody>()));
        KinematicOn();
        InvokeRepeating(nameof(Buble), 1f, 7f);
    }

    private void Buble()
    {
        if (_oxygen <= 0) return;
       _objP = Instantiate(_buble, _ps.position, Quaternion.identity);
       Destroy(_objP, 2f);
        StartCoroutine(nameof(BubbleSound));
        _oxygen -= 4f;
        _chargeLight -= 1;
        _light.intensity = _startCharge*_chargeLight/100;
        ChangeOxy?.Invoke(_oxygen, _chargeLight);
        PlayerPosition?.Invoke(transform.position.y);
    }
    
    IEnumerator BubbleSound()
    {
        yield return new WaitForSeconds(0.3f);
        
        for (int i = 0; i < Random.Range(2, 6); i++)
        {
            _audio.PlayOneShot(_clip[0]);
            yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
            if (i==1) _objP.transform.position = _ps.position;
        }

    }

    private void KinematicOn()
    {
        for (int i=0; i<_partsOfBody.Count; i++)
        {
            _partsOfBody[i].isKinematic = true;
        }
    }

    private void Death(float ox, float charge)
    {
        if (ox <= 0)
        {
            _audio.PlayOneShot(_clip[1]);
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
        ChangeOxy?.Invoke(_oxygen, _chargeLight);
        if (other.collider.gameObject.CompareTag("Enemy"))
        {
            _oxygen = 0;
            ChangeOxy?.Invoke(_oxygen, _chargeLight);
            //Death(_oxygen, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Oxygen"))
        {
            _audio.PlayOneShot(_clip[2]);
            _oxygen += 30;
            if (_oxygen > 100) _oxygen = 100;
            TakeOxy?.Invoke(other.gameObject);
            ChangeOxy?.Invoke(_oxygen, _chargeLight);
        }
        if (other.CompareTag("Instruments"))
        {
            _audio.PlayOneShot(_clip[2]);
            TakeInsruments?.Invoke(other.gameObject);
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
