using System;

namespace Recipe.Utils
{
    public enum MeasureType
    { Imperial, Metric }

    /// <summary>
    /// Provides functional measurement conversion utilities
    /// </summary>
    public static class MeasureConverters
    {
        public static string TargetUnitForType(MeasureType target, string sourceUnit, double value)
        {
            if (target == MeasureType.Imperial)
            {
                return sourceUnit switch
                {
                    "oz" => "oz",
                    "g" => "oz",
                    "c" when value > 2 && value < 8 => "pt",
                    "c" when value >= 8 => "g",
                    "ts" => "tb",
                    "l" when value < .3 => "c",
                    "l" when value >= .3 && value < 1 => "pt",
                    "l" when value >= 1 => "g",
                    _ => throw new NotSupportedException($"Conversion from {sourceUnit} is not supported.")
                };
            }
            else if (target == MeasureType.Metric)
            {
                return sourceUnit switch
                {
                    "oz" => "g",
                    "g" => "oz",
                    "c " => "l",
                    "pt" => "l",
                    "ts" => "ts",
                    _ => throw new NotSupportedException($"Conversion from {sourceUnit} is not supported.")
                };
            }
            return sourceUnit;

        }
        public static double Convert(MeasureType targetType, string sourceUnit, double value)
        {
            return targetType switch
            {
                MeasureType.Imperial => ConvertToImperial(sourceUnit, value),
                MeasureType.Metric => ConvertToMetric(sourceUnit, value),
                _ => throw new NotSupportedException($"Conversion to {targetType} is not supported.")
            };
        }
        private static double ConvertToImperial(string sourceUnit, double value)
        {
            return sourceUnit switch
            {
                "oz" => value, // Ounces remain the same
                "g" => GramsToOunces(value), // Convert grams to ounces
                "c" => CupsToPints(value), // Convert cups to pints
                "pt" => PintsToGallons(value), // Convert pints to gallons
                "ts" => TeaspoonsToTablespoons(value), // Convert teaspoons to tablespoons
                "l" => LitersToGallons(value), // Convert liters to gallons
                _ => throw new NotSupportedException($"Conversion from {sourceUnit} is not supported.")
            };
        }
        private static double ConvertToMetric(string sourceUnit, double value)
        {
            return sourceUnit switch
            {
                "oz" => OuncesToGrams(value), // Convert ounces to grams
                "c" => CupsToLiters(value), // Convert cups to liters
                "pt" => PintsToLiters(value), // Convert pints to liters
                "g" => GallonsToLiters(value), // Convert gallons to liters
                "ts" => TablespoonsToTeaspoons(value), // Convert tablespoons to teaspoons
                _ => throw new NotSupportedException($"Conversion from {sourceUnit} is not supported.")
            };
        }
        public static double Convert(string sourceUnit, string targetUnit, double value)
        {
            // Normalize unit abbreviations
            sourceUnit = NormalizeUnitAbbreviation(sourceUnit);
            targetUnit = NormalizeUnitAbbreviation(targetUnit);
            
            return sourceUnit switch
            {
                "oz" when targetUnit == "g" => OuncesToGrams(value),
                "g" when targetUnit == "oz" => GramsToOunces(value),
                "c" when targetUnit == "pt" => CupsToPints(value),
                "pt" when targetUnit == "c" => PintsToCups(value),
                "pt" when targetUnit == "g" => PintsToGallons(value),
                "g" when targetUnit == "pt" => GallonsToPints(value),
                "c" when targetUnit == "g" => CupsToGallons(value),
                "g" when targetUnit == "c" => GallonsToCups(value),
                "ts" when targetUnit == "tb" => TeaspoonsToTablespoons(value),
                "tb" when targetUnit == "ts" => TablespoonsToTeaspoons(value),
                // Add new conversions to and from liters
                "c" when targetUnit == "l" => CupsToLiters(value),
                "l" when targetUnit == "c" => LitersToCups(value),
                "pt" when targetUnit == "l" => PintsToLiters(value),
                "l" when targetUnit == "pt" => LitersToPints(value),
                "g" when targetUnit == "l" => GallonsToLiters(value),
                "l" when targetUnit == "g" => LitersToGallons(value),
                _ => throw new NotSupportedException($"Conversion from {sourceUnit} to {targetUnit} is not supported.")
            };
        }

        /// <summary>
        /// Normalizes unit abbreviations to their standard form
        /// </summary>
        /// <param name="unit">The unit abbreviation to normalize</param>
        /// <returns>The normalized unit abbreviation</returns>
        private static string NormalizeUnitAbbreviation(string unit) => unit switch
        {
            "T" => "tb", // Convert uppercase T to tablespoon
            "t" => "ts", // Convert lowercase t to teaspoon
            "liter" => "l", // Convert full word "liter" to abbreviation
            "liters" => "l", // Convert full word "liters" to abbreviation
            "L" => "l", // Convert uppercase L to lowercase l for liters
            null => string.Empty,
            _ => unit.ToLower()    // Return the original unit if no normalization needed
        };
        
        /// <summary>
        /// The conversion factor from ounces to grams (1 oz = 28.349523125 g)
        /// </summary>
        private static readonly double OuncesToGramsFactor = 28.349523125;

        /// <summary>
        /// The conversion factor from cups to pints (1 pint = 2 cups)
        /// </summary>
        private static readonly double CupsToPintsFactor = 0.5;

        /// <summary>
        /// The conversion factor from pints to gallons (1 gallon = 8 pints)
        /// </summary>
        private static readonly double PintsToGallonsFactor = 0.125;

        /// <summary>
        /// The conversion factor from teaspoons to tablespoons (1 tablespoon = 3 teaspoons)
        /// </summary>
        private static readonly double TeaspoonsToTablespoonsFactor = 1.0 / 3.0;

        /// <summary>
        /// The conversion factor from cups to liters (1 cup = 0.2365882365 liters)
        /// </summary>
        private static readonly double CupsToLitersFactor = 0.2365882365;

        /// <summary>
        /// The conversion factor from pints to liters (1 pint = 0.473176473 liters)
        /// </summary>
        private static readonly double PintsToLitersFactor = 0.473176473;

        /// <summary>
        /// The conversion factor from gallons to liters (1 gallon = 3.78541178 liters)
        /// </summary>
        private static readonly double GallonsToLitersFactor = 3.78541178;

        /// <summary>
        /// Creates a converter function that converts from one unit to another using the specified factor
        /// </summary>
        /// <typeparam name="T">The numeric type (double or decimal)</typeparam>
        /// <param name="factor">The conversion factor to multiply by</param>
        /// <returns>A function that performs the conversion</returns>
        private static Func<T, T> CreateConverter<T>(T factor) where T : IConvertible =>
            value => (T)System.Convert.ChangeType(
                System.Convert.ToDouble(value) * System.Convert.ToDouble(factor),
                typeof(T));

        /// <summary>
        /// Creates an inverse converter function based on the specified factor
        /// </summary>
        /// <typeparam name="T">The numeric type (double or decimal)</typeparam>
        /// <param name="factor">The conversion factor to divide by</param>
        /// <returns>A function that performs the inverse conversion</returns>
        private static Func<T, T> CreateInverseConverter<T>(T factor) where T : IConvertible =>
            value => (T)System.Convert.ChangeType(
                System.Convert.ToDouble(value) / System.Convert.ToDouble(factor),
                typeof(T));

        #region Ounces and Grams Conversions

        /// <summary>
        /// Returns a function that converts ounces to grams
        /// </summary>
        /// <returns>A function that converts ounces to grams</returns>
        public static Func<double, double> OuncesToGramsConverter() =>
            CreateConverter<double>(OuncesToGramsFactor);

        /// <summary>
        /// Returns a function that converts ounces to grams
        /// </summary>
        /// <returns>A function that converts ounces to grams</returns>
        public static Func<decimal, decimal> OuncesToGramsConverterDecimal() =>
            CreateConverter<decimal>((decimal)OuncesToGramsFactor);

        /// <summary>
        /// Returns a function that converts grams to ounces
        /// </summary>
        /// <returns>A function that converts grams to ounces</returns>
        public static Func<double, double> GramsToOuncesConverter() =>
            CreateInverseConverter<double>(OuncesToGramsFactor);

        /// <summary>
        /// Returns a function that converts grams to ounces
        /// </summary>
        /// <returns>A function that converts grams to ounces</returns>
        public static Func<decimal, decimal> GramsToOuncesConverterDecimal() =>
            CreateInverseConverter<decimal>((decimal)OuncesToGramsFactor);

        /// <summary>
        /// Converts ounces to grams
        /// </summary>
        /// <param name="ounces">The amount in ounces</param>
        /// <returns>The equivalent amount in grams</returns>
        public static double OuncesToGrams(double ounces) =>
            OuncesToGramsConverter()(ounces);

        /// <summary>
        /// Converts ounces to grams
        /// </summary>
        /// <param name="ounces">The amount in ounces</param>
        /// <returns>The equivalent amount in grams</returns>
        public static decimal OuncesToGrams(decimal ounces) =>
            OuncesToGramsConverterDecimal()(ounces);

        /// <summary>
        /// Converts grams to ounces
        /// </summary>
        /// <param name="grams">The amount in grams</param>
        /// <returns>The equivalent amount in ounces</returns>
        public static double GramsToOunces(double grams) =>
            GramsToOuncesConverter()(grams);

        /// <summary>
        /// Converts grams to ounces
        /// </summary>
        /// <param name="grams">The amount in grams</param>
        /// <returns>The equivalent amount in ounces</returns>
        public static decimal GramsToOunces(decimal grams) =>
            GramsToOuncesConverterDecimal()(grams);

        #endregion

        #region Cups, Pints, and Gallons Conversions

        /// <summary>
        /// Returns a function that converts cups to pints
        /// </summary>
        /// <returns>A function that converts cups to pints</returns>
        public static Func<double, double> CupsToPintsConverter() =>
            CreateConverter<double>(CupsToPintsFactor);

        /// <summary>
        /// Returns a function that converts cups to pints
        /// </summary>
        /// <returns>A function that converts cups to pints</returns>
        public static Func<decimal, decimal> CupsToPintsConverterDecimal() =>
            CreateConverter<decimal>((decimal)CupsToPintsFactor);

        /// <summary>
        /// Returns a function that converts pints to cups
        /// </summary>
        /// <returns>A function that converts pints to cups</returns>
        public static Func<double, double> PintsToCupsConverter() =>
            CreateInverseConverter<double>(CupsToPintsFactor);

        /// <summary>
        /// Returns a function that converts pints to cups
        /// </summary>
        /// <returns>A function that converts pints to cups</returns>
        public static Func<decimal, decimal> PintsToCupsConverterDecimal() =>
            CreateInverseConverter<decimal>((decimal)CupsToPintsFactor);

        /// <summary>
        /// Returns a function that converts pints to gallons
        /// </summary>
        /// <returns>A function that converts pints to gallons</returns>
        public static Func<double, double> PintsToGallonsConverter() =>
            CreateConverter<double>(PintsToGallonsFactor);

        /// <summary>
        /// Returns a function that converts pints to gallons
        /// </summary>
        /// <returns>A function that converts pints to gallons</returns>
        public static Func<decimal, decimal> PintsToGallonsConverterDecimal() =>
            CreateConverter<decimal>((decimal)PintsToGallonsFactor);

        /// <summary>
        /// Returns a function that converts gallons to pints
        /// </summary>
        /// <returns>A function that converts gallons to pints</returns>
        public static Func<double, double> GallonsToPintsConverter() =>
            CreateInverseConverter<double>(PintsToGallonsFactor);

        /// <summary>
        /// Returns a function that converts gallons to pints
        /// </summary>
        /// <returns>A function that converts gallons to pints</returns>
        public static Func<decimal, decimal> GallonsToPintsConverterDecimal() =>
            CreateInverseConverter<decimal>((decimal)PintsToGallonsFactor);

        /// <summary>
        /// Returns a function that converts cups to gallons
        /// </summary>
        /// <returns>A function that converts cups to gallons</returns>
        public static Func<double, double> CupsToGallonsConverter() =>
            ComposeConverters(CupsToPintsConverter(), PintsToGallonsConverter());

        /// <summary>
        /// Returns a function that converts cups to gallons
        /// </summary>
        /// <returns>A function that converts cups to gallons</returns>
        public static Func<decimal, decimal> CupsToGallonsConverterDecimal() =>
            ComposeConverters(CupsToPintsConverterDecimal(), PintsToGallonsConverterDecimal());

        /// <summary>
        /// Returns a function that converts gallons to cups
        /// </summary>
        /// <returns>A function that converts gallons to cups</returns>
        public static Func<double, double> GallonsToCupsConverter() =>
            ComposeConverters(GallonsToPintsConverter(), PintsToCupsConverter());

        /// <summary>
        /// Returns a function that converts gallons to cups
        /// </summary>
        /// <returns>A function that converts gallons to cups</returns>
        public static Func<decimal, decimal> GallonsToCupsConverterDecimal() =>
            ComposeConverters(GallonsToPintsConverterDecimal(), PintsToCupsConverterDecimal());

        /// <summary>
        /// Converts cups to pints
        /// </summary>
        /// <param name="cups">The amount in cups</param>
        /// <returns>The equivalent amount in pints</returns>
        public static double CupsToPints(double cups) =>
            CupsToPintsConverter()(cups);

        /// <summary>
        /// Converts cups to pints
        /// </summary>
        /// <param name="cups">The amount in cups</param>
        /// <returns>The equivalent amount in pints</returns>
        public static decimal CupsToPints(decimal cups) =>
            CupsToPintsConverterDecimal()(cups);

        /// <summary>
        /// Converts pints to cups
        /// </summary>
        /// <param name="pints">The amount in pints</param>
        /// <returns>The equivalent amount in cups</returns>
        public static double PintsToCups(double pints) =>
            PintsToCupsConverter()(pints);

        /// <summary>
        /// Converts pints to cups
        /// </summary>
        /// <param name="pints">The amount in pints</param>
        /// <returns>The equivalent amount in cups</returns>
        public static decimal PintsToCups(decimal pints) =>
            PintsToCupsConverterDecimal()(pints);

        /// <summary>
        /// Converts pints to gallons
        /// </summary>
        /// <param name="pints">The amount in pints</param>
        /// <returns>The equivalent amount in gallons</returns>
        public static double PintsToGallons(double pints) =>
            PintsToGallonsConverter()(pints);

        /// <summary>
        /// Converts pints to gallons
        /// </summary>
        /// <param name="pints">The amount in pints</param>
        /// <returns>The equivalent amount in gallons</returns>
        public static decimal PintsToGallons(decimal pints) =>
            PintsToGallonsConverterDecimal()(pints);

        /// <summary>
        /// Converts gallons to pints
        /// </summary>
        /// <param name="gallons">The amount in gallons</param>
        /// <returns>The equivalent amount in pints</returns>
        public static double GallonsToPints(double gallons) =>
            GallonsToPintsConverter()(gallons);

        /// <summary>
        /// Converts gallons to pints
        /// </summary>
        /// <param name="gallons">The amount in gallons</param>
        /// <returns>The equivalent amount in pints</returns>
        public static decimal GallonsToPints(decimal gallons) =>
            GallonsToPintsConverterDecimal()(gallons);

        /// <summary>
        /// Converts cups to gallons
        /// </summary>
        /// <param name="cups">The amount in cups</param>
        /// <returns>The equivalent amount in gallons</returns>
        public static double CupsToGallons(double cups) =>
            CupsToGallonsConverter()(cups);

        /// <summary>
        /// Converts cups to gallons
        /// </summary>
        /// <param name="cups">The amount in cups</param>
        /// <returns>The equivalent amount in gallons</returns>
        public static decimal CupsToGallons(decimal cups) =>
            CupsToGallonsConverterDecimal()(cups);

        /// <summary>
        /// Converts gallons to cups
        /// </summary>
        /// <param name="gallons">The amount in gallons</param>
        /// <returns>The equivalent amount in cups</returns>
        public static double GallonsToCups(double gallons) =>
            GallonsToCupsConverter()(gallons);

        /// <summary>
        /// Converts gallons to cups
        /// </summary>
        /// <param name="gallons">The amount in gallons</param>
        /// <returns>The equivalent amount in cups</returns>
        public static decimal GallonsToCups(decimal gallons) =>
            GallonsToCupsConverterDecimal()(gallons);

        #endregion

        #region Cups, Pints, and Gallons to Liters Conversions

        /// <summary>
        /// Returns a function that converts cups to liters
        /// </summary>
        /// <returns>A function that converts cups to liters</returns>
        public static Func<double, double> CupsToLitersConverter() =>
            CreateConverter<double>(CupsToLitersFactor);

        /// <summary>
        /// Returns a function that converts cups to liters
        /// </summary>
        /// <returns>A function that converts cups to liters</returns>
        public static Func<decimal, decimal> CupsToLitersConverterDecimal() =>
            CreateConverter<decimal>((decimal)CupsToLitersFactor);

        /// <summary>
        /// Returns a function that converts liters to cups
        /// </summary>
        /// <returns>A function that converts liters to cups</returns>
        public static Func<double, double> LitersToCupsConverter() =>
            CreateInverseConverter<double>(CupsToLitersFactor);

        /// <summary>
        /// Returns a function that converts liters to cups
        /// </summary>
        /// <returns>A function that converts liters to cups</returns>
        public static Func<decimal, decimal> LitersToCupsConverterDecimal() =>
            CreateInverseConverter<decimal>((decimal)CupsToLitersFactor);

        /// <summary>
        /// Returns a function that converts pints to liters
        /// </summary>
        /// <returns>A function that converts pints to liters</returns>
        public static Func<double, double> PintsToLitersConverter() =>
            CreateConverter<double>(PintsToLitersFactor);

        /// <summary>
        /// Returns a function that converts pints to liters
        /// </summary>
        /// <returns>A function that converts pints to liters</returns>
        public static Func<decimal, decimal> PintsToLitersConverterDecimal() =>
            CreateConverter<decimal>((decimal)PintsToLitersFactor);

        /// <summary>
        /// Returns a function that converts liters to pints
        /// </summary>
        /// <returns>A function that converts liters to pints</returns>
        public static Func<double, double> LitersToPintsConverter() =>
            CreateInverseConverter<double>(PintsToLitersFactor);

        /// <summary>
        /// Returns a function that converts liters to pints
        /// </summary>
        /// <returns>A function that converts liters to pints</returns>
        public static Func<decimal, decimal> LitersToPintsConverterDecimal() =>
            CreateInverseConverter<decimal>((decimal)PintsToLitersFactor);

        /// <summary>
        /// Returns a function that converts gallons to liters
        /// </summary>
        /// <returns>A function that converts gallons to liters</returns>
        public static Func<double, double> GallonsToLitersConverter() =>
            CreateConverter<double>(GallonsToLitersFactor);

        /// <summary>
        /// Returns a function that converts gallons to liters
        /// </summary>
        /// <returns>A function that converts gallons to liters</returns>
        public static Func<decimal, decimal> GallonsToLitersConverterDecimal() =>
            CreateConverter<decimal>((decimal)GallonsToLitersFactor);

        /// <summary>
        /// Returns a function that converts liters to gallons
        /// </summary>
        /// <returns>A function that converts liters to gallons</returns>
        public static Func<double, double> LitersToGallonsConverter() =>
            CreateInverseConverter<double>(GallonsToLitersFactor);

        /// <summary>
        /// Returns a function that converts liters to gallons
        /// </summary>
        /// <returns>A function that converts liters to gallons</returns>
        public static Func<decimal, decimal> LitersToGallonsConverterDecimal() =>
            CreateInverseConverter<decimal>((decimal)GallonsToLitersFactor);

        /// <summary>
        /// Converts cups to liters
        /// </summary>
        /// <param name="cups">The amount in cups</param>
        /// <returns>The equivalent amount in liters</returns>
        public static double CupsToLiters(double cups) =>
            CupsToLitersConverter()(cups);

        /// <summary>
        /// Converts cups to liters
        /// </summary>
        /// <param name="cups">The amount in cups</param>
        /// <returns>The equivalent amount in liters</returns>
        public static decimal CupsToLiters(decimal cups) =>
            CupsToLitersConverterDecimal()(cups);

        /// <summary>
        /// Converts liters to cups
        /// </summary>
        /// <param name="liters">The amount in liters</param>
        /// <returns>The equivalent amount in cups</returns>
        public static double LitersToCups(double liters) =>
            LitersToCupsConverter()(liters);

        /// <summary>
        /// Converts liters to cups
        /// </summary>
        /// <param name="liters">The amount in liters</param>
        /// <returns>The equivalent amount in cups</returns>
        public static decimal LitersToCups(decimal liters) =>
            LitersToCupsConverterDecimal()(liters);

        /// <summary>
        /// Converts pints to liters
        /// </summary>
        /// <param name="pints">The amount in pints</param>
        /// <returns>The equivalent amount in liters</returns>
        public static double PintsToLiters(double pints) =>
            PintsToLitersConverter()(pints);

        /// <summary>
        /// Converts pints to liters
        /// </summary>
        /// <param name="pints">The amount in pints</param>
        /// <returns>The equivalent amount in liters</returns>
        public static decimal PintsToLiters(decimal pints) =>
            PintsToLitersConverterDecimal()(pints);

        /// <summary>
        /// Converts liters to pints
        /// </summary>
        /// <param name="liters">The amount in liters</param>
        /// <returns>The equivalent amount in pints</returns>
        public static double LitersToPints(double liters) =>
            LitersToPintsConverter()(liters);

        /// <summary>
        /// Converts liters to pints
        /// </summary>
        /// <param name="liters">The amount in liters</param>
        /// <returns>The equivalent amount in pints</returns>
        public static decimal LitersToPints(decimal liters) =>
            LitersToPintsConverterDecimal()(liters);

        /// <summary>
        /// Converts gallons to liters
        /// </summary>
        /// <param name="gallons">The amount in gallons</param>
        /// <returns>The equivalent amount in liters</returns>
        public static double GallonsToLiters(double gallons) =>
            GallonsToLitersConverter()(gallons);

        /// <summary>
        /// Converts gallons to liters
        /// </summary>
        /// <param name="gallons">The amount in gallons</param>
        /// <returns>The equivalent amount in liters</returns>
        public static decimal GallonsToLiters(decimal gallons) =>
            GallonsToLitersConverterDecimal()(gallons);

        /// <summary>
        /// Converts liters to gallons
        /// </summary>
        /// <param name="liters">The amount in liters</param>
        /// <returns>The equivalent amount in gallons</returns>
        public static double LitersToGallons(double liters) =>
            LitersToGallonsConverter()(liters);

        /// <summary>
        /// Converts liters to gallons
        /// </summary>
        /// <param name="liters">The amount in liters</param>
        /// <returns>The equivalent amount in gallons</returns>
        public static decimal LitersToGallons(decimal liters) =>
            LitersToGallonsConverterDecimal()(liters);

        #endregion

        #region Teaspoons and Tablespoons Conversions

        /// <summary>
        /// Returns a function that converts teaspoons to tablespoons
        /// </summary>
        /// <returns>A function that converts teaspoons to tablespoons</returns>
        public static Func<double, double> TeaspoonsToTablespoonsConverter() =>
            CreateConverter<double>(TeaspoonsToTablespoonsFactor);

        /// <summary>
        /// Returns a function that converts teaspoons to tablespoons
        /// </summary>
        /// <returns>A function that converts teaspoons to tablespoons</returns>
        public static Func<decimal, decimal> TeaspoonsToTablespoonsConverterDecimal() =>
            CreateConverter<decimal>((decimal)TeaspoonsToTablespoonsFactor);

        /// <summary>
        /// Returns a function that converts tablespoons to teaspoons
        /// </summary>
        /// <returns>A function that converts tablespoons to teaspoons</returns>
        public static Func<double, double> TablespoonsToTeaspoonsConverter() =>
            CreateInverseConverter<double>(TeaspoonsToTablespoonsFactor);

        /// <summary>
        /// Returns a function that converts tablespoons to teaspoons
        /// </summary>
        /// <returns>A function that converts tablespoons to teaspoons</returns>
        public static Func<decimal, decimal> TablespoonsToTeaspoonsConverterDecimal() =>
            CreateInverseConverter<decimal>((decimal)TeaspoonsToTablespoonsFactor);

        /// <summary>
        /// Converts teaspoons to tablespoons
        /// </summary>
        /// <param name="teaspoons">The amount in teaspoons</param>
        /// <returns>The equivalent amount in tablespoons</returns>
        public static double TeaspoonsToTablespoons(double teaspoons) =>
            TeaspoonsToTablespoonsConverter()(teaspoons);

        /// <summary>
        /// Converts teaspoons to tablespoons
        /// </summary>
        /// <param name="teaspoons">The amount in teaspoons</param>
        /// <returns>The equivalent amount in tablespoons</returns>
        public static decimal TeaspoonsToTablespoons(decimal teaspoons) =>
            TeaspoonsToTablespoonsConverterDecimal()(teaspoons);

        /// <summary>
        /// Converts tablespoons to teaspoons
        /// </summary>
        /// <param name="tablespoons">The amount in tablespoons</param>
        /// <returns>The equivalent amount in teaspoons</returns>
        public static double TablespoonsToTeaspoons(double tablespoons) =>
            TablespoonsToTeaspoonsConverter()(tablespoons);

        /// <summary>
        /// Converts tablespoons to teaspoons
        /// </summary>
        /// <param name="tablespoons">The amount in tablespoons</param>
        /// <returns>The equivalent amount in teaspoons</returns>
        public static decimal TablespoonsToTeaspoons(decimal tablespoons) =>
            TablespoonsToTeaspoonsConverterDecimal()(tablespoons);

        #endregion

        /// <summary>
        /// Applies a measurement conversion function to a value
        /// </summary>
        /// <typeparam name="T">The numeric type</typeparam>
        /// <param name="value">The value to convert</param>
        /// <param name="converter">The conversion function to apply</param>
        /// <returns>The converted value</returns>
        public static T ApplyConverter<T>(T value, Func<T, T> converter) where T : IConvertible =>
            converter(value);

        /// <summary>
        /// Creates a composed conversion function that applies the second conversion after the first
        /// </summary>
        /// <typeparam name="T">The numeric type</typeparam>
        /// <param name="first">The first conversion function</param>
        /// <param name="second">The second conversion function</param>
        /// <returns>A function that applies both conversions in sequence</returns>
        public static Func<T, T> ComposeConverters<T>(Func<T, T> first, Func<T, T> second) where T : IConvertible =>
            value => second(first(value));
    }
}
