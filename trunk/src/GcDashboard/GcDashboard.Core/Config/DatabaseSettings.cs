using System;
using Nini;
using Nini.Config;


namespace GcDashboard.Core.Config
{
    
    /// <summary>
    /// Provides database settings in a typesafe manner.
    /// </summary>
    /// <remarks>
    /// This class, like most of the classes in the ClearView.Core.Config
    /// namespace, provides a layer of abstraction between Nini and the
    /// application.  This class will interact with Nini to read and
    /// write settings and provide them to the application using .Net
    /// types.
    /// </remarks>
    public class DatabaseSettings
    {

        #region [ Constants (1) ]

        //used to perform encryption
        private const string _encryptionKey = "f92h3t0ing";

        #endregion [ Constants ]

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseSettings"/> class.
        /// </summary>
        /// <remarks>
        /// Internal to prevent construction of this class by any other
        /// assembly.  This class is intended to be constructed by the
        /// <see cref="Configuration"/> class only.
        /// </remarks>
        /// <param name="configurationSource">Reference to the
        /// configuration source that contains the configuration file.</param>
        internal DatabaseSettings(XmlConfigSource configurationSource)
        {
            _config = configurationSource.Configs["Database"];

            if (_config == null)
            {
                _config = configurationSource.AddConfig("Database");
                SetDefaults();
            }
        }

        #endregion [ Constructors ]

        #region [ Fields (1) ]

        //holds a reference to the Database section
        private IConfig _config = null;

        #endregion [ Fields ]

        #region [ Properties (8) ]

        /// <summary>
        /// Gets or sets the password used to make
        /// a connection to the database.
        /// </summary>
        public string ConnectionPassword
        {
            get
            {
                string read = _config.GetString("ConnectionPassword");
                string decrypted = Utility.Encryption.Decrypt(read, _encryptionKey);
                return decrypted;
            }
            set
            {
                string encrypted = Utility.Encryption.Encrypt(value, _encryptionKey);
                _config.Set("ConnectionPassword", encrypted);
            }
        }

        /// <summary>
        /// Gets or sets the base of the connection string.
        /// </summary>
        public string ConnectionStringTemplate
        {
            get
            {
                return _config.GetString("ConnectionStringTemplate");
            }
            set
            {
                _config.Set("ConnectionStringTemplate", value);
            }
        }

        /// <summary>
        /// Gets or sets the Username used to make
        /// a connection to the database.
        /// </summary>
        public string ConnectionUsername
        {
            get
            {
                return _config.GetString("ConnectionUsername");
            }
            set
            {
                _config.Set("ConnectionUsername", value);
            }
        }

        /// <summary>
        /// Gets or sets the Name of the database.
        /// </summary>
        public string DatabaseName
        {
            get
            {
                return _config.GetString("DatabaseName");
            }
            set
            {
                _config.Set("DatabaseName", value);
            }
        }

        /// <summary>
        /// Gets / Sets the name of the connection provider that
        /// NHibernate will use.
        /// </summary>
        public string NhConnectionProvider
        {
            get { return _config.GetString("NhConnectionProvider"); }
            set { _config.Set("NhConnectionProvier", value); }
        }

        /// <summary>
        /// Gets / Sets the name of the dialect that NHibernate will
        /// use.
        /// </summary>
        public string NhDialect
        {
            get { return _config.GetString("NhDialect"); }
            set { _config.Set("NhDialect", value); }
        }

        /// <summary>
        /// Gets / Sets the name of the driver that NHibernate
        /// will use.
        /// </summary>
        public string NhDriverClass
        {
            get { return _config.GetString("NhDriverClass"); }
            set { _config.Set("NhDriverClass", value); }
        }

        /// <summary>
        /// Gets / Sets a value indicating whether NHibernate will
        /// show Sql statements.
        /// </summary>
        public string NhShowSql
        {
            get { return _config.GetString("NhShowSql"); }
            set { _config.Set("NhShowSql", value); }
        }

        #endregion [ Properties ]

        #region [ Private Methods (1) ]

        private void SetDefaults()
        {
            this.DatabaseName = string.Empty;
            this.ConnectionPassword = string.Empty;
            this.ConnectionStringTemplate = string.Empty;
            this.ConnectionUsername = string.Empty;
        }

        #endregion [ Private Methods ]

    }

}
