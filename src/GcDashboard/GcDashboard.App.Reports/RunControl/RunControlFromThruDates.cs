using System;

namespace GcDashboard.App.Report.RunControl
{
    /// <summary>
    /// Used to run reports & processes for a given date.
    /// </summary>
    public class RunControlFromThruDates : RunControlStandard
    {

        #region [ Constructors (1) ]

        public RunControlFromThruDates()
            : base()
        { }

        #endregion [ Constructors ]

        #region [ Fields (6) ]

        private DateTime? _fromDate;
        private DateTime? _thruDate;

        #endregion [ Fields ]

        #region [ Properties (6) ]

        public DateTime? FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }

        public DateTime? ThruDate
        {
            get { return _thruDate; }
            set { _thruDate = value; }
        }

        #endregion [ Properties ]

    }
}
