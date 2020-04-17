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
            if(Vector3.SqrMagnitude(targetTrans.position - transform.position) > 1)
            {
                Vector3 target = CalculateTargetPos();
                transform.position += (target - transform.position) / smoothing;
            }
        }
    }

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
