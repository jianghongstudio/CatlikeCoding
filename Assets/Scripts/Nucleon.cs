using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Nucleon : MonoBehaviour {

    public float force;

    Rigidbody body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        body.AddForce(transform.localPosition * -force);
	}
}
