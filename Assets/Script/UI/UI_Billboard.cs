using UnityEngine;
using System.Collections;

public class UI_Billboard : MonoBehaviour {

    public Camera my_camera;

    void Start()
    {
        my_camera = Camera.main;
    }

    void Update ()
    {
        transform.LookAt(transform.position + my_camera.transform.rotation * Vector3.back, my_camera.transform.rotation * Vector3.up);
	}
}
