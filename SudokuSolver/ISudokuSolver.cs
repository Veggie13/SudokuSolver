using System.Collections.Generic;
namespace SudokuSolver
{
    public interface ISudokuSolver
    {
        SudokuGrid FindFirstSolution(SudokuGrid original);
        List<SudokuGrid> FindAllSolutions(SudokuGrid original);
    }
}