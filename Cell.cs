using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathDirecting
{
    public class Cell : MonoBehaviour
    {
        // Cell's world position
        Vector2Int position;
        // Cell's zeroed world position for use as an index
        Vector2Int zeroedPosition;

        // Update this if the cell has an obstacle on it blocking traversal
        bool isWalkable = true;

        // updated as pathing completes. Cells passing a lower value to this cell will
        // cause this cell's previous path (if existent) to update to the shorter route
        int stepsToTarget = 9999;

        // Filled from Director
        int maxSteps;

        // Which direction the agents that walk on this cell should go next
        public Vector2Int traversalDirection { get; private set; }

        // Call once after grid is created and cache - shouldn't change
        [SerializeField] Cell[] neighbors;


        public void Initialize(Director director, Vector2Int cellPos, Vector2Int cellZeroedPos)
        {
            position = cellPos;
            zeroedPosition = cellZeroedPos;
            neighbors = director.GetNeighbors(zeroedPosition).ToArray();
            maxSteps = director.maxStepsToTarget;
        }
        public void SetWalkable(bool walkable)
        {
            isWalkable = walkable;
        }



        // Called from Director to begin radiating outward from the target cell
        public void BeginPropogation()
        {
            Propogate(-1, position);
        }

        // Direction is the vector that points toward the parent cell
        void Propogate(int neighborStepsToTarget, Vector2Int direction)
        {
            if (stepsToTarget <= neighborStepsToTarget + 1 || neighborStepsToTarget == maxSteps
                || !isWalkable)
            {  // Already have a shorter route in place or can't traverse this cell
                return;
            }

            stepsToTarget = neighborStepsToTarget + 1;
            traversalDirection = direction;

            for (int i = 0; i < neighbors.Length; i++)
            {
                // Note, this will also call the parent cell's method
                // But will return immediately. Could potentially optimize
                Cell neighbor = neighbors[i];
                neighbor.Propogate(stepsToTarget, this.position - neighbor.position);
            }
            //Debug.Log($"Steps to target : {stepsToTarget}. Position : {position}");
        }
    }
}