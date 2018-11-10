using System;
using System.Globalization;
using System.IO;
using ErrorIsHuman.Utils;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace ErrorIsHuman.Editor
{
    /// <summary>
    /// Static class containing helper methods for the Unity editor with build versioning
    /// </summary>
    [InitializeOnLoad]
    public static class BuildHelper
    {
        #region Constants
        /// <summary>
        /// Readme file name
        /// </summary>
        private const string readme  = "readme.txt";
        /// <summary>
        /// Credits file name
        /// </summary>
        private const string credits = "credits.txt";

        /// <summary>
        /// File path of the BuildID
        /// </summary>
        private static readonly string filePath    = Path.Combine(Application.dataPath, Versioning.FileName);
        /// <summary>
        /// Readme file path
        /// </summary>
        private static readonly string readmePath  = Path.Combine(Application.dataPath, readme);
        /// <summary>
        /// Credits file path
        /// </summary>
        private static readonly string creditsPath = Path.Combine(Application.dataPath, credits);
        #endregion

        #region Static fields
        //Version number
        private static int major, minor, build, revision;
        //Last build date
        private static string date;
        #endregion

        #region Static properties
        /// <summary>
        /// Version number of the game
        /// </summary>
        public static string Version => $"{major}.{minor}.{build}.{revision}";

        /// <summary>
        /// Current DateTime string, correctly formatted
        /// </summary>
        public static string CurrentDate => DateTime.UtcNow.ToString(Versioning.TimeFormat, CultureInfo.InvariantCulture);
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes BuildID components
        /// </summary>
        static BuildHelper()
        {
            //Check if file exists
            if (File.Exists(filePath))
            {
                try
                {
                    //Try and load file info
                    string[] info = File.ReadAllLines(filePath)[0].Trim().Split(Versioning.Delimiter, StringSplitOptions.RemoveEmptyEntries);
                    date = info[0];

                    Version version = new Version(info[1]);
                    major    = version.Major;
                    minor    = version.Minor;
                    build    = version.Build;
                    revision = version.Revision;
                }
                //Throw if something went wrong
                catch (Exception e) { throw new FileLoadException("Game version file could not be read properly", filePath, e); }
               
            }
            else
            {
                //Else set the version to 0.1.0.0
                minor = 1;
                date = CurrentDate;
            }
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Runs after each game build process, and saves the new BuildID to the game folder
        /// </summary>
        /// <param name="target">Target platform of the build</param>
        /// <param name="pathToBuild">Full file path of the build executable</param>
        [PostProcessBuild]
        public static void OnBuild(BuildTarget target, string pathToBuild)
        {
            if (string.IsNullOrEmpty(pathToBuild)) { return; }

            //Get correct build folder
            string buildDataFolder;
            switch (target)
            {
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneWindows64:
                    //On windows it is ApplicationName_Data/
                    buildDataFolder = Path.ChangeExtension(pathToBuild, null) + "_Data";
                    pathToBuild = Path.GetDirectoryName(pathToBuild);
                    break;

                case BuildTarget.StandaloneOSX:
                    //On OSX it is Contents/
                    pathToBuild = Path.ChangeExtension(pathToBuild, null) + ".app";
                    buildDataFolder = Path.Combine(pathToBuild, "Contents");
                    break;

                default:
                    //No support for other platforms
                    return;
            }

            //Setup build file
            date = CurrentDate;
            string version = Version;
            string[] lines =  { date + Versioning.Delimiter[0] + version };

            //Write build file to current and build folder
            File.WriteAllLines(filePath, lines);

            //Write build file to build folder
            File.WriteAllLines(Path.Combine(buildDataFolder, Versioning.FileName), lines);

            //Make sure the build folder is valid
            if (string.IsNullOrEmpty(pathToBuild))
            {
                Debug.Log("[BuildHelper]: Could not find build folder");
                return;
            }

            //Copy additional files
            File.WriteAllBytes(Path.Combine(pathToBuild, readme),  File.ReadAllBytes(readmePath));
            File.WriteAllBytes(Path.Combine(pathToBuild, credits), File.ReadAllBytes(creditsPath));

            //Log result
            Debug.Log($"[BuildHelper]: Built v{version}, at {date} UTC, for {EnumUtils.GetNameTitleCase(target)}");
        }

        /// <summary>
        /// Increments the Major version number
        /// </summary>
        [MenuItem("Version/Increase/Major")]
        public static void IncreaseMajor()
        {
            major++;
            LogVersion();
        }

        /// <summary>
        /// Increments the Minor version number
        /// </summary>
        [MenuItem("Version/Increase/Minor")]
        public static void IncreaseMinor()
        {
            minor++;
            LogVersion();
        }

        /// <summary>
        /// Increments the Build version number
        /// </summary>
        [MenuItem("Version/Increase/Build")]
        public static void IncreaseBuild()
        {
            build++;
            LogVersion();
        }
        
        /// <summary>
        /// Increments the Revision version number
        /// </summary>
        public static void IncreaseRevision() => revision++;

        /// <summary>
        /// Decreases the Revision version number
        /// </summary>
        public static void DecreaseRevision() => revision--;

        /// <summary>
        /// Resets the Major version number to zero
        /// </summary>
        [MenuItem("Version/Reset/Major")]
        public static void ResetMajor()
        {
            major = 0;
            LogVersion();
        }

        /// <summary>
        /// Resets the Minor version number to zero
        /// </summary>
        [MenuItem("Version/Reset/Minor")]
        public static void ResetMinor()
        {
            minor = 0;
            LogVersion();
        }

        /// <summary>
        /// Resets the Build version number to zero
        /// </summary>
        [MenuItem("Version/Reset/Build")]
        public static void ResetBuild()
        {
            build = 0;
            LogVersion();
        }

        /// <summary>
        /// Resets the Revision version number to zero
        /// </summary>
        [MenuItem("Version/Reset/Revision")]
        public static void ResetRevision()
        {
            revision = 0;
            LogVersion();
        }

        /// <summary>
        /// Logs the current BuildID
        /// </summary>
        [MenuItem("Version/Log Version")]
        private static void LogVersion() => Debug.Log($"[BuildHelper]: Current version: v{Version}, last built on {date} UTC");
        #endregion
    }
}