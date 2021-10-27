using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

public static class AStarAlgorithm 
{
    static int ApproximateManhattanHeuristic(Cell currentCell, Cell goalCell)
    {
        // Manhattan distance on a square grid
        return Math.Abs(currentCell.CellCoord.x - goalCell.CellCoord.x) +
               Math.Abs(currentCell.CellCoord.z - goalCell.CellCoord.z);
    }
    
    public static List<Cell> RecordDistancesFromOrigin(Cell originCell, Cell goalCell)
    {
        
        //  cells that are being considered to find the shortest path
        List<Cell> openQueue = new List<Cell>(); // dictionary are hash table, no ordered!!
        // cells that does not have to consider it again 
        List<Cell> closedList = new List<Cell>();
        openQueue.Add(originCell);
        originCell.Fdistance = 0;
        while (openQueue.Any()) // if empty there is no path
        {
            var lowestFCellValue = openQueue.Min(x => x.Fdistance);
            Cell lowestFCell = openQueue.First(c => c.Fdistance == lowestFCellValue);
            closedList.Add(lowestFCell);
            openQueue.Remove(lowestFCell);
            if (closedList.Contains(goalCell)) break;
                
                foreach (var neighbour in lowestFCell.ReturnNeighboursCellWithPassage())
                {
                    if (closedList.Contains(neighbour)) continue;
                    if (!openQueue.Contains(neighbour))
                    {
                        int newCost = lowestFCellValue + 1;
                        int priorityFValue = newCost + ApproximateManhattanHeuristic(lowestFCell, goalCell);
                        // save the cell and distance from current cell
                        neighbour.Fdistance = priorityFValue;
                        openQueue.Add(neighbour);
                        neighbour.ParentPathCell = lowestFCell;
                    }
                    
                }
                
        }

        return closedList;
    }
}
