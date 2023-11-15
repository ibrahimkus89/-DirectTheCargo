using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [Header("-----BOX MANAGEMENT")] [SerializeField]
    private List<GameObject> _BoxPool;
    [SerializeField] private Transform _BoxExitPoint;
    private int _BoxPoolIndex;
    public float _CheckOutTime;

    void Awake()
    {
        if (Instance ==null)
        {
            Instance = this;
        }
        else
        {
            Destroy(Instance);
        }
    }

    void Start()
    {
        StartCoroutine(SendBox());
    }

    IEnumerator SendBox()
    {
        _BoxPool[_BoxPoolIndex].transform.position = _BoxExitPoint.position;
        _BoxPool[_BoxPoolIndex].SetActive(true);
        _BoxPoolIndex++;

        while (true)
        {
            yield return new WaitForSeconds(_CheckOutTime);

            _BoxPool[_BoxPoolIndex].transform.position = _BoxExitPoint.position;
            _BoxPool[_BoxPoolIndex].SetActive(true);

            if (_BoxPoolIndex==_BoxPool.Count-1)
            {
                _BoxPoolIndex = 0;
            }
            else
            {
                _BoxPoolIndex++;
            }
        }
    }

    void Update()
    {
        
    }
}
