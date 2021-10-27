
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DijkstraAlgorithm 
{
    /*
     * build grid
        binary tree
        start = grid[0,0]
        Distance distance = start.distances;
        
        "path from northwest corner to southwest corner:"
        grid.distances = distances.path_to(grid[grid.rows - 1, 0])
     */
    
    /*
    * fill distance dictionary
    * with connected cells
    * with connected cells
    * using Dijkstra
    */
    public static Distances RecordDistancesFromOrigin(Cell originCell, Cell goalCell)
    {
        Distances distances = new Distances(originCell);
        // cells that have possible neighbours not included in distances dictionary yet
        List<Cell> cellsWithNeighboursForDistancesDictionary = new List<Cell>();
        //init with start cell
        cellsWithNeighboursForDistancesDictionary.Add(originCell); 
        while (cellsWithNeighboursForDistancesDictionary.Any()) // while there are distance not measured
        {
            List<Cell> aux = new List<Cell>(); 
            // into actual cell
            foreach (var actualCell in cellsWithNeighboursForDistancesDictionary)
            {
                if (actualCell == goalCell) return distances; // stop save distance if we found goal distance
                // take actual cell´s neighbours
                foreach (var neighbour in actualCell.ReturnNeighboursCellWithPassage())
                {
                    if (!distances.HasCellSaved(neighbour))
                    {
                        // save in distance the cell and distance from origin
                        distances[neighbour] = distances[actualCell] + 1;
                        aux.Add( neighbour);
                    }
                }
            }
            // actual cells will be the cells that just were saved in distances
            cellsWithNeighboursForDistancesDictionary = aux;
        }
        
        return distances;
    }
    
    
}
