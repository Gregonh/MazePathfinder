using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Monobehaviours
{
    public class MazeRenderer : MonoBehaviour
    {
        private MazeGrid mazeGrid;
        private Cell startCell;
        private Cell goalCell;
        [SerializeField] private int cellSize = 10;
        [SerializeField] private Transform wallPrefab = null;
        [SerializeField] private Transform floorPrefab = null;
        [SerializeField] private Transform startPrefab;
        [SerializeField] private Transform goalPrefab;
        [SerializeField] private Transform pathCube;

        private void Start()
        {
            mazeGrid = new MazeGrid(10, 10);
            startCell = mazeGrid.GetStartingCell();
            goalCell = mazeGrid.GetRandomCell();
            //BinaryTreeAlgorithm.BuildMaze(mazeGrid);
            //RecursiveBacktrackerAlgorithm.BuildMaze(mazeGrid);
            RecursiveDivisionAlgorithm.CreateMaze(mazeGrid);
            DrawMaze();
            DrawStartEndDecoratorCell();
            StartCoroutine(nameof(TracePath));
        }

        void DrawMaze()
        {
            /*
        int width = cellSize * mazeGrid.Columns;
        int height = cellSize * mazeGrid.Rows;*/

            var floor = Instantiate(floorPrefab, transform);
            floor.localScale = new Vector3(cellSize, 1, cellSize);
            floor.position = new Vector3(cellSize * 5, 1, cellSize * 5);
            foreach (var cell in mazeGrid.GetCellOneByOne())
            {
                var cellColCoord = cell.CellCoord.x;
                var cellRowCoord = cell.CellCoord.z;
                var xPos1 = cellColCoord * cellSize;
                var zPos1 = cellRowCoord * cellSize;
                var xPos2 = (cellColCoord + 1) * cellSize;
                var zPos2 = (cellRowCoord + 1) * cellSize;
                Vector3 startVector;
                Vector3 endVector;
                Vector3 midPointWallVector;
                if (cell.North == null)
                {
                    startVector = new Vector3(xPos1, 1, zPos1);
                    endVector = new Vector3(xPos2, 1, zPos1);
                    midPointWallVector = FindPositionOfWallPivot(startVector, endVector);
                    BuildWall(midPointWallVector, true);
                }

                if (cell.West == null)
                {
                    startVector = new Vector3(xPos1, 1, zPos1);
                    endVector = new Vector3(xPos1, 1, zPos2);
                    midPointWallVector = FindPositionOfWallPivot(startVector, endVector);
                    BuildWall(midPointWallVector, false);
                }

                if (!cell.IsJoinedWith(cell.East))
                {
                    startVector = new Vector3(xPos2, 1, zPos1);
                    endVector = new Vector3(xPos2, 1, zPos2);
                    midPointWallVector = FindPositionOfWallPivot(startVector, endVector);
                    BuildWall(midPointWallVector, false);
                }

                if (!cell.IsJoinedWith(cell.South))
                {
                    startVector = new Vector3(xPos1, 1, zPos2);
                    endVector = new Vector3(xPos2, 1, zPos2);
                    midPointWallVector = FindPositionOfWallPivot(startVector, endVector);
                    BuildWall(midPointWallVector, true);
                }
            }
        }

        void BuildWall(Vector3 midPointPosition, bool isHorizontal)
        {
            var wall = Instantiate(wallPrefab, transform) as Transform;

            if (!isHorizontal)
            {
                wall.transform.Rotate(0, 90, 0);
            }

            wall.localScale = new Vector3(cellSize, wall.localScale.y, wall.localScale.z);
            wall.position = midPointPosition;
        }

        Vector3 FindPositionOfWallPivot(Vector3 start, Vector3 end)
        {
            var vectorBetweenCoords = (end - start);
            var midPoint = start + (vectorBetweenCoords * 0.5f);
            return midPoint;
        }

        void DrawStartEndDecoratorCell()
        {
            // in the middle of the cell
            Vector3 startPrefabCellPosition = CalculateCellMidPoint(startCell);
            startPrefabCellPosition.x *= -1;
            Vector3 goalPrefabCellPosition = CalculateCellMidPoint(goalCell);
            var startTransform = Instantiate(startPrefab, transform);
            startTransform.position = startPrefabCellPosition;
            var goalTransform = Instantiate(goalPrefab, transform);
            goalTransform.position = goalPrefabCellPosition;
        }

        Vector3 CalculateCellMidPoint(Cell cell)
        {
            float cellMidpointXCoord = (cell.CellCoord.x * cellSize) + cellSize / 2;
            float cellMidpointZCoord = (cell.CellCoord.z * cellSize) + cellSize / 2;
            return new Vector3(cellMidpointXCoord, 1, cellMidpointZCoord);
        }

        IEnumerator TracePath()
        {
            IEnumerable<Cell> pathCells = PrepareTracePath();
            foreach (var cell in pathCells)
            {
                var partPath = Instantiate(pathCube, transform);
                partPath.position = CalculateCellMidPoint(cell);
                yield return new WaitForSeconds(.1f);
            }
        }

        IEnumerable<Cell> PrepareTracePath()
        {
            //var allDistanceRecords = DijkstraAlgorithm.RecordDistancesFromOrigin(startCell, goalCell);
            //Distances distanceFromGoalToStart = allDistanceRecords.TraceShortestPathFromEndToStart(goalCell);
            //return distanceFromGoalToStart.GetAllCells();
            var allDistanceRecords = AStarAlgorithm.RecordDistancesFromOrigin(startCell, goalCell);
            allDistanceRecords.Reverse();
            yield return allDistanceRecords.First();
            var currentFather = allDistanceRecords.First().ParentPathCell;
            for (int i = 0; i < allDistanceRecords.Count; i++)
            {
                if (currentFather == allDistanceRecords[i])
                {
                    yield return allDistanceRecords[i];
                    currentFather = currentFather.ParentPathCell;
                    if (currentFather == goalCell) break;
                }
            }
        }

        void Imprime(Distances distanceNames)
        {
            foreach (var cellKeys in distanceNames.GetAllCells())
            {
                Debug.Log(cellKeys.NameCellCoord);
            }
        }
    }
}