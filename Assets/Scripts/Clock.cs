using UnityEngine;
using System.Collections;
using System;

public class Clock : MonoBehaviour {

    const float degreesPerHour = 30f;
    const float degreesPerMinute = 6f;
    const float degreesPerSecond = 6f;


    public Transform HoursArm;
    public Transform MintesArm;
    public Transform SecArm;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        DateTime time = DateTime.Now;
        Debug.Log(time);
        HoursArm.localRotation =
            Quaternion.Euler(0f, time.Hour * degreesPerHour, 0f);
        MintesArm.localRotation =
            Quaternion.Euler(0f, time.Minute * degreesPerMinute, 0f);
        SecArm.localRotation =
            Quaternion.Euler(0f, time.Second * degreesPerSecond, 0f);
    }
}
