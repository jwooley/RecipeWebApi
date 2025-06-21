using Recipe.Utils;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Recipe.Test
{
    public class MeasureConvertersTests
    {
        private readonly ITestOutputHelper _output;
        private const double Tolerance = 0.0001;

        public MeasureConvertersTests(ITestOutputHelper output)
        {
            _output = output;
        }

        #region Convert Method Tests

        [Theory]
        [InlineData("oz", "g", 1.0, 28.349523125)] // Ounces to Grams
        [InlineData("g", "oz", 28.349523125, 1.0)] // Grams to Ounces
        [InlineData("c", "pt", 2.0, 1.0)] // Cups to Pints
        [InlineData("pt", "c", 1.0, 2.0)] // Pints to Cups
        [InlineData("pt", "g", 8.0, 1.0)] // Pints to Gallons
        [InlineData("g", "pt", 1.0, 8.0)] // Gallons to Pints
        [InlineData("c", "g", 16.0, 1.0)] // Cups to Gallons
        [InlineData("g", "c", 1.0, 16.0)] // Gallons to Cups
        [InlineData("ts", "tb", 3.0, 1.0)] // Teaspoons to Tablespoons
        [InlineData("tb", "ts", 1.0, 3.0)] // Tablespoons to Teaspoons
        public void Convert_StandardUnits_ConvertsCorrectly(string sourceUnit, string targetUnit, double value, double expected)
        {
            // Act
            double actual = MeasureConverters.Convert(sourceUnit, targetUnit, value);

            // Assert
            _output.WriteLine($"Converting {value} {sourceUnit} to {targetUnit}: Expected {expected}, got {actual}");
            Assert.Equal(expected, actual, Tolerance);
        }

        [Theory]
        [InlineData("T", "ts", 1.0, 3.0)] // 'T' (tablespoon) to teaspoons
        [InlineData("t", "tb", 3.0, 1.0)] // 't' (teaspoon) to tablespoons
        [InlineData("t", "T", 3.0, 1.0)] // 't' (teaspoon) to 'T' (tablespoon)
        public void Convert_WithAbbreviations_NormalizesAndConvertsCorrectly(string sourceUnit, string targetUnit, double value, double expected)
        {
            // Act
            double actual = MeasureConverters.Convert(sourceUnit, targetUnit, value);

            // Assert
            _output.WriteLine($"Converting {value} {sourceUnit} to {targetUnit}: Expected {expected}, got {actual}");
            Assert.Equal(expected, actual, Tolerance);
        }

        [Theory]
        [InlineData("unknown", "g", 1.0)]
        [InlineData("oz", "unknown", 1.0)]
        [InlineData("unknown1", "unknown2", 1.0)]
        public void Convert_UnsupportedUnits_ThrowsNotSupportedException(string sourceUnit, string targetUnit, double value)
        {
            // Act & Assert
            var exception = Assert.Throws<NotSupportedException>(() =>
                MeasureConverters.Convert(sourceUnit, targetUnit, value));

            _output.WriteLine($"Exception message: {exception.Message}");
            Assert.Contains(sourceUnit, exception.Message);
            Assert.Contains(targetUnit, exception.Message);
        }

        #endregion

        #region Ounces and Grams Tests

        [Theory]
        [InlineData(1.0, 28.349523125)]
        [InlineData(0.0, 0.0)]
        [InlineData(2.5, 70.87380781249999)]
        [InlineData(16.0, 453.5923699999999)]
        public void OuncesToGrams_Double_ConvertsCorrectly(double ounces, double expectedGrams)
        {
            // Act
            double actualGrams = MeasureConverters.OuncesToGrams(ounces);

            // Assert
            _output.WriteLine($"Converting {ounces} oz to grams: Expected {expectedGrams}, got {actualGrams}");
            Assert.Equal(expectedGrams, actualGrams, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 28.349523125)]
        [InlineData(0.0, 0.0)]
        [InlineData(2.5, 70.87380781249999)]
        [InlineData(16.0, 453.5923699999999)]
        public void OuncesToGrams_UsingConvertMethod_ConvertsCorrectly(double ounces, double expectedGrams)
        {
            // Act
            double actualGrams = MeasureConverters.Convert("oz", "g", ounces);

            // Assert
            _output.WriteLine($"Using Convert method: {ounces} oz to grams: Expected {expectedGrams}, got {actualGrams}");
            Assert.Equal(expectedGrams, actualGrams, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 0.035273962)]
        [InlineData(0.0, 0.0)]
        [InlineData(100.0, 3.5273962)]
        [InlineData(28.349523125, 1.0)]
        public void GramsToOunces_Double_ConvertsCorrectly(double grams, double expectedOunces)
        {
            // Act
            double actualOunces = MeasureConverters.GramsToOunces(grams);

            // Assert
            _output.WriteLine($"Converting {grams} g to ounces: Expected {expectedOunces}, got {actualOunces}");
            Assert.Equal(expectedOunces, actualOunces, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 0.035273962)]
        [InlineData(0.0, 0.0)]
        [InlineData(100.0, 3.5273962)]
        [InlineData(28.349523125, 1.0)]
        public void GramsToOunces_UsingConvertMethod_ConvertsCorrectly(double grams, double expectedOunces)
        {
            // Act
            double actualOunces = MeasureConverters.Convert("g", "oz", grams);

            // Assert
            _output.WriteLine($"Using Convert method: {grams} g to ounces: Expected {expectedOunces}, got {actualOunces}");
            Assert.Equal(expectedOunces, actualOunces, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 28.349523125)]
        [InlineData(0.0, 0.0)]
        [InlineData(2.5, 70.87380781249999)]
        [InlineData(16.0, 453.5923699999999)]
        public void OuncesToGramsConverter_ReturnsCorrectFunction(double ounces, double expectedGrams)
        {
            // Arrange
            var converter = MeasureConverters.OuncesToGramsConverter();

            // Act
            double actualGrams = converter(ounces);

            // Assert
            _output.WriteLine($"Using converter function: {ounces} oz to grams: Expected {expectedGrams}, got {actualGrams}");
            Assert.Equal(expectedGrams, actualGrams, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 0.035273962)]
        [InlineData(0.0, 0.0)]
        [InlineData(100.0, 3.5273962)]
        [InlineData(28.349523125, 1.0)]
        public void GramsToOuncesConverter_ReturnsCorrectFunction(double grams, double expectedOunces)
        {
            // Arrange
            var converter = MeasureConverters.GramsToOuncesConverter();

            // Act
            double actualOunces = converter(grams);

            // Assert
            _output.WriteLine($"Using converter function: {grams} g to ounces: Expected {expectedOunces}, got {actualOunces}");
            Assert.Equal(expectedOunces, actualOunces, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 28.349523125)]
        [InlineData(0.0, 0.0)]
        [InlineData(2.5, 70.87380781249999)]
        [InlineData(16.0, 453.5923699999999)]
        public void OuncesToGrams_Decimal_ConvertsCorrectly(decimal ounces, decimal expectedGrams)
        {
            // Act
            decimal actualGrams = MeasureConverters.OuncesToGrams(ounces);

            // Assert
            _output.WriteLine($"Converting {ounces} oz to grams (decimal): Expected {expectedGrams}, got {actualGrams}");
            Assert.Equal(expectedGrams, actualGrams, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(1.0, 0.035273962)]
        [InlineData(0.0, 0.0)]
        [InlineData(100.0, 3.5273962)]
        [InlineData(28.349523125, 1.0)]
        public void GramsToOunces_Decimal_ConvertsCorrectly(decimal grams, decimal expectedOunces)
        {
            // Act
            decimal actualOunces = MeasureConverters.GramsToOunces(grams);

            // Assert
            _output.WriteLine($"Converting {grams} g to ounces (decimal): Expected {expectedOunces}, got {actualOunces}");
            Assert.Equal(expectedOunces, actualOunces, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(1.0, 28.349523125)]
        [InlineData(0.0, 0.0)]
        [InlineData(2.5, 70.87380781249999)]
        public void OuncesToGramsConverterDecimal_ReturnsCorrectFunction(decimal ounces, decimal expectedGrams)
        {
            // Arrange
            var converter = MeasureConverters.OuncesToGramsConverterDecimal();

            // Act
            decimal actualGrams = converter(ounces);

            // Assert
            _output.WriteLine($"Using decimal converter function: {ounces} oz to grams: Expected {expectedGrams}, got {actualGrams}");
            Assert.Equal(expectedGrams, actualGrams, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(1.0, 0.035273962)]
        [InlineData(0.0, 0.0)]
        [InlineData(100.0, 3.5273962)]
        public void GramsToOuncesConverterDecimal_ReturnsCorrectFunction(decimal grams, decimal expectedOunces)
        {
            // Arrange
            var converter = MeasureConverters.GramsToOuncesConverterDecimal();

            // Act
            decimal actualOunces = converter(grams);

            // Assert
            _output.WriteLine($"Using decimal converter function: {grams} g to ounces: Expected {expectedOunces}, got {actualOunces}");
            Assert.Equal(expectedOunces, actualOunces, 4); // 4 decimal places precision
        }

        #endregion

        #region Cups, Pints, and Gallons Tests

        [Theory]
        [InlineData(2.0, 1.0)]     // 2 cups = 1 pint
        [InlineData(0.0, 0.0)]     // 0 cups = 0 pints
        [InlineData(4.0, 2.0)]     // 4 cups = 2 pints
        [InlineData(0.5, 0.25)]    // 0.5 cups = 0.25 pints
        public void CupsToPints_Double_ConvertsCorrectly(double cups, double expectedPints)
        {
            // Act
            double actualPints = MeasureConverters.CupsToPints(cups);

            // Assert
            _output.WriteLine($"Converting {cups} cups to pints: Expected {expectedPints}, got {actualPints}");
            Assert.Equal(expectedPints, actualPints, Tolerance);
        }

        [Theory]
        [InlineData(2.0, 1.0)]     // 2 cups = 1 pint
        [InlineData(0.0, 0.0)]     // 0 cups = 0 pints
        [InlineData(4.0, 2.0)]     // 4 cups = 2 pints
        [InlineData(0.5, 0.25)]    // 0.5 cups = 0.25 pints
        public void CupsToPints_UsingConvertMethod_ConvertsCorrectly(double cups, double expectedPints)
        {
            // Act
            double actualPints = MeasureConverters.Convert("c", "pt", cups);

            // Assert
            _output.WriteLine($"Using Convert method: {cups} cups to pints: Expected {expectedPints}, got {actualPints}");
            Assert.Equal(expectedPints, actualPints, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 2.0)]     // 1 pint = 2 cups
        [InlineData(0.0, 0.0)]     // 0 pints = 0 cups
        [InlineData(2.0, 4.0)]     // 2 pints = 4 cups
        [InlineData(0.25, 0.5)]    // 0.25 pints = 0.5 cups
        public void PintsToCups_Double_ConvertsCorrectly(double pints, double expectedCups)
        {
            // Act
            double actualCups = MeasureConverters.PintsToCups(pints);

            // Assert
            _output.WriteLine($"Converting {pints} pints to cups: Expected {expectedCups}, got {actualCups}");
            Assert.Equal(expectedCups, actualCups, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 2.0)]     // 1 pint = 2 cups
        [InlineData(0.0, 0.0)]     // 0 pints = 0 cups
        [InlineData(2.0, 4.0)]     // 2 pints = 4 cups
        [InlineData(0.25, 0.5)]    // 0.25 pints = 0.5 cups
        public void PintsToCups_UsingConvertMethod_ConvertsCorrectly(double pints, double expectedCups)
        {
            // Act
            double actualCups = MeasureConverters.Convert("pt", "c", pints);

            // Assert
            _output.WriteLine($"Using Convert method: {pints} pints to cups: Expected {expectedCups}, got {actualCups}");
            Assert.Equal(expectedCups, actualCups, Tolerance);
        }

        [Theory]
        [InlineData(8.0, 1.0)]     // 8 pints = 1 gallon
        [InlineData(0.0, 0.0)]     // 0 pints = 0 gallons
        [InlineData(16.0, 2.0)]    // 16 pints = 2 gallons
        [InlineData(4.0, 0.5)]     // 4 pints = 0.5 gallons
        public void PintsToGallons_Double_ConvertsCorrectly(double pints, double expectedGallons)
        {
            // Act
            double actualGallons = MeasureConverters.PintsToGallons(pints);

            // Assert
            _output.WriteLine($"Converting {pints} pints to gallons: Expected {expectedGallons}, got {actualGallons}");
            Assert.Equal(expectedGallons, actualGallons, Tolerance);
        }

        [Theory]
        [InlineData(8.0, 1.0)]     // 8 pints = 1 gallon
        [InlineData(0.0, 0.0)]     // 0 pints = 0 gallons
        [InlineData(16.0, 2.0)]    // 16 pints = 2 gallons
        [InlineData(4.0, 0.5)]     // 4 pints = 0.5 gallons
        public void PintsToGallons_UsingConvertMethod_ConvertsCorrectly(double pints, double expectedGallons)
        {
            // Act
            double actualGallons = MeasureConverters.Convert("pt", "g", pints);

            // Assert
            _output.WriteLine($"Using Convert method: {pints} pints to gallons: Expected {expectedGallons}, got {actualGallons}");
            Assert.Equal(expectedGallons, actualGallons, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 8.0)]     // 1 gallon = 8 pints
        [InlineData(0.0, 0.0)]     // 0 gallons = 0 pints
        [InlineData(2.0, 16.0)]    // 2 gallons = 16 pints
        [InlineData(0.5, 4.0)]     // 0.5 gallons = 4 pints
        public void GallonsToPints_Double_ConvertsCorrectly(double gallons, double expectedPints)
        {
            // Act
            double actualPints = MeasureConverters.GallonsToPints(gallons);

            // Assert
            _output.WriteLine($"Converting {gallons} gallons to pints: Expected {expectedPints}, got {actualPints}");
            Assert.Equal(expectedPints, actualPints, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 8.0)]     // 1 gallon = 8 pints
        [InlineData(0.0, 0.0)]     // 0 gallons = 0 pints
        [InlineData(2.0, 16.0)]    // 2 gallons = 16 pints
        [InlineData(0.5, 4.0)]     // 0.5 gallons = 4 pints
        public void GallonsToPints_UsingConvertMethod_ConvertsCorrectly(double gallons, double expectedPints)
        {
            // Act
            double actualPints = MeasureConverters.Convert("g", "pt", gallons);

            // Assert
            _output.WriteLine($"Using Convert method: {gallons} gallons to pints: Expected {expectedPints}, got {actualPints}");
            Assert.Equal(expectedPints, actualPints, Tolerance);
        }

        [Theory]
        [InlineData(16.0, 1.0)]    // 16 cups = 1 gallon
        [InlineData(0.0, 0.0)]     // 0 cups = 0 gallons
        [InlineData(32.0, 2.0)]    // 32 cups = 2 gallons
        [InlineData(8.0, 0.5)]     // 8 cups = 0.5 gallons
        public void CupsToGallons_Double_ConvertsCorrectly(double cups, double expectedGallons)
        {
            // Act
            double actualGallons = MeasureConverters.CupsToGallons(cups);

            // Assert
            _output.WriteLine($"Converting {cups} cups to gallons: Expected {expectedGallons}, got {actualGallons}");
            Assert.Equal(expectedGallons, actualGallons, Tolerance);
        }

        [Theory]
        [InlineData(16.0, 1.0)]    // 16 cups = 1 gallon
        [InlineData(0.0, 0.0)]     // 0 cups = 0 gallons
        [InlineData(32.0, 2.0)]    // 32 cups = 2 gallons
        [InlineData(8.0, 0.5)]     // 8 cups = 0.5 gallons
        public void CupsToGallons_UsingConvertMethod_ConvertsCorrectly(double cups, double expectedGallons)
        {
            // Act
            double actualGallons = MeasureConverters.Convert("c", "g", cups);

            // Assert
            _output.WriteLine($"Using Convert method: {cups} cups to gallons: Expected {expectedGallons}, got {actualGallons}");
            Assert.Equal(expectedGallons, actualGallons, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 16.0)]    // 1 gallon = 16 cups
        [InlineData(0.0, 0.0)]     // 0 gallons = 0 cups
        [InlineData(2.0, 32.0)]    // 2 gallons = 32 cups
        [InlineData(0.5, 8.0)]     // 0.5 gallons = 8 cups
        public void GallonsToCups_Double_ConvertsCorrectly(double gallons, double expectedCups)
        {
            // Act
            double actualCups = MeasureConverters.GallonsToCups(gallons);

            // Assert
            _output.WriteLine($"Converting {gallons} gallons to cups: Expected {expectedCups}, got {actualCups}");
            Assert.Equal(expectedCups, actualCups, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 16.0)]    // 1 gallon = 16 cups
        [InlineData(0.0, 0.0)]     // 0 gallons = 0 cups
        [InlineData(2.0, 32.0)]    // 2 gallons = 32 cups
        [InlineData(0.5, 8.0)]     // 0.5 gallons = 8 cups
        public void GallonsToCups_UsingConvertMethod_ConvertsCorrectly(double gallons, double expectedCups)
        {
            // Act
            double actualCups = MeasureConverters.Convert("g", "c", gallons);

            // Assert
            _output.WriteLine($"Using Convert method: {gallons} gallons to cups: Expected {expectedCups}, got {actualCups}");
            Assert.Equal(expectedCups, actualCups, Tolerance);
        }

        [Theory]
        [InlineData(2.0, 1.0)]     // 2 cups = 1 pint
        [InlineData(0.0, 0.0)]     // 0 cups = 0 pints
        [InlineData(4.0, 2.0)]     // 4 cups = 2 pints
        [InlineData(0.5, 0.25)]    // 0.5 cups = 0.25 pints
        public void CupsToPints_Decimal_ConvertsCorrectly(decimal cups, decimal expectedPints)
        {
            // Act
            decimal actualPints = MeasureConverters.CupsToPints(cups);

            // Assert
            _output.WriteLine($"Converting {cups} cups to pints (decimal): Expected {expectedPints}, got {actualPints}");
            Assert.Equal(expectedPints, actualPints, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(1.0, 2.0)]     // 1 pint = 2 cups
        [InlineData(0.0, 0.0)]     // 0 pints = 0 cups
        [InlineData(2.0, 4.0)]     // 2 pints = 4 cups
        [InlineData(0.25, 0.5)]    // 0.25 pints = 0.5 cups
        public void PintsToCups_Decimal_ConvertsCorrectly(decimal pints, decimal expectedCups)
        {
            // Act
            decimal actualCups = MeasureConverters.PintsToCups(pints);

            // Assert
            _output.WriteLine($"Converting {pints} pints to cups (decimal): Expected {expectedCups}, got {actualCups}");
            Assert.Equal(expectedCups, actualCups, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(8.0, 1.0)]     // 8 pints = 1 gallon
        [InlineData(0.0, 0.0)]     // 0 pints = 0 gallons
        [InlineData(16.0, 2.0)]    // 16 pints = 2 gallons
        [InlineData(4.0, 0.5)]     // 4 pints = 0.5 gallons
        public void PintsToGallons_Decimal_ConvertsCorrectly(decimal pints, decimal expectedGallons)
        {
            // Act
            decimal actualGallons = MeasureConverters.PintsToGallons(pints);

            // Assert
            _output.WriteLine($"Converting {pints} pints to gallons (decimal): Expected {expectedGallons}, got {actualGallons}");
            Assert.Equal(expectedGallons, actualGallons, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(1.0, 8.0)]     // 1 gallon = 8 pints
        [InlineData(0.0, 0.0)]     // 0 gallons = 0 pints
        [InlineData(2.0, 16.0)]    // 2 gallons = 16 pints
        [InlineData(0.5, 4.0)]     // 0.5 gallons = 4 pints
        public void GallonsToPints_Decimal_ConvertsCorrectly(decimal gallons, decimal expectedPints)
        {
            // Act
            decimal actualPints = MeasureConverters.GallonsToPints(gallons);

            // Assert
            _output.WriteLine($"Converting {gallons} gallons to pints (decimal): Expected {expectedPints}, got {actualPints}");
            Assert.Equal(expectedPints, actualPints, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(16.0, 1.0)]    // 16 cups = 1 gallon
        [InlineData(0.0, 0.0)]     // 0 cups = 0 gallons
        [InlineData(32.0, 2.0)]    // 32 cups = 2 gallons
        [InlineData(8.0, 0.5)]     // 8 cups = 0.5 gallons
        public void CupsToGallons_Decimal_ConvertsCorrectly(decimal cups, decimal expectedGallons)
        {
            // Act
            decimal actualGallons = MeasureConverters.CupsToGallons(cups);

            // Assert
            _output.WriteLine($"Converting {cups} cups to gallons (decimal): Expected {expectedGallons}, got {actualGallons}");
            Assert.Equal(expectedGallons, actualGallons, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(1.0, 16.0)]    // 1 gallon = 16 cups
        [InlineData(0.0, 0.0)]     // 0 gallons = 0 cups
        [InlineData(2.0, 32.0)]    // 2 gallons = 32 cups
        [InlineData(0.5, 8.0)]     // 0.5 gallons = 8 cups
        public void GallonsToCups_Decimal_ConvertsCorrectly(decimal gallons, decimal expectedCups)
        {
            // Act
            decimal actualCups = MeasureConverters.GallonsToCups(gallons);

            // Assert
            _output.WriteLine($"Converting {gallons} gallons to cups (decimal): Expected {expectedCups}, got {actualCups}");
            Assert.Equal(expectedCups, actualCups, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(2.0, 1.0)]     // 2 cups = 1 pint
        [InlineData(0.0, 0.0)]     // 0 cups = 0 pints
        [InlineData(4.0, 2.0)]     // 4 cups = 2 pints
        public void CupsToPintsConverter_ReturnsCorrectFunction(double cups, double expectedPints)
        {
            // Arrange
            var converter = MeasureConverters.CupsToPintsConverter();

            // Act
            double actualPints = converter(cups);

            // Assert
            _output.WriteLine($"Using converter function: {cups} cups to pints: Expected {expectedPints}, got {actualPints}");
            Assert.Equal(expectedPints, actualPints, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 2.0)]     // 1 pint = 2 cups
        [InlineData(0.0, 0.0)]     // 0 pints = 0 cups
        [InlineData(2.0, 4.0)]     // 2 pints = 4 cups
        public void PintsToCupsConverter_ReturnsCorrectFunction(double pints, double expectedCups)
        {
            // Arrange
            var converter = MeasureConverters.PintsToCupsConverter();

            // Act
            double actualCups = converter(pints);

            // Assert
            _output.WriteLine($"Using converter function: {pints} pints to cups: Expected {expectedCups}, got {actualCups}");
            Assert.Equal(expectedCups, actualCups, Tolerance);
        }

        [Theory]
        [InlineData(8.0, 1.0)]     // 8 pints = 1 gallon
        [InlineData(0.0, 0.0)]     // 0 pints = 0 gallons
        [InlineData(16.0, 2.0)]    // 16 pints = 2 gallons
        public void PintsToGallonsConverter_ReturnsCorrectFunction(double pints, double expectedGallons)
        {
            // Arrange
            var converter = MeasureConverters.PintsToGallonsConverter();

            // Act
            double actualGallons = converter(pints);

            // Assert
            _output.WriteLine($"Using converter function: {pints} pints to gallons: Expected {expectedGallons}, got {actualGallons}");
            Assert.Equal(expectedGallons, actualGallons, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 8.0)]     // 1 gallon = 8 pints
        [InlineData(0.0, 0.0)]     // 0 gallons = 0 pints
        [InlineData(2.0, 16.0)]    // 2 gallons = 16 pints
        public void GallonsToPintsConverter_ReturnsCorrectFunction(double gallons, double expectedPints)
        {
            // Arrange
            var converter = MeasureConverters.GallonsToPintsConverter();

            // Act
            double actualPints = converter(gallons);

            // Assert
            _output.WriteLine($"Using converter function: {gallons} gallons to pints: Expected {expectedPints}, got {actualPints}");
            Assert.Equal(expectedPints, actualPints, Tolerance);
        }

        [Theory]
        [InlineData(16.0, 1.0)]    // 16 cups = 1 gallon
        [InlineData(0.0, 0.0)]     // 0 cups = 0 gallons
        [InlineData(32.0, 2.0)]    // 32 cups = 2 gallons
        public void CupsToGallonsConverter_ReturnsCorrectFunction(double cups, double expectedGallons)
        {
            // Arrange
            var converter = MeasureConverters.CupsToGallonsConverter();

            // Act
            double actualGallons = converter(cups);

            // Assert
            _output.WriteLine($"Using converter function: {cups} cups to gallons: Expected {expectedGallons}, got {actualGallons}");
            Assert.Equal(expectedGallons, actualGallons, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 16.0)]    // 1 gallon = 16 cups
        [InlineData(0.0, 0.0)]     // 0 gallons = 0 cups
        [InlineData(2.0, 32.0)]    // 2 gallons = 32 cups
        public void GallonsToCupsConverter_ReturnsCorrectFunction(double gallons, double expectedCups)
        {
            // Arrange
            var converter = MeasureConverters.GallonsToCupsConverter();

            // Act
            double actualCups = converter(gallons);

            // Assert
            _output.WriteLine($"Using converter function: {gallons} gallons to cups: Expected {expectedCups}, got {actualCups}");
            Assert.Equal(expectedCups, actualCups, Tolerance);
        }

        #endregion

        #region Teaspoons and Tablespoons Tests

        [Theory]
        [InlineData(3.0, 1.0)]     // 3 teaspoons = 1 tablespoon
        [InlineData(0.0, 0.0)]     // 0 teaspoons = 0 tablespoons
        [InlineData(6.0, 2.0)]     // 6 teaspoons = 2 tablespoons
        [InlineData(1.5, 0.5)]     // 1.5 teaspoons = 0.5 tablespoons
        public void TeaspoonsToTablespoons_Double_ConvertsCorrectly(double teaspoons, double expectedTablespoons)
        {
            // Act
            double actualTablespoons = MeasureConverters.TeaspoonsToTablespoons(teaspoons);

            // Assert
            _output.WriteLine($"Converting {teaspoons} teaspoons to tablespoons: Expected {expectedTablespoons}, got {actualTablespoons}");
            Assert.Equal(expectedTablespoons, actualTablespoons, Tolerance);
        }

        [Theory]
        [InlineData(3.0, 1.0)]     // 3 teaspoons = 1 tablespoon
        [InlineData(0.0, 0.0)]     // 0 teaspoons = 0 tablespoons
        [InlineData(6.0, 2.0)]     // 6 teaspoons = 2 tablespoons
        [InlineData(1.5, 0.5)]     // 1.5 teaspoons = 0.5 tablespoons
        public void TeaspoonsToTablespoons_UsingConvertMethod_ConvertsCorrectly(double teaspoons, double expectedTablespoons)
        {
            // Act
            double actualTablespoons = MeasureConverters.Convert("ts", "tb", teaspoons);

            // Assert
            _output.WriteLine($"Using Convert method: {teaspoons} teaspoons to tablespoons: Expected {expectedTablespoons}, got {actualTablespoons}");
            Assert.Equal(expectedTablespoons, actualTablespoons, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 3.0)]     // 1 tablespoon = 3 teaspoons
        [InlineData(0.0, 0.0)]     // 0 tablespoons = 0 teaspoons
        [InlineData(2.0, 6.0)]     // 2 tablespoons = 6 teaspoons
        [InlineData(0.5, 1.5)]     // 0.5 tablespoons = 1.5 teaspoons
        public void TablespoonsToTeaspoons_Double_ConvertsCorrectly(double tablespoons, double expectedTeaspoons)
        {
            // Act
            double actualTeaspoons = MeasureConverters.TablespoonsToTeaspoons(tablespoons);

            // Assert
            _output.WriteLine($"Converting {tablespoons} tablespoons to teaspoons: Expected {expectedTeaspoons}, got {actualTeaspoons}");
            Assert.Equal(expectedTeaspoons, actualTeaspoons, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 3.0)]     // 1 tablespoon = 3 teaspoons
        [InlineData(0.0, 0.0)]     // 0 tablespoons = 0 teaspoons
        [InlineData(2.0, 6.0)]     // 2 tablespoons = 6 teaspoons
        [InlineData(0.5, 1.5)]     // 0.5 tablespoons = 1.5 teaspoons
        public void TablespoonsToTeaspoons_UsingConvertMethod_ConvertsCorrectly(double tablespoons, double expectedTeaspoons)
        {
            // Act
            double actualTeaspoons = MeasureConverters.Convert("tb", "ts", tablespoons);

            // Assert
            _output.WriteLine($"Using Convert method: {tablespoons} tablespoons to teaspoons: Expected {expectedTeaspoons}, got {actualTeaspoons}");
            Assert.Equal(expectedTeaspoons, actualTeaspoons, Tolerance);
        }

        [Theory]
        [InlineData(3.0, 1.0)]     // 3 teaspoons = 1 tablespoon
        [InlineData(0.0, 0.0)]     // 0 teaspoons = 0 tablespoons
        [InlineData(6.0, 2.0)]     // 6 teaspoons = 2 tablespoons
        [InlineData(1.5, 0.5)]     // 1.5 teaspoons = 0.5 tablespoons
        public void TeaspoonsToTablespoons_Decimal_ConvertsCorrectly(decimal teaspoons, decimal expectedTablespoons)
        {
            // Act
            decimal actualTablespoons = MeasureConverters.TeaspoonsToTablespoons(teaspoons);

            // Assert
            _output.WriteLine($"Converting {teaspoons} teaspoons to tablespoons (decimal): Expected {expectedTablespoons}, got {actualTablespoons}");
            Assert.Equal(expectedTablespoons, actualTablespoons, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(1.0, 3.0)]     // 1 tablespoon = 3 teaspoons
        [InlineData(0.0, 0.0)]     // 0 tablespoons = 0 teaspoons
        [InlineData(2.0, 6.0)]     // 2 tablespoons = 6 teaspoons
        [InlineData(0.5, 1.5)]     // 0.5 tablespoons = 1.5 teaspoons
        public void TablespoonsToTeaspoons_Decimal_ConvertsCorrectly(decimal tablespoons, decimal expectedTeaspoons)
        {
            // Act
            decimal actualTeaspoons = MeasureConverters.TablespoonsToTeaspoons(tablespoons);

            // Assert
            _output.WriteLine($"Converting {tablespoons} tablespoons to teaspoons (decimal): Expected {expectedTeaspoons}, got {actualTeaspoons}");
            Assert.Equal(expectedTeaspoons, actualTeaspoons, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(3.0, 1.0)]     // 3 teaspoons = 1 tablespoon
        [InlineData(0.0, 0.0)]     // 0 teaspoons = 0 tablespoons
        [InlineData(6.0, 2.0)]     // 6 teaspoons = 2 tablespoons
        public void TeaspoonsToTablespoonsConverter_ReturnsCorrectFunction(double teaspoons, double expectedTablespoons)
        {
            // Arrange
            var converter = MeasureConverters.TeaspoonsToTablespoonsConverter();

            // Act
            double actualTablespoons = converter(teaspoons);

            // Assert
            _output.WriteLine($"Using converter function: {teaspoons} teaspoons to tablespoons: Expected {expectedTablespoons}, got {actualTablespoons}");
            Assert.Equal(expectedTablespoons, actualTablespoons, Tolerance);
        }

        [Theory]
        [InlineData(1.0, 3.0)]     // 1 tablespoon = 3 teaspoons
        [InlineData(0.0, 0.0)]     // 0 tablespoons = 0 teaspoons
        [InlineData(2.0, 6.0)]     // 2 tablespoons = 6 teaspoons
        public void TablespoonsToTeaspoonsConverter_ReturnsCorrectFunction(double tablespoons, double expectedTeaspoons)
        {
            // Arrange
            var converter = MeasureConverters.TablespoonsToTeaspoonsConverter();

            // Act
            double actualTeaspoons = converter(tablespoons);

            // Assert
            _output.WriteLine($"Using converter function: {tablespoons} tablespoons to teaspoons: Expected {expectedTeaspoons}, got {actualTeaspoons}");
            Assert.Equal(expectedTeaspoons, actualTeaspoons, Tolerance);
        }

        [Theory]
        [InlineData(3.0, 1.0)]     // 3 teaspoons = 1 tablespoon
        [InlineData(0.0, 0.0)]     // 0 teaspoons = 0 tablespoons
        [InlineData(6.0, 2.0)]     // 6 teaspoons = 2 tablespoons
        public void TeaspoonsToTablespoonsConverterDecimal_ReturnsCorrectFunction(decimal teaspoons, decimal expectedTablespoons)
        {
            // Arrange
            var converter = MeasureConverters.TeaspoonsToTablespoonsConverterDecimal();

            // Act
            decimal actualTablespoons = converter(teaspoons);

            // Assert
            _output.WriteLine($"Using decimal converter function: {teaspoons} teaspoons to tablespoons: Expected {expectedTablespoons}, got {actualTablespoons}");
            Assert.Equal(expectedTablespoons, actualTablespoons, 4); // 4 decimal places precision
        }

        [Theory]
        [InlineData(1.0, 3.0)]     // 1 tablespoon = 3 teaspoons
        [InlineData(0.0, 0.0)]     // 0 tablespoons = 0 teaspoons
        [InlineData(2.0, 6.0)]     // 2 tablespoons = 6 teaspoons
        public void TablespoonsToTeaspoonsConverterDecimal_ReturnsCorrectFunction(decimal tablespoons, decimal expectedTeaspoons)
        {
            // Arrange
            var converter = MeasureConverters.TablespoonsToTeaspoonsConverterDecimal();

            // Act
            decimal actualTeaspoons = converter(tablespoons);

            // Assert
            _output.WriteLine($"Using decimal converter function: {tablespoons} tablespoons to teaspoons: Expected {expectedTeaspoons}, got {actualTeaspoons}");
            Assert.Equal(expectedTeaspoons, actualTeaspoons, 4); // 4 decimal places precision
        }

        #endregion

        #region General Converter Tests

        [Fact]
        public void ApplyConverter_AppliesConversionFunction()
        {
            // Arrange
            double ounces = 5.0;
            var converter = MeasureConverters.OuncesToGramsConverter();
            double expected = converter(ounces);

            // Act
            double actual = MeasureConverters.ApplyConverter(ounces, converter);

            // Assert
            _output.WriteLine($"ApplyConverter result: Expected {expected}, got {actual}");
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ComposeConverters_CombinesTwoConversionFunctions()
        {
            // Arrange
            double originalValue = 10.0;
            var ouncesToGrams = MeasureConverters.OuncesToGramsConverter();
            var gramsToOunces = MeasureConverters.GramsToOuncesConverter();

            // Creating a composed function that should essentially be an identity function
            // (converts oz to g and back to oz)
            var composed = MeasureConverters.ComposeConverters(ouncesToGrams, gramsToOunces);

            // Act
            double result = composed(originalValue);

            // Assert
            _output.WriteLine($"Composed converter result: Input {originalValue}, Output {result}");
            Assert.Equal(originalValue, result, Tolerance);
        }

        [Theory]
        [InlineData(5.0, 2.0, 10.0)] // 5 * 2 = 10
        [InlineData(0.0, 100.0, 0.0)] // 0 * 100 = 0
        [InlineData(25.0, 0.5, 12.5)] // 25 * 0.5 = 12.5
        public void CreateConverter_CreatesCorrectFunction(double input, double factor, double expected)
        {
            // Arrange - We'll need to use reflection since CreateConverter is private
            var method = typeof(MeasureConverters).GetMethod("CreateConverter",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var genericMethod = method.MakeGenericMethod(typeof(double));

            // Act
            var converter = (Func<double, double>)genericMethod.Invoke(null, new object[] { factor });
            double result = converter(input);

            // Assert
            _output.WriteLine($"CreateConverter with factor {factor}: Input {input}, Output {result}, Expected {expected}");
            Assert.Equal(expected, result, Tolerance);
        }

        [Theory]
        [InlineData(10.0, 2.0, 5.0)] // 10 / 2 = 5
        [InlineData(0.0, 100.0, 0.0)] // 0 / 100 = 0
        [InlineData(25.0, 0.5, 50.0)] // 25 / 0.5 = 50
        public void CreateInverseConverter_CreatesCorrectFunction(double input, double factor, double expected)
        {
            // Arrange - We'll need to use reflection since CreateInverseConverter is private
            var method = typeof(MeasureConverters).GetMethod("CreateInverseConverter",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var genericMethod = method.MakeGenericMethod(typeof(double));

            // Act
            var converter = (Func<double, double>)genericMethod.Invoke(null, new object[] { factor });
            double result = converter(input);

            // Assert
            _output.WriteLine($"CreateInverseConverter with factor {factor}: Input {input}, Output {result}, Expected {expected}");
            Assert.Equal(expected, result, Tolerance);
        }

        #endregion
    }
}