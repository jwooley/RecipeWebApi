using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe.Utils
{
    public static class DecimalExtensions
    {
        // Common fractions that may not be represented exactly in decimal
        private static readonly Dictionary<decimal, (int, int)> CommonFractions = new Dictionary<decimal, (int, int)>
        {
            { 0.333333333333333m, (1, 3) },
            { 0.666666666666667m, (2, 3) },
            { 0.166666666666667m, (1, 6) },
            { 0.833333333333333m, (5, 6) },
            { 0.142857142857143m, (1, 7) },
            { 0.285714285714286m, (2, 7) },
            { 0.428571428571429m, (3, 7) },
            { 0.571428571428571m, (4, 7) },
            { 0.714285714285714m, (5, 7) },
            { 0.857142857142857m, (6, 7) },
        };

        // Threshold for detecting common fractions (epsilon)
        private const decimal Tolerance = 0.01m;

        public static string ToFractionString(this string value) => 
            decimal.TryParse(value, out decimal result) ? result.ToFractionString() : value;
        
        public static string ToFractionString(this decimal value, int precision = 3)
        {
            if (precision < 0) 
                throw new ArgumentOutOfRangeException(nameof(precision), "Precision must be non-negative.");
            
            // Handle special case of zero precision
            if (precision == 0)
            {
                return Math.Round(value).ToString();
            }

            // Handle negative values
            bool isNegative = value < 0;
            value = Math.Abs(value);
            
            // Extract whole part and fractional part
            decimal wholePart = Math.Floor(value);
            decimal fractionalPart = value - wholePart;

            // If fractional part is zero, return just the whole part
            if (fractionalPart == 0)
            {
                return isNegative ? $"-{wholePart}" : wholePart.ToString();
            }

            // Check for common fractions first
            foreach (var commonFraction in CommonFractions)
            {
                if (Math.Abs(fractionalPart - commonFraction.Key) < Tolerance)
                {
                    int numerator = commonFraction.Value.Item1;
                    int denominator = commonFraction.Value.Item2;

                    if (wholePart == 0)
                    {
                        return isNegative ? $"-{numerator}/{denominator}" : $"{numerator}/{denominator}";
                    }
                    else
                    {
                        return isNegative ? $"-{wholePart} {numerator}/{denominator}" : $"{wholePart} {numerator}/{denominator}";
                    }
                }
            }

            // Convert fractional part to a fraction with the specified precision
            int standardDenominator = (int)Math.Pow(10, precision);
            int standardNumerator = (int)Math.Round(fractionalPart * standardDenominator);

            // Handle the case where rounding causes numerator to equal denominator
            if (standardNumerator == standardDenominator)
            {
                wholePart += 1;
                standardNumerator = 0;
            }

            // Reduce the fraction if possible
            int gcd = GCD(standardNumerator, standardDenominator);
            if (gcd > 0)
            {
                standardNumerator /= gcd;
                standardDenominator /= gcd;
            }

            // Format the result
            string result;
            if (wholePart == 0 && standardNumerator > 0)
            {
                result = $"{standardNumerator}/{standardDenominator}";
            }
            else if (standardNumerator == 0)
            {
                result = wholePart.ToString();
            }
            else
            {
                result = $"{wholePart} {standardNumerator}/{standardDenominator}";
            }

            // Apply negative sign if needed
            return isNegative ? $"-{result}" : result;
        }

        static int GCD(int a, int b)
        {
            // Ensure positive values for GCD
            a = Math.Abs(a);
            b = Math.Abs(b);
            
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
