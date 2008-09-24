using System;

namespace GcDashboard.Core
{

    /// <summary>
    /// Contract for entities that provide self-validation.
    /// </summary>
    public interface IValidatable
    {
        /// <summary>
        /// Performs validation rules checking and returns
        /// a flag indicating whether all of the rules passed.
        /// </summary>
        /// <returns>True, if the object has no broken validation rules; otherwise, false.</returns>
        bool IsValid();

        /// <summary>
        /// Gets a text description of the validation problems
        /// </summary>
        string ValidationErrorMessage();
    }

}
