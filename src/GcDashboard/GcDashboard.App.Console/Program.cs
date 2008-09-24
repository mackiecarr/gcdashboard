using System;
using System.IO;

using GcDashboard.Core.Entities;


namespace GcDashboard.App.AdminConsole
{

    class Program
    {

        #region [ log4net (1) ]

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion [ log4net ]

        #region [ Fields (1) ]

        public static Exception LastThrownException;

        #endregion [ Fields ]

        #region [ Private Static Methods (2) ]

        static void Main(string[] args)
        {
            //greeting
            Console.WriteLine(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
            Console.WriteLine("Version: {0}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            Console.WriteLine();
            Console.WriteLine();


            bool validConfig = false;

            /*
            while (!validConfig)
            {
                //prompt for config name
                string configName = ConsoleFunctions.Prompt("Enter a configuration name");

                //set configuration option
                Core.Configuration.CommandLine.ConfigurationName = configName;

                //check non-default config
                FileInfo configFilename = Core.Utility.Path.GetConfigurationFile(GcDashboard.Core.Utility.ConfigFileType.Application);
                if (configFilename.Name.StartsWith("default"))
                {
                    Console.WriteLine("You have entered an invalid configuration name.");
                }
                else
                {
                    validConfig = true;
                }
            }
            */

            // configure logging
            FileInfo logConfigFile = Core.Utility.Path.GetConfigurationFile(
                Core.Utility.ConfigFileType.Logging);
            log4net.Config.XmlConfigurator.Configure(logConfigFile);
            log.Info("SysUtility started.");
            log.Debug("Using log configuration:  " + logConfigFile.FullName);


            //login
            Console.WriteLine("Welcome.");
            Console.WriteLine();
            //string s = ConsoleFunctions.Prompt("Password", true);

            //if (!Core.Utility.Md5.AreMatches(s, "1a31a6f65cc993ff6bd9a5b85f0520b0"))
            //    return;

            Console.WriteLine();
            Console.WriteLine();

            while (ShowPrompt() != 1)
            { }
        }

        private static int ShowPrompt()
        {
            Console.Write(">");
            string command = Console.ReadLine();

            string typeName = ActionAliasHandler.Translate(command);

            if (string.IsNullOrEmpty(typeName))
                typeName = string.Format("GcDashboard.App.AdminConsole.Actions.{0}Action", command);

            Type t = Type.GetType(typeName, false, true);

            if (t == null)
            {
                Console.WriteLine("Unknown command.");
                Console.WriteLine();
                return 0;
            }

            object o = Activator.CreateInstance(t);
            IActionHandler actionhandler = o as IActionHandler;

            if (actionhandler == null)
            {
                Console.WriteLine("Bad command.");
                Console.WriteLine();
                return 0;
            }
            else
            {
                return actionhandler.Run();
            }
        }

        #endregion [ Private Static Methods ]

    }

}

