using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
        for(int x = 107; x < 114 ; x++)
        {
            for(int y = 175; y < 182;y++)
            {
                var tileGO = new GameObject($"{x}_{y}");

                tileGO.AddComponent<MeshFilter>();
                tileGO.AddComponent<MeshRenderer>();
                var tile = tileGO.AddComponent<EnlargeMapExample>();
                tile.Source = MapSource.MapLargeF32;
                tile.X = x;
                tile.Y = y;
                tile.Z = 9;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
