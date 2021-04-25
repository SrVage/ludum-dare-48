using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    [SerializeField] public GameObject floor;
    [SerializeField] public GameObject ceiling;
    [SerializeField] private GameObject _ballon;
    [SerializeField] private GameObject[] _stones;
    [SerializeField] private GameObject _fish;
    [SerializeField] private GameObject _coral;
    [SerializeField] private GameObject _instruments;
    void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.name == "ceiling") ceiling = transform.GetChild(i).gameObject;
            if (transform.GetChild(i).gameObject.name == "floor") floor = transform.GetChild(i).gameObject;
        }
        floor.transform.position = new Vector3(Random.Range(-160, 160), -33, 0);
        ceiling.transform.position = new Vector3(Random.Range(-160, 160), 64, 0);
        Invoke(nameof(CreateBallons), 0.4f);
        Invoke(nameof(CreateInstruments), 0.6f);
        Invoke(nameof(CreateStones), 0.1f);
        Invoke(nameof(CreateCoral), 0.1f);
        InvokeRepeating(nameof(CreateFish), 1f,20f);
    }

    private void CreateFish()
    {
       var obj3 = Instantiate(_fish, new Vector3(transform.position.x + 180, transform.position.y + Random.Range(-30, 30), 0), Quaternion.Euler(new Vector3(0,-90,0)));
        Destroy(obj3, 80);
    }

    private void CreateBallons()
    {
        for (int i = 0; i < Random.Range(2, 5); i++)
        {
            Instantiate(_ballon, new Vector3(Random.Range(transform.position.x-180, transform.position.x+180), transform.position.y, 0), Quaternion.identity);
        }
    }

    private void CreateStones()
    {
        for (int i = 0; i < Random.Range(100, 200); i++)
        {
            int k = Random.Range(0, 3);
            var obj = Instantiate(_stones[k], new Vector3(Random.Range(transform.position.x-180, transform.position.x+180), transform.position.y-35f, Random.Range(-10,10)), Quaternion.identity);
            obj.transform.localScale *= Random.Range(0.1f+obj.transform.position.z/200, 3f+obj.transform.position.z/8);
            if (k==2) obj.transform.localScale *= 0.4f;
           obj.GetComponent<Rigidbody>().mass *= obj.transform.localScale.x;
        }
    }

    private void CreateCoral()
    {
        for (int i = 0; i < Random.Range(30, 100); i++)
        {
            var obj2 = Instantiate(_coral, new Vector3(Random.Range(transform.position.x-180, transform.position.x+180), transform.position.y-35f, transform.position.z+Random.Range(5,30)), Quaternion.Euler(new Vector3(-90,0,0)));
            obj2.transform.localScale *= Random.Range(0.5f, 2f);
        }
    }

    public void CreateFinishStone()
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = floor.transform.position-new Vector3(0,10,0);
        cube.transform.localScale = new Vector3(100, 2, 100);
        Instantiate(_instruments, new Vector3(Random.Range(transform.position.x-180, transform.position.x+180), transform.position.y-10f, 0), Quaternion.Euler(new Vector3(-90,0,0)));
    }
    
    public void CreateFirstStone()
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = ceiling.transform.position-new Vector3(0,9,0);
        cube.transform.localScale = new Vector3(100, 2, 100);
    }

    private void CreateInstruments()
    {
        for (int i = 0; i < Random.Range(0, 2); i++)
        {
            Instantiate(_instruments, new Vector3(Random.Range(transform.position.x-180, transform.position.x+180), transform.position.y-10f, 0), Quaternion.Euler(new Vector3(-90,0,0)));
        }
    }
}
