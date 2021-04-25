using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GM : MonoBehaviour
{
    public event Action<int> Instruments; 
    
    [SerializeField] private GameObject _ballon;
    [SerializeField] private GameObject _level;
    [SerializeField] private GameObject[] _levels;
    [SerializeField] private List <GameObject> _instruments = new List<GameObject>();
    [SerializeField] private Player _player;
    private int _countOfInstruments;
    private Vector3 _offset;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(BuildLevel));
        _player.GetComponent<Player>().TakeInsruments += InstrumentTake;
        Invoke(nameof(FindInstruments), 1f);
    }

    private void FindInstruments()
    {
        _instruments.AddRange(GameObject.FindGameObjectsWithTag("Instruments"));
        _countOfInstruments = _instruments.Count;
        Instruments?.Invoke(_countOfInstruments);
    }

    private void InstrumentTake(GameObject obj)
    {
        _countOfInstruments--;
        Instruments?.Invoke(_countOfInstruments);
    }

    IEnumerator BuildLevel()
    {
        WaitForSeconds ws = new WaitForSeconds(0.1f);
        int k = Random.Range(4, 15);
        GameObject[] _levels = new GameObject[k];
        for (int j = 0; j < k; j++)
        {
            _levels[j] = Instantiate(_level, Vector3.zero, Quaternion.identity);
            _levels[j].transform.localScale = new Vector3(Random.Range(0.9f, 1.3f), 1, 1);
            if (j > 0)
            {
                _offset = _levels[j].GetComponent<Level>().ceiling.transform.position -
                          _levels[j - 1].GetComponent<Level>().floor.transform.position;
                _levels[j].transform.position -= (_offset + new Vector3(0, -1, 0));
            }

            if (j == k-1)
                _levels[j].GetComponent<Level>().CreateFinishStone();
        }
        yield return ws;
        }
    }


