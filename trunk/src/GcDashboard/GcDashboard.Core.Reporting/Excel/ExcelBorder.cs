using System;
using Microsoft.Office.Interop.Excel;

namespace ClearView.Core.Reporting.Excel
{

    /// <summary>
    /// A specific border of a range of cells.
    /// </summary>
    public class ExcelBorder
    {

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ExcelBorder"/> class to represent the given <see cref="Border"/>.
        /// </summary>
        /// <param name="border"></param>
        internal ExcelBorder(Border border)
        { _border = border; }

        #endregion [ Constructors ]

        #region [ Fields (1) ]

        private Border _border;

        #endregion [ Fields ]

        #region [ Properties (3) ]

        /// <summary>
        /// Color of the border.
        /// </summary>
        public System.Drawing.Color Color
        {
            get
            {
                return ExcelConversions.GetColorFromOldRgbValue(Convert.ToInt32(_border.Color));
            }
            set
            {
                _border.Color = ExcelConversions.GetOldRgbValueFromColor(value);
            }
        }

        /// <summary>
        /// Linestyle of the border.
        /// </summary>
        public ExcelLineStyle LineStyle
        {
            get { return (ExcelLineStyle)_border.LineStyle; }
            set { _border.LineStyle = value; }
        }

        /// <summary>
        /// Thickness of the border.
        /// </summary>
        public float Weight
        {
            get { return Convert.ToSingle(_border.Weight); }
            set { _border.Weight = value; }
        }

        #endregion [ Properties ]

    }

}
