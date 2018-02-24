﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralCube : MonoBehaviour {

    public int width, height, depth;
    public int roundness;


    int[] triangles;
    Vector3[] vertices;
    Vector3[] normals;

    Mesh mesh;

    private void Awake()
    {
        Generate();
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnDrawGizmos()
    {
        if(vertices==null)
        {
            return;
        }

        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(vertices[i], 0.1f);
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(vertices[i], normals[i]);

        }
    }

    void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Produral Cube";

        CreateVertices();
        CreateTriangles();
    }

    void CreateVertices()
    {
        int cornerVertices = 8;
        int edgeVertices = (width + height + depth - 3) * 4;
        int faceVertices = (
            (width - 1) * (height - 1) +
            (width - 1) * (depth - 1) +
            (height - 1) * (depth - 1)) * 2;

        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        normals = new Vector3[vertices.Length];

        int index = 0;

        #region Create Vertices like Tutorials

        for (int y = 0; y <= height; y++)
        {
            for (int x = 0; x <= width; x++)
            {
                SetVertex(index++,x, y, 0);
            }

            for (int z = 1; z <= depth; z++)
            {
                SetVertex(index++, width, y, z);                
            }

            for (int x = width - 1; x >= 0; x--)
            {
                SetVertex(index++, x, y, depth);
            }

            for (int z = depth - 1; z > 0; z--)
            {
                SetVertex(index++, 0, y, z);
            }
        }

        for (int z = 1; z < depth; z++)
        {
            for (int x = 1; x < width; x++)
            {
                SetVertex(index++, x, height, z);
            }
        }

        for (int z = 1; z < depth; z++)
        {
            for (int x = 1; x < width; x++)
            {
                SetVertex(index++, x, 0, z);
            }
        }
        #endregion

        #region Create vertices by my way
        #endregion

        mesh.vertices = vertices;

    }

    void CreateTriangles()
    {
        int quadSize = (width * height + width * depth + height * depth) * 2;

        triangles = new int[quadSize * 6];

        int ring = (width + depth) * 2;
        int t = 0, v = 0;

        for (int y = 0; y < height; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }

        t = CreateTopFace(triangles, t, ring);
        t = CreateBottomFace(triangles, t, ring);
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
        return i + 6;
    }

    private int CreateTopFace(int[] triangles, int t, int ring)
    {
        int v = ring * height;
        for (int x = 0; x < width - 1; x++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
        }
        t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);

        int vMin = ring * (height + 1) - 1;
        int vMid = vMin + 1;
        int vMax = v + 2;
        for (int z = 1; z < depth - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + width - 1);
            for (int x = 1; x < width - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid, vMid + 1, vMid + width - 1, vMid + width);
            }
            t = SetQuad(triangles, t, vMid, vMax, vMid + width - 1, vMax + 1);
        }

        int vTop = vMin - 2;
        t = SetQuad(triangles, t, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < width - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
        }
        t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);
        return t;
    }

    private int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = vertices.Length - (width - 1) * (depth - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < width - 1; x++, v++, vMid++)
        {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);

        int vMin = ring - 2;
        vMid -= width - 2;
        int vMax = v + 2;

        for (int z = 1; z < depth - 1; z++, vMin--, vMid++, vMax++)
        {
            t = SetQuad(triangles, t, vMin, vMid + width - 1, vMin + 1, vMid);
            for (int x = 1; x < width - 1; x++, vMid++)
            {
                t = SetQuad(
                    triangles, t,
                    vMid + width - 1, vMid + width, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + width - 1, vMax + 1, vMid, vMax);
        }

        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < width - 1; x++, vTop--, vMid++)
        {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }
        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);

        return t;
    }

    private void SetVertex(int i, int x, int y, int z)
    {
        Vector3 inner = vertices[i] = new Vector3(x, y, z);

        if (x < roundness)
        {
            inner.x = roundness;
        }
        else if (x > width - roundness)
        {
            inner.x = width - roundness;
        }
        if (y < roundness)
        {
            inner.y = roundness;
        }
        else if (y > height - roundness)
        {
            inner.y = height - roundness;
        }
        if (z < roundness)
        {
            inner.z = roundness;
        }
        else if (z > depth - roundness)
        {
            inner.z = depth - roundness;
        }
        normals[i] = (vertices[i] - inner).normalized;
        vertices[i] = inner + normals[i] * roundness;

    }
}
