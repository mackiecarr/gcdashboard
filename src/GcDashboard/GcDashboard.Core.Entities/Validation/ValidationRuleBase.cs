
/* **********************************************************************
 * Used With Permission
 * Author:  Paul Stovell
 * http://www.codeproject.com/csharp/DelegateBusinessObjects.asp
 * **********************************************************************/

using System;
using GcDashboard.Core.Entities;


namespace GcDashboard.Core.Entities.Validation
{

    /// <summary>
    /// An abstract class that contains information about a rule
    /// as well as a method to validate it.
    /// </summary>
    /// <remarks>
    /// This class is primarily designed to be used on a domain object to validate a business rule.
    /// In most cases, you will want to use the concrete class SimpleRule, which just needs
    /// you to supply a delegate used for validation. For custom, complex business rules, you can 
    /// extend this class and provide your own method to validate the rule.
    /// </remarks>
    public abstract class ValidationRuleBase
    {


        #region log4net

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion




        #region constructors


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="propertyName">The name of the property the rule is based on.
        /// This may be blank if the rule is not for any specific property.</param>
        /// <param name="brokenDescription">A description of the rule.</param>
        public ValidationRuleBase(string propertyName, string description)
        {
            this.Description = description;
            this.PropertyName = propertyName;
        }


        #endregion




        #region Private Instance Fields


        private string _propertyName;
        private string _description;
        private string _ruleBrokenMessage;


        #endregion




        #region Public Instance Properties


        /// <summary>
        /// Gets the name of the property the rule belongs to.
        /// </summary>
        public virtual string PropertyName
        {
            get { return (_propertyName ?? string.Empty).Trim(); }
            protected set { _propertyName = value; }
        }




        /// <summary>
        /// Gets descriptive text about this rule.
        /// </summary>
        public virtual string Description
        {
            get { return _description; }
            protected set { _description = value; }
        }




        /// <summary>
        /// Gets a message indicating why the rule is broken.
        /// </summary>
        public virtual string RuleBrokenMessage
        {
            get
            {
                if (!string.IsNullOrEmpty(_ruleBrokenMessage))
                    return _ruleBrokenMessage;
                else if (!string.IsNullOrEmpty(_description))
                    return _description;
                else
                    return "Failed validation";
            }
            protected set { _ruleBrokenMessage = value; }
        }


        #endregion




        #region Public Abstract Methods


        /// <summary>
        /// Determines if the given entity complies with this rule.
        /// </summary>
        /// <returns>True if the entity complies; otherwise false.</returns>
        public abstract bool IsValid(EntityBase entity);


        #endregion




        #region Overrides


        /// <summary>
        /// Gets a string representation of this rule.
        /// </summary>
        /// <returns>A string containing the description of the rule.</returns>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_ruleBrokenMessage))
                return this.Description;
            else
                return this._ruleBrokenMessage;
        }




        /// <summary>
        /// Serves as a hash function for a particular type. System.Object.GetHashCode()
        /// is suitable for use in hashing algorithms and data structures like a hash
        /// table.
        /// </summary>
        /// <returns>A hash code for the current rule.</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }


        #endregion

    }
}
