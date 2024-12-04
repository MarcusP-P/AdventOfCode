/*
Your calculation isn't quite right. It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, six, seven, eight, and nine also count as valid "digits".

Equipped with this new information, you now need to find the real first and last digit on each line. For example:

two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen
In this example, the calibration values are 29, 83, 13, 24, 42, 14, and 76. Adding these together produces 281.

*/

using (var sr = new StreamReader("dataset.txt")) {
    // We will go through each line and store the results in the accumulator
    var accumulator = 0;

    // Where we will store each line of the file
    string? currentLine;

    // keep on going until we run out of lines
    while ((currentLine = await sr.ReadLineAsync()) != null) {
        // Add the current line's values to what we already have.
        accumulator += getNumbersFromString(currentLine);
    }

    // Print the answer
    Console.WriteLine($"The calibration value is {accumulator}");
}

// Gets the two digit number from the string, by getting the first and the last numeric character
int getNumbersFromString(string source) {
    int firstDigit;
    int lastDigit;

    // A map of the names of numbers and numbers to their numeric value
    var numberText = new Dictionary<string, int>()
    {
        {"zero", 0},
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9},
        {"0", 0},
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
        {"5", 5},
        {"6", 6},
        {"7", 7},
        {"8", 8},
        {"9", 9},
    };

    // For each entry in the previous dictionary, reverse the name of the key for the backwards search
    var reverseNumberText = numberText
        .ToDictionary(
            i => string.Concat(i.Key.Reverse()),
            i => i.Value);

    // go through the string to find the first number
    // because the first character is the most significant digit, it's in the 10s column, so remember to multiply it by 10
    firstDigit = findFirstNumber(source, numberText) * 10;
    lastDigit = findFirstNumber(string.Concat(source.Reverse()), reverseNumberText);

    return firstDigit + lastDigit;
}

// find and return the first number in an array of char
int findFirstNumber(string source, Dictionary<string, int> names) {
    //Remember the position of the current value, and what value it is
    int? position = null;
    int? value = null;

    // go through each character in the source array
    foreach (var key in names.Keys) {
        // Look for the position of the current key
        var currentPosition = source.IndexOf(key, StringComparison.Ordinal);

        // If we've found it
        if (currentPosition != -1
            // and we're before the previous position (or previous position is null)
            && currentPosition <= (position ?? currentPosition)) {
            position = currentPosition;
            value = names[key];
        }
    }

    // if we haven't found a result, return 0, on the assumption that we will not be adding anyhting from, this line.
    // Otherwise return the earliest value
    return value ?? 0;
}
