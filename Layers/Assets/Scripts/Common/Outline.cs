using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SchneiderAR
{
    public class Outline : MonoBehaviour
    {
        private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();


        [Serializable]
        private class ListVector3
        {
            public List<Vector3> data;
        }

        [SerializeField]
        private Color outlineColor = Color.white;

        [SerializeField, Range(0f, 10f)]
        private float outlineWidth = 5f;

        [Header("Optional")]

        [SerializeField, HideInInspector]
        private List<Mesh> bakeKeys = new List<Mesh>();

        [SerializeField, HideInInspector]
        private List<ListVector3> bakeValues = new List<ListVector3>();


        //public Material GetFillMaterial()
        //{
        //    return OutlineFillMaterial;
        //}


        void Awake()
        {
            LoadSmoothNormals();
        }




        void LoadSmoothNormals()
        {

            var meshFilter = GetComponent<MeshFilter>();

            // Skip if smooth normals have already been adopted
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
            {
                return;
            }

            // Retrieve or generate smooth normals
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);

            // Clear UV3 on skinned mesh renderers
            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
                {
                    skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
                }
            }
        }

        List<Vector3> SmoothNormals(Mesh mesh)
        {
            // Group vertices by location
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

            // Copy normals to a new list
            var smoothNormals = new List<Vector3>(mesh.normals);

            // Average normals for grouped vertices
            foreach (var group in groups)
            {

                // Skip single vertices
                if (group.Count() == 1)
                {
                    continue;
                }

                // Calculate the average normal
                var smoothNormal = Vector3.zero;

                foreach (var pair in group)
                {
                    smoothNormal += mesh.normals[pair.Value];
                }

                smoothNormal.Normalize();

                // Assign smooth normal to each vertex
                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }

    }

} 

