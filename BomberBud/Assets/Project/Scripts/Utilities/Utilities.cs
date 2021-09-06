using Project.Scripts.Characters;
using Project.Scripts.Managers;
using UnityEngine;

namespace Project.Scripts.Utilities
{
    public static class Utilities
    {
        public static Vector3 GetWorldFromIndex(int index,Vector2Int matrixScale)
        {
            return GetWorldFromCoordinate(GetCoordFromIndex(index,matrixScale),matrixScale);
        }
        public static Vector3 GetWorldFromCoordinate(Vector2Int coordinate,Vector2Int matrixScale)
        {
            return new Vector2(coordinate.x + 0.5f, coordinate.y + 0.5f) - new Vector2((float)matrixScale.x/2,(float)matrixScale.y/2);
        }
        public static int GetIndexFromCoord(Vector2Int coordinate, Vector2Int matrixScale)
        {
            return (coordinate.y * matrixScale.x) + coordinate.x;
        }
        public static Vector2Int GetCoordFromIndex(int index, Vector2Int matrixScale)
        {
            int x = index % matrixScale.x;
            int y = (index - x) / matrixScale.x;
            return new Vector2Int(x, y);
        }
    }
}