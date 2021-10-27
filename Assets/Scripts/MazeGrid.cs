using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

/*
 * container for cells
 */
public class MazeGrid
{
    public int Rows { get; }
    public int Columns { get; }
    private readonly int size;

    public int Size
    {
        get => size;
    }
    
    // [,] for rectangular, but has worst performance, for this is jagged
    private Cell[][] cellsMatrix; 
    public MazeGrid(int rows, int cols)
    {
        Rows = rows;
        Columns = cols;
        size = rows * cols;
        FillGridWithCells();
        InitialiceCellsNeighboursDirectionField();
    }

    private void FillGridWithCells()
    {
        cellsMatrix = new Cell[Rows][];
        for (int rowIndex = 0; rowIndex < Rows; rowIndex++)
        {
            cellsMatrix[rowIndex] = new Cell[Columns];
            for (int colIndex = 0; colIndex < Columns; colIndex++)
            {
                cellsMatrix[rowIndex] [colIndex] = new Cell(rowIndex, colIndex);
            }
        }
    }

    /*
     * inicialize direction cell, cells that surround our
     * current cell
     */
    private void InitialiceCellsNeighboursDirectionField()
    {
        IEnumerable<Cell> allCells = GetCellOneByOne();
        foreach (var cell in allCells)
        {
            var cellColCoord = cell.CellCoord.x;
            var cellRowCoord = cell.CellCoord.z;
            cell.North = this[cellRowCoord - 1, cellColCoord];
            cell.South = this[cellRowCoord + 1, cellColCoord];
            cell.East = this[cellRowCoord, cellColCoord + 1];
            cell.West = this[cellRowCoord, cellColCoord - 1];
        }
    }
    
    public IEnumerable<Cell> GetCellOneByOne()
    {
        foreach (var rowsCells in cellsMatrix)
        {
            foreach (var cell in rowsCells)
            {
                yield return cell;
            }
        }
    }
    
    /* Indexer, array accessor method
        start cell will be this[0,0]
        build grid
        binary tree
        start = grid[0,0]
        Distance distance = start.distances;
        
     */
    public Cell this[int row, int column]
    {
        get
        {
            // do bounds checking, compare cell coord with grid bounds
            if (row < 0 || row > Rows - 1) return null;
            if (column < 0 || column > Columns - 1) return null;
            return cellsMatrix[row][column];
        }
        set => cellsMatrix[row] [column] = value;
    }
    
    public IEnumerable<Cell[]> GetArrayWithAllCellsInRow()
    {
        foreach (var rowsCells in cellsMatrix)
        {
            yield return rowsCells;
        }
    }
    
    /*
     * granting random access to arbitrary cells in the grid 
     */

    public Cell GetRandomCell()
    {
        var randomSeed = new Random();
        var randomRow = randomSeed .Next(1, Rows); // avoid starting row
        var randomCol = randomSeed.Next(1, Columns);
        return this[randomRow, randomCol];
    }

    public Cell GetStartingCell()
    {
        return this[0, 0];
    }

    public Cell GetLastRowFirstColCell()
    {
        return this[Rows-1,0];
    }

    public string ToString(Func<Cell, string> format) {
        string output = "\n+" + string.Concat(Enumerable.Repeat("---+", Columns)) + "\n";
        for (int row = 0; row < Rows; row++) {
            string cellRow = "|";
            string lowerWall = "+";
            for (int col = 0; col < Columns; col++) {
                Cell cell = this[row, col];
                cellRow += format(cell) + (cell?.IsJoinedWith(cell.East) ?? false ? " " : "|");
                lowerWall += (cell?.IsJoinedWith(cell.South) ?? false ? "   " : "---") + "+";
            }
            output += cellRow + "\n" + lowerWall + "\n";
        }
        return output;
    }

    public override string ToString() =>
        ToString(c => "   ");

    public List<Cell> GetDeadEndCells()
    {
        List<Cell> deadEndCell = new List<Cell>();
        foreach (var cell in GetCellOneByOne())
        {
            if (cell.ReturnNeighboursCellWithPassage().Count() == 1)
            {
                deadEndCell.Add(cell);
            }
        }

        return deadEndCell;
    }
    
}
