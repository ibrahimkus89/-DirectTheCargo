using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLine : MonoBehaviour
{
    public string _CityName;
    [SerializeField] private Sprite[] _Lights;
    [SerializeField] private SpriteRenderer _LightSpite;
    public Transform _Destination;

    public void Choose()
    {
        _LightSpite.sprite = _Lights[1];
    }
    public void LeaveTheChoice()
    {
        _LightSpite.sprite = _Lights[0];
    }

    //Optional

    //public void LightOption(int situation)
    //{
    //    _LightSpite.sprite = _Lights[situation];
    //}
}
