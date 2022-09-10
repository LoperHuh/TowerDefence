using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Environment.Map
{
    public static class TileInGridInitialized
    {
        public enum Corner
        {
            /// <summary>
            /// Upper Left corner.
            /// </summary>
            UpperLeft = 0,
            /// <summary>
            /// Upper Right corner.
            /// </summary>
            UpperRight = 1,
            /// <summary>
            /// Lower Left corner.
            /// </summary>
            LowerLeft = 2,
            /// <summary>
            /// Lower Right corner.
            /// </summary>
            LowerRight = 3
        }

        public static T[,] InstantiateTileGrid<T>(Vector2Int gridSize, Vector3 tileMeshSize, T tile, Transform parenTransform) where T : MonoBehaviour
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning($"Unity is not playing silly");
                return null;
            }
            // Vector2Int gridSize;// = iMapDataAsset.GetGridSize();
            // Vector3 tileMeshSize = editableTilePrefab.ScaledMeshSize;
            float xStart = (tileMeshSize.x * (gridSize.x - 1)) / 2;
            float yStart = (tileMeshSize.z * (gridSize.y - 1)) / 2;
            float tileElevation = 0.1f;
            Vector3 startPosition = new Vector3(-xStart, tileElevation, -yStart);
            T[,] instantiatedTiles = new T[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    instantiatedTiles[x, y] = MonoBehaviour.Instantiate(tile, startPosition + new Vector3(tileMeshSize.x * x, 0, tileMeshSize.z * y), Quaternion.identity, parenTransform);
                }
            }
            return instantiatedTiles;
        }
    }
}