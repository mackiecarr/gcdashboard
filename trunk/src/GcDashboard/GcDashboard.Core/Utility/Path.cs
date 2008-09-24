using System;
using System.IO;


namespace GcDashboard.Core.Utility
{

    /// <summary>
    /// List of configuration types.
    /// </summary>
    public enum ConfigFileType
    {
        /// <summary>
        /// The main application configuration file.
        /// </summary>
        Application,
        
        /// <summary>
        /// The logging configuration file.
        /// </summary>
        Logging,
    }


    /// <summary>
    /// Provides methods to allow standardization of configuration
    /// file locations.
    /// </summary>
    public static class Path
    {

        #region [ log4net (1) ]

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion [ log4net ]

        #region [ Public Methods (2) ]

        /// <summary>
        /// Provides Path combining similar to System.IO.Path.Combine(),
        /// except allows numerous paths to combine.
        /// </summary>
        public static string Combine(params string[] args)
        {
            if (args.Length > 2)
            {
                string index0and1 = System.IO.Path.Combine(args[0], args[1]);
                string[] newArgs = new string[args.Length - 1];
                newArgs[0] = index0and1;
                for (int i = 2; i < args.Length; i++)
                {
                    newArgs[i - 1] = args[i];
                }
                return Combine(newArgs);
            }
            else if (args.Length == 2)
            {
                return System.IO.Path.Combine(args[0], args[1]);
            }
            else if (args.Length == 1)
            {
                return args[0];
            }
            else
            {
                throw new InvalidOperationException("Invalid arguments");
            }
        }

        /// <summary>
        /// Find the configuration file for the given <see cref="ConfigFileType"/>.
        /// </summary>
        /// <param name="type">The type of configuration file.</param>
        /// <returns>A <see cref="FileInfo"/> object for the configuration file.</returns>
        public static FileInfo GetConfigurationFile(ConfigFileType type)
        {
            string filename = string.Empty;
            switch (type)
            {
                case ConfigFileType.Application:
                    filename = "GcDashboard.config.xml";
                    break;
                case ConfigFileType.Logging:
                    filename = "log4net.config.xml";
                    break;
            }


            //check config directory
            FileInfo f1 = new FileInfo(filename);
            if (f1.Exists)
                return f1;

            ////check environment directory
            //FileInfo f2 = new FileInfo(Combine(
            //    "C:\\ClearView\\env\\",
            //    Configuration.CommandLine.ConfigurationName,
            //    filename));

            //if (f2.Exists)
            //    return f2;
            //else
            //    log.Debug("File does not exist:  " + f2.FullName);

            return null;
        }

        #endregion [ Public Methods ]

    }

}
