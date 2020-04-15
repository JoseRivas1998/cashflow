using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{

    public enum RatRaceSpaceTypes
    {
        Deals,
        Payday,
        Doodad,
        Market,
        Baby,
        Downsized,
        Charity
    }

    private static readonly Dictionary<RatRaceSpaceTypes, Color> ratRaceSpaceColors = new Dictionary<RatRaceSpaceTypes, Color> {
        {RatRaceSpaceTypes.Deals, new Color(131/255f, 164/255f, 75/255f) },
        {RatRaceSpaceTypes.Payday, new Color(218/255f, 135/255f, 67/255f) },
        {RatRaceSpaceTypes.Doodad, new Color(185/255f, 88/255f, 71/255f) },
        {RatRaceSpaceTypes.Market, new Color(110/255f, 145/255f, 213/255f) },
        {RatRaceSpaceTypes.Baby, new Color(114/255f, 82/255f, 122/255f) },
        {RatRaceSpaceTypes.Downsized, new Color(124/255f, 88/255f, 130/255f) },
        {RatRaceSpaceTypes.Charity, new Color(132/255f, 56/255f, 125/255f) }
    };

    private static readonly RatRaceSpaceTypes[] ratRaceSpaces = {
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Doodad,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Charity,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Payday,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Market,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Doodad,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Downsized,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Payday,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Market,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Doodad,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Baby,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Payday,
        RatRaceSpaceTypes.Deals,
        RatRaceSpaceTypes.Market
    };

    public bool debug;
    public bool debugRatRaceDonut;
    public bool debugRatRaceArcs;
    public bool debugRatRaceCenters;
    public float ratRaceOuterRadius;
    public float ratRaceInnerRadius;
    public Vector3 ratRaceCenterOffset;
    public float ratRaceStartSpaceAngleOffset;
    public float ratRaceSpaceCenterThreshold;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < ratRaceSpaces.Length; i++)
        {
            RatRaceSpaceTypes space = ratRaceSpaces[i];
            Debug.Log(space);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (debug)
        {
            if(debugRatRaceDonut)
            {
                UnityEditor.Handles.color = new Color(0, 0, 1, 0.25f);
                UnityEditor.Handles.DrawSolidDisc(transform.position + ratRaceCenterOffset, Vector3.up, ratRaceOuterRadius);
                UnityEditor.Handles.color = new Color(0, 1, 0, 0.25f);
                UnityEditor.Handles.DrawSolidDisc(transform.position + ratRaceCenterOffset, Vector3.up, ratRaceInnerRadius);
            }
            for (int i = 0; i < ratRaceSpaces.Length; i++)
            {
                
                RatRaceSpaceTypes space = ratRaceSpaces[i];
                Color c = ratRaceSpaceColors[space];
                UnityEditor.Handles.color = new Color(c.r, c.g, c.b, 0.5f);
                if (debugRatRaceArcs)
                {
                    float fromAngle = SpaceStartAngleDeg(i) * Mathf.Deg2Rad;
                    Vector3 from = new Vector3(Mathf.Cos(fromAngle), 0, Mathf.Sin(fromAngle));
                    UnityEditor.Handles.DrawSolidArc(transform.position + ratRaceCenterOffset, Vector3.up, from, SpaceArcAngleDeg(), ratRaceOuterRadius);
                }
                if(debugRatRaceCenters)
                {
                    Vector3 center = SpaceCenter(i);
                    UnityEditor.Handles.DrawSolidDisc(center, Vector3.up, ratRaceSpaceCenterThreshold);
                }
            }
        }
    }

    public float SpaceArcAngleDeg()
    {
        return 360 / ratRaceSpaces.Length;
    }

    public float SpaceLength()
    {
        return ratRaceOuterRadius - ratRaceInnerRadius;
    }

    public float SpaceStartAngleDeg(int space)
    {
        return ratRaceStartSpaceAngleOffset - (SpaceArcAngleDeg() * space);
    }

    public Vector3 SpaceCenter(int space)
    {
        float distanceFromCenter = ratRaceInnerRadius + (SpaceLength() * 0.5f);
        float angleDeg = SpaceStartAngleDeg(space) - (SpaceArcAngleDeg() * 0.5f);
        float angleRad = angleDeg * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad)) * distanceFromCenter;
        return transform.position + ratRaceCenterOffset + offset;
    }

}
