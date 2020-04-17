using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Shoutout to https://answers.unity.com/questions/1215416/rolling-a-3d-dice-detect-which-number-faces-up.html 
 */
public class DiceDirection : MonoBehaviour
{
    private Dictionary<Vector3, int> lookUp;

    // Start is called before the first frame update
    void Awake()
    {
        lookUp = new Dictionary<Vector3, int>();
        // Positives
        lookUp[Vector3.up] = 1;
        lookUp[Vector3.right] = 3;
        lookUp[Vector3.forward] = 5;

        // Negatives
        // These are hard coded for now
        lookUp[Vector3.down] = 7 - lookUp[Vector3.up];
        lookUp[Vector3.left] = 7 - lookUp[Vector3.right];
        lookUp[Vector3.back] = 7 - lookUp[Vector3.forward];
    }

    public int getNumber(float epsilonDeg = 5f)
    {
        // here I would assert lookup is not empty, epsilon is positive and larger than smallest possible float etc
        // Transform reference up to object space
        Vector3 referenceObjectSpace = transform.InverseTransformDirection(Vector3.up);

        // Find smallest difference to object space direction
        float min = float.MaxValue;
        Vector3 minKey = Vector3.zero;
        foreach (Vector3 key in lookUp.Keys)
        {
            float a = Vector3.Angle(referenceObjectSpace, key);
            if (a <= epsilonDeg && a < min)
            {
                min = a;
                minKey = key;
            }
        }
        return (min < epsilonDeg) ? lookUp[minKey] : -1; // -1 as error code for not within bounds
    }

}