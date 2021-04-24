using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    [SerializeField] private GameObject _ballon;
    [SerializeField] private GameObject _level;
    [SerializeField] private GameObject[] _levels;
    private Vector3 _offset;
    
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(nameof(BuildLevel));

    }

    IEnumerator BuildLevel()
    {
        WaitForSeconds ws = new WaitForSeconds(0.1f);
        int k = Random.Range(4, 15);
        GameObject[] _levels = new GameObject[k];
        for (int j = 0; j<k; j++)
        {
            _levels[j] = Instantiate(_level, Vector3.zero, Quaternion.identity);
            if (j > 0)
                _offset = _levels[j].GetComponent<Level>().ceiling.transform.position - _levels[j - 1].GetComponent<Level>().floor.transform.position;
            _levels[j].transform.position -= (_offset+new Vector3(0,-1, 0));
            yield return ws;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
