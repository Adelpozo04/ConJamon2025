using System.Collections.Generic;
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

    List<PlayerMovement> _sombrasActivas = new List<PlayerMovement>();
    List<int> _sombrasIndices = new List<int>();

    double _startTime = 0;

    private void Start()
    {
        //crear las sombras

        for (int i = 0; i < SombraStorage.Instance._records.Count; i++)
        {
            GameObject newSombra = Instantiate(sombrasPrefab, transform.position, Quaternion.identity);

            _sombrasIndices.Add(0);
            _sombrasActivas.Add(newSombra.GetComponent<PlayerMovement>());
            
            newSombra.GetComponent<PlayerMovement>().setRecording(false);
            newSombra.GetComponent<PlayerInput>().enabled = false;  
        }

        print("starttt" + _sombrasIndices.Count);

        _startTime = Time.time;
    }



    //controller
    void Update()
    {
        //actualizar todos las sombras
        for(int i = 0; i < SombraStorage.Instance._records.Count; i++)
        {
            print(i);
            print("sombraStorage size:" +SombraStorage.Instance._records.Count);
            print("sombras indices size:" +_sombrasIndices.Count);

            if(SombraStorage.Instance._records[i].Count >  _sombrasIndices[i])
            {
                print("sombraa: " +SombraStorage.Instance._records[i].Count );

                double currTime = Time.time - _startTime;
                double actionTime = SombraStorage.Instance._records[i][_sombrasIndices[i]].time;

                print("CurrTime: " + currTime);
                print("ActionTime: " + actionTime);

                if (currTime >= actionTime)
                {
                    SombraStorage.runAction(SombraStorage.Instance._records[i][_sombrasIndices[i]], _sombrasActivas[i]);
                    _sombrasIndices[i]++;
                }
            }
        }
      
    }

    //controller
    public void stopRecording(InputAction.CallbackContext callback)
    {
        if (callback.started)
        {
            if(SombraStorage.Instance._records.Count < _maxRecords)
            {
                SombraStorage.Instance._records.Add(new List<SombraStorage.SombraAction>(_currentRecord));
                _currentRecord.Clear();
            }
            reloadScene();
        }
    }

    //controller
    void reloadScene()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
        SceneManager.LoadScene(gameObject.scene.name);
    }


}
