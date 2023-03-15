using System;

namespace BinaryCalculator.Helpers
{
    public static class BinaryConvertHelper
    {
        public static string ConvertDecimalToBinary(int value)
        {
            return Convert.ToString(value, 2);
        }

        public static int ConvertBinaryToDecimal(string value)
        {
            try
            {
                return Convert.ToInt32(value, 2);
            }
            catch
            {
                return 0;
            }
        }
    }
}
