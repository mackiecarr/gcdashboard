using System;
using System.Drawing;

namespace ClearView.Core.Reporting.Excel
{

    /// <summary>
    /// Conversions for working with Excel objects.
    /// </summary>
    internal static class ExcelConversions
    {

        #region Color Conversions

        /* Excel uses the old RBG integer value for storing colors,
         * while the .Net Framwork uses the new ARGB format.
         * 
         * This provides a means of converting between to the two.
         * 
         * Based on posting at http://forums.msdn.microsoft.com/en-US/csharpgeneral/thread/8a5e0d88-3695-43e3-b930-5b4a95bbf154/
         * 
         * 
         * errmm..  guess I/we were wrong.   it turns out the code sample shown above doesn't work.
         * It is because the Red and Blue were switched between RGB and ARGB
         * 
         * Visual Studio 6.0 RGB numeric format: BBBBBBBBGGGGGGGGRRRRRRRR
         * 
         * Visual Studio .NET 2005 ARGB numeric format: AAAAAAARRRRRRRRGGGGGGGGBBBBBBBB
         * 
         * where A=alpha, R=red, G=green, B=blue.
         * 
         * So you must actually flip around the red and blue components.
         * Code to convert old RGB into new ARGB:
         * 
         * Color clr = Color.FromArgb(Convert.ToInt32(pRGBColor));
         * clr = Color.FromArgb(255, (int)clr.B, (int)clr.G, (int)clr.R);
         * 
         * Code to convert new ARGB back into old RGB
         * 
         * Color clr = cmdButton.BackColor;
         * clr = Color.FromArgb(0, clr.B, clr.G, clr.R);
         * 
         */

        /// <summary>
        /// Convert a <see cref="System.Drawing.Color"/> object that uses
        /// the new ARGB format to a int value that Excel recognizes.
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        internal static int GetOldRgbValueFromColor(Color color)
        {
            Color newColor = Color.FromArgb(0, color.B, color.G, color.R);
            return newColor.ToArgb();
        }

        /// <summary>
        /// Convert an old RGB interger value to a .Net color.
        /// </summary>
        /// <param name="oldValue"></param>
        /// <returns></returns>
        internal static Color GetColorFromOldRgbValue(int oldValue)
        {
            var color = Color.FromArgb(oldValue);
            var color2 = Color.FromArgb(255, (int)color.B, (int)color.G, (int)color.R);
            return color2;
        }

        #endregion

    }

}
