
using System.Collections.Generic;
/*
 * Record distance of each cell from staring point,
 * after add cell properties Fdistance and ParentPathCell
 * it doesn´t need this class
 */

public class Distances
{
    private readonly Cell startingCell; 
    private Dictionary<Cell, int> cellsAndDistance = new Dictionary<Cell, int>();
    private Dictionary<Cell, int> walkBackPath = new Dictionary<Cell, int>();
    public Distances(Cell originCell)
    {
        startingCell = originCell;
        cellsAndDistance[startingCell] = 0;
    }
    
    public Dictionary<Cell, int> CellsAndDistance
    {
        get => cellsAndDistance;
    }

    public void AddEntryToDictionary(Cell cell, int distance)
    {
        if (!cellsAndDistance.ContainsKey(cell))
        {
            cellsAndDistance.Add(cell, distance);
        }
    }
    
    public IEnumerable<Cell> GetAllCells()
    {
       return walkBackPath.Keys;
    }

    
    // get cell distance from origin, and set it
    public int this[Cell cell]
    {
        get => cellsAndDistance[cell];

        set => cellsAndDistance[cell] = value;
    }       
    
    public bool HasCellSaved(Cell cell)
    {
       return cellsAndDistance.ContainsKey(cell);
    }

    /*
     * will be call after Dijkstra save distances
     */
    public Distances TraceShortestPathFromEndToStart(Cell endGoal)
    {
        Cell currentCell = endGoal;
        // equalize current cell distance to distance of the same cell in the Dictionary
        walkBackPath[currentCell] = this[currentCell];
        while (currentCell != startingCell)
        {
            // compare neighbour distance from origin to know which is more near than current cell
            foreach (var neighbourCell in currentCell.ReturnNeighboursCellWithPassage())
            {
                if (cellsAndDistance.ContainsKey(neighbourCell) && walkBackPath[currentCell] > this[neighbourCell]) // if neighbour cell distance is less than current
                {
                    currentCell = neighbourCell; 
                    walkBackPath[neighbourCell] = this[neighbourCell]; // save the distance of new actual cell
                    break;
                }
            }
        }

        return this;
    }
    
    
    
    
}
