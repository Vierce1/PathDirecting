using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathDirecting
{
    public class Director : MonoBehaviour
    {
        // Jagged array of cells. Their world position (Vector2Int) can be used to look them up quickly
        // There will be empty indices where walkable cells do not exist
        public Cell[][] cells;
        // Record the min and max positions to center the array on zero
        public int minX { get; private set; }
        public int maxX { get; private set; }
        public int minY { get; private set; }
        public int maxY { get; private set; }

        // Use this variable to stop propogation after x steps
        // Useful if you don't want the entire map of agents to follow the path
        public int maxStepsToTarget;

        public void BuildGrid()
        {
            Cell[] foundCells = FindObjectsOfType<Cell>();
            // Initialize ints to hold bounds of array
            minX = minY = 9999;
            maxX = maxY = -9999;
            // Collect the bounds
            for (int i = 0; i < foundCells.Length; i++)
            {
                Cell cell = foundCells[i];
                Vector2Int position = Vector2Int.RoundToInt(cell.transform.position);
                minX = position.x < minX ? position.x : minX;
                maxX = position.x > maxX ? position.x : maxX;
                minY = position.y < minY ? position.y : minY;
                maxY = position.y > maxY ? position.y : maxY;
            }

            // Now have the bounds - initialize the jagged array
            Debug.Log($"creating grid of size {maxX - minX + 1} :  {maxY - minY + 1}");
            cells = new Cell[maxX - minX + 1][];
            for (int x = 0; x < cells.Length; x++)
            {
                cells[x] = new Cell[maxY - minY + 1];
            }

            // Iterate through the found cells again and fill the array at their positional index
            for (int i = 0; i < foundCells.Length; i++)
            {
                Cell cell = foundCells[i];
                Vector2Int position = Vector2Int.RoundToInt(cell.transform.position);
                // Offset the cell's position by minX and minY to center on zero
                Vector2Int zeroedPos = new Vector2Int(position.x - minX, position.y - minY);
                cells[zeroedPos.x][zeroedPos.y] = cell;
            }

            // Iterate one more time to cache the cells' neighbors
            for (int i = 0; i < foundCells.Length; i++)
            {
                Cell cell = foundCells[i];
                Vector2Int zeroedPos = Vector2Int.RoundToInt(new Vector2(cell.transform.position.x - minX,
                    cell.transform.position.y - minY));
                cell.Initialize(this, Vector2Int.RoundToInt(cell.transform.position), zeroedPos);
            }
        }

        // Call this as needed - for turn based games 1x per turn. For realtime, in an update loop
        // every x frames (balanced between performance & responsiveness)
        public void DirectPathing(Vector2Int targetPosition)
        {
            Cell targetCell = cells[targetPosition.x - minX][targetPosition.y - minY];
            targetCell.BeginPropogation();
        }


        // Called by each cell to get its neighbors to pass its route to
        // Note, need to pass the zeroed positoin here to use as an index
        // To allow diagonal routes, add those checks here (left-up, etc)
        // This can just be called at once and cached by each cell for optimization
        public List<Cell> GetNeighbors(Vector2Int cellZeroedPos)
        {
            List<Cell> neighbors = new List<Cell>();

            // check left
            if (cellZeroedPos.x > 0)
            {
                Cell cell = cells[cellZeroedPos.x - 1][cellZeroedPos.y];
                if (cell != null)
                {  // Index could be empty - remember grid is square around the bounds
                    neighbors.Add(cell);
                }

            }
            // check right
            if (cellZeroedPos.x < cells.Length - 1)
            {
                Cell cell = cells[cellZeroedPos.x + 1][cellZeroedPos.y];
                if (cell != null)
                {  // Index could be empty - remember grid is square around the bounds
                    neighbors.Add(cell);
                }
            }
            // check down
            if (cellZeroedPos.y > 0)
            {
                Cell cell = cells[cellZeroedPos.x][cellZeroedPos.y - 1];
                if (cell != null)
                {  // Index could be empty - remember grid is square around the bounds
                    neighbors.Add(cell);
                }
            }
            // check up
            if (cellZeroedPos.y < cells[0].Length - 1)  // check length of first row
            {
                Cell cell = cells[cellZeroedPos.x][cellZeroedPos.y + 1];
                if (cell != null)
                {  // Index could be empty - remember grid is square around the bounds
                    neighbors.Add(cell);
                }
            }

            return neighbors;
        }


        public Cell GetCellAtV2(Vector2Int position)
        {
            return cells[position.x - minX][position.y - minY];
        }
        public Cell GetCellAtV3(Vector3 position)
        {
            return cells[Mathf.RoundToInt(position.x) - minX][Mathf.RoundToInt(position.y) - minY];
        }
    }
}