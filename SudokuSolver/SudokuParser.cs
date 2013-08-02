using System;
using System.IO;
namespace SudokuSolver
{
    /// <summary>
    /// SudokuParser provides methods for converting a SudokuGrid to and from text.
    /// </summary>
    public class SudokuParser
    {
        /// <summary>
        /// Parses a source string into a SudokuGrid structure. Ignores all non-alphabet or non-blank characters.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <param name="alphabet">The ordered alphabet of characters to treat as readable from src.</param>
        /// <param name="unknowns">The set of characters to treat as blank from src. Blank means unknown in the puzzle, not invalid.</param>
        public SudokuGrid ParseString(string src, string alphabet, string unknowns)
        {
            uint size = (uint)alphabet.Length;
            uint dimensionality = (uint)Math.Sqrt(size);
            if (size != dimensionality * dimensionality)
                throw new Exception("Invalid alphabet: size must be square of integer dimensionality.");

            string clean = "";
            for (int i = 0; i < src.Length; i++)
            {
                string curChar = src.Substring(i,1);
                if (alphabet.Contains(curChar) || unknowns.Contains(curChar))
                    clean += curChar;
            }

            if (clean.Length != size * size)
                throw new Exception("Invalid source string: puzzle wrong size for dimensionality.");

            SudokuGrid result = new SudokuGrid(dimensionality);
            for (uint row = 0, n = 0; row < size; row++)
            for (uint col = 0; col < size; col++, n++)
            {
                string curChar = clean.Substring((int)n, 1);
                int val = alphabet.Contains(curChar) ? alphabet.IndexOf(curChar) : -1;
                result[row, col] = val;
            }

            return result;
        }

        /// <summary>
        /// Renders a SudokuGrid to a string.
        /// </summary>
        /// <param name="alphabet">The alphabet of characters to write to the output string.</param>
        /// <param name="grid">The SudokuGrid to render.</param>
        public string Render(string alphabet, SudokuGrid grid)
        {
            if (alphabet.Length != grid.Size)
                throw new Exception("Invalid alphabet: length must be equal to grid Size.");

            string output = "", horizRule = new string('-', 2 * (int)grid.Dimensionality + 1);

            for (uint blockRow = 0; blockRow < grid.Dimensionality; blockRow++)
            {
                for (uint innerRow = 0; innerRow < grid.Dimensionality; innerRow++)
                {
                    for (uint blockCol = 0; blockCol < grid.Dimensionality; blockCol++)
                    {
                        for (uint innerCol = 0; innerCol < grid.Dimensionality; innerCol++)
                        {
                            int val = grid[blockRow,blockCol,innerRow,innerCol];
                            if (val < 0)
                                output += " .";
                            else if (val >= alphabet.Length)
                                output += " ?";
                            else
                                output += " " + alphabet[val];
                        }

                        output += " |";
                    }

                    output += "\n";
                }

                for (uint blockCol = 0; blockCol < grid.Dimensionality; blockCol++)
                    output += horizRule + "+";
                output += "\n";
            }

            return output;
        }
    }
}