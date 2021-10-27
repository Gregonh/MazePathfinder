using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RecursiveDivisionAlgorithm
{
    public static void CreateMaze(MazeGrid grid)
    {
        foreach (var cell in grid.GetCellOneByOne())
        {
            foreach (var neighbour in cell.NeighboursAround)
            {
                // not bidireccional because we are going to iterate over every cell, so reduce cost
                cell.JoinTwoNeighbourCells(neighbour, false);
            }
        }

        DivideGrid(grid, 0, 0, grid.Rows, grid.Columns);
    }

    private static void DivideGrid(MazeGrid grid, int row, int col, int height, int width)
    {
        //region too small to subdivide
        if (height <= 1 || width <= 1)
        {
            return;
        }

        /*
         * decide to divide area horiz. or vertically, can be randomly
         * but dividing based on aspect ratio of the region give
         * good results and avoid long vertical or horizontal passages
         */
        if (height > width)
        {
            DivideHorizontally(grid, row, col, height, width);
        }
        else
        {
            DivideVertically(grid, row, col, height, width);
        }
    }

    private static void DivideVertically(MazeGrid grid, int row, int col, int height, int width)
    {
        var verticalWallCol = Random.Range(0, width-1);
        var passageAtRowIndex = Random.Range(0, height);
        for (int rowIndex = 0; rowIndex < height; rowIndex++)
        {
            if (rowIndex != passageAtRowIndex)
            {
                var actualCell = grid[row + rowIndex, col + verticalWallCol];
                actualCell.DisjoinTwoNeighbourCells(actualCell.East);
            }
        }
        
        DivideGrid(grid, row, col, height, verticalWallCol+1);
        DivideGrid(grid, row, col + verticalWallCol +1, height, width - verticalWallCol - 1);
        
    }

    private static void DivideHorizontally(MazeGrid grid, int row, int col, int height, int width)
    {
        int horizontalWallRow = Random.Range(0, height-1);
        int passageAtColIndex = Random.Range(0, width);

        /*
         * disjoin all the cells of one row, except one at specific col index
         * we disjoin the cell and his south neighbour (bidireccional)
         */
        for (int colIndex = 0; colIndex < width; colIndex++)
        {
            if (colIndex != passageAtColIndex)
            {
                var actualCell = grid[row + horizontalWallRow, col + colIndex];
                actualCell.DisjoinTwoNeighbourCells(actualCell.South);
            }
        }
        DivideGrid(grid, row, col, horizontalWallRow+1, width);
        DivideGrid(grid, row + horizontalWallRow + 1, col, height - horizontalWallRow -1, width);
    }
}
