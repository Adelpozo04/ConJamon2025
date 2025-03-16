using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gestor del paso entre niveles y otras variables globales.
/// Singleton. Se autogestiona para asegurarse de que sólo existe una instancia.
/// Debe crearse en la primera escena de juego. Automáticamente persistirá al resto.
/// </summary>
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private String[] levels;
        private int _currentLevel;
        private ResetSombrasAndLevel _resetSombrasAndLevel;
        private int lastGoal = -1;
        public enum FState
        {
            Won, //Cuando el jugador gane la partida
            Restart, //Cuando se reinicia el 
            Ramificado //Cuando el enemigo le da a la c o muere
        }

        public FState state;
        [SerializeField] private Image fadeInImageColored;
        [SerializeField] private Image fadeInImageFondo;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeInDurationFondo;
        
        [SerializeField] private float fadeOutDuration;
        [SerializeField] private float fadeOutDurationFondo;

        [Header("Entre niveles")]
        [SerializeField] private Color colorWon;

        [Header("Reinicio del nivel")]
        [SerializeField] private Color colorRestart;
        
        [Header("Final de iteración")]
        [SerializeField] private Color colorRamificar;
        public static LevelManager Instance { get; private set; }
        
        // ejecute solo una vez la primera cancion
        private bool playStartSong;

        //El awake asegura que la instancia es única y que no se el objeto no se destruye al cambiar de escena
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                state = FState.Restart;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
                playStartSong = true;
                updateCurrentLevel();
            }
            else
            {
                Destroy(gameObject);
            }
        }

    private void Start()
    {
        if (playStartSong && AudioManager.Instance != null)
        {
                AudioManager.Instance.PlaySong(1);
                playStartSong = false;
        }
    }


    void updateCurrentLevel()
    {
        string name = SceneManager.GetActiveScene().name;

        for (int i = 0; i < levels.Length; i++)
        {

            if (levels[i] == name)
            {
                _currentLevel = i;
                break;
            }
        }
        Debug.Log("current level" + _currentLevel);
    }
 

    /// <summary>
    /// Reproduce la animación y cambia a la siguiente escena.
    /// Las escenas de Niveles deben estar ordenadas en los ajustes de la build.
    /// Llama a un fade in.
    /// </summary>
    private void NextLevel()
    {
        if (_currentLevel < levels.Length)
        {
            SceneManager.UnloadSceneAsync(levels[_currentLevel]);
            _currentLevel++;
            LoadLevel(_currentLevel);
        }
        else
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void NextIterationLoad()
    {
        state = FState.Ramificado;
        StartCoroutine(FadeOut(colorRamificar));
    }

        /// <summary>
        /// Para cargar directamente un nivel.
        /// </summary>
        /// <param name="lvl">El número del nivel (en el array de levels)</param>
        public void LoadLevel(int lvl)
        {
            _currentLevel = lvl;
            SceneManager.LoadScene(levels[lvl]);
        }
        /// <summary>
        /// Queremos que al iniciar la escena, haga fade in.
        /// </summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (state == FState.Won)
            {
                StartCoroutine(FadeIn(colorWon));
            } else if (state == FState.Ramificado)
            {
                StartCoroutine(FadeIn(colorRamificar));
            }
            else
            {
                StartCoroutine(FadeIn(colorRestart));
            }
        }
        
        /// <summary>
        /// Maneja el fade in.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeIn(Color color)
        {        
            float elapsedTime = 0f;
            while (elapsedTime < fadeOutDuration)
            {          
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(1, 0, elapsedTime / fadeInDuration); // Reduce alpha
                fadeInImageColored.color = color;
                color.a = Mathf.Lerp(1, 0, elapsedTime / fadeInDurationFondo); // Reduce alpha
                fadeInImageFondo.color = color;
                yield return null;
            }
            if (CameraFollow.Instance != null)
            {
                CameraFollow.Instance.destroyGoalTransform();
            }
    }

        /// <summary>
        /// Procesa el fadeout y una vez termina, lanza la nueva escena.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeOut(Color color)
        {        
            float elapsedTime = 0f;

            float lDuration = fadeOutDuration;
            float lDurationFondo = fadeInDurationFondo;
            if (state != FState.Won)
            {
                lDuration /= 5;
                lDurationFondo /= 5;
            }
            while (elapsedTime < lDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(0, 1, elapsedTime / lDuration); // Increase alpha
                fadeInImageColored.color = color;
                color.a = Mathf.Lerp(0, 1, elapsedTime / lDurationFondo); // Increase alpha
                fadeInImageFondo.color = color;
                yield return null;
            }
            //Cutrísimo esto. Lo siento, es una jam.
            if (state == FState.Won)
            {
                NextLevel();
            }
            else
            {
                SceneManager.UnloadSceneAsync(levels[_currentLevel]);
                LoadLevel(_currentLevel);
            }
        }

        public void RestartLevel()
        {
            state = FState.Restart;
            StartCoroutine(FadeOut(colorRestart));
        }
        
        /// <summary>
        /// A ser llamado cuando el jugador gana el nivel.
        /// Activa un fade out.
        /// </summary>
        public void Won()
        {
            state = FState.Won;
            StartCoroutine(FadeOut(colorWon));
        }

        public bool checkGoalTransform()
        {
            if(lastGoal == _currentLevel) return false;
            else
            {
                lastGoal = _currentLevel;
                return true;
            }
        }

        public void LoadFromMenu(int i)
        {
            SceneManager.UnloadSceneAsync("LevelSelection");
            LoadLevel(i);
        }
    }