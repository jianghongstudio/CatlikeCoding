using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralCube : MonoBehaviour {

    public int width, height, depth;

    int[] triangles;
    Vector3[] vertices;
    Mesh mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
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
        Gizmos.color = Color.black;
        for(int i=0;i<vertices.Length;i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    IEnumerator Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Produral Cube";

        WaitForSeconds wait = new WaitForSeconds(0.05f);

        int cornerVertices = 8;
        int edgeVertices = (width + height + depth - 3) * 4;
        int faceVertices = (
            (width - 1) * (height - 1) +
            (width - 1) * (depth - 1) +
            (height - 1) * (depth - 1)) * 2;

        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int index = 0;

        #region Create Vertices like Tutorials

        for(int y=0;y<=height;y++)
        {
            for (int x = 0; x <= width; x++)
            {
                vertices[index++] = new Vector3(x, y, 0);
                yield return wait;
            }

            for (int z = 1; z <= depth; z++)
            {
                vertices[index++] = new Vector3(width, y, z);
                yield return wait;
            }

            for (int x = width - 1; x >= 0; x--)
            {
                vertices[index++] = new Vector3(x, y, depth);
                yield return wait;
            }

            for (int z = depth - 1; z > 0; z--)
            {
                vertices[index++] = new Vector3(0, y, z);
                yield return wait;
            }
        }

        for (int z = 1; z < depth; z++)
        {
            for (int x = 1; x < width; x++)
            {
                vertices[index++] = new Vector3(x, height, z);
                yield return wait;
            }
        }

        for (int z = 1; z < depth; z++)
        {
            for (int x = 1; x < width; x++)
            {
                vertices[index++] = new Vector3(x, 0, z);
                yield return wait;
            }
        }
        #endregion

        #region Create vertices by my way
        #endregion

        mesh.vertices = vertices;

        int quadSize = (width * height + width * depth + height * depth) * 2;

        triangles = new int[quadSize * 6];

        int ring = (width + depth) * 2;
        int t = 0, v = 0;

        for(int y=0;y<height;y++,v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
                mesh.triangles = triangles;
                yield return wait;
            }
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
            mesh.triangles = triangles;
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
}
