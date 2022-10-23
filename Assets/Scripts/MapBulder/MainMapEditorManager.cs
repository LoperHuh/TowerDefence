using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environment.Map
{
    public class MainMapEditorManager : MonoBehaviour
    {
        [SerializeField] MapCreator mapCreator;
        [Zenject.Inject] TileMeshLibrary meshLibrary;
        [SerializeField] TileSetLibraryPicker tileSetLibraryPicker;
        
        Bounds? mapBounds = null;
        public void Start()
        {
            mapCreator.InstantiateMap(meshLibrary);
            mapBounds = mapCreator.GetMapBounds();
            tileSetLibraryPicker.InstantiatePickableGrid(mapBounds.Value.max, TileInGridInitialized.Corner.UpperLeft);
        }
        public void OnApplicationQuit()
        {
            mapCreator.SerializeOnSceneMap();
        }
        private void OnDrawGizmos()
        {
            if (mapBounds.HasValue)
            {

                Gizmos.color = Color.red;
                Vector3 size = mapBounds.Value.size;
                Gizmos.DrawWireCube(mapBounds.Value.center, size);
            }
        }
    }
}