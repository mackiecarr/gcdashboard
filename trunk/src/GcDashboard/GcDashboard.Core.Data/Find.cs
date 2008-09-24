using System;
using System.Collections;
using System.Collections.Generic;
using NHibernate;


namespace GcDashboard.Core.Data
{

    /// <summary>
    /// Provides search functionality.
    /// </summary>
    public static class Find
    {

        #region [ log4net (1) ]

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion [ log4net ]

        #region [ Public Methods (12) ]

        /// <summary>
        /// Performs a search using HQL.
        /// </summary>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="hql">A complete HQL query string.</param>
        /// <returns>A specific, unique result.</returns>
        public static object FindByHql(int sessionNumber, string hql)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).CreateQuery(hql);
            return q.UniqueResult();
        }

        /// <summary>
        /// Performs a search using HQL.
        /// </summary>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="hql">A complete HQL query string.</param>
        /// <returns>A specific, unique result.</returns>
        public static object FindByHql(int sessionNumber, string hql, Dictionary<string,object> namedParameters)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).CreateQuery(hql);

            foreach (KeyValuePair<string, object> e in namedParameters)
            {
                SetParameter(q, e.Key, e.Value);
            }

            return q.UniqueResult();
        }

        /// <summary>
        /// Performs a search using HQL for the given type.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="hql">A complete HQL query string.</param>
        /// <returns>A specific, unique result as T.</returns>
        public static T FindByHql<T>(int sessionNumber, string hql)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).CreateQuery(hql);
            return q.UniqueResult<T>();
        }

        /// <summary>
        /// Performs a search using HQL and a list of parameters.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="hql">A complete HQL query string.</param>
        /// <param name="namedParameters">A dictionary of name/value parameters referenced by the hql query.</param>
        /// <returns>A specific, unique result as T.</returns>
        public static T FindByHql<T>(int sessionNumber, string hql, Dictionary<string, object> namedParameters)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).CreateQuery(hql);
            foreach (KeyValuePair<string, object> e in namedParameters)
            {
                SetParameter(q, e.Key, e.Value);
            }
            return q.UniqueResult<T>();

        }

        public static T FindByNamedQuery<T>(int sessionNumber, string queryName)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).GetNamedQuery(queryName.ToString());
            return q.UniqueResult<T>();
        }

        /// <summary>
        /// Performs a search using an object as the criteria.
        /// </summary>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="exampleObject">The object which represents the criteria.</param>
        /// <returns>A result list of objects.</returns>
        public static IList FindListByExample(int sessionNumber, object exampleObject)
        {
            NHibernate.Criterion.Example e =
                NHibernate.Criterion.Example.Create(exampleObject);

            e.ExcludeZeroes();
            e.ExcludeNulls();
            e.IgnoreCase();
            e.EnableLike(NHibernate.Criterion.MatchMode.Start);

            ICriteria c =
                SessionManager.GetSession(sessionNumber).CreateCriteria(exampleObject.GetType());
            c.Add(e);

            return c.List();
        }

        /// <summary>
        /// Performs a search using an object as the criteria.
        /// </summary>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="exampleObject">The object which represents the criteria.</param>
        /// <returns>A result list of T.</returns>
        public static IList<T> FindListByExample<T>(int sessionNumber, T exampleObject)
        {
            NHibernate.Criterion.Example e =
                NHibernate.Criterion.Example.Create(exampleObject);

            e.ExcludeZeroes();
            e.ExcludeNulls();
            e.IgnoreCase();
            e.EnableLike(NHibernate.Criterion.MatchMode.Start);
            e.SetPropertySelector(new NhQbeNoValuePropertySelector());

            ICriteria c =
                SessionManager.GetSession(sessionNumber).CreateCriteria(exampleObject.GetType());
            c.Add(e);

            return c.List<T>();
        }

        /// <summary>
        /// Performs a search using an object as the criteria.
        /// </summary>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="exampleObject">The object which represents the criteria.</param>
        /// <param name="inPropertyName">The name of the property to apply the in filter against.</param>
        /// <param name="inPropertyValues">The values to include in the results.</param>
        /// <returns>A result list of T.</returns>
        public static IList<T> FindListByExampleWithInCriterion<T>(int sessionNumber,
            T exampleObject, string inPropertyName, object[] inPropertyValues)
        {
            NHibernate.Criterion.Example e =
                NHibernate.Criterion.Example.Create(exampleObject);

            e.ExcludeZeroes();
            e.ExcludeNulls();
            e.IgnoreCase();
            e.EnableLike(NHibernate.Criterion.MatchMode.Start);

            ICriteria c =
                SessionManager.GetSession(sessionNumber).CreateCriteria(exampleObject.GetType());
            c.Add(e);
            c.Add(new NHibernate.Criterion.InExpression(inPropertyName, inPropertyValues));

            return c.List<T>();
        }

        /// <summary>
        /// Performs a search using HQL.
        /// </summary>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="hql">A complete HQL query string.</param>
        /// <returns>A result list of objects.</returns>
        public static object FindListByHql(int sessionNumber, string hql)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).CreateQuery(hql);
            return q.List();
        }

        /// <summary>
        /// Performs a search using HQL for the given type.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="hql">A complete HQL query string.</param>
        /// <returns>A result list of T.</returns>
        public static IList<T> FindListByHql<T>(int sessionNumber, string hql)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).CreateQuery(hql);
            return q.List<T>();
        }

        /// <summary>
        /// Performs a search using HQL and a list of parameters.
        /// </summary>
        /// <typeparam name="T">The type of object to return.</typeparam>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="hql">A complete HQL query string.</param>
        /// <param name="namedParameters">A dictionary of name/value parameters referenced by the hql query.</param>
        /// <returns>A list result of T.</returns>
        public static IList<T> FindListByHql<T>(int sessionNumber, string hql, Dictionary<string, object> namedParameters)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).CreateQuery(hql);
            foreach (KeyValuePair<string, object> e in namedParameters)
            {
                SetParameter(q, e.Key, e.Value);
            }
            return q.List<T>();

        }

        public static IList<T> FindListByNamedQuery<T>(int sessionNumber, NamedQuery query)
        {
            ISession s = SessionManager.GetSession(sessionNumber);
            IQuery q = s.GetNamedQuery(query.ToString());

            return q.List<T>();
        }

        /// <summary>
        /// Performs a search using a named HQL query and the given bind parameters.
        /// </summary>
        /// <typeparam name="T">The type of object to return</typeparam>
        /// <param name="sessionNumber">The session number to perform the query against.</param>
        /// <param name="query">The name of the query</param>
        /// <param name="parameters">A dictionary of parameters used by the HQL query.</param>
        /// <returns>A result list of T.</returns>
        public static IList<T> FindListByNamedQuery<T>(int sessionNumber, NamedQuery query,
            Dictionary<string, object> namedParameters)
        {
            IQuery q = SessionManager.GetSession(sessionNumber).GetNamedQuery(query.ToString());

            foreach (KeyValuePair<string, object> e in namedParameters)
            {
                SetParameter(q, e.Key, e.Value);
            }

            return q.List<T>();
        }

        #endregion [ Public Methods ]

        #region [ Private Static Methods (1) ]

        /// <summary>
        /// Adds type-specific parameters to a query.
        /// </summary>
        private static void SetParameter(IQuery q, string name, object value)
        {
            if (value.GetType().IsEnum)
            {
                q.SetEnum(name, (Enum)value);
                return;
            }

            switch (value.GetType().Name.ToLower())
            {
                case "datetime":
                    q.SetDateTime(name, (DateTime)value);
                    break;

                case "int32":
                    q.SetInt32(name, (int)value);
                    break;

                case "boolean":
                    q.SetBoolean(name, (bool)value);
                    break;

                case "string":
                    q.SetString(name, (string)value);
                    break;

                case "decimal":
                    q.SetDecimal(name, (decimal)value);
                    break;

                case "guid":
                    q.SetGuid(name, (Guid)value);
                    break;

                default:
                    q.SetEntity(name, value);
                    break;
            }
        }

        #endregion [ Private Static Methods ]

    }

}
