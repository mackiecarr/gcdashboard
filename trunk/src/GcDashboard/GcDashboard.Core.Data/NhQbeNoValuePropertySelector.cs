using System;
using NHibernate.Criterion;
using NHibernate.Type;


namespace GcDashboard.Core.Data
{

    /// <summary>
    /// Implementation of <see cref="IPropertySelector"/> that includes the
    /// properties that are not <c>null</c> and do not have an <see cref="String.Empty"/>
    /// returned by <c>propertyValue.ToString()</c>.
    /// </summary>
    /// <remarks>
    /// This selector is not present in H2.1. It may be useful if nullable types
    /// are used for some properties.
    /// </remarks>
    internal class NhQbeNoValuePropertySelector : Example.IPropertySelector
    {

        #region [ Public Methods (1) ]

        public bool Include(object propertyValue, String propertyName, IType type)
        {
            if (propertyValue == null)
            {
                return false;
            }

            if (propertyValue is string)
            {
                return ((string)propertyValue).Length != 0;
            }

            if (IsZero(propertyValue))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion [ Public Methods ]

        #region [ Private Static Methods (1) ]

        private static bool IsZero(object value)
        {
            // Only try to check IConvertibles, to be able to handle various flavors
            // of nullable numbers, etc. Skip strings.
            if (value is IConvertible && !(value is string))
            {
                try
                {
                    return Convert.ToInt64(value) == 0L;
                }
                catch (FormatException)
                {
                    // Ignore
                }
                catch (InvalidCastException)
                {
                    // Ignore
                }
            }

            return false;
        }

        #endregion [ Private Static Methods ]

    }

}
