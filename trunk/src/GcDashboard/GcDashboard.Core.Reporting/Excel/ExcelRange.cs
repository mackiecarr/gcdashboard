using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace GcDashboard.Core.Reporting.Excel
{
    /// <summary>
    /// A range of cells.
    /// </summary>
    public class ExcelRange : IDisposable
    {

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelRange"/>
        /// class representing the given Excel <see cref="Range"/>.
        /// </summary>
        /// <param name="range"></param>
        internal ExcelRange(Range range)
        {
            _range = range;
        }

        #endregion [ Constructors ]

        #region [ Fields (1) ]

        private Range _range;

        #endregion [ Fields ]

        #region [ Properties (3) ]

        /// <summary>
        /// Borders of the range.
        /// </summary>
        public ExcelBorderCollection Borders
        {
            get { return new ExcelBorderCollection(_range); }
        }

        /// <summary>
        /// Font style for the range.
        /// </summary>
        public ExcelFont Font
        {
            get { return new ExcelFont(_range); }
        }

        /// <summary>
        /// Value2 in the range.
        /// </summary>
        public object Value2
        {
            get { return _range.Value2; }
            set { _range.Value2 = value; }
        }

        #endregion [ Properties ]

        #region [ Public Methods (1) ]

        /// <summary>
        /// Gets the specific border requested for the range.
        /// </summary>
        /// <param name="index">The index of the desired border.</param>
        /// <returns>A <see cref="ExcelBorder"/> object representing the desired border.</returns>
        public ExcelBorder Border(ExcelBorderIndex index)
        {
            return Borders[index];
        }

        /// <summary>
        /// Copys the range to a new place in the worksheet.
        /// </summary>
        public void CopyTo(string destination)
        {
            // Rows("8:8").Select
            // Selection.Copy
            // Rows("9:9").Select
            // Selection.Insert Shift:=xlDown
            // Range("A11").Select
            // Application.CutCopyMode = False
        }

        /// <summary>
        /// Fill the top row down to the bottom of the selection.
        /// </summary>
        public void FillDown()
        {
            _range.FillDown();
        }

        #endregion [ Public Methods ]

        #region [ Private Methods (1) ]

        void IDisposable.Dispose()
        {
            Marshal.ReleaseComObject(_range);
            _range = null;
        }

        #endregion [ Private Methods ]

        internal Range RangeObject
        {
            get { return _range; }
            set { _range = value; }
        }

    }

}
