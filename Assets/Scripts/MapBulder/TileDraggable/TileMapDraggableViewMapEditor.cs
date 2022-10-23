using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.Environment.Map
{
    public class TileMapDraggableViewMapEditor : MonoBehaviour
    {
        private ITileTargetDroppable currentTile;
        public ITileTargetDroppable CurrentTile => currentTile;
        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position,Vector3.down,out hit ))
            {
                ITileTargetDroppable tileSelectable = hit.collider.gameObject.GetComponent<ITileTargetDroppable>();
              
                if (tileSelectable != null&& currentTile!=tileSelectable)
                {
                    currentTile?.DeselectTarget();
                    currentTile = tileSelectable;
                    currentTile.SelectTarget();
                }
            }

        }
        private void OnDestroy()
        {
            currentTile?.DeselectTarget();
        }
    }
}