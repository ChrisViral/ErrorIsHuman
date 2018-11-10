using System;
using DG.Tweening;
using ErrorIsHuman.Base;
using ErrorIsHuman.Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ErrorIsHuman
{
    /// <summary>
    /// GameScenes
    /// </summary>
    public enum GameScenes
    {
        MENU = 0
    }
    
    /// <summary>
    /// Controls game wide related logic
    /// </summary>
    /// <inheritdoc/>
    [RequireComponent(typeof(AudioSource))]
    public class GameLogic : Singleton<GameLogic>
    {
        #region Events
        /// <summary>
        /// On game pause event
        /// </summary>
        public static event Action<bool> OnPause;
        #endregion

        #region Constants
        /// <summary>
        /// Pause button axis
        /// </summary>
        private const string pauseButton = "Pause";
        #endregion

        #region Fields
        //Inspector fields
        [SerializeField, Header("Music")]
        private AudioClip[] music;

        //Private fields
        private AudioSource source;
        #endregion

        #region Static properties
        /// <summary>
        /// Current loaded scene
        /// </summary>
        public static GameScenes LoadedScene { get; private set; }

        private static bool isPaused;
        /// <summary>
        /// If the game is currently paused
        /// </summary>
        public static bool IsPaused
        {
            get => isPaused;
            internal set
            {
                //Check if the value has changed
                if (isPaused != value)
                {
                    //Set value and stop Unity time
                    isPaused = value;
                    Time.timeScale = isPaused ? 0f : 1f;

                    //Log current state
                    Instance.Log($"Game {(isPaused ? "paused" : "unpaused")}");

                    //Fire pause event
                    OnPause?.Invoke(isPaused);
                }
            }
        }

        /// <summary>
        /// If the current loaded scene is a game scene
        /// </summary>
        public static bool IsGame => LoadedScene != GameScenes.MENU;
        #endregion

        #region Static methods
        /// <summary>
        /// Loads a given scene
        /// </summary>
        /// <param name="scene">Scene to load</param>
        internal static void LoadScene(GameScenes scene) => SceneManager.LoadScene((int)scene);

        /// <summary>
        /// Reloads the current scene
        /// </summary>
        internal static void ReloadScene() => LoadScene(LoadedScene);

        /// <summary>
        /// Closes the instance of the game, regardless of the current play mode
        /// </summary>
        internal static void Quit()
        {
            Instance.Log("Quitting game...");

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
        #endregion

        #region Methods
        /// <summary>
        /// Game scene loaded event
        /// </summary>
        /// <param name="scene">Loaded scene</param>
        /// <param name="mode">Load mode</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            GameScenes loadedScene = (GameScenes)scene.buildIndex;

            //Play level music if possible
            if (this.music.Length > scene.buildIndex)
            {
                AudioClip clip = this.music[scene.buildIndex];
                if (clip != null)
                {
                    this.source.clip = clip;
                    this.source.Play();
                }
            }

            //Scene specific tasks
            switch (loadedScene)
            {
                case GameScenes.MENU:
                    if (IsPaused) { IsPaused = false; }
                    break;
            }

            //Log scene change
            this.Log($"Scene loaded - {EnumUtils.GetNameTitleCase(loadedScene)}");
            LoadedScene = loadedScene;
        }
        #endregion
      
        #region Functions
        protected override void OnAwake()
        {
            //Opening message
            this.Log("Running Viral Curse v" + Versioning.VersionString);

            //Add scene load event
            SceneManager.sceneLoaded += OnSceneLoaded;

            //Setup audio
            this.source = GetComponent<AudioSource>();
            this.source.loop = true;

            //Setup DOTween
            DOTween.Init(true, true, LogBehaviour.Verbose);
        }

        private void Update()
        {
            //Pauses the game
            if (!IsPaused && LoadedScene != GameScenes.MENU && Input.GetButtonDown(pauseButton))
            {
                IsPaused = true;
            }
        }
        
        //Make sure to remove events
        private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;
        #endregion
    }
}
