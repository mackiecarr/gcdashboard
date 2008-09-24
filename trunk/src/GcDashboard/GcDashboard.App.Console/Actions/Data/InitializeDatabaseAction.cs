using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SQLite;
using System.Reflection;


namespace GcDashboard.App.AdminConsole.Actions.Data
{
    [CommandAlias("Initialize Database")]
    [System.ComponentModel.Description(
        "Initialize a database file for use with GcDashboard.")]
    public class InitializeDatabaseAction : ActionBase
    {

        #region [ Properties (1) ]

        /// <summary>
        /// The filename of the database file to initialize.
        /// </summary>
        public string Filename { get; set; }

        #endregion [ Properties ]

        #region [ Public Methods (1) ]

        public override void RunHandler()
        {

            if (string.IsNullOrEmpty(Filename))
            {
                Filename = ConsoleFunctions.Prompt("Enter target filename for the database", Core.Configuration.Database.DatabaseName);
            }

            Assembly entityAsm = Assembly.Load("GcDashboard.Core.Entities");
            List<string> sqlFilenames = new List<string>(10);

            foreach (string resource in entityAsm.GetManifestResourceNames())
            {
                if (resource.EndsWith(".sql"))
                    sqlFilenames.Add(resource);
            }

            using (SQLiteConnection cn = new SQLiteConnection("Data Source=" + Filename + ";Version=3"))
            {
                cn.Open();

                foreach (string sqlScriptFilename in sqlFilenames)
                {
                    using (StreamReader reader = new StreamReader(entityAsm.GetManifestResourceStream(sqlScriptFilename)))
                    {
                        using (SQLiteCommand cmd = cn.CreateCommand())
                        {
                            cmd.CommandText = reader.ReadToEnd();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                cn.Close();
            }
        }

        #endregion [ Public Methods ]

    }

}
