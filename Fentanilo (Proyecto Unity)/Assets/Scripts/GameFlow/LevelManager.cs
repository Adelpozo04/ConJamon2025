using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        public enum FState
        {
            Won, //Cuando el jugador gane la partida
            Restart, //Cuando se reinicia el 
            Ramificado //Cuando el enemigo le da a la c o muere
        }

        private FState state;
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

        //El awake asegura que la instancia es única y que no se el objeto no se destruye al cambiar de escena
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                state = FState.Restart;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// A ser llamado cuando el jugador gana el nivel.
        /// Activa un fade out.
        /// </summary>
        public void Won()
        {
            SetState(FState.Won);
            StartCoroutine(FadeOut(colorWon));
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
                _currentLevel++;
                SceneManager.LoadScene(levels[_currentLevel]);
            }
            else
            {
                SceneManager.LoadScene("MainMenu");
            }
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
        }

        /// <summary>
        /// Procesa el fadeout y una vez termina, lanza la nueva escena.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeOut(Color color)
        {
            float elapsedTime = 0f;

            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(0, 1, elapsedTime / fadeOutDuration); // Reduce alpha
                fadeInImageColored.color = color;
                color.a = Mathf.Lerp(0, 1, elapsedTime / fadeOutDurationFondo); // Reduce alpha
                fadeInImageFondo.color = color;
                yield return null;
            }
            if(state == FState.Won) 
                NextLevel();
        }
        
        //Asigna la situación para que el level manager sepa que tipo de transición hacer
        public void SetState(FState state)
        {
            this.state = state;
        }
    }