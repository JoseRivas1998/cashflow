using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePiece : MonoBehaviour
{

    public Renderer render;
    public Vector3 origin;

    private float targetAngle;

    public bool onFastTrack;
    public int fastTrackSpace { get; private set; }
    private int currentFastTrackAngle;

    // Start is called before the first frame update
    void Start()
    {
        onFastTrack = false;
        fastTrackSpace = 0;
        currentFastTrackAngle = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!onFastTrack)
        {
            Vector3 diff = transform.position - origin;
            targetAngle = (Mathf.Atan2(diff.x, diff.z) * Mathf.Rad2Deg + 180);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
        }
    }

    public void SetColor(Color color)
    {
        render.material.color = color;
    }

    public void SetFastTrackSpace(int space)
    {
        this.fastTrackSpace = space;
        int targetAngle = BoardManager.FastTrackSpaceAngle(space);
        if (currentFastTrackAngle != targetAngle)
        {
            LeanTween.rotateY(gameObject, targetAngle < currentFastTrackAngle ? targetAngle + 360 : targetAngle, 0.1f);
            currentFastTrackAngle = targetAngle;
        }
    }

}
