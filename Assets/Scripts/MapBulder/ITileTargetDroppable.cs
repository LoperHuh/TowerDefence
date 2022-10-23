using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITileTargetDroppable 
{
    public Action<bool> TileSelected();
    public void SelectTarget();
    public void DeselectTarget();
}
