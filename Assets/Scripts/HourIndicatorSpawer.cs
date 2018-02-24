using UnityEngine;
using System.Collections;

public class HourIndicatorSpawer : MonoBehaviour {
    const int HourIndicatorCount = 12;
    public GameObject HourIndicator;
    public float Radius = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Awake()
    {
        for(int i = 0;i< HourIndicatorCount;i++)
        {
            Transform hourIndicator = Instantiate<GameObject>(HourIndicator).transform;
            float angle = i * 30.0f;
            hourIndicator.localRotation = Quaternion.Euler(0, angle, 0);
            hourIndicator.localPosition = new Vector3(Mathf.Sin(angle*Mathf.PI/180)*4, 0.2f, Mathf.Cos(angle * Mathf.PI / 180)*4);
        }
    }
}
