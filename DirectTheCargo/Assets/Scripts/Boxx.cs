using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boxx : MonoBehaviour
{
    private bool _move;
    private Transform _Destination;
    public string _BoxCityName;
    private string _ActiveLineCityName;
   
    void Update()
    {
        if (_move)
        {
            transform.position = Vector3.Lerp(transform.position,_Destination.transform.position,.2f);

            if (Vector3.Distance(transform.position,_Destination.transform.position) < .50f)
            {
                _move =false;

                if (_BoxCityName==_ActiveLineCityName)
                {
                    GameManager.Instance.TransferSuccessful();
                }
                else
                {
                    GameManager.Instance.TransferFailed();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("TransportPoint"))
        {
            if (GameManager.Instance.LineAcceptancePoint() !=null)
            {
                _Destination =GameManager.Instance.LineAcceptancePoint();
                _ActiveLineCityName = GameManager.Instance.LineCityName();
                _move = true;
            }
        }
        else if (other.gameObject.CompareTag("InactiveObject"))
        {
            gameObject.SetActive(false);
            transform.position = Vector3.zero;
            transform.localPosition = Vector3.zero;
            transform.rotation = Quaternion.Euler(-90, 0, -90);
        }
    }
}
