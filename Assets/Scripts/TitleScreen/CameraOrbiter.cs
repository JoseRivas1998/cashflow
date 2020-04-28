using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbiter : MonoBehaviour
{

    public BoardManager board;

    public float degPerSecond = 1f;

    private float radPerSecond { get { return degPerSecond * Mathf.Deg2Rad; } }
    private Vector3 newPos {get { return new Vector3(distance * Mathf.Cos(angle), transform.position.y, distance * Mathf.Sin(angle)); } }

    private Vector3 lookingAt;
    private float distance;
    private float angle;


    // Start is called before the first frame update
    void Start()
    {
        lookingAt = board.transform.position + board.ratRaceCenterOffset;
        distance = Vector3.Distance(new Vector3(lookingAt.x, 0, lookingAt.z), new Vector3(transform.position.x, 0, transform.position.z));
        angle = Mathf.Atan2(transform.position.z - lookingAt.z, transform.position.x - lookingAt.x);
        transform.position = newPos;
    }

    // Update is called once per frame
    void Update()
    {
        angle += radPerSecond * Time.deltaTime;
        transform.position = newPos;
        transform.LookAt(lookingAt);
    }
}
