
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell 
{
    /*
     * neighboring cells dictionary with joined passage, bool for bidirectional
     * cell always join with a north or east neighbour (in binary tree)
     * at same time this joined neighbour join with the cell
     * (a passage for both)
     */
    private IDictionary<Cell, bool> neighboringCellsWithPassage { get; }
    
    // record coordinates
    public Vector3Int CellCoord { get; }
    public string NameCellCoord { get; }
    
    // save possible surrounded neighbours cell in each direction
    public Cell North { get; set; }
    public Cell South { get; set; }
    public Cell East { get; set; }      
    public Cell West { get; set; }

    // cells around connected or not
    public List<Cell> NeighboursAround
    {
        get
        {
            var neighbourList = new List<Cell>();
            if (North != null)
            {
                neighbourList.Add(North);
            }

            if (South != null)
            {
                neighbourList.Add(South);
            }
            
            if (East != null)
            {
                neighbourList.Add(East);
            }
            
            if (West != null)
            {
                neighbourList.Add(West);
            }

            return neighbourList;
        }
        
    }
    
    public Cell ParentPathCell { get; set; }
    public int Fdistance { get; set; }

    // constructor has a list to track what neighboring cells are linked (joined by passage) to this cell
    public Cell(int cellRowCoord, int cellColCoord)
    {
        CellCoord = new Vector3Int(cellColCoord,1, cellRowCoord);
        NameCellCoord = "Cell - " + cellRowCoord + " - " + cellColCoord;
        neighboringCellsWithPassage = new Dictionary<Cell, bool>();
    }

    /*
     * connect current cell with cell parameter (a linked neighbour cell)
     * want to be bidirectional, link in the actual cell and in the neighbour
     * connected cell
     */
    public void JoinTwoNeighbourCells(Cell cell, bool bidirectional = true)
    {
        if (!neighboringCellsWithPassage.ContainsKey(cell))
        {
            neighboringCellsWithPassage.Add(cell, true);
        }

        if (bidirectional)
        {
            cell.JoinTwoNeighbourCells(this, false);
        }

    }
    
    public bool IsJoinedWith(Cell cell)
    {
        if (cell == null) return false;
        return neighboringCellsWithPassage.ContainsKey(cell);
    }

    public IEnumerable<Cell> ReturnNeighboursCellWithPassage()
    {
        return neighboringCellsWithPassage.Keys;
    }
    
    
    

    // disconnect two cells in a bidirectional way
    public void DisjoinTwoNeighbourCells(Cell cell, bool bidirectional = true)
    {
        if (neighboringCellsWithPassage.ContainsKey(cell))
        {
            neighboringCellsWithPassage.Remove(cell);
        }
        
        if (bidirectional)
        {
            cell.DisjoinTwoNeighbourCells(this, false);
        }

    }

    #region not used
    /*
     * check possible surround cells by coordinal properties (North, East...)
     * return a list of neighbours cells avaible
     * joined or not to this cell
     */
    private List<Cell> querySurroundNeighbours =>
        new List<Cell> {North, South, East, West}.Where(surroundedDirectionCell => surroundedDirectionCell != null).ToList();

    #endregion
}
