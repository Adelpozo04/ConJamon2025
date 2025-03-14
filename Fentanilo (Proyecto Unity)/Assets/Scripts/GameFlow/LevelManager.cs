using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private Image fadeInImage;
        [SerializeField] private float fadeInDuration;
        [SerializeField] private float fadeOutDuration;

        public static LevelManager Instance { get; private set; }

        //El awake asegura que la instancia es única y que no se el objeto no se destruye al cambiar de escena
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
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
            StartCoroutine(FadeOut());
        }
        /// <summary>
        /// Reproduce la animación y cambia a la siguiente escena.
        /// Las escenas de Niveles deben estar ordenadas en los ajustes de la build.
        /// Llama a un fade in.
        /// </summary>
        private void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        /// <summary>
        /// Queremos que al iniciar la escena, haga fade in.
        /// </summary>
        private void OnEnable()
        {
            StartCoroutine(FadeIn());
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(FadeIn());
        }
        
        /// <summary>
        /// Manja el fade in.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeIn()
        {
            float elapsedTime = 0f;
            Color color = fadeInImage.color;
            
            while (elapsedTime < fadeOutDuration)
            {          
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(1, 0, elapsedTime / fadeInDuration); // Reduce alpha
                fadeInImage.color = color;
                yield return null;
            }
        }
        /// <summary>
        /// Procesa el fadeout y una vez termina, lanza la nueva escena.
        /// </summary>
        /// <returns></returns>
        private IEnumerator FadeOut()
        {
            float elapsedTime = 0f;
            Color color = fadeInImage.color;
            
            while (elapsedTime < fadeOutDuration)
            {
                elapsedTime += Time.deltaTime;
                color.a = Mathf.Lerp(0, 1, elapsedTime / fadeInDuration); // Reduce alpha
                fadeInImage.color = color;
                yield return null;
            }
            NextLevel();
        }

    }