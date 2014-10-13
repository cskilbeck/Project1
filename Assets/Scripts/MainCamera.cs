using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour
{
    void Awake()
    {
        camera.orthographicSize = Screen.height / 100f / 2f;
        Debug.Log("Setup camera complete");
        Vector3 v = Vector3.zero;
        v = Camera.main.ScreenToWorldPoint(v);
        Debug.Log("1: " + v.ToString());
    }

	void Start ()
    {
	}
	
	void Update ()
    {
	}
}
