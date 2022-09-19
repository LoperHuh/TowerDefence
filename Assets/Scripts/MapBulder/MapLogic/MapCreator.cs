using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;

namespace Game.Environment.Map
{
    public class MapCreator : MonoBehaviour
    {
        [SerializeField] Vector2Int gridSize = new Vector2Int(10, 10);
        [SerializeField] MapDataJsonSerializer mapDataSerializer;
        IMapData iMapDataAsset;
        TileMeshLibrary meshLibrary;
        [SerializeField] EditableTile editableTilePrefab;
        EditableTile[,] instantiatedTiles = new EditableTile[0, 0];

        public void SerializeOnSceneMap()
        {
            Debug.Log("Map was saved");
            mapDataSerializer.SerializeMap();
        }
        public Bounds GetMapBounds()
        {
            if (instantiatedTiles.Length > 0)
            {
                Vector3 cellSize = instantiatedTiles[0, 0].ScaledMeshSize;
                Vector3 totalSize = new Vector3(cellSize.x * gridSize.x, cellSize.y, cellSize.z * gridSize.y);
                Bounds outputBounds = new Bounds(this.transform.position, totalSize);
                return outputBounds;
            }
            return new Bounds(Vector3.zero, Vector3.zero);
        }
        public void InstantiateMap(TileMeshLibrary meshLibrary)
        {
            this.meshLibrary = meshLibrary;
            iMapDataAsset = mapDataSerializer.GetMapData();
            LoadMap();
        }
        public void LoadMap()
        {
            Vector2Int gridSizeFromMapData = iMapDataAsset.GetGridSize();
            Extensions.DisposeMatrix(instantiatedTiles);
            instantiatedTiles = TileInGridInitialized.InstantiateTileGrid(gridSizeFromMapData, editableTilePrefab.ScaledMeshSize, editableTilePrefab, this.transform);
            for (int x = 0; x < gridSizeFromMapData.x; x++)
            {
                for (int y = 0; y < gridSizeFromMapData.y; y++)
                {
                    iMapDataAsset.GetTileGridCells()[x, y].tileGridPosition = new Vector2Int(x, y);
                    instantiatedTiles[x, y].name = $"{x} {y} {instantiatedTiles[x, y].name} ";
                    ApplySettingsToTile(instantiatedTiles[x, y], iMapDataAsset.GetTileGridCells()[x, y]);
                }
            }
        }

        public void OldLoadMap()
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning($"Unity is not playing silly");
                return;
            }
            Vector2Int gridSizeFromMapData = iMapDataAsset.GetGridSize();
            Vector3 meshSize = editableTilePrefab.ScaledMeshSize;
            float xStart = (meshSize.x * gridSizeFromMapData.x / 2) - (0.5f * meshSize.x);
            float yStart = (meshSize.z * gridSizeFromMapData.y / 2) - (0.5f * meshSize.z);
            float tileElevation = 0.1f;
            Vector3 startPosition = new Vector3(-xStart, tileElevation, -yStart);
            Extensions.DisposeMatrix(instantiatedTiles);
            instantiatedTiles = new EditableTile[gridSizeFromMapData.x, gridSizeFromMapData.y];
            for (int x = 0; x < gridSizeFromMapData.x; x++)
            {
                for (int y = 0; y < gridSizeFromMapData.y; y++)
                {
                    iMapDataAsset.GetTileGridCells()[x, y].tileGridPosition = new Vector2Int(x, y);
                    instantiatedTiles[x, y] = Instantiate(editableTilePrefab, startPosition + new Vector3(meshSize.x * x, 0, meshSize.z * y), Quaternion.identity, this.transform);
                    instantiatedTiles[x, y].name = $"{x} {y} {instantiatedTiles[x, y].name} ";
                    ApplySettingsToTile(instantiatedTiles[x, y], iMapDataAsset.GetTileGridCells()[x, y]);

                }
            }
        }
        public void UpdateTileMesh(string meshName, ISelectable editableTile)
        {
            if (editableTile is EditableTile)
            {
                UpdateTileMesh(meshName, (EditableTile)editableTile);
            }
        }
        public void UpdateTileMesh(string meshName, EditableTile editableTile)
        {
            Vector2Int index = GetTileIndexByTile(editableTile);
            iMapDataAsset.GetTileGridCells()[index].tileMeshName = meshName;
            ApplySettingsToTile(editableTile, iMapDataAsset.GetTileGridCells()[index]);
        }
        public void RotateTile(EditableTile editableTile)
        {
            Vector2Int index = GetTileIndexByTile(editableTile);
            iMapDataAsset.GetTileGridCells()[index].TileMeshRotation += 90;
            ApplySettingsToTile(editableTile, iMapDataAsset.GetTileGridCells()[index]);
        }
        private void ApplySettingsToTile(EditableTile editableTile, TileMapData tileMapData)
        {
            editableTile.SetMaterial(meshLibrary.MainPalette);
            editableTile.RotateTile(tileMapData.TileMeshRotation);
            Mesh mesh = meshLibrary.GetMeshByString(tileMapData.tileMeshName);
            editableTile.SetTileMesh(mesh);
        }
        private Vector2Int GetTileIndexByTile(EditableTile editableTile)
        {
            return instantiatedTiles.CoordinatesOfVector2(editableTile);
        }

        public List<EditableTile> GetTileListByTileCorners(EditableTile editableTile1, EditableTile editableTile2)
        {

            Vector2Int index1 = GetTileIndexByTile(editableTile1);
            Vector2Int index2 = GetTileIndexByTile(editableTile2);
            Vector2Int startIndex = new Vector2Int(Mathf.Min(index1.x, index2.x), Mathf.Min(index1.y, index2.y));
            Vector2Int endIndex = new Vector2Int(Mathf.Max(index1.x, index2.x), Mathf.Max(index1.y, index2.y));
            List<EditableTile> outputList = new List<EditableTile>();
            for (int x = startIndex.x; x <= endIndex.x; x++)
            {
                for (int y = startIndex.y; y <= endIndex.y; y++)
                {
                    outputList.Add(instantiatedTiles[x, y]);
                }
            }
            return outputList;
        }

        public void RegenerateMap()
        {
            if (!Application.isPlaying)
            {
                Debug.Log($"Unity is not playing silly");
                return;
            }
            iMapDataAsset.CreateTileMap(gridSize);
            LoadMap();
        }

    }
}
