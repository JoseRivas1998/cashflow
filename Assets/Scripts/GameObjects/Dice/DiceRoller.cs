using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{

    private enum RollState
    {
        RollReady,
        Shaking,
        Rolling,
        RollComplete
    }

    public Rigidbody rb;
    public Transform pos;
    public float maxDist;
    public float tossForce;
    public float minForceOffset;
    public float maxForceOffset;
    public float minTorque;
    public float maxTorque;

    private RollState state;
    private Vector3 shakeCenter;

    // Start is called before the first frame update
    void Start()
    {
        ResetRoll();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case RollState.Shaking:
                UpdateShaking();
                break;
            case RollState.Rolling:
                UpdateRolling();
                break;
        }
    }

    public void Shake()
    {
        rb.velocity = Vector3.zero;
        if (state == RollState.RollReady)
        {
            shakeCenter = pos.position;
            state = RollState.Shaking;
        }
    }

    public void Roll()
    {
        if (state == RollState.RollReady || state == RollState.Shaking)
        {
            Vector3 forceDirection;
            do
            {
                forceDirection = Random.onUnitSphere;
            } while (forceDirection.y < 0);
            float force = tossForce + (Random.Range(minForceOffset, maxForceOffset) * Utility.RandomSign());
            Vector3 torqueDirection = Random.onUnitSphere;
            float torque = Random.Range(minTorque, maxTorque);
            rb.AddForce(force * forceDirection);
            rb.AddTorque(torque * torqueDirection);
            rb.useGravity = true;
            state = RollState.Rolling;
        }
    }

    public void ResetRoll()
    {
        state = RollState.RollReady;
        rb.useGravity = false;
        shakeCenter = pos.position;
        rb.velocity = Vector3.zero;
    }

    public bool RollReady()
    {
        return state == RollState.RollReady;
    }

    public bool RollComplete()
    {
        return state == RollState.RollComplete;
    }

    void UpdateShaking()
    {
        Vector3 forceDirection;
        float sqDist = Vector3.SqrMagnitude(pos.position - shakeCenter);
        float force = (maxForceOffset - minForceOffset) * 0.15f;
        if (sqDist > (maxDist * maxDist))
        {
            forceDirection = (shakeCenter - pos.position).normalized;
        }
        else
        {
            forceDirection = Random.onUnitSphere;
            force *= Utility.RandomSign();
        }
        Vector3 torqueDirection = Random.onUnitSphere;
        rb.AddForce(forceDirection * force);
        rb.AddTorque(torqueDirection * Random.Range(minTorque, maxTorque));
    }

    void UpdateRolling()
    {
        if (rb.IsSleeping())
        {
            state = RollState.RollComplete;
        }
    }

}
