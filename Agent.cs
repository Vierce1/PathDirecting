using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathDirecting
{
    public class Agent : MonoBehaviour
    {
        // Iterate through each Agent after the Director updates pathing
        // Can call FindPath() whenver it is needed
        // Have the Agent get the cell it's on by using the zeroed-index of its position
        // Detect the direction 
        // Move there
        Director director;

        [SerializeField] Vector3 dir;

        private void Start()
        {
            director = FindObjectOfType<Director>();
        }
        public void FindPath()
        {
            // Get zeroed position (zeroed on the grid of cells)
            Vector2Int zeroedPos = Vector2Int.RoundToInt(new Vector2(
                transform.position.x - director.minX, transform.position.y - director.minY));
            Cell currentCell = director.cells[zeroedPos.x][zeroedPos.y];

            StartCoroutine(Move(currentCell.traversalDirection));
        }

        IEnumerator Move(Vector2Int direction)
        {
            dir = new Vector3(direction.x, direction.y);
            Vector3 start = transform.position;
            Vector3 end = start + new Vector3(direction.x, direction.y);
            float loops = 20f;
            for (float i = 0; i < loops; i++)
            {
                transform.position = Vector3.Lerp(start, end, i / loops);
                yield return null;
            }
        }
    }
}