using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class RecursiveBacktrackerAlgorithm
{
    public static void BuildMaze(MazeGrid grid)
    {
        Cell startingCell = grid.GetStartingCell();
        Stack<Cell> cellStack = new Stack<Cell>();
        cellStack.Push(startingCell);
        while (cellStack.Any())
        {
            var currentCell = cellStack.Peek();
            ControlCurrentCellInStackAccordingNotVisitedNeighbour(currentCell, cellStack);
        }
    }

    private static void ControlCurrentCellInStackAccordingNotVisitedNeighbour(Cell currentCell, Stack<Cell> cellStack)
    {
        Cell notVisitedNeighbour = ReturnNotVisitedRandomNeighbour(currentCell);
        if (notVisitedNeighbour != null)
        {
            currentCell.JoinTwoNeighbourCells(notVisitedNeighbour);
            cellStack.Push(notVisitedNeighbour);
        }
        else
        {
            cellStack.Pop();
            
        }
    }

    private static Cell ReturnNotVisitedRandomNeighbour(Cell cell)
    {
        /*
             List<Cell> notVisitedNeighbours =
                new List<Cell> { cell.North, cell.South, cell.East, cell.West }.Where(c => c != null)
                    .Where(s => !cell.ReturnNeighboursCellWithPassage().Contains(s)).ToList();
                    
            var notVisitedNeighbours =
                cell.NeighboursAround.Where(c => !c.ReturnNeighboursCellWithPassage().Any()).ToList();
                
                List<Cell> notVisitedNeighbours =(
                new List<Cell> { cell.North, cell.South, cell.East, cell.West }.Where(c => c != null))
                .Except(cell.ReturnNeighboursCellWithPassage()).ToList();
            */
        try
        {
            var notVisitedNeighbours =
                cell.NeighboursAround.Where(c => !c.ReturnNeighboursCellWithPassage().Any()).ToList();
            
            int randomNeighbour = Random.Range(0, notVisitedNeighbours.Count);
            if (randomNeighbour < notVisitedNeighbours.Count)
            {
                var neighbourToJoin = notVisitedNeighbours[randomNeighbour];
                return neighbourToJoin;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }


        return null;
    }
    
}