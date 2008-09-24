using System;
using Drawing = System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;


namespace ClearView.Core.Reporting.Excel
{

    /// <summary>
    /// Provides a means of writing and manipulating Excel workbooks for reporting.
    /// </summary>
    public class ExcelWorkbook : IDisposable
    {

        #region [ Constructors (1) ]

        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelWorkbook"/> class.
        /// </summary>
        public ExcelWorkbook()
        {
            _excelApp = new Application();
        }

        #endregion [ Constructors ]

        #region [ Fields (3) ]

        private Application _excelApp;
        private Workbook _currentWorkbook;
        private Worksheet _currentSheet;

        #endregion [ Fields ]

        #region [ Public Methods (10) ]

        /// <summary>
        /// Gets a cell.
        /// </summary>
        /// <param name="address">The address of the cell.</param>
        public ExcelRange Cell(string address)
        {
            return new ExcelRange(_currentSheet.get_Range(address, address));
        }

        /// <summary>
        /// Create a new workbook.
        /// </summary>
        public void CreateNewWorkbook()
        {
            ReleaseCurrentWorkbook();
            _currentWorkbook = _excelApp.Workbooks.Add(Type.Missing);
            _currentSheet = (Worksheet)_currentWorkbook.Worksheets[1];
        }

        /// <summary>
        /// Create a new worksheet.
        /// </summary>
        /// <param name="name">Name of the worksheet.</param>
        public void CreateNewWorkSheet(string name)
        {
            if (_currentWorkbook == null)
                throw new InvalidOperationException("Cannot create a new sheet when there is no current workbook.");

            ReleaseCurrentWorksheet();
            _currentSheet = (Worksheet)_currentWorkbook.Worksheets.Add(Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            _currentSheet.Name = name;
        }

        /// <summary>
        /// Open an existing workbook.
        /// </summary>
        /// <param name="filename"></param>
        public void Open(string filename)
        {
            _currentWorkbook = _excelApp.Workbooks.Open(filename,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            _currentSheet = (Worksheet)_currentWorkbook.Worksheets[1];
        }

        /// <summary>
        /// Gets a range of cells.
        /// </summary>
        /// <param name="startingCell">The address of the first cell in the range.</param>
        /// <param name="endingCell">The address of the last cell in the range.</param>
        /// <returns>An <see cref="ExcelRange"/> object representing the range of cells.</returns>
        public ExcelRange Range(string startingCell, string endingCell)
        {
            return new ExcelRange(_currentSheet.get_Range(startingCell, endingCell));
        }

        /// <summary>
        /// Saves the current workbook.
        /// </summary>
        /// <remarks>
        /// This appears to work only if the workbook was opened
        /// from an existing file, or if the <see cref="SaveAs"/> method
        /// has been called.
        /// </remarks>
        public void Save()
        {
            _currentWorkbook.Save();
        }

        /// <summary>
        /// Saves the current workbook using the given filename.
        /// </summary>
        /// <param name="filename">The full filename to save the workbook with.</param>
        public void SaveAs(string filename)
        {
            _excelApp.DisplayAlerts = false;  //dont warn me about overwriting.
            _currentWorkbook.SaveAs(filename, XlFileFormat.xlOpenXMLWorkbook, Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            _excelApp.DisplayAlerts = true;
        }

        /// <summary>
        /// Switch to a worksheet.
        /// </summary>
        /// <param name="name">Name of the worksheet to switch to.</param>
        public void SwitchToSheet(string name)
        {
            foreach (Worksheet s in _currentWorkbook.Worksheets)
            {
                if (s.Name == name)
                {
                    ReleaseCurrentWorksheet();

                    _currentSheet = s;
                    return;
                }
            }
        }

        /// <summary>
        /// Enter a value into a cell.
        /// </summary>
        public void Write(string cellAddress, object value)
        {
            _currentSheet.get_Range(cellAddress, cellAddress).Value2 = value;
        }

        /// <summary>
        /// Enter a value into a cell.
        /// </summary>
        /// <param name="row">Row number of the cell, staring with 1.</param>
        /// <param name="column">Column number of the cell, column A is 1.</param>
        /// <param name="value">Value to put into the cell.</param>
        public void Write(int row, int column, object value)
        {
            _currentSheet.Cells[row, column] = value;
        }

        #endregion [ Public Methods ]

        #region [ Private Methods (3) ]

        /// <summary>
        /// Release unmanaged object references.
        /// </summary>
        void IDisposable.Dispose()
        {
            try
            {
                Marshal.ReleaseComObject(_currentSheet);
                _currentSheet = null;
            }
            catch (Exception) { }

            try
            {
                Marshal.ReleaseComObject(_currentWorkbook);
                _currentWorkbook = null;
            }
            catch (Exception) { }

            try
            {
                _excelApp.Quit();
                Marshal.ReleaseComObject(_excelApp);
                _excelApp = null;
            }
            catch (Exception) { }
        }

        /// <summary>
        /// Release reference to the current worksheet.
        /// </summary>
        private void ReleaseCurrentWorksheet()
        {
            if (_currentSheet != null)
            {
                Marshal.ReleaseComObject(_currentSheet);
                _currentSheet = null;
            }
        }

        /// <summary>
        /// Release reference to the current workbook.
        /// </summary>
        private void ReleaseCurrentWorkbook()
        {
            ReleaseCurrentWorksheet();

            if (_currentWorkbook != null)
            {
                Marshal.ReleaseComObject(_currentWorkbook);
                _currentWorkbook = null;
            }
        }

        #endregion [ Private Methods ]

    }

}
