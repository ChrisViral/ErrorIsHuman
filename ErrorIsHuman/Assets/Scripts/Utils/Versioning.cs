using System;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace ErrorIsHuman.Utils
{
    /// <summary>
    /// Static class holding the game's Version info
    /// </summary>
    public static class Versioning
    {
        #region Constants
        /// <summary>
        /// Name of the BuildID file
        /// </summary>
        public const string FileName = "errorishuman.build";
        /// <summary>
        /// Format of the saved date time
        /// </summary>
        public const string TimeFormat = "dd/MM/yyyy-HH:mm:ss";
        /// <summary>
        /// Separator of the build time and version number
        /// </summary>
        public static readonly string[] Delimiter = { "UTC | v" };
        #endregion

        #region Static properties
        /// <summary>
        /// Major version number
        /// </summary>
        public static int Major { get; }
        /// <summary>
        /// Minor version number
        /// </summary>
        public static int Minor { get; }
        /// <summary>
        /// Build version number
        /// </summary>
        public static int Build { get; }
        /// <summary>
        /// Revision version number
        /// </summary>
        public static int Revision { get; }
        /// <summary>
        /// Full version string
        /// </summary>
        public static string VersionString { get; }
        /// <summary>
        /// Current game version
        /// </summary>
        public static Version Version { get; }
        /// <summary>
        /// UTC time of the Build
        /// </summary>
        public static DateTime BuildTime { get; }
        #endregion

        #region Constructors
        /// <summary>
        /// Loads the current game version from the BuildID file
        /// </summary>
        static Versioning()
        {
            //Get file path
            string path = Path.Combine(Application.dataPath, FileName);
            
            //Check if file exists
            if (!File.Exists(path)) { throw new FileNotFoundException("Game version file could not be found", path); }
            try
            {
                //Read version info from file
                string[] info = File.ReadAllLines(path)[0].Trim().Split(Delimiter, StringSplitOptions.RemoveEmptyEntries);
                BuildTime = DateTime.SpecifyKind(DateTime.ParseExact(info[0].Trim(), TimeFormat, CultureInfo.InvariantCulture), DateTimeKind.Utc);
                VersionString = info[1].Trim();
                Version = new Version(VersionString);

                //Get version fields
                Major    = Version.Major;
                Minor    = Version.Minor;
                Build    = Version.Build;
                Revision = Version.Revision;
            }
            catch (Exception e) 
            {
                //Throw if something goes wrong
                throw new FileLoadException("Game version file could not be read properly", path, e);
            }
        }
        #endregion
    }
}