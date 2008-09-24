using System;
using Nini;
using Nini.Config;


namespace GcDashboard.Core.Config
{

    /// <summary>
    /// Provides general settings in a typesafe manner.
    /// </summary>
    /// <remarks>
    /// This class, like most of the classes in the GcDashboard.Core.Config
    /// namespace, provides a layer of abstraction between Nini and the
    /// application.  This class will interact with Nini to read and
    /// write settings and provide them to the application using .Net
    /// types.
    /// </remarks>
    public class GeneralSettings
    {

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralSettings"/> class.
        /// </summary>
        /// <remarks>
        /// Internal to prevent construction of this class by any other
        /// assembly.  This class is intended to be constructed by the
        /// <see cref="Configuration"/> class only.
        /// </remarks>
        /// <param name="configurationSource">Reference to the
        /// configuration source that contains the configuration file.</param>
        internal GeneralSettings(XmlConfigSource configurationSource)
        {
            _config = configurationSource.Configs["General"];

            if (_config == null)
            {
                _config = configurationSource.AddConfig("General");
                SetDefaults();
            }
        }

        #endregion [ Constructors ]

        #region [ Fields (1) ]

        //holds a reference to the Database section
        private IConfig _config = null;

        #endregion [ Fields ]

        #region [ Properties (4) ]

        /// <summary>
        /// The character used to seperate accounts when showing the full name
        /// for an account.
        /// </summary>
        public char AccountSeperator
        {
            get
            {
                string c = _config.Get("AccountSeperator");

                if (c.Length > 1)
                    throw new InvalidOperationException("AccountSeperator cannot be longer than one character.");

                return c.ToCharArray()[0];
            }
            set { _config.Set("AccountSeperator", value); }
        }

        #endregion [ Properties ]

        #region [ Private Methods (1) ]

        private void SetDefaults()
        {
            AccountSeperator = '.';
        }

        #endregion [ Private Methods ]

    }

}
