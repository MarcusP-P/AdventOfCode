/*
The newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the Elves now need to recover. On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.

For example:

1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet
In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.

Consider your entire calibration document. What is the sum of all of the calibration values?
*/

using (var sr = new StreamReader("dataset.txt"))
{
    // We will go through each line and store the results in the accumulator
    var accumulator = 0;

    // Where we will store each line of the file
    string? currentLine;

    // keep on going until we run out of lines
    while ((currentLine=await sr.ReadLineAsync()) != null)
    {
        // Add the current line's values to what we already have.
        accumulator += getNumbersFromString(currentLine);
    }

    // Print the answer
    Console.WriteLine($"The calibration value is {accumulator}");
}

// Gets the two digit number from the string, by getting the first and the last numeric character
int getNumbersFromString(string source)
{
    int firstDigit;
    int lastDigit;

    // go through the string to find the first number
    // because the first character is the most significant digit, it's in the 10s column, so remember to multiply it by 10
    firstDigit=findFirstNumber(source.ToCharArray()) * 10;
    lastDigit=findFirstNumber(source.Reverse().ToArray());

    return firstDigit+lastDigit;
}

// find and return the first number in an array of char
int findFirstNumber(char[] source)
{
    // go through each character in the source array
    foreach (var current in source)
    {
        // Check if we have a numeric by checking that it's in the range of 0-9
        if (current >= '0' && current <= '9')
        {
            // convert the current value to a number by subtracting th value of the lowest 
            // number
            return current - '0';
        }
    }

    // if we haven't found a result, return 0, on the assumption that we will not be adding anyhting from, this line.
    return 0;
}