using System;
using System.Collections.Generic;
using NHibernate;


namespace GcDashboard.Core.Data
{

    /// <summary>
    /// Manages database sessions.
    /// </summary>
    public static class SessionManager
    {

        #region [ log4net (1) ]

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion [ log4net ]

        #region [ Constants (2) ]

        /// <summary>
        /// Maximum number of sessions before an exception
        /// is thrown when new sessions are requested.
        /// </summary>
        public const int SessionMax = 30;

        /// <summary>
        /// Name of the system session.
        /// </summary>
        public const string SystemSessionName = "SystemSession";

        #endregion [ Constants ]

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes the <see cref="SessionManager"/>.
        /// </summary>
        static SessionManager()
        {
            _sessions = new Dictionary<int, ISession>(SessionMax);
            _sessionNames = new Dictionary<string, int>(SessionMax);
            //_sessionHeaders = new List<SessionHeader>(SessionMax);
            _systemSessionNumber = GetSession(SystemSessionName);
        }

        #endregion [ Constructors ]

        #region [ Fields (5) ]

        private static Dictionary<int, ISession> _sessions = null;
        private static Dictionary<string, int> _sessionNames = null;
        private static int _lastSessionNumber = -1;
        private static int _systemSessionNumber = -1;
        //private static List<SessionHeader> _sessionHeaders = null;
        private static string _systemSessionUser = string.Empty;
        private static bool _systemSessionInUse = false;

        #endregion [ Fields ]

        #region [ Properties (3) ]

        /// <summary>
        /// Lists the names of the current open sessions.
        /// </summary>
        /// <remarks>
        /// Useful in troubleshooting if sessions are being disposed of properly,
        /// this returns a list of the sessions that the <see cref="SessionManager"/>
        /// has open.
        /// </remarks>
        public static List<string> OpenSessionNames
        {
            get
            {
                List<string> names = new List<string>(_sessionNames.Count);

                foreach (KeyValuePair<string, int> entry in _sessionNames)
                {
                    names.Add(string.Format("Session:  {0,-4} Owner: {1}", entry.Value, entry.Key));
                }

                return names;
            }
        }

        /// <summary>
        /// Gets the number of open sessions.
        /// </summary>
        public static int OpenSessions
        {
            get { return _sessions.Count; }
        }

        /// <summary>
        /// Gets the session number for the System session.
        /// </summary>
        /// <remarks>
        /// The System Session is a gloabl session that is used to
        /// perform application work.  For more information, see
        /// <see cref="SystemSessionNumber"/>.
        /// </remarks>
        public static int SystemSessionNumber
        {
            get { return _systemSessionNumber; }
        }

        #endregion [ Properties ]

        #region [ Public Methods (9) ]

        /// <summary>
        /// Evicts the object from the session.
        /// </summary>
        /// <remarks>
        /// For more information, see NHibernate documentation.
        /// </remarks>
        public static void Evict(int sessionNumber, object o)
        {
            GetSession(sessionNumber).Evict(o);
        }

        /// <summary>
        /// Finds the session number for the given name.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no matching session is found.</exception>
        /// <param name="name">The name of the session to locate.</param>
        /// <returns>The session number for the name.</returns>
        public static int FindSession(string name)
        {
            if (_sessionNames.ContainsKey(name))
                return _sessionNames[name];
            else
                throw new InvalidOperationException("Page does not own a session.");
        }

        /// <summary>
        /// Gets a session under the requested name.  If no such session exists,
        /// a new session is created.
        /// </summary>
        /// <param name="name">The name for the session.</param>
        /// <returns>The session number for the name.</returns>
        public static int GetSession(string name)
        {
            if (_sessionNames.ContainsKey(name))
                return FindSession(name);
            else
                return OpenSession(name);
        }

        /// <summary>
        /// Returns a flag indicating whether the session contains changes
        /// that have not been persisted to the database.
        /// </summary>
        public static bool IsDirty(int session)
        {
            try
            {
                ISession s = GetSession(session);
                return s.IsDirty();
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Returns a flag indicating whether the session is open and available.
        /// </summary>
        public static bool IsValidSession(int session)
        {
            try
            {
                ISession s = GetSession(session);
                if (s != null)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }



        /// <summary>
        /// Closes a session.
        /// </summary>
        /// <param name="sessionNumber">The session number to close.</param>
        public static void ReleaseSession(int sessionNumber)
        {
            Connection.DestorySession(_sessions[sessionNumber], false);
            _sessions.Remove(sessionNumber);

            string name = null;

            foreach (KeyValuePair<string, int> entry in _sessionNames)
            {
                if (entry.Value == sessionNumber)
                    name = entry.Key;
            }

            if (!string.IsNullOrEmpty(name))
                _sessionNames.Remove(name);
        }

        /// <summary>
        /// Determines if the given name owns a session.
        /// </summary>
        /// <param name="name">The name of the session.</param>
        /// <returns>True if a session exists for the given name; otherwise, false.</returns>
        public static bool SessionExists(string name)
        {
            return _sessionNames.ContainsKey(name);
        }

        /// <summary>
        /// Marks the system session in use.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The system session number.</returns>
        public static int UseSystemSession(string name)
        {
            log.InfoFormat("System session in use by '{0}'", name);
            return _systemSessionNumber;
        }

        #endregion [ Public Methods ]

        #region [ Private Static Methods (3) ]

        internal static void Reset()
        {
            //BUG: ReleaseSession modifies the _sessionNames dictionary
            //     which then breaks the enumerator.

            //get all current session numbers
            List<int> sessionNumbers = new List<int>(_sessions.Count);
            foreach (KeyValuePair<string, int> entry in _sessionNames)
            {
                sessionNumbers.Add(entry.Value);
            }
            foreach (int i in sessionNumbers)
            {
                ReleaseSession(i);
            }

            _sessions.Clear();
            _sessionNames.Clear();
            _lastSessionNumber = -1;
            _systemSessionNumber = -1;
            _systemSessionUser = string.Empty;

            _sessions = new Dictionary<int, ISession>(SessionMax);
            _sessionNames = new Dictionary<string, int>(SessionMax);
            _systemSessionNumber = GetSession("SystemSession");
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="name">The name of the session.</param>
        /// <returns>The session number of the newly created session.</returns>
        internal static int OpenSession(string name)
        {
            try
            {
                log.DebugFormat("OpenSession requested for {0}", name);
            }
            catch (Exception)
            { }

            if (_sessions.Count + 1 > SessionMax)
            {
                log.DebugFormat("There are {0} sessions in use already", _sessions.Count);
                throw new InvalidOperationException("No sessions available.");
            }

            log.InfoFormat("Opening session for {0}", name);

            ISession session = Connection.CreateSession();
            _lastSessionNumber++;
            int sessionNumber = _lastSessionNumber;

            log.DebugFormat("Attempting to add session {0}", sessionNumber);

            _sessions.Add(sessionNumber, session);
            _sessionNames.Add(name, sessionNumber);

            return sessionNumber;
        }

        /// <summary>
        /// Gets the <see cref="ISession"/> for the given session number.
        /// </summary>
        /// <param name="sessionNumber">The session number to use.</param>
        /// <returns>The <see cref="ISession"/> for the given number.</returns>
        internal static ISession GetSession(int sessionNumber)
        {
            if (sessionNumber < 0)  // any negitive number is a substitute for system session number
            {
                return _sessions[_systemSessionNumber];
            }
            else
                return _sessions[sessionNumber];
        }

        #endregion [ Private Static Methods ]

    }

}
