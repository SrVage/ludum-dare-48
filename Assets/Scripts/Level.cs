using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] public GameObject floor;
    [SerializeField] public GameObject ceiling;
    [SerializeField] private GameObject _ballon;
    [SerializeField] private GameObject[] _stones;
    [SerializeField] private GameObject _fish;
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ceiling") ceiling = transform.GetChild(i).gameObject;
            if (transform.GetChild(i).gameObject.name == "floor") floor = transform.GetChild(i).gameObject;
        }
        floor.transform.position = new Vector3(Random.Range(-170, 170), -33, 0);
        ceiling.transform.position = new Vector3(Random.Range(-170, 170), 64, 0);
        Invoke(nameof(CreateBallons), 0.4f);
        Invoke(nameof(CreateStones), 0.1f);
        InvokeRepeating(nameof(CreateFish), 1f,20f);
    }

    private void CreateFish()
    {
       var obj = Instantiate(_fish, new Vector3(transform.position.x + 180, transform.position.y + Random.Range(-30, 30), 0), Quaternion.Euler(new Vector3(0,-90,0)));
        Destroy(obj, 60);
    }

    private void CreateBallons()
    {
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            Instantiate(_ballon, new Vector3(Random.Range(transform.position.x-150, transform.position.x+150), transform.position.y, 0), Quaternion.identity);
        }
    }

    private void CreateStones()
    {
        for (int i = 0; i < Random.Range(100, 200); i++)
        {
           var obj = Instantiate(_stones[Random.Range(0,2)], new Vector3(Random.Range(transform.position.x-150, transform.position.x+150), transform.position.y-30f, Random.Range(-10,10)), Quaternion.identity);
           
           obj.transform.localScale *= Random.Range(0.1f+obj.transform.position.z/200, 3f+obj.transform.position.z/8);
        }
    }
}
