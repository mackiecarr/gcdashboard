using System;

namespace GcDashboard.App.Report.RunControl
{
    public class RunControlStandard
    {

		#region [ Constructors (1) ]

        public RunControlStandard()
        {
            this.RunDateTime = DateTime.Now;
        }

		#endregion [ Constructors ]

		#region [ Fields (2) ]

        private DateTime _runDateTime;

		#endregion [ Fields ]

		#region [ Properties (1) ]

        public DateTime RunDateTime
        {
            get { return _runDateTime; }
            set { _runDateTime = value; }
        }

		#endregion [ Properties ]

    }
}
