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

    public int _C_BoxNumber;
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
        _C_BoxNumber++;
        GameSpeed(_C_BoxNumber);
        Debug.Log("BoxNumber" +_C_BoxNumber);

        /* if (_C_BoxNumber ==5)
         {
             foreach (var item in _TransformLines)
             {
                 item._Speed = .6f;
             }
             _CheckOutTime = 1.5f;
         }
         else if (_C_BoxNumber == 10)
         {
             foreach (var item in _TransformLines)
             {
                 item._Speed = .7f;
             }
             _CheckOutTime = 1.3f;
         }*/

        /*switch (_C_BoxNumber)
        {
              case 5:
                foreach (var item in _TransformLines)
                {
                    item._Speed = .6f;
                }
                _CheckOutTime = 1.5f;
                break;
              case 10:
                foreach (var item in _TransformLines)
                {
                    item._Speed = .7f;
                }
                _CheckOutTime = 1.3f;
                break;
              case 15:
                  foreach (var item in _TransformLines)
                  {
                      item._Speed = .8f;
                  }
                  _CheckOutTime = 1.2f;
                  break;
              case 20:
                  foreach (var item in _TransformLines)
                  {
                      item._Speed = .9f;
                  }
                  _CheckOutTime = 1f;
                  break;
              case 25:
                  foreach (var item in _TransformLines)
                  {
                      item._Speed = 1f;
                  }
                  _CheckOutTime = .9f;
                  break;

        }*/
    }

public void GameSpeed(int limitValue)
{
if (limitValue ==5 || limitValue == 10 || limitValue == 15 || limitValue == 20 || limitValue == 25)
{
    foreach (var item in _TransformLines)
    {
        item._Speed += .1f;
    }
    _CheckOutTime -= .1f;
}
}
public void TransferFailed()
{
Debug.Log("Fail");

}
}
