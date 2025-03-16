using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SombrasController : MonoBehaviour
{
    [SerializeField]
    GameObject sombrasPrefab;

    //maximas sombras esta escena
    [SerializeField]
    int _maxRecords;


    //grabacion actual
    public List<SombraStorage.SombraAction> _currentRecord = new List<SombraStorage.SombraAction>();

    public List<PlayerMovement> _sombrasActivas = new List<PlayerMovement>();
    public List<int> _sombrasIndices = new List<int>();

    double _startTime = 0;

    private bool stoppedRecording = false;



    //el indice de la sombra que se va a estar grabando esta iteracion
    public int _currentRecordIndex;


    private bool done = false;

    private void Start()
    {
        //inicializar las listas segun el storage

        for(int i = 0; i < SombraStorage.Instance.maxSombras; i++)
        {
            //creamos las listas con tama�o adecuado y vacias
            _sombrasActivas.Add(null);
            _sombrasIndices.Add(0);
        }



        //crear las sombras

        //para cada posible sombra
        for (int i = 0; i < SombraStorage.Instance.maxSombras; i++)
        {
            //si no esamos usando la sombra, no hacemos nada y pasamos
            if (!SombraStorage.Instance._usedSombras[i]) continue;


            //si estamos usando la sombra, la creamos 
            GameObject newSombra = Instantiate(sombrasPrefab, transform.position, Quaternion.identity);

            //el indice de _sombrasIndices ya esta en 0,  (se ha inicializado asi) 

            //asignamos la sombra activa
            _sombrasActivas[i] = (newSombra.GetComponent<PlayerMovement>());

            //le asignamos su indice
            newSombra.GetComponent<PlayerMovement>()._controllerIndex = i;  

            //para que las sombras no cambien
            newSombra.GetComponent<PlayerMovement>().setRecording(false);
            newSombra.GetComponent<PlayerInput>().enabled = false;

            //cambiarles el color
            newSombra.GetComponentInChildren<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,1);
            newSombra.GetComponentInChildren<ColorIndicator>().SetColor(i);

            //para que solo haya un audio listener
            Destroy(newSombra.GetComponent<AudioListener>());
        }

        //print("starttt" + _sombrasIndices.Count);


        _startTime = Time.time;

        //print("start , sombras indices size" + _sombrasIndices.Count);


        GameUI.Instance.StartUI(_maxRecords, SombraStorage.Instance._records.Count);
    }



    //controller
    void Update()
    {
        //ahora las instancias preguntan por su input
        return;
        if (_sombrasIndices.Count == 0) return;

        //actualizar todos las sombras
        for(int i = 0; i < SombraStorage.Instance._records.Count; i++)
        {
            //print("update , sombrasStorage records[0] size" + SombraStorage.Instance._records[i].Count);
            //print("update , sombras indices size" + _sombrasIndices.Count);



            if (SombraStorage.Instance._records[i].Count >  _sombrasIndices[i])
            {

                double currTime = Time.time - _startTime;
                double actionTime = SombraStorage.Instance._records[i][_sombrasIndices[i]].time;

                //print("CurrTime: " + currTime);
                //print("ActionTime: " + actionTime);

                if (currTime >= actionTime)
                {
                    SombraStorage.runAction(SombraStorage.Instance._records[i][_sombrasIndices[i]], _sombrasActivas[i]);
                    _sombrasIndices[i]++;
                }
            }
            else if (SombraStorage.Instance._records[i].Count ==  _sombrasIndices[i]) //cuando se ha dejado de detectar el input
            {
                //cancelar todos los inputs
                if (_sombrasActivas[i] != null)
                {
                    _sombrasActivas[i].colliderOnDead.SetActive(true);
                    _sombrasIndices[i]++;
                }
            }



        }
      
    }

    public void KillShadow()
    {

    }

    //controller
    public void stopRecording(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            stopRecording();
        }
    }

    //cuando se pulsa parar de grabar (solo hay uno, las sombras no tienen un SombrasController, solo lo tiene el prefab de Player y Sombras)
    //guarda la grabacion en el slot de _currentRecordIndex
    public void stopRecording()
    {

        if (SombraStorage.Instance._records.Count < _maxRecords && !stoppedRecording)
        {
            //guardamos esta sombra en el indice indicado en _currentRecordIndex
            SombraStorage.Instance._records[_currentRecordIndex] = (new List<SombraStorage.SombraAction>(_currentRecord));

            //asignamos que estamos usando esta sombra a partir de ahora (para que luego se creen)
            SombraStorage.Instance._usedSombras[_currentRecordIndex] = true;


            _currentRecord.Clear();
        }
        stoppedRecording = true;
        reloadScene();
    }



    //controller
    void reloadScene()
    {
        if (!done)
        {
            LevelManager.Instance.NextIterationLoad();
            done = true;
        }

    }


}
