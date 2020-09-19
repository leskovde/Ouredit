using System;
using System.Collections.Generic;
using System.Text;

namespace Components
{
    /// <summary>
    /// Sets the entire boolean array to its default value - false.
    /// </summary>
    [Leskovar]
    public static class ExtensionMethods
    {
        public static void Clear(this bool[] boolArray)
        {
            for (var i = 0; i < boolArray.Length; i++)
            {
                boolArray[i] = false;
            }
        }
    }
}
