using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Environment.Map
{
    public class MainMapEditorManager : MonoBehaviour
    {
        [SerializeField] MapCreator mapCreator;
        [Zenject.Inject] TileMeshLibrary meshLibrary;
        Bounds? mapBounds = null;
        public void Start()
        {
            mapCreator.InstantiateMap(meshLibrary);
            mapBounds = mapCreator.GetMapBounds();
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