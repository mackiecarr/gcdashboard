using System;


namespace GcDashboard.Core.Entities.Validation
{

    public class NotNullRule : ValidationRuleBase
    {
        #region log4net

        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.
            MethodBase.GetCurrentMethod().DeclaringType);

        #endregion


        public NotNullRule(string propertyName)
            : base(propertyName, propertyName + " must have a value.")
        { }

        public override bool IsValid(EntityBase entity)
        {
            try
            {
                Type entityType = Core.Data.Connection.GetConcreteType(entity);

                object propertyValue = entityType.GetProperty(PropertyName).GetValue(entity,
                    System.Reflection.BindingFlags.GetProperty, null, null, null);

                return propertyValue != null;
            }
            catch (Exception ex)
            {
                log.Error(string.Format("Failed while validating {0}", PropertyName), ex);
                return true;
            }
        }

    }

}
