using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiaMedFight.Utilities
{
    /// <summary>
    /// Static class containing methods for manipulating strings
    /// </summary>
    internal static class StringUtils
    {

        /// <summary>
        /// Gets all numerical characters from a string.
        /// </summary>
        /// <returns>A series of numerical characters as a string.</returns>
        internal static int ExtractIntFromString(string str)
        {
            string numbers = "";
            int result;
            foreach (char c in str)
                if (char.IsDigit(c))
                    numbers += c;
            if (int.TryParse(numbers, out result)) return result;
            return 0;
        }
    }
}
