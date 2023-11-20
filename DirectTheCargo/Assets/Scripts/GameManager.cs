using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private ExitLine _activeLine;
    //[SerializeField] private ExitLine[] _exitLines;
    [SerializeField] private ExitLine _DefaultExitLines;
    [SerializeField] private TransportLine[] _TransformLines;


    [Header("-----BOX MANAGEMENT")]
    [SerializeField] private List<GameObject> _BoxPool;
    [SerializeField] private Transform _BoxExitPoint;
    private int _BoxPoolIndex;
    public float _CheckOutTime;
    int _C_BoxNumber;
    private int _sceneIndex;

    [Header("-----SOUND MANAGEMENT")]
    [SerializeField] AudioSource[] _AudioSources;
    [SerializeField] private Image[] _SoundSettingsButtons;
    [SerializeField] private Sprite[] _SpriteObj;

        [Header("-----SOUND MANAGEMENT")]
    [SerializeField] private GameObject[] _Panels;
    [SerializeField] private TextMeshProUGUI[] _ScoreTexts;
    void Awake()
    {
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;

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
        _activeLine = _DefaultExitLines;
         SceneProcess();
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
                if (!UII())
                {
                    if (Physics.Raycast(_Ray, out RaycastHit hit, 100))
                    {
                        if (hit.transform.gameObject.CompareTag("Line"))
                        {
                            if (_activeLine != null)
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
        PlaySound(2);
        _C_BoxNumber++;
        _ScoreTexts[0].text = _C_BoxNumber.ToString();
        GameSpeed(_C_BoxNumber);
      

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
        if (limitValue == 5 || limitValue == 10 || limitValue == 15 || limitValue == 20 || limitValue == 25)
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
        PlaySound(3);
        if (_C_BoxNumber > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", _C_BoxNumber);
            Debug.Log("NEW SCORE : " + _C_BoxNumber);
        }
        _ScoreTexts[2].text = _C_BoxNumber.ToString();
        _ScoreTexts[3].text = PlayerPrefs.GetInt("HighScore").ToString();
        PanelProcess(0, true);
        Time.timeScale = 0;

    }

    void SceneProcess()
    {
        if (PlayerPrefs.GetInt("GameSound") == 1)
        {
            
            _SoundSettingsButtons[0].sprite = _SpriteObj[0];
            _AudioSources[0].mute = false;
        }
        else
        {
            
            _SoundSettingsButtons[0].sprite = _SpriteObj[1];
            _AudioSources[0].mute = true;
        }

        if (PlayerPrefs.GetInt("EffectSound") == 1)
        {
           
            _SoundSettingsButtons[1].sprite = _SpriteObj[2];

            for (int i = 1; i < _AudioSources.Length; i++)
            {
                _AudioSources[i].mute = false;
            }
        }
        else
        {
            
            _SoundSettingsButtons[1].sprite = _SpriteObj[3];

            for (int i = 1; i < _AudioSources.Length; i++)
            {
                _AudioSources[i].mute = true;
            }
        }

        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.SetInt("GameSound", 1);
            PlayerPrefs.SetInt("EffectSound", 1);

        }

        _ScoreTexts[1].text = PlayerPrefs.GetInt("HighScore").ToString();
    }

    void PlaySound(int Indexx)
    {
        _AudioSources[Indexx].Play();
    }

    void PanelProcess(int Index,bool situation)
    {
        _Panels[Index].SetActive(situation);
    }

    public void ButtonProcess(string Valuee)
    {
        switch (Valuee)
        {
            case "Start":
                PlaySound(1);
                PanelProcess(0,false);
                PanelProcess(4, true);
                StartCoroutine(SendBox());
                break;

            case "Pause":
                PlaySound(1);
                PanelProcess(1, true);
                Time.timeScale = 0;
                break;

            case "Continue":
                PlaySound(1);
                PanelProcess(1, false);
                Time.timeScale = 1;
                break;

            case "TryAgain":
                PlaySound(1);
                SceneManager.LoadScene(_sceneIndex);
                Time.timeScale = 1;
                break;

            case "Exit":
                PlaySound(1);
                PanelProcess(3, true);
                break;

            case "Yes":
                PlaySound(1);
                Debug.Log("Quit");
                Application.Quit();
                break;
            case "No":
                PlaySound(1);
                PanelProcess(3, false);
                break;
            case "GameSoundSettings":
                PlaySound(1); 
                
                if (PlayerPrefs.GetInt("GameSound") ==0)
                {
                    PlayerPrefs.SetInt("GameSound",1);
                    _SoundSettingsButtons[0].sprite = _SpriteObj[0];
                    _AudioSources[0].mute =false;
                }
                else
                {
                    PlayerPrefs.SetInt("GameSound",0);
                    _SoundSettingsButtons[0].sprite = _SpriteObj[1];
                    _AudioSources[0].mute = true;
                }
                break;
            case "EffectSoundSettings":
                PlaySound(1);

                if (PlayerPrefs.GetInt("EffectSound") == 0)
                {
                    PlayerPrefs.SetInt("EffectSound", 1);
                    _SoundSettingsButtons[1].sprite = _SpriteObj[2];

                    for (int i = 1 ;i < _AudioSources.Length; i++)
                    {
                        _AudioSources[i].mute = false;
                    }
                }
                else
                {
                    PlayerPrefs.SetInt("EffectSound", 0);
                    _SoundSettingsButtons[1].sprite = _SpriteObj[3];

                    for (int i = 1; i < _AudioSources.Length; i++)
                    {
                        _AudioSources[i].mute = true;
                    }
                }
                break;
        }
    }

    bool UII()
    {
        if (Input.touchCount >0 && Input.touches[0].phase ==TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            {
                return true;
            }
        }
        return false; 
    }
}
