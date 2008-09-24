using System;

namespace GcDashboard.App.Report.RunControl
{
    /// <summary>
    /// Used to run reports & processes for a given date.
    /// </summary>
    public class RunControlAsOfDate : RunControlStandard
    {

		#region [ Constructors (1) ]

        public RunControlAsOfDate()
            : base()
        { }

		#endregion [ Constructors ]

		#region [ Fields (1) ]

        private DateTime? _asOfDate;

		#endregion [ Fields ]

		#region [ Properties (1) ]

        public DateTime? AsOfDate
        {
            get { return _asOfDate ; }
            set { _asOfDate = value; }
        }

		#endregion [ Properties ]

    }
}
