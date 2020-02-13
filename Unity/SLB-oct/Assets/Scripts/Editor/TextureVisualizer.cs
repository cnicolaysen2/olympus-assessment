using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class TextureVisualizer : EditorWindow
{
    private int X;
    private int Y;
    private byte[,][] Data;

    private Texture2D[,] Textures;
    private Rect[,] Rects;

    public void Initialize(int x, int y, byte[,][] data)
    {
        X = x;
        Y = y;
        Data = data;

        CreateTextures();
    }

    private void CreateTextures()
    {
        Texture2D[,] temp = new Texture2D[X, Y];
        Rects = new Rect[X, Y];

        //int x_size = 0;
        //int y_size = 0;
               
        for (int y = 0; y < Y; y++)
        {
            for (int x = 0; x < X; x++)
            {
                int size = (int)Math.Sqrt(Data[x, y].Length / 4);

                //x_size = size * X;
                //y_size = size * Y;

                temp[x, y] = new Texture2D(size, size, TextureFormat.RGBA32, false);
                temp[x, y].LoadRawTextureData(Data[x, y]);
                temp[x, y].Apply();

                Rects[x, y] = new Rect((X - 1 - x) * size, y * size, size, size);
            }
        }

        Textures = temp;

        //Texture2D textureToSave = new Texture2D(x_size, y_size, TextureFormat.RGBAFloat, false);
        //List<byte> fullArray = new List<byte>();

        //foreach (Texture2D tex in Textures)
        //{
        //    foreach (byte b in tex.GetRawTextureData())
        //    {
        //        fullArray.Add(b);
        //    }
        //}

        //textureToSave.LoadRawTextureData(fullArray.ToArray());
        //textureToSave.Apply();

        //byte[] bytes = textureToSave.EncodeToPNG();
        //File.WriteAllBytes("Assets/BIN/dataImage.png", bytes);
    }

    private void OnGUI()
    {
        if (Textures != null)
        {
            for (int y = 0; y < Y; y++)
            {
                GUILayout.BeginHorizontal();

                for (int x = 0; x < X; x++)
                {
                    GUI.DrawTexture(Rects[x, y], Textures[x, y]);
                    //GUI.Box(Rects[x, y], Textures[x, y]);
                }

                GUILayout.EndHorizontal();
            }
        }
    }

    private void OnDestroy()
    {
        Textures = null;
        Rects = null;
    }
}
