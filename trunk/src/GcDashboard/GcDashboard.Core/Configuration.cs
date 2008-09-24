using System;
using System.IO;
using Nini;
using Nini.Config;
using GcDashboard.Core.Config;


namespace GcDashboard.Core
{

    /// <summary>
    /// The configuration class provides an easy to use
    /// interface into the configuration classes.
    /// </summary>
    public static class Configuration
    {

        #region [ log4net (1) ]

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion [ log4net ]

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes the <see cref="Configuration"/> class.
        /// </summary>
        /// <remarks>
        /// By default, Configuration only loads the command line
        /// arguments.  When an attempt is made to access a
        /// configuration class, at that time the configuration
        /// file is parsed and loaded.
        /// </remarks>
        static Configuration()
        { }

        #endregion [ Constructors ]

        #region [ Fields (9) ]

        private static bool _newFile = false;
        private static bool _configInitialized = false;
        private static GeneralSettings _generalSettings = null;
        private static DatabaseSettings _databaseSettings = null;
        //private static PathSettings _pathSettings = null;
        private static string _configFilename = null;
        private static XmlConfigSource _configSource = null;

        #endregion [ Fields ]

        #region [ Properties (6) ]

        /// <summary>
        /// Gets the full path to the configuration file
        /// that is loaded.
        /// </summary>
        public static string ConfigurationFilename
        {
            get { return _configFilename; }
        }

        /// <summary>
        /// Exposes the database settings class.
        /// </summary>
        public static DatabaseSettings Database
        {
            get { Init(); return _databaseSettings; }
            set { Init(); _databaseSettings = value; }
        }

        /// <summary>
        /// Exposes the General settings class.
        /// </summary>
        public static GeneralSettings General
        {
            get { Init(); return _generalSettings; }
            set { Init(); _generalSettings = value; }
        }

        ///// <summary>
        ///// Exposes the path settings class.
        ///// </summary>
        //public static PathSettings Path
        //{
        //    get { Init(); return _pathSettings; }
        //    set { Init(); _pathSettings = value; }
        //}

        #endregion [ Properties ]

        #region [ Public Methods (4) ]

        /// <summary>
        /// Resets the configuration values and loads
        /// the values from the configuration file.
        /// </summary>
        public static void Reload()
        {
            _configSource.Reload();
        }

        /// <summary>
        /// Reset the configuration and reload it.
        /// </summary>
        public static void Reset()
        {
            _configSource = null;
            _configInitialized = false;
            Init();
        }

        /// <summary>
        /// Saves the configuration settings.
        /// </summary>
        public static void Save()
        {
            Init();
            if (_newFile)
            {
                Save(_configFilename);
            }
            else
            {
                _configSource.Save();
            }
        }

        /// <summary>
        /// Saves the configuration settings to
        /// the file given.
        /// </summary>
        /// <param name="filename">The full path
        /// to the file to save to.</param>
        public static void Save(string filename)
        {
            _configSource.Save(filename);
        }

        #endregion [ Public Methods ]

        #region [ Private Static Methods (2) ]

        /// <summary>
        /// Used to initialize the configuration file.
        /// </summary>
        private static void Init()
        {
            if (_configInitialized)
                return;

            FileInfo configFile = Utility.Path.GetConfigurationFile(
                Utility.ConfigFileType.Application);
            log.DebugFormat("Using configuration:  {0}", configFile.FullName);

            Init(configFile);

            _configInitialized = true;
        }

        /// <summary>
        /// Helper method that performs the actual
        /// file initialization.
        /// </summary>
        /// <param name="file">File to save and load from.</param>
        private static void Init(FileInfo file)
        {
            if (!file.Exists)
            {
                _newFile = true;
                _configSource = new XmlConfigSource();
            }
            else
            {
                _newFile = false;
                _configSource = new XmlConfigSource(file.FullName);
            }

            _configFilename = file.FullName;

            //init Config classes here...

            _generalSettings = new GeneralSettings(_configSource);
            _databaseSettings = new DatabaseSettings(_configSource);
            //_pathSettings = new PathSettings(_configSource);
        }

        #endregion [ Private Static Methods ]

    }

}
