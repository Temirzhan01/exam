To write a function in JavaScript that formats numbers with spaces as thousand separators, you can use a combination of regular expressions and the String.replace() method. Here's a function to achieve that:

javascript
Копировать код
function formatNumberWithSpaces(number) {
    // Convert the number to a string
    let numStr = number.toString();

    // Split the string on the decimal point
    let parts = numStr.split('.');

    // Use a regular expression to add spaces as thousand separators
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ' ');

    // Rejoin the parts with the decimal point (if there was any)
    return parts.join('.');
}

// Examples
console.log(formatNumberWithSpaces(400000));     // Output: "400 000"
console.log(formatNumberWithSpaces(1000000));    // Output: "1 000 000"
console.log(formatNumberWithSpaces(100000.89));  // Output: "100 000.89"
Explanation:
Convert the Number to a String: The number is first converted to a string using number.toString().
Split on the Decimal Point: The string is split into two parts, the integer part and the fractional part (if any), using split('.').
Add Spaces as Thousand Separators: The integer part is processed using replace() with a regular expression \B(?=(\d{3})+(?!\d)), which matches positions in the string where a space should be inserted.
Rejoin the Parts: Finally, the parts are joined back together using join('.').
Regular Expression Breakdown:
\B: A non-word boundary (this helps to avoid putting a space at the beginning of the string).
(?=(\d{3})+(?!\d)): A positive lookahead that ensures the presence of groups of three digits that are followed by a digit.
This approach ensures that the number is correctly formatted with spaces as thousand separators, regardless of whether it has a fractional part.
