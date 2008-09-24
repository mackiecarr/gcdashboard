/* **********************************************************************
 * IEditableObject Interface Implemenation
 * Original Author: Galien Iliev
 * http://www.codeproject.com/cs/database/IEditableObject.asp
 * **********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using GcDashboard.Core.Entities.Validation;


namespace GcDashboard.Core.Entities
{

    /// <summary>
    /// Base class for all other entities.
    /// </summary>
    public abstract class EntityBase
        : IValidatable, IEditAware, IAuditable, IDataErrorInfo, INotifyPropertyChanged, IEditableObject
    {
        
        #region log4net

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion
        

        #region Public Events

        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string property)
        {
            _isDirty = true;
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        #endregion

        
        #region Public Instance Constructors


        public EntityBase()
        {
            _entityProperties = new List<string>();
            _entityCollectionProperties = new List<string>();
        }


        #endregion
        

        #region Private Instance Fields
        
        private int? _id = null;
        private int? _nhVersion = null;
        
        private DateTime? _created = null;
        //private int? _createdById = null;
        private DateTime? _lastUpdated = null;
        //private int? _lastUpdatedById = null;

        private List<string> _entityProperties = null;
        private List<string> _entityCollectionProperties = null;

        private Hashtable _props = null;
        private bool? _isDirty = null;

        #endregion
        

        #region Public Instance Properties
        
        public virtual int? ID
        {
            get { return _id; }
            set { _id = value; }
        }


        public virtual int? NhVersion
        {
            get { return _nhVersion; }
            private set { _nhVersion = value; }
        }


        public virtual DateTime? Created
        {
            get { return _created; }
            protected set
            {
                if (_created == null)
                    _created = value;
            }
        }


        //public virtual int? CreatedById
        //{
        //    get { return _createdById; }
        //    protected set
        //    {
        //        if (_createdById == null)
        //            _createdById = value;
        //    }
        //}


        public virtual DateTime? LastUpdated
        {
            get { return _lastUpdated; }
            protected set { _lastUpdated = value; }
        }


        //public virtual int? LastUpdatedById
        //{
        //    get { return _lastUpdatedById; }
        //    protected set { _lastUpdatedById = value; }
        //}


        public virtual bool IsDirty
        {
            get
            {
                return _isDirty ?? false;
            }
            set { _isDirty = value; }
        }

        #endregion
        

        #region Public Instance Methods

        /// <summary>
        /// Determines if this object has been persisted to the database.
        /// </summary>
        /// <returns>True if the object has been persisted, otherwise false.</returns>
        public virtual bool IsTransient()
        {
            return _id == null;
        }

        /// <summary>
        /// Determines if a given object has been persisted to the database.
        /// </summary>
        /// <param name="entity">The object to examine.</param>
        /// <returns>True if the object has been persisted, otherwise false.</returns>
        public static bool IsTransient(EntityBase entity)
        {
            return entity.ID == null;
        }

        /// <summary>
        /// Adds a property (by name) to the list of entity properties.
        /// </summary>
        /// <remarks>
        /// The list of entity properties is used by the <see cref="ValidationRuleBase" />
        /// to walk object and validate the entire object graph, not just the top level object.
        /// </remarks>
        /// <param name="propertyName">The name of the property.</param>
        protected void RegisterEntityProperty(string propertyName)
        {
            _entityProperties.Add(propertyName);
        }

        /// <summary>
        /// Adds a property (by name) to the list of entity collection properties.
        /// </summary>
        /// <remarks>
        /// The list of entity collection properties is used by the <see cref="ValidationRuleBase" />
        /// to walk object and validate the entire object graph, not just the top level object.
        /// </remarks>
        /// <param name="collectionPropertyName">The name of the property.</param>
        protected void RegisterNestedBusinessObjectCollection(string collectionPropertyName)
        {
            _entityCollectionProperties.Add(collectionPropertyName);
        }
        

        /// <summary>
        /// Starts a reversable edit operation.
        /// </summary>
        public virtual void BeginEdit()
        {
            //get the actual class for this instance
            System.Type t = Core.Data.Connection.GetConcreteType(this);

            PropertyInfo[] properties = t.GetProperties(BindingFlags.Public |
                BindingFlags.Instance);

            _props = new Hashtable(properties.Length - 1);

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetSetMethod() != null)
                {
                    object value = properties[i].GetValue(this, null);
                    _props.Add(properties[i].Name, value);
                }
            }
        }
        
        /// <summary>
        /// Reverses an edit operation.
        /// </summary>
        public virtual void CancelEdit()
        {
            if (_props == null)
                return;

            //get the actual class for this instance
            System.Type t = Core.Data.Connection.GetConcreteType(this);

            PropertyInfo[] properties = t.GetProperties(BindingFlags.Public |
                BindingFlags.Instance);

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].GetSetMethod() != null)
                {
                    object value = _props[properties[i].Name];
                    properties[i].SetValue(this, value, null);
                }
            }

            _props = null;
        }
        

        /// <summary>
        /// Completes an edit operation.
        /// </summary>
        public virtual void EndEdit()
        {
            _props = null;
        }


        #endregion
        

        #region Overwritable Methods

        /// <summary>
        /// Determines if this object is equal to a given object.
        /// </summary>
        /// <param name="obj">The object to compare to this object.</param>
        /// <returns>True if the two objects are equal, otherwise false.</returns>
        public virtual bool Equals(object obj)
        {
            if (this == obj)
                return true;

            EntityBase compareTo = obj as EntityBase;

            // if compareTo is null, then it isn't a BO
            // and they can't be equal

            if (compareTo == null) return false;

            // compareTo is a BO, so see if it has an ID
            // and if so, try to compare their IDs to see
            // if they are equal

            if (compareTo.ID.HasValue && this.ID.HasValue)
            {
                return compareTo.ID.Value == this.ID.Value;
            }

            // one of the objects is transient, so
            // compare their properties to see if they are
            // equal by their contents.

            if (this.IsTransient() && compareTo.IsTransient())
            {
                return this.HasSameBusinessSignature(compareTo);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// See <see cref="object.GetHashCode"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            if (_id.HasValue)
                return _id.GetHashCode();
            else
                return 0;
        }
        
        #endregion
        

        #region Private Instance Methods
        
        /// <summary>
        /// Checks if given object has the same
        /// business signature as the object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected bool HasSameBusinessSignature(EntityBase obj)
        {
            return GetHashCode().Equals(obj.GetHashCode());
        }


        #endregion
        

        #region Validation

        /// <summary>
        /// Contains the object's rules
        /// </summary>
        private List<ValidationRuleBase> _rules;

        /// <summary>
        /// Contains the rules that are broken
        /// based on the last validation of the object
        /// </summary>
        private List<ValidationRuleBase> _broken = new List<ValidationRuleBase>();

        /// <summary>
        /// Initializes the object's rules list.
        /// </summary>
        /// <remarks>
        /// This is typically overriden by a child class
        /// </remarks>
        /// <returns></returns>
        protected virtual List<ValidationRuleBase> InitRules()
        {
            return new List<ValidationRuleBase>();
        }


        /// <summary>
        /// Helper method to get the business object value
        /// of a property by name.
        /// </summary>
        private static EntityBase GetBusinessObjectFromProperty(string property, EntityBase bob)
        {
            Type t = Core.Data.Connection.GetConcreteType(bob);

            PropertyInfo p = t.GetProperty(property,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);

            if (p == null)
                throw new ArgumentException("Property " + property + " does not exist or is inaccessable.");

            object o = p.GetValue(bob, BindingFlags.GetProperty, null, null, null);

            return (EntityBase)o;
        }

        /// <summary>
        /// Helper method that fetches a business object based on a property name.
        /// </summary>
        private EntityBase GetBusinessObjectFromProperty(string property)
        {
            return GetBusinessObjectFromProperty(property, this);
        }


        /// <summary>
        /// Helper method that fetches a collection based on a property name.
        /// </summary>
        private static IList<EntityBase> GetBusinessObjectCollectionFromProperty(string collectionProperty, EntityBase bob)
        {
            Type t = Core.Data.Connection.GetConcreteType(bob);

            PropertyInfo p = t.GetProperty(collectionProperty,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);

            if (p == null)
                throw new ArgumentException("Property " + collectionProperty + " does not exist or is inaccessable.");

            object o = p.GetValue(bob, BindingFlags.GetProperty, null, null, null);

            if (o is IList<EntityBase>)
                return (IList<EntityBase>)o;
            else
            {
                IList list = o as IList;

                List<EntityBase> genericList = new List<EntityBase>();
                foreach (EntityBase bobItem in list)
                {
                    genericList.Add(bobItem);
                }

                return genericList;
            }
        }

        /// <summary>
        /// Helper method that fetches a collection based on a property name.
        /// </summary>
        private IList<EntityBase> GetBusinessObjectCollectionFromProperty(string collectionProperty)
        {
            return GetBusinessObjectCollectionFromProperty(collectionProperty, this);
        }


        /// <summary>
        /// Determines if any rules for the given property are broken.
        /// </summary>
        /// <remarks>
        /// This does not cause the object to validate itself.  This does not
        /// walk the object tree.
        /// </remarks>
        public virtual bool IsRuleBroken(string propertyName)
        {
            foreach (ValidationRuleBase rule in _broken)
            {
                if (rule.PropertyName == propertyName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Determines if this object has any broken rules.
        /// </summary>
        /// <remarks>
        /// This does not cause the object to validate itself.  This does not
        /// walk the object tree.
        /// </remarks>
        public virtual bool HasBrokenRules
        {
            get { return _broken.Count != 0; }
        }




        public virtual int BrokenRulesCount()
        {
            return BrokenRulesCount(this);
        }

        private static int BrokenRulesCount(EntityBase bob)
        {
            if (bob == null)
                return 0;

            int broken = 0;

            broken += bob._broken.Count;

            foreach (string s in bob._entityProperties)
            {
                broken += BrokenRulesCount(GetBusinessObjectFromProperty(s, bob));
            }

            foreach (string s in bob._entityCollectionProperties)
            {
                IList<EntityBase> bobList = GetBusinessObjectCollectionFromProperty(s, bob);
                foreach (EntityBase bobItem in bobList)
                {
                    broken += BrokenRulesCount(bobItem);
                }
            }

            return broken;
        }






        /// <summary>
        /// Determines if the business object has any broken rules.
        /// This causes the object to validate itself.
        /// </summary>
        /// <returns>True, if the object has no errors, otherwise, false.</returns>
        public virtual bool IsValid()
        {
            // perform validation

            this.ValidateAll();

            // count the errors

            return BrokenRulesCount() == 0;
        }






        public virtual string ValidationErrorMessage()
        {
            return ValidationErrorMessage(this, true);
        }

        public virtual string ValidationErrorMessage(bool includeNestedObjects)
        {
            return ValidationErrorMessage(this, includeNestedObjects);
        }


        public static string ValidationErrorMessage(EntityBase bob, bool includeNestedObjects)
        {
            if (bob == null)
                return string.Empty;

            string message = string.Empty;

            foreach (ValidationRuleBase r in bob._broken)
            {
                if (!string.IsNullOrEmpty(message))
                    message += Environment.NewLine;

                message += string.Format("{0}: {1}", r.PropertyName, r.RuleBrokenMessage);
            }

            if (!includeNestedObjects)
                return message;

            foreach (string s in bob._entityProperties)
            {
                message += ValidationErrorMessage(GetBusinessObjectFromProperty(s, bob), includeNestedObjects);
            }

            foreach (string s in bob._entityCollectionProperties)
            {
                IList<EntityBase> bobList = GetBusinessObjectCollectionFromProperty(s, bob);
                foreach (EntityBase bobItem in bobList)
                {
                    message += ValidationErrorMessage(bobItem, includeNestedObjects);
                }
            }

            return message;
        }




        /// <summary>
        /// Gets an error message indicating what is wrong with
        /// this object. The default is an empty string.
        /// </summary>
        public virtual string Error
        {
            get
            {
                IDataErrorInfo de = (IDataErrorInfo)this;
                string result = de[string.Empty];
                if (result != null && result.Trim().Length == 0)
                {
                    result = null;
                }
                return result;
            }
        }




        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <param name="propertyName">The name of the property whose error message to get.</param>
        /// <returns>The error message for the property. The default is an empty string.</returns>
        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                if (_broken == null)
                    return string.Empty;

                string message = string.Empty;
                foreach (ValidationRuleBase r in _broken)
                {
                    if (r.PropertyName == propertyName)
                        message += r.ToString();
                }

                return message;
            }
        }




        /// <summary>
        /// Validates the object and any nested
        /// objects that can be validated.
        /// </summary>
        public virtual void ValidateAll()
        {
            ValidateAll(this);
        }

        private static void ValidateAll(EntityBase bob)
        {
            if (bob == null)
                return;

            bob.Validate(); // validate the object itself.

            // validate any bob properties of the object, (and walk into those objects)

            foreach (string s in bob._entityProperties)
            {
                EntityBase nestedObject = GetBusinessObjectFromProperty(s, bob);
                ValidateAll(nestedObject);
            }

            // validate any bob collections on the object, (and walk into those collections)

            foreach (string s in bob._entityCollectionProperties)
            {
                IList<EntityBase> list = GetBusinessObjectCollectionFromProperty(s, bob);
                foreach (EntityBase bobItem in list)
                {
                    ValidateAll(bobItem);
                }
            }
        }







        /// <summary>
        /// Validates the object.  This does not walk the object tree.
        /// </summary>
        public virtual ReadOnlyCollection<ValidationRuleBase> Validate()
        {
            return Validate(null);
        }



        /// <summary>
        /// Validates the object.  This does not walk the object tree.
        /// </summary>
        /// <remarks>
        /// If property is not given (null or empty string), then this
        /// will validate all rules, otherwise it will validate only
        /// the specific rules for the given property.
        /// </remarks>
        public virtual ReadOnlyCollection<ValidationRuleBase> Validate(string property)
        {
            property = (property ?? string.Empty).Trim();

            // if this is an uninitialized proxy, then
            // initialize the proxy now

            if (Core.Data.Connection.ProxyIsInitialized(this))
                Core.Data.Connection.InitializeProxy(this);

            // If we haven't yet created the rules, create them now.
            if (_rules == null)
            {
                _rules = new List<ValidationRuleBase>();
                _rules.AddRange(this.InitRules());
            }
            List<ValidationRuleBase> propertyBrokenRules = new List<ValidationRuleBase>();

            // clear broken rules --
            // if we're validating a specific property, then only clear
            // that property's broken rules, otherwise clear all broken
            // rules
            if (string.IsNullOrEmpty(property))
                _broken.Clear();
            else
                _broken.RemoveAll(delegate(ValidationRuleBase r)
                {
                    return r.PropertyName == property;
                });

            foreach (ValidationRuleBase r in _rules)
            {
                // validate based on given property or all if no
                // property was given
                if (r.PropertyName == property ||
                    string.IsNullOrEmpty(property))
                {
                    bool isRuleBroken = !r.IsValid(this);
                    if (isRuleBroken)
                    {
                        if (!_broken.Contains(r))
                            _broken.Add(r);

                        propertyBrokenRules.Add(r);
                    }
                }
            }

            return propertyBrokenRules.AsReadOnly();
        }


        #endregion

    }

}
