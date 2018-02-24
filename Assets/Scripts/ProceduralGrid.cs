using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralGrid : MonoBehaviour {

    public int width, height;

    Vector3[] vertices;
    int[] triangles;

    Mesh mesh;

    void Awake()
    {
        StartCoroutine(Generate());
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        if(vertices==null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for(int i =0;i<vertices.Length;i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    IEnumerator Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Grid";

        vertices = new Vector3[(width + 1) * (height + 1)];

        var wait = new WaitForSeconds(0.25f);

        Vector2[] uv = new Vector2[vertices.Length];
        for (int y = 0,index = 0; y <= height; y++)
        {
            for(int x =0;x<=width;x++,index++)
            {
                vertices[index] = new Vector3(x, y, 0);
                uv[index] = new Vector2((float)x / width, (float)y / height);
                yield return wait;
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;

        triangles = new int[width * height * 6];
        
        for (int ti = 0, vi = 0, y = 0; y < height; y++, vi++)
        {
            for (int x = 0; x < width; x++, ti += 6, vi++)
            {
                triangles[ti] = vi;
                triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                triangles[ti + 4] = triangles[ti + 1] = vi + width + 1;
                triangles[ti + 5] = vi + width + 2;
                mesh.triangles = triangles;
                yield return wait;
            }
        }

        mesh.RecalculateNormals();
    }
}
