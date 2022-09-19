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
            LowerRight = 3,
            /// <summary>
            /// Center
            /// </summary>
            Center = 4
        }

        public static T[,] InstantiateTileGrid<T>(Vector2Int gridSize, Vector3 tileMeshSize, T tile, Transform parenTransform,  Vector3 initStartPosition, Corner corner = Corner.Center) where T : MonoBehaviour
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning($"Unity is not playing silly");
                return null;
            }
         
            float tileElevation = 0.1f;
            Vector3 startPosition = CellInitiationStart(corner, tileMeshSize,gridSize)+ new Vector3(0, tileElevation, 0);
            T[,] instantiatedTiles = new T[gridSize.x, gridSize.y];
            T previousPlaced = null;
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    instantiatedTiles[x, y] = MonoBehaviour.Instantiate(tile, initStartPosition+ startPosition + new Vector3(tileMeshSize.x * x, 0, tileMeshSize.z * y), Quaternion.identity, parenTransform);
                    if (previousPlaced != null)
                    {
                        Debug.DrawLine(previousPlaced.gameObject.transform.position, instantiatedTiles[x, y].gameObject.transform.position, Color.Lerp(Color.white, Color.red, x/(float)gridSize.x),60f);
                    }
                    previousPlaced = instantiatedTiles[x, y];
                }
            }
            
            return instantiatedTiles;
        }
        private static IEnumerator LatePlacer( List<(GameObject,Vector3)> tileAndPosititon)
        {
            yield return null;
            float speed = 1;
            foreach((GameObject go,Vector3 pos) item in tileAndPosititon)
            {
                Vector3 oldPos = item.go.transform.position;
                float progress = 0;
                while (progress < 1)
                {
                    progress += Time.deltaTime*speed;
                    item.go.transform.position = Vector3.Lerp(oldPos, item.pos, progress);
                    yield return null;
                } 
             
            }
        }
        private static Vector3 CellInitiationStart(Corner corner, Vector3 cellSize,Vector2 cellCount)
        {
            Vector2 planeTotalSize = new Vector2(cellSize.x * cellCount.x, cellSize.z * cellCount.y);
            switch (corner)
            {
                case Corner.Center:
                    {
                        float xStart = (planeTotalSize.x / 2) - (0.5f * cellSize.x);
                        float yStart = (planeTotalSize.y / 2) - (0.5f * cellSize.z);
                        return new Vector3(-xStart, 0, -yStart);
                    }
                case Corner.LowerLeft:
                    {
                        return Vector3.zero;
                    }
                case Corner.LowerRight:
                    {
                        return new Vector3(-planeTotalSize.x, 0, 0);
                    }
                case Corner.UpperLeft:
                    {
                        return new Vector3(0, 0, -planeTotalSize.y);
                    }
                case Corner.UpperRight:
                    {
                        return new Vector3(-planeTotalSize.x, 0, -planeTotalSize.y);
                    }
            }
            return Vector3.zero;
        }
    }
}