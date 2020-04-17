using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempSpanwer : MonoBehaviour
{
    public float spawnTimer;
    public float spawnForce;
    public GameObject gamePiecePrefab;

    private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTime += Time.deltaTime;
        if (spawnTime >= spawnTimer)
        {
            spawnTime = 0;
            GameObject go = Instantiate(gamePiecePrefab);
            Rigidbody rb = go.GetComponent<Rigidbody>();
            GamePiece gp = go.GetComponent<GamePiece>();
            gp.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
            go.transform.position = transform.position;
            rb.AddForceAtPosition(Random.onUnitSphere * spawnForce, Random.onUnitSphere * 0.5f, ForceMode.Impulse);
        }
    }
}
