using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static Game.Environment.Map.TileInGridInitialized;

namespace Game.Environment.Map
{
    public class TileSetLibraryPicker : MonoBehaviour
    {
        [SerializeField] PickableTileMapEditor tileDraggablePrefab;
        List<PickableTileMapEditor> draggableList = new List<PickableTileMapEditor>();
        [Inject] TileMeshLibrary meshLibrary;
        [SerializeField] int gridHeight = 6;
        [SerializeField] float additionalGap = 5f;
        public void InstantiatePickableGrid(Vector3 startPosition, Corner corner)
        {
            draggableList.Clear();
            var avalableMeshList = meshLibrary.GetAllTileMeshes();
            int gridWidth = avalableMeshList.Count / gridHeight;
            PickableTileMapEditor[,] instantiated = TileInGridInitialized.InstantiateTileGrid(new Vector2Int(gridWidth, gridHeight), tileDraggablePrefab.ScaledMeshSize + Vector3.one* additionalGap, tileDraggablePrefab, this.transform, startPosition, corner);
            for (int x = 0; x < instantiated.GetLength(0); x++)
            {
                for (int y = 0; y < instantiated.GetLength(1); y++)
                {
                    if (instantiated[x, y] != null)
                    {
                        var tileData = avalableMeshList[x + (instantiated.GetLength(0) * y)];
                        instantiated[x, y].SetTileView(tileData.meshName, tileData.Mesh);
                        instantiated[x, y].OnTileDropped += TileSetLibraryPicker_OnTileDropped;
                    }
                }
            }
        }

        private void TileSetLibraryPicker_OnTileDropped(ITileTargetDroppable obj)
        {
            Debug.Log($"Dropped on cell" );
        }
    }
}