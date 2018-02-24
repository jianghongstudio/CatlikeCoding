using UnityEngine;
using System.Collections;

public class NucleonSpawner : MonoBehaviour {

    public Nucleon[] nucleonPrefabs;
    public float MaxDistance = 10;

    [Range(0f, 1f)]
    public float SpawnTime;

    float timeSinceLastSpawn = 0;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

    }

    void FixedUpdate()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= SpawnTime)
        {
            Instantiate(nucleonPrefabs[Random.Range(0, nucleonPrefabs.Length)]).transform.localPosition = Random.onUnitSphere * Random.Range(0, MaxDistance);
            timeSinceLastSpawn = 0f;
        }
    }

}
