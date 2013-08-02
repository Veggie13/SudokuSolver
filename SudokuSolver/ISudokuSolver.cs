using System.Collections.Generic;
namespace SudokuSolver
{
    public interface ISudokuSolver
    {
        IList<SudokuGrid> FindAllSolutions(SudokuGrid original);
    }
}