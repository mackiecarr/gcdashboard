using System;
using Microsoft.Office.Interop.Excel;

namespace ClearView.Core.Reporting.Excel
{

    /// <summary>
    /// Styles for border lines.
    /// </summary>
    public enum ExcelLineStyle
    {
        /// <summary>
        /// Continuous, solid line.
        /// </summary>
        Continuous = XlLineStyle.xlContinuous,
        
        /// <summary>
        /// Series of dashs.
        /// </summary>
        Dash = XlLineStyle.xlDash,

        /// <summary>
        /// A dash followed by a dot.
        /// </summary>
        DashDot = XlLineStyle.xlDashDot,

        /// <summary>
        /// A dash follwed by two dots.
        /// </summary>
        DashDotDot = XlLineStyle.xlDashDotDot,

        /// <summary>
        /// Dotted line.
        /// </summary>
        Dot = XlLineStyle.xlDot,

        /// <summary>
        /// Two lines.
        /// </summary>
        Double = XlLineStyle.xlDouble,

        /// <summary>
        /// None.
        /// </summary>
        None = XlLineStyle.xlLineStyleNone,

        /// <summary>
        /// Slanted dash followed by a dot.
        /// </summary>
        SlantDashDot = XlLineStyle.xlSlantDashDot,
    }

}
