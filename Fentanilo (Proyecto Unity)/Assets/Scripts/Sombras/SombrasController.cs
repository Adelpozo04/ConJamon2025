using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static SombraStorage;

public class SombrasController : MonoBehaviour
{
    [SerializeField]
    GameObject sombrasPrefab;

    //maximas sombras esta escena
    [SerializeField]
    int _maxRecords;


    //grabacion actual
    public List<SombraStorage.SombraAction> _currentRecord = new List<SombraStorage.SombraAction>();

    List<SombraStorage.SombraState> _sombrasActivas = new List<SombraStorage.SombraState>();
    List<int> _sombrasIndices = new List<int>();

    double _startTime = 0;

   

    private void Start()
    {
        _startTime = Time.fixedTime;

        //crear las sombras
        for (int i = 0; i < SombraStorage.Instance._records.Count; i++)
        {
            GameObject newSombra = Instantiate(sombrasPrefab, transform.position, Quaternion.identity);

            _sombrasIndices.Add(0);
            _sombrasActivas.Add(new SombraStorage.SombraState(newSombra.GetComponent<PlayerMovement>()));
            
            //newSombra.GetComponent<PlayerMovement>().setRecording(false);
            //newSombra.GetComponent<PlayerInput>().enabled = false;
            newSombra.GetComponentInChildren<SpriteRenderer>().color = new Color(0.5f,0.5f,0.5f,1);
        }

        GameUI.Instance.StartUI(_maxRecords, SombraStorage.Instance._records.Count);
    }



    //controller
    void FixedUpdate()
    {
        if (_sombrasIndices.Count == 0) return;

        //actualizar todos las sombras
        for(int i = 0; i < SombraStorage.Instance._records.Count; i++)
        {
            //print("update , sombrasStorage records[0] size" + SombraStorage.Instance._records[i].Count);
            //print("update , sombras indices size" + _sombrasIndices.Count);
            if (SombraStorage.Instance._records[i].Count > _sombrasIndices[i])
            {

                double currTime = Time.fixedTime - _startTime;
                double actionTime = SombraStorage.Instance._records[i][_sombrasIndices[i]].time;

                //print("CurrTime: " + currTime);
                //print("ActionTime: " + actionTime);

                if (currTime >= actionTime)
                {
                    //Debug.Log("BEFORE" + _sombrasActivas[i].movementDirection + SombraStorage.Instance._records[i][_sombrasIndices[i]].direction);
                    //runAction(SombraStorage.Instance._records[i][_sombrasIndices[i]], _sombrasActivas[i]);

                    //if else con todas las funciones
                    if (SombraStorage.Instance._records[i][_sombrasIndices[i]].type == ActionType.JUMP)
                    {
                        _sombrasActivas[i].jumpPressed = SombraStorage.Instance._records[i][_sombrasIndices[i]].pressed;
                    }
                    else if (SombraStorage.Instance._records[i][_sombrasIndices[i]].type == ActionType.MOVE)
                    {
                        _sombrasActivas[i].movementDirection = SombraStorage.Instance._records[i][_sombrasIndices[i]].direction;
                    }
                    else if (SombraStorage.Instance._records[i][_sombrasIndices[i]].type == ActionType.SHOOT)
                    {
                        _sombrasActivas[i].shootPressed = SombraStorage.Instance._records[i][_sombrasIndices[i]].pressed;
                        //target.playerMovement.gameObject.GetComponentInChildren<Shoot>().OnShoot(sombraAction.callback);
                    }
                    else if (SombraStorage.Instance._records[i][_sombrasIndices[i]].type == ActionType.AIM)
                    {
                        _sombrasActivas[i].movementDirection = SombraStorage.Instance._records[i][_sombrasIndices[i]].direction;
                        //target.gameObject.GetComponentInChildren<Shoot>().OnAim(sombraAction.callback);
                    }
                    else if (SombraStorage.Instance._records[i][_sombrasIndices[i]].type == ActionType.STOP_RECORDING)
                    {
                        _sombrasActivas[i].nextIterationPressed = SombraStorage.Instance._records[i][_sombrasIndices[i]].pressed;

                        if (SombraStorage.Instance._records[i][_sombrasIndices[i]].pressed)
                        {
                            PerkBehaviour perk = _sombrasActivas[i].playerMovement.GetComponent<PerkBehaviour>();
                            if (perk != null)
                            {
                                perk.ActivateEffect();
                                Destroy(_sombrasActivas[i].playerMovement.gameObject);
                            }
                        }
                    }
                    //Debug.Log("after" + _sombrasActivas[i].movementDirection);
                    _sombrasIndices[i]++;
                }
            }
            else if (SombraStorage.Instance._records[i].Count == _sombrasIndices[i]) //cuando se ha dejado de detectar el input
            {
                //cancelar todos los inputs
                if (_sombrasActivas[i].playerMovement != null)
                {
                    _sombrasActivas[i].playerMovement.colliderOnDead.SetActive(true);
                    _sombrasIndices[i]++;
                }
            }


            if (_sombrasActivas[i].alive)
            {
                //Debug.Log("updateing" + _sombrasActivas[i].movementDirection + " " + _sombrasActivas[i].jumpPressed);
                _sombrasActivas[i].playerMovement.MovementUpdate(_sombrasActivas[i].movementDirection, _sombrasActivas[i].jumpPressed);
            }
        }
      
    }

    public void KillShadow()
    {

    }

    public void stopRecording()
    {
        if (SombraStorage.Instance._records.Count < _maxRecords)
        {
            SombraStorage.Instance._records.Add(new List<SombraStorage.SombraAction>(_currentRecord));
            _currentRecord.Clear();
        }
        reloadScene();
    }



    //controller
    void reloadScene()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
        SceneManager.LoadScene(gameObject.scene.name);
    }


}
