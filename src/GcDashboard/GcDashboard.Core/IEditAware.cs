using System;


namespace GcDashboard.Core
{

    /// <summary>
    /// Contract for entities that are aware of their edited state.
    /// </summary>
    public interface IEditAware
    {
        /// <summary>
        /// Gets or sets a flag indicating whether the
        /// object has changes that have not been persisted.
        /// </summary>
        bool IsDirty
        {
            get;
            set;
        }
    }

}
