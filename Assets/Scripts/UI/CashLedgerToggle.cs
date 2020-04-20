using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashLedgerToggle : MonoBehaviour
{
    public CashLedger ledger;
    public float smoothing = 25f;
    public RectTransform rect;

    private bool open;
    private float targetX;

    // Start is called before the first frame update
    void Start()
    {
        open = false;
        targetX = 0;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = open ? ledger.GetWidth() : 0;
        if (Mathf.Abs(transform.position.x - targetX) > 1)
        {
            transform.position += Vector3.right * ((targetX - transform.position.x) / smoothing);
        }
        else
        {
            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }

    }

    public void ToggleCashLedger()
    {
        if (open)
        {
            ledger.StopUpdatingToPlayer();
        }
        else
        {
            ledger.BeginUpdatingToPlayer();
        }
        open = !open;
    }

    public void Close()
    {
        if (open)
        {
            ToggleCashLedger();
        }
    }
}
