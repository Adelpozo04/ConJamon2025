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

    private void Start()
    {
        //crear las sombras

        for (int i = 0; i < SombraStorage.Instance._records.Count; i++)
        {
            GameObject newSombra = Instantiate(sombrasPrefab, transform.position, Quaternion.identity);

            _sombrasIndices.Add(0);
            _sombrasActivas.Add(newSombra.GetComponent<PlayerMovement>());

            newSombra.GetComponent<PlayerMovement>()._controllerIndex = i;  


            newSombra.GetComponent<PlayerMovement>().setRecording(false);
            newSombra.GetComponent<PlayerInput>().enabled = false;

            newSombra.GetComponentInChildren<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,1);
            newSombra.GetComponentInChildren<ColorIndicator>().SetColor(i);
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

    public void stopRecording()
    {
        if (SombraStorage.Instance._records.Count < _maxRecords && !stoppedRecording)
        {
            SombraStorage.Instance._records.Add(new List<SombraStorage.SombraAction>(_currentRecord));
            _currentRecord.Clear();

            stoppedRecording = true;
            Invoke("reloadScene", 0.767f);
        }
    }



    //controller
    void reloadScene()
    {
        //por si acaso, si falla LevelManager mirar aqui
        if(LevelManager.Instance != null)
        {
            LevelManager.Instance.state = LevelManager.FState.Ramificado;
        }
        
        SceneManager.UnloadSceneAsync(gameObject.scene);
        SceneManager.LoadScene(gameObject.scene.name);
    }


}
