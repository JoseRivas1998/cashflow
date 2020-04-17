using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSound : MonoBehaviour
{

    public AudioSource audio;
    public float minImpactVelocity = 0.1f;
    public float maxImpactVelocity = 1.0f;

    public float minPitch = 0.5f;
    public float maxPitch = 1.5f;


    private void OnCollisionEnter(Collision collision)
    {
        float impactVelocity = collision.relativeVelocity.magnitude;

        if(impactVelocity > minImpactVelocity)
        {
            audio.volume = Mathf.Lerp(0.0f, 1.0f, (impactVelocity - minImpactVelocity) / maxImpactVelocity);
            audio.pitch = Mathf.Lerp(minPitch, maxPitch, (impactVelocity - minImpactVelocity) / maxImpactVelocity);
            audio.Play();
        }

    }

}
