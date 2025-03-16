using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class SelectShadow : MonoBehaviour
{

    public List<GameObject> _iconButtons = new List<GameObject>();
    public List<Color> _colorButtons = new List<Color>();


    public Sprite _iconSprite;
    public Sprite _blokedSprite;

    public SombrasController _sombrasController;
    public int _maxRecords;


    public int _currentSelected;



    private void Awake()
    {
        Time.timeScale = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


        LevelManager.Instance.gameObject.SetActive(false);   
        //print("aqui");
        _maxRecords = _sombrasController._maxRecords;

        //activar y desactivar segun los datos
        for (int i = 0; i <SombraStorage.Instance.maxSombras; i++) { 
            
            if(i < _maxRecords){

                //elegir que sprite poner segun si se ha grabado o no

                //si esta usado
                if (SombraStorage.Instance._usedSombras[i])
                {
                    _iconButtons[i].GetComponent<Image>().color = _colorButtons[i];
                }
                else
                {
                    _iconButtons[i].GetComponent<Image>().color = Color.white;

                }


            }
            else //poner sprite bloqueado
            {
                _iconButtons[i].GetComponent<Image>().sprite = _blokedSprite;
            }
        }



    }



    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < _iconButtons.Count; i++)
        {
            _iconButtons[i].transform.GetChild(0).gameObject.SetActive(false);
        }

        _iconButtons[_currentSelected].transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnMove(InputAction.CallbackContext callback)
    {
        if (callback.started) {

            string key = callback.control.name; // Obtener el nombre de la tecla

            if (key == "a" || key == "leftArrow")
            {
                _currentSelected = (_currentSelected + _maxRecords -1) % _maxRecords;
            }
            else if (key == "d" || key == "rightArrow")
            {
                _currentSelected = (_currentSelected +1) % _maxRecords;
            }
        }
    }

    public void OnSelect(InputAction.CallbackContext callback)
    {
        if (callback.started) {

            //desactivar el objeto
            gameObject.SetActive(false);

            //continuar el tiempo
            Time.timeScale = 1.0f;


            //marcar la actual como no usado
            SombraStorage.Instance._usedSombras[_currentSelected] = false;

            print("setear a false");

            //asignar el current al controller
            _sombrasController._currentRecordIndex = _currentSelected;
            //encender el controller
            _sombrasController.gameObject.SetActive(true);



            //activar el level manager para el fade in
            LevelManager.Instance.gameObject.SetActive(true);
        }


    }



}
