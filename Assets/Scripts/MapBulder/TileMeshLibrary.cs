using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "TileMeshCollection")]
public class TileMeshLibrary : ScriptableObject
{
    public class TileMesh
    {
        public string meshName;
        public Mesh Mesh;
    }
    [SerializeField] private List<TileMesh> tileMeshCollection = new List<TileMesh>();
    Dictionary<string, Mesh> tileMeshDictionary = new Dictionary<string, Mesh>();
    
    public Mesh GetMeshByString(string meshName)
    {
        if (tileMeshDictionary.Count <= 0)
        {
            BuildDictionary();
        }
        return tileMeshDictionary[meshName];
    } 
    private void BuildDictionary()
    {
        tileMeshDictionary.Clear();
        Debug.Log("Building Dictionary");
        foreach (var item in tileMeshCollection)
        { 
            tileMeshDictionary[item.meshName] = item.Mesh;
        }
    }
    private List<TileMesh> GetTileMeshes()
    {
        var output = new List<TileMesh>();
        output.AddRange(tileMeshCollection);
        return output; 
    }
    private void OnValidate()
    {
        if(tileMeshCollection.Count!= tileMeshDictionary.Count)
        {
            Debug.Log($"List collection count {tileMeshCollection.Count} was not equal { tileMeshDictionary.Count}{ Environment.NewLine} rebuilding Dictionary");
            BuildDictionary();
        }
        else
        {
            Debug.Log($"Dictionary check passed. Current collectionCount is {tileMeshDictionary.Count}");
        }
    }
}
