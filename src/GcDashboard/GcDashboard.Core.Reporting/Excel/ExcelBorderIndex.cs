using System;
using Microsoft.Office.Interop.Excel;

namespace ClearView.Core.Reporting.Excel
{
    /// <summary>
    /// Borders of a range.
    /// </summary>
    public enum ExcelBorderIndex
    {
        /// <summary>
        /// Top border.
        /// </summary>
        Top = XlBordersIndex.xlEdgeTop,

        /// <summary>
        /// Bottom border.
        /// </summary>
        Bottom = XlBordersIndex.xlEdgeBottom,

        /// <summary>
        /// Left border.
        /// </summary>
        Left = XlBordersIndex.xlEdgeLeft,

        /// <summary>
        /// Right border.
        /// </summary>
        Right = XlBordersIndex.xlEdgeRight,
    }

}
