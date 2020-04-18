using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
    public Vector3 origin;
    public float planarDistanceFromObject = 5;
    public float smoothing = 25f;
    private Transform lookingAt;
    private Transform targetTrans;
    private Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        lookingAt = null;
        targetTrans = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(lookingAt == null)
        {
            transform.LookAt(origin);
        } 
        else
        {
            transform.LookAt(lookingAt);
        }
        if(targetTrans != null)
        {
            target = CalculateTargetPos();
            if ( SquareDistanceFromTarget > 1)
            {
                transform.position += (target - transform.position) / smoothing;
            }
        }
    }

    public float SquareDistanceFromTarget { get { return targetTrans == null ? float.MaxValue : Vector3.SqrMagnitude(target - transform.position); } }

    public Vector3 CalculateTargetPos()
    {
        Vector3 originDiff = targetTrans.position - origin;
        float dist = originDiff.magnitude + planarDistanceFromObject;
        Vector3 newTarget = (originDiff.normalized) * dist;
        return new Vector3(newTarget.x, transform.position.y, newTarget.z);
    }

    public void TrackObject(Transform transform)
    {
        lookingAt = transform;
        targetTrans = transform;
    }

}
