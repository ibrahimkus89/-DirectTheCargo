using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private ExitLine _activeLine;
    //[SerializeField] private ExitLine[] _exitLines;
    [SerializeField] private ExitLine _DefaultExitLines;
    [SerializeField] private TransportLine[] _TransformLines;


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

        _activeLine = _DefaultExitLines;
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

        if (Time.timeScale !=0)
        {
            Ray _Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (Physics.Raycast(_Ray,out RaycastHit hit,100))
                {
                    if (hit.transform.gameObject.CompareTag("Line"))
                    {
                        if (_activeLine !=null)
                        {
                           _activeLine.LightProcess(0);
                        }

                       _activeLine = hit.transform.gameObject.GetComponent<ExitLine>();
                        _activeLine.LightProcess(1);
                        //Debug.Log("City Name : " +_activeLine._CityName);

                    }
                }
            }
        }

    }

    public Transform LineAcceptancePoint()
    {
        if (_activeLine !=null)
        {
            return _activeLine._Destination.transform;
        }
        else
        {
            return null;
        }
    }

    public string LineCityName()
    {
        if (_activeLine != null)
        {
            return _activeLine._CityName;
        }
        else
        {
            return null;
        }
    }

    public void TransferSuccessful()
    {
        Debug.Log("Successful");
    }

    public void TransferFailed()
    {
        Debug.Log("Fail");

    }
}
