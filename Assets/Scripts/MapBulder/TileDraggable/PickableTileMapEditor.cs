using Game.GameSystem.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Game.Environment.Map
{
    public class PickableTileMapEditor : MonoBehaviour, IMouse
    {
        [SerializeField] TileMapDraggableViewMapEditor tileDraggablePrefab;
        [SerializeField] MeshFilter tileViewMeshFilter;
        private TileMapDraggableViewMapEditor currentDraggable;
        [SerializeField] string tileMeshName;
        public event Action<ITileTargetDroppable> OnTileDropped;
        public Vector3 ScaledMeshSize => Vector3.Scale(tileViewMeshFilter.sharedMesh.bounds.size, tileViewMeshFilter.transform.lossyScale);
        public string TileName { get; private set; }
        public void SetTileView(string tileName, Mesh tileMesh)
        {
            TileName = tileName;
            tileViewMeshFilter.mesh = tileMesh;
        }
        private void CreateDraggableTile()
        {
            if (currentDraggable != null) { 
                Debug.LogWarning($"Trying create draggable that already exist! This should not be happen");
            }
            currentDraggable = Instantiate(tileDraggablePrefab, this.transform.position, Quaternion.identity, this.transform);
        }
        public void OnInputMouseDown(InputAction.CallbackContext context)
        {
            CreateDraggableTile();
        }

        public void OnInputMouseDragWorldCoordinates(Vector3 delta)
        {
            currentDraggable.transform.position += delta;
        }

        public void OnInputMouseUp(InputAction.CallbackContext context)
        {
            OnTileDropped?.Invoke(currentDraggable.CurrentTile);
            //mapCreator.UpdateTileMesh(tileMeshName, currentDraggable.CurrentTile);
            Destroy(currentDraggable.gameObject);
        }
    }
}