using System;
using Microsoft.Office.Interop.Excel;


namespace ClearView.Core.Reporting.Excel
{

    /// <summary>
    /// Font settings for a range.
    /// </summary>
    public class ExcelFont
    {

        #region [ Constructors (1) ]

        internal ExcelFont(Range range)
        {
            _range = range;
        }

        #endregion [ Constructors ]

        #region [ Fields (1) ]

        private Range _range;

        #endregion [ Fields ]

        #region [ Properties (7) ]

        /// <summary>
        /// Display the text with a bold style.
        /// </summary>
        public bool Bold
        {
            get { return Convert.ToBoolean(_range.Font.Bold); }
            set { _range.Font.Bold = value; }
        }

        /// <summary>
        /// Color of the text.
        /// </summary>
        public System.Drawing.Color Color
        {
            get
            {
                return ExcelConversions.GetColorFromOldRgbValue(Convert.ToInt32(_range.Font.Color));
            }
            set
            {
                _range.Font.Color = ExcelConversions.GetOldRgbValueFromColor(value);
            }
        }

        /// <summary>
        /// Display the font with an italic style.
        /// </summary>
        public bool Italic
        {
            get { return Convert.ToBoolean(_range.Font.Italic); }
            set { _range.Font.Italic = value; }
        }

        /// <summary>
        /// Name of the font.
        /// </summary>
        public string Name
        {
            get { return Convert.ToString(_range.Font.Name); }
            set { _range.Font.Name = value; }
        }

        /// <summary>
        /// Size of the font.
        /// </summary>
        public Single Size
        {
            get { return Convert.ToSingle(_range.Font.Size); }
            set { _range.Font.Size = value; }
        }

        /// <summary>
        /// Style of the font.  Such as bold, italic.
        /// </summary>
        public string Style
        {
            get { return Convert.ToString(_range.Font.FontStyle); }
            set { _range.Font.FontStyle = value; }
        }

        /// <summary>
        /// Display the font with an underline.
        /// </summary>
        public bool Underline
        {
            get { return Convert.ToBoolean(_range.Font.Underline); }
            set { _range.Font.Underline = value; }
        }

        #endregion [ Properties ]

    }

}
