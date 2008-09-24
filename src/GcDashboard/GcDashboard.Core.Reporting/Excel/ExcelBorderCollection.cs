using System;
using Microsoft.Office.Interop.Excel;

namespace GcDashboard.Core.Reporting.Excel
{

    /// <summary>
    /// Borders for a range.
    /// </summary>
    public class ExcelBorderCollection
    {
        internal ExcelBorderCollection(Range range)
        {
            _borders = range.Borders;
        }

        private Borders _borders;

        /// <summary>
        /// Gets the border with the given index.
        /// </summary>
        public ExcelBorder this[ExcelBorderIndex index]
        {
            get { return new ExcelBorder(_borders.get_Item((XlBordersIndex)index)); }
        }

        /// <summary>
        /// Thickness of the border.
        /// </summary>
        public float Weight
        {
            get { return Convert.ToSingle(_borders.Weight); }
            set { _borders.Weight = value; }
        }

        /// <summary>
        /// Color of the border.
        /// </summary>
        public System.Drawing.Color Color
        {
            get
            {
                return ExcelConversions.GetColorFromOldRgbValue(Convert.ToInt32(_borders.Color));
            }
            set
            {
                _borders.Color = ExcelConversions.GetOldRgbValueFromColor(value);
            }
        }

        /// <summary>
        /// Linestyle of the border.
        /// </summary>
        public ExcelLineStyle LineStyle
        {
            get { return (ExcelLineStyle)_borders.LineStyle; }
            set { _borders.LineStyle = value; }
        }

    }

}
