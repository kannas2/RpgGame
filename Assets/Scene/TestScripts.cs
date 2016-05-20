using UnityEngine;
using System.Collections;

public class TestScripts : MonoBehaviour
{
    public int maxSpeed;
    public float x_RotSpeed;

    private Vector3 startPosition;

    void Start()
    {
        maxSpeed = 3;
        x_RotSpeed = 75.0f;

        startPosition = transform.position;
    }

    void Update()
    {
        MoveVertical();
    }

    void MoveVertical()
    {
        transform.position = new Vector3(startPosition.x, (Mathf.Sin(Time.time * maxSpeed) * 0.5f) + 2.5f, startPosition.z);

        if (transform.position.y > transform.position.y+0.5f)
        {
            transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }
        else if (transform.position.y < transform.position.y-0.5f)
        {
            transform.position = new Vector3(startPosition.x, startPosition.y, startPosition.z);
        }

        transform.Rotate(0, x_RotSpeed * Time.deltaTime, 0);
    }
}
