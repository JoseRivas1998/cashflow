using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    public Renderer render;
    public Vector3 origin;

    private float targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = transform.position - origin;
        targetAngle = (Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg + 180);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }

    public void SetColor(Color color)
    {
        render.material.color = color;
    }

}
