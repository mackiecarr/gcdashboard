using System;


namespace GcDashboard.App.AdminConsole
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
    public class CommandAliasAttribute : System.Attribute
    {

		#region [ Constructors (1) ]

        public CommandAliasAttribute(string alias)
        {
            Alias = alias;
        }

		#endregion [ Constructors ]

		#region [ Fields (1) ]

        public readonly string Alias;

		#endregion [ Fields ]

    }

}
