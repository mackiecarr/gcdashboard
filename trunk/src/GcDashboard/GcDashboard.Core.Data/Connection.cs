using System;
using NHibernate;


namespace GcDashboard.Core.Data
{

    /// <summary>
    /// Provides a wrapper around NHibernate.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Connection"/> class provides
    /// configuration and creation of an NH session factory.
    /// </para>
    /// <para>
    /// All interaction with the database
    /// should come from this class, or one of the
    /// other Data namespace classes.
    /// </para>
    /// </remarks>
    public static class Connection
    {

        #region [ log4net (1) ]

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion [ log4net ]

        #region [ Events and Delegates (4) ]

        /// <summary>
        /// Provides an external library the ability to hook in
        /// to update an IAuditable business object.
        /// </summary>
        /// <param name="entity"></param>
        public delegate void AuditableEventHandler(IAuditable entity);
        /// <summary>
        /// Provides an external library the ability to hook in
        /// and validate an IValidatable business object.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>True if the entity is valid, otherwise false.</returns>
        public delegate bool ValidationEventHandler(IValidatable entity);
        /// <summary>
        /// Occurs when an object is audited.
        /// </summary>
        public static event AuditableEventHandler Audit;

        /// <summary>
        /// Occurs when an object is validated.
        /// </summary>
        public static event ValidationEventHandler Validate;

        #endregion [ Events and Delegates ]

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes the <see cref="Connection"/> class.
        /// </summary>
        static Connection()
        {
            Initialize();
        }

        #endregion [ Constructors ]

        #region [ Fields (3) ]

        private static bool _isInitialized = false;
        private static ISessionFactory _nhSessionFactory = null;
        private static NHibernate.Cfg.Configuration _nhConfiguration = null;

        #endregion [ Fields ]

        #region [ Public Methods (12) ]

        /// <summary>
        /// Flushes a session.
        /// </summary>
        /// <remarks>
        /// See NHibernate documentation for more information.
        /// </remarks>
        /// <param name="sessionNumber">The session number identifying the session to flush.</param>
        public static void Flush(int sessionNumber)
        {
            ISession s = SessionManager.GetSession(sessionNumber);
            s.Flush();
        }

        /// <summary>
        /// Provides access to the NHibernate method:
        /// NHibernateUtil.GetClass().
        /// </summary>
        public static System.Type GetConcreteType(object obj)
        {
            if (obj is Type)
                throw new ArgumentException("Pass an instance of a Type, not the Type itself");

            return NHibernateUtil.GetClass(obj);
        }

        /// <summary>
        /// Returns a connection to the underlying database.
        /// </summary>
        /// <remarks>
        /// This is used to bypass NHibernate and perform SQL directly
        /// against the database.
        /// </remarks>
        public static System.Data.IDbConnection GetNativeConnection()
        {
            return SessionManager.GetSession(-1).Connection;
        }

        /// <summary>
        /// Gets the state of the database connection.
        /// </summary>
        /// <returns>A <see cref="System.Data.ConnectionState"/> object.</returns>
        public static System.Data.ConnectionState GetState()
        {
            try
            {
                ISession s = SessionManager.GetSession(-1);
                return s.Connection.State;
            }
            catch (Exception ex)
            {
                log.Error("Unable to get connection state", ex);
                return System.Data.ConnectionState.Broken;
            }
        }

        /// <summary>
        /// Initializes a proxy object.
        /// </summary>
        /// <remarks>
        /// See NHibernate documentation for more information.
        /// </remarks>
        /// <param name="obj">The object to initialize.</param>
        public static void InitializeProxy(object obj)
        {
            NHibernateUtil.Initialize(obj);
        }

        /// <summary>
        /// Loads the object from the database.
        /// </summary>
        /// <param name="objectType">The object's class.</param>
        /// <param name="id">The key identifier for the object.</param>
        /// <returns>The object from the database, if found; otherwise
        /// null.</returns>
        public static object Load(int sessionNumber, Type objectType, object id)
        {
            try
            {
                return SessionManager.GetSession(sessionNumber).Load(objectType, id);
            }
            catch (Exception ex)
            {
                log.Error("Failed to load requested object", ex);
                throw;
            }
        }

        /// <summary>
        /// Loads the object from the database.
        /// </summary>
        /// <typeparam name="T">The type of object.</typeparam>
        /// <param name="id">The key for the object.</param>
        /// <returns></returns>
        public static T Load<T>(int sessionNumber, object id)
        {
            try
            {
                return SessionManager.GetSession(sessionNumber).Load<T>(id);
            }
            catch (Exception ex)
            {
                log.Error("Failed to load requested object", ex);
                throw;
            }
        }

        /// <summary>
        /// Gets a flag indicating whether the given object is initialized.
        /// </summary>
        /// <param name="obj">The object to examine.</param>
        /// <returns>True if the object is initialized; otherwise, false.</returns>
        public static bool ProxyIsInitialized(object obj)
        {
            return NHibernateUtil.IsInitialized(obj);
        }

        /// <summary>
        /// Resets the connection class.
        /// </summary>
        /// <remarks>
        /// This causes the connection to be reset which allows the ability
        /// to point to a new database or make a new clean connection.
        /// </remarks>
        public static void Reset()
        {
            log.Debug("Reset");

            _isInitialized = false;
            Initialize();

            //dump any existing sessions
            SessionManager.Reset();
        }

        /// <summary>
        /// Perform a non-query SQL command.
        /// </summary>
        /// <param name="sql">The sql statement to perform.</param>
        public static void RunNativeSQL(string sql)
        {
            try
            {
                int sessionNumber = SessionManager.UseSystemSession("RunNativeSQL");

                using (System.Data.IDbCommand cmd = SessionManager.GetSession(sessionNumber).Connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error("Failed to execute requested sql", ex);
                throw;
            }
        }

        /// <summary>
        /// Persists the given object.
        /// </summary>
        /// <param name="objectToSave">The object
        /// to persist.</param>
        public static bool Save(int sessionNumber, object objectToSave)
        {
            return Save(sessionNumber, objectToSave, true);
        }

        /// <summary>
        /// Persists the given object.
        /// </summary>
        /// <param name="objectToSave">The object
        /// to persist.</param>
        public static bool Save(int sessionNumber, object entity, bool flush)
        {

            if (entity == null)
            {
                log.Warn("Null object reference, cannot save.");
                return false;
            }

            ISession session = SessionManager.GetSession(sessionNumber);

            Core.IEditAware editAwareEntity = entity as Core.IEditAware;

            if (editAwareEntity != null)
            {
                if (!editAwareEntity.IsDirty)
                {
                    log.InfoFormat("Object '{0}': '{1}' does not have any changes to save.", editAwareEntity.GetType().FullName, entity.ToString());
                    return false;
                }
            }

            ITransaction transaction = null;
            try
            {

                transaction = session.BeginTransaction();
                log.Debug("Session transaction started.");

                try
                {
                    if (entity is IAuditable)
                    {
                        log.Debug("Object is IAuditable");

                        IAuditable auditableObject = entity as IAuditable;

                        if (Audit != null)
                            Audit(auditableObject);
                        else
                            log.Warn("No event handlers for Audit event.");
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Unable to set IAuditable properties", ex);
                }

                if (entity is IValidatable)
                {
                    log.Debug("Object is IValidatable");

                    IValidatable validatableObject = entity as IValidatable;

                    if (Validate != null)
                    {
                        bool valid = Validate(validatableObject);
                        if (!valid)
                        {
                            log.Info("Object cannot be saved, it has errors.");

                            if (log.IsDebugEnabled)
                                log.DebugFormat("Object validiation errors:  {0}",
                                    validatableObject.ValidationErrorMessage());

                            throw new InvalidOperationException(
                                "You cannot save this object as it is invalid:" +
                                System.Environment.NewLine +
                                validatableObject.ValidationErrorMessage());
                        }
                    }
                }

                session.SaveOrUpdate(entity);

                transaction.Commit();
                if (flush)
                    session.Flush();

                if (editAwareEntity != null)
                {
                    editAwareEntity.IsDirty = false;
                }

                return true;
            }
            catch (NHibernate.StaleObjectStateException stale)
            {
                transaction.Rollback();
                log.Info("User changes were not saved - another user has already changed the data.", stale);

                throw new InvalidOperationException("Another user has updated this record.", stale);
            }
            catch (NHibernate.HibernateException ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (ADOException)
                { }
                catch (Exception ex2)
                {
                    log.Error("There was an error rolling back the current transaction", ex2);
                }

                log.Error("Error updating database.", ex);
                throw;
            }
        }

        #endregion [ Public Methods ]

        #region [ Private Static Methods (3) ]

        /// <summary>
        /// Initializes the <see cref="Connection"/> class.
        /// </summary>
        private static void Initialize()
        {
            if (_isInitialized)
                return;

            _isInitialized = true;

            try
            {
                _nhConfiguration = new NHibernate.Cfg.Configuration();
                _nhConfiguration.Properties.Add("connection.provider", Configuration.Database.NhConnectionProvider);
                _nhConfiguration.Properties.Add("connection.driver_class", Configuration.Database.NhDriverClass);
                _nhConfiguration.Properties.Add("show_sql", Configuration.Database.NhShowSql);
                _nhConfiguration.Properties.Add("dialect", Configuration.Database.NhDialect);
                _nhConfiguration.Properties.Add("connection.connection_string",
                    string.Format(
                    Configuration.Database.ConnectionStringTemplate,
                    Configuration.Database.DatabaseName));
                _nhConfiguration.Properties.Add("query.substitutions", "true=1;false=0");
                _nhConfiguration.AddAssembly("GcDashboard.Core.Entities");
                _nhSessionFactory = _nhConfiguration.BuildSessionFactory();


                /*
                _nhConfiguration = new NHibernate.Cfg.Configuration();
                _nhConfiguration.Properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                _nhConfiguration.Properties.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
                //_nhConfiguration.Properties.Add("connection.driver_class", "NHibernate.Driver.SQLiteDriver");
                _nhConfiguration.Properties.Add("show_sql", "false");
                _nhConfiguration.Properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");
                _nhConfiguration.Properties.Add("connection.connection_string", @"Data Source=C:\Documents and Settings\pelton\Desktop\GcDashboard\mydb.db;Version=3");
                _nhConfiguration.Properties.Add("query.substitutions", "true=1;false=0");
                _nhConfiguration.AddAssembly("GcDashboard.Core.Entities");
                _nhSessionFactory = _nhConfiguration.BuildSessionFactory();
                */
            }
            catch (Exception ex)
            {
                log.Fatal("Unable to configure NHibernate.", ex);
                throw;
            }
        }

        /// <summary>
        /// Creates a new ISession.
        /// </summary>
        internal static ISession CreateSession()
        {
            try
            {
                log.Debug("CreateSession");

                ISession temp = _nhSessionFactory.OpenSession();
                temp.FlushMode = FlushMode.Never;
                return temp;
            }
            catch (Exception ex)
            {
                log.Warn("Failed to create ISession", ex);
                throw;
            }
        }

        /// <summary>
        /// Closes and disposes of an ISession
        /// </summary>
        /// <param name="session">Session to dispose</param>
        /// <param name="flush">Indicate if session should be flushed
        /// prior to disposing</param>
        internal static void DestorySession(ISession session, bool flush)
        {
            try
            {
                log.Debug("DestorySession");

                if (session == null)
                {
                    log.Warn("Null session reference, cannot destory session.");
                    return;
                }

                if (session.IsConnected && flush)
                {
                    session.Flush();
                    log.Debug("Session flushed.");
                }

                session.Disconnect();
                session.Dispose();
                log.Info("Session disposed.");
            }
            catch (Exception ex)
            {
                log.Warn("Failed to destroy session", ex);
                throw;
            }
        }

        #endregion [ Private Static Methods ]

    }

}
