using System;


namespace GcDashboard.Core
{

    /// <summary>
    /// Contract for entities that provide change auditing.
    /// </summary>
    public interface IAuditable
    {
        /// <summary>
        /// Date the object was created.
        /// </summary>
        DateTime? Created { get; }

        //// <summary>
        //// Identifier of the user that created the object.
        //// </summary>
        //int? CreatedById { get; }
        
        /// <summary>
        /// Date the object was last updated.
        /// </summary>
        DateTime? LastUpdated { get; }
        
        //// <summary>
        //// Identifier of the user that last updated the object.
        //// </summary>
        //int? LastUpdatedById { get; }
    }

}
