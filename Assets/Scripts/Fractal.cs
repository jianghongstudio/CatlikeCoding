using UnityEngine;
using System.Collections;

public class Fractal : MonoBehaviour {

    public Mesh mesh;
    public Material material;
    public int MaxDepth;
    private int depth;
	// Use this for initialization
	void Start () {
        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = material;
        if (depth< MaxDepth)
        {
            StartCoroutine(CreateChildren());
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
    private void Initialize(Fractal parent,Vector3 direction,Quaternion orientation)
    {
        transform.localRotation = orientation;
        mesh = parent.mesh;
        material = parent.material;
        MaxDepth = parent.MaxDepth;
        depth = parent.depth + 1;
        transform.parent = parent.transform;
        transform.localPosition = direction * (0.5f + 0.5f * 0.5f);
        transform.localScale = Vector3.one * 0.5f;
    }
    private IEnumerator CreateChildren()
    {
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").
            AddComponent<Fractal>().Initialize(this, Vector3.up,Quaternion.Euler(0,0,0));
        yield return new WaitForSeconds(0.5f);
        new GameObject("Fractal Child").
            AddComponent<Fractal>().Initialize(this, Vector3.right, Quaternion.Euler(0, 0, -90));
        //yield return new WaitForSeconds(0.5f);
        //new GameObject("Fractal Child").
        //    AddComponent<Fractal>().Initialize(this, Vector3.forward, Quaternion.Euler(0, 0, 90));
        //yield return new WaitForSeconds(0.5f);
        //new GameObject("Fractal Child").
        //    AddComponent<Fractal>().Initialize(this, Vector3.back, Quaternion.Euler(0, 90, 0));
    }
}
