
using System;
using System.Collections.Generic;
using System.Linq;

/*
 * To create a binary tree maze,
 * for each cell flip a coin to decide
 * whether to add a passage leading up or left.
 * Other diagonal sets can be: south/west , East/South
 * Always pick the same direction for cells on the boundary,
 * and the end result will be a valid simply connected
 * maze that looks like a binary tree, with the upper left corner its root.
 *
 * Downside : two of the four sides of the maze will be spanned by a single corridor
 */

public static class BinaryTreeAlgorithm 
{
    public static void BuildMaze(MazeGrid mazeGrid)
    {
        Random randomSeed = new Random();
        foreach (var cell in mazeGrid.GetCellOneByOne())
        {
            List<Cell> northEastNeighbours = ReturnNorthEastNeighboursNotJoined(cell);
            int randomNeighbour = randomSeed.Next(0, northEastNeighbours.Count);
            if (randomNeighbour < northEastNeighbours.Count) // cell in northeast corner has no neighbors
            {
                Cell neighbourToJoin = northEastNeighbours[randomNeighbour];
                cell.JoinTwoNeighbourCells(neighbourToJoin);
            }

        }
    }

    private static List<Cell> ReturnNorthEastNeighboursNotJoined(Cell cell)
    {
        List<Cell> neighbours = new List<Cell>{ cell.North, cell.East }.
                                Where(c => c != null).ToList();
        
        return neighbours;
    }
}
