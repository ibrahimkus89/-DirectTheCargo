using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TransportLine : MonoBehaviour
{
    [SerializeField] private Renderer _Renderer;
    public float _Speed =.5f;
   

    void Update()
    {
        if (Time.timeScale !=0)
        {
            _Renderer.material.SetTextureOffset("_MainTex",new Vector2(0, -Time.time * _Speed));
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (Time.timeScale != 0)
        {
            other.transform.Translate((_Speed * 3) * Time.deltaTime * Vector3.back,Space.World);
        }
    }
}
