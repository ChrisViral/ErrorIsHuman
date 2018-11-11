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
        MENU = 0,
        GAME = 1
    }
    
    /// <summary>
    /// Controls game wide related logic
    /// </summary>
    /// <inheritdoc/>
    [RequireComponent(typeof(AudioSource))]
    public class GameLogic : Singleton<GameLogic>
    {
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

        /// <summary>
        /// If the current loaded scene is a game scene
        /// </summary>
        public static bool IsGame => LoadedScene != GameScenes.MENU;

        private static bool visible = true;
        public static bool CursorVisible
        {
            get => visible;
            set
            {
                visible = value;
                Cursor.visible = visible;
            }
        }
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

            switch (loadedScene)
            {
                case GameScenes.MENU:
                    CursorVisible = true;
                    break;

                case GameScenes.GAME:
                    this.Log("turning cursor off");
                    CursorVisible = false;
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
            this.Log("Running Error Is Human v" + Versioning.VersionString);

            //Add scene load event
            SceneManager.sceneLoaded += OnSceneLoaded;

            //Setup audio
            this.source = GetComponent<AudioSource>();
            this.source.loop = true;

            //Setup DOTween
            DOTween.Init(true, true, LogBehaviour.Verbose);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (hasFocus) { Cursor.visible = CursorVisible; }
        }

        private void Update()
        {
            if (!CursorVisible && Cursor.visible) { Cursor.visible = false; }
        }

        //Make sure to remove events
        private void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;
        #endregion
    }
}