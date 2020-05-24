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

    private static readonly FastTrackSpace[] fastTrackSpaces = {
        new FastTrackSpace(FastTrackSpaceType.HealthCare),
        new FastTrackSpace(FastTrackSpaceType.Charity),
        new BusinessInvestment("Burger Shop", 300000, 9500),
        new BusinessInvestment("Heat and A/C Service", 200000, 10000),
        new BusinessInvestment("Quick Food Market", 120000, 5000),
        new BusinessInvestment("Assisted Living Center", 400000, 8000),
        new FastTrackSpace(FastTrackSpaceType.Lawsuit),
        new BusinessInvestment("Ticket Sales Company", 150000, 5000),
        new BusinessInvestment("Hobby Supply Store", 100000, 3000),
        new FastTrackSpace(FastTrackSpaceType.CashFlowDay),
        new BusinessInvestment("Fried Chicken Restaurant", 300000, 10000),
        new BusinessInvestment("Dry Dock Storage", 250000, 10000),
        new BusinessInvestment("Beauty Salon", 250000, 10000),
        new FastTrackSpace(FastTrackSpaceType.TaxAudit),
        new BusinessInvestment("Auto Repair Shop", 150000, 6000),
        new BusinessInvestment("Extreme Sports Equipment Rental", 150000, 5000),
        new FastTrackSpace(FastTrackSpaceType.ForeignOilDeal),
        new FastTrackSpace(FastTrackSpaceType.CashFlowDay),
        new BusinessInvestment("Movie Theater", 150000, 6000),
        new BusinessInvestment("Research Center for Diseases", 300000, 8000),
        new FastTrackSpace(FastTrackSpaceType.BadPartner),
        new BusinessInvestment("App Development Company", 150000, 5000),
        new FastTrackSpace(FastTrackSpaceType.SoftwareCoIPO),
        new BusinessInvestment("Coffee Shop", 120000, 5000),
        new BusinessInvestment("400-Unit Apartment Building", 300000, 8000),
        new BusinessInvestment("Island Vacation Rentals", 100000, 3000),
        new FastTrackSpace(FastTrackSpaceType.Divorce),
        new BusinessInvestment("Build Pro Golf Course", 150000, 6000),
        new BusinessInvestment("Pizza Shop", 225000, 7000),
        new FastTrackSpace(FastTrackSpaceType.CashFlowDay),
        new BusinessInvestment("Collectibles Store", 100000, 3000),
        new BusinessInvestment("Frozen Yogurt Shop", 120000, 5000),
        new FastTrackSpace(FastTrackSpaceType.BioTecCoIPO),
        new FastTrackSpace(FastTrackSpaceType.UnforeseenRepairs),
        new BusinessInvestment("200-Unit Mini Storage", 200000, 6000),
        new BusinessInvestment("Dry Cleaning Business", 100000, 3000),
        new BusinessInvestment("Mobile Home Park", 400000, 9000),
        new FastTrackSpace(FastTrackSpaceType.CashFlowDay),
        new BusinessInvestment("Family Restaurant", 300000, 14000),
        new BusinessInvestment("Private Wildlife Preserve", 120000, 5000)
    };

    private static readonly Dictionary<FastTrackSpaceType, Color> fastTrackTypeColors = new Dictionary<FastTrackSpaceType, Color>() 
    {
        { FastTrackSpaceType.CashFlowDay, new Color(254f/255f, 226f/255f, 84f/255f)},
        { FastTrackSpaceType.BusinessInvestments, new Color(163f/255f, 186f/255f, 54f/255f)},
        { FastTrackSpaceType.HealthCare, new Color(214f/255f, 62f/255f, 48f/255f)},
        { FastTrackSpaceType.Charity, new Color(122f/255f, 41f/255f, 107f/255f)},
        { FastTrackSpaceType.TaxAudit, new Color(214f/255f, 62f/255f, 48f/255f)},
        { FastTrackSpaceType.Divorce, new Color(214f/255f, 62f/255f, 48f/255f)},
        { FastTrackSpaceType.Lawsuit, new Color(214f/255f, 62f/255f, 48f/255f)},
        { FastTrackSpaceType.BadPartner, new Color(214f/255f, 62f/255f, 48f/255f)},
        { FastTrackSpaceType.UnforeseenRepairs, new Color(214f/255f, 62f/255f, 48f/255f)},
        { FastTrackSpaceType.ForeignOilDeal, new Color(163f/255f, 186f/255f, 54f/255f)},
        { FastTrackSpaceType.SoftwareCoIPO, new Color(163f/255f, 186f/255f, 54f/255f)},
        { FastTrackSpaceType.BioTecCoIPO, new Color(163f/255f, 186f/255f, 54f/255f)},
    };

    private static readonly Dictionary<int, int> dieRollStartSpaces = new Dictionary<int, int>()
    {
        {1, 0}, {2, 6}, {3, 13}, {4, 20}, {5, 26}, {6, 33}
    };

    public bool debug;
    public bool debugRatRaceDonut;
    public bool debugRatRaceArcs;
    public bool debugRatRaceCenters;
    public bool debugFastTrackCenters;
    public float ratRaceOuterRadius;
    public float ratRaceInnerRadius;
    public Vector3 ratRaceCenterOffset;
    public float ratRaceStartSpaceAngleOffset;
    public float ratRaceSpaceCenterThreshold;
    public float downSizedTurnOneDistance;
    public float downSizedTurnTwoDistance;
    public PolarPoint corner37;
    public PolarPoint corner9;
    public PolarPoint corner17;
    public PolarPoint corner29;
    public float spaceWidth;
    public float spaceHeight;
    public float sqRatRaceSpaceCenterThreshold { get { return ratRaceSpaceCenterThreshold * ratRaceSpaceCenterThreshold; } }

    private const int DOWNSIZED_SPACE = 11;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ratRaceSpaces.Length; i++)
        {
            RatRaceSpaceTypes space = ratRaceSpaces[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
#if UNITY_EDITOR
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
            if(debugRatRaceCenters)
            {
                UnityEditor.Handles.color = new Color(1f, 0f, 1f, 0.25f);
                Vector3 center = DownSizedSpace(1);
                UnityEditor.Handles.DrawSolidDisc(center, Vector3.up, ratRaceSpaceCenterThreshold);
                center = DownSizedSpace(2);
                UnityEditor.Handles.DrawSolidDisc(center, Vector3.up, ratRaceSpaceCenterThreshold);
            }
            if (debugFastTrackCenters)
            {
                for (int i = 0; i < fastTrackSpaces.Length; i++)
                {
                    FastTrackSpaceType space = fastTrackSpaces[i].type;
                    Color c = fastTrackTypeColors[space];
                    UnityEditor.Handles.color = new Color(c.r, c.g, c.b, 0.5f);
                    Vector3 center = FastTrackSpaceCenter(i);
                    UnityEditor.Handles.DrawSolidDisc(center, Vector3.up, ratRaceSpaceCenterThreshold);
                }
            }
        }
    }
#endif

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

    public int NormalizeSpace(int space)
    {
        return space % ratRaceSpaces.Length;
    }

    public int NormalizeFastTrackSpace(int space)
    {
        return space % fastTrackSpaces.Length;
    }

    public int PayDays(int startingSpace, int numSpaces)
    {
        int sum = 0;
        for (int i = 1; i <= numSpaces; i++)
        {
            int spaceIndex = NormalizeSpace(startingSpace + i);
            RatRaceSpaceTypes space = ratRaceSpaces[spaceIndex];
            if(space == RatRaceSpaceTypes.Payday)
            {
                sum++;
            }
        }
        return sum;
    }

    public int CashFlowDays(int startingSpace, int numSpaces)
    {
        int sum = 0;
        for (int i = 1; i <= numSpaces; i++)
        {
            int spaceIndex = NormalizeFastTrackSpace(startingSpace + i);
            FastTrackSpace space = fastTrackSpaces[spaceIndex];
            if (space.type == FastTrackSpaceType.CashFlowDay)
            {
                sum++;
            }
        }
        return sum;
    }

    public Vector3 DownSizedSpace(int downSizedTurn)
    {
        float dist;
        if(downSizedTurn == 1)
        {
            dist = downSizedTurnOneDistance;
        } 
        else if (downSizedTurn == 2)
        {
            dist = downSizedTurnTwoDistance;
        } 
        else 
        {
            dist = ratRaceInnerRadius + (SpaceLength() * 0.5f);
        }
        return transform.position + (SpaceCenter(DOWNSIZED_SPACE) - transform.position - ratRaceCenterOffset).normalized * dist;
    }

    public RatRaceSpaceTypes GetSpaceType(int space)
    {
        return ratRaceSpaces[NormalizeSpace(space)];
    }

    private bool isCorner(int space)
    {
        return space == 37 || space == 9 || space == 17 || space == 29;
    }

    public Vector3 FastTrackSpaceCenter(int space)
    {
        if(isCorner(space)) {
            switch(space)
            {
                case 37:
                    return this.transform.position + ratRaceCenterOffset + corner37.toVector3XZ;
                case 9:
                    return this.transform.position + ratRaceCenterOffset + corner9.toVector3XZ;
                case 17:
                    return this.transform.position + ratRaceCenterOffset + corner17.toVector3XZ;
                case 29:
                    return this.transform.position + ratRaceCenterOffset + corner29.toVector3XZ;
            }
        }
        int cornerSpace = space;
        int spacesToCorner = 0;
        while (!isCorner(cornerSpace))
        {
            cornerSpace = (cornerSpace + fastTrackSpaces.Length - 1) % fastTrackSpaces.Length; // todo dont make this hard coded
            spacesToCorner++;
        }
        Vector3 cornerCenter = FastTrackSpaceCenter(cornerSpace);
        float xOffset = 0;
        float zOffset = 0;
        if (cornerSpace == 9)
        {
            zOffset = spaceHeight * spacesToCorner;
        }
        else if (cornerSpace == 17)
        {
            xOffset = spaceWidth * spacesToCorner;
        }
        else if (cornerSpace == 29)
        {
            zOffset = -(spaceHeight * spacesToCorner);
        }
        else if (cornerSpace == 37)
        {
            xOffset = -(spaceWidth * spacesToCorner);
        }
        return cornerCenter + new Vector3(xOffset, 0, zOffset);
    }

    public int FastTractStartSpace(int dieRoll)
    {
        return dieRollStartSpaces[dieRoll];
    }

    public static int FastTrackSpaceAngle(int space)
    {
        if (space >= 37)
        {
            return 0;
        }
        if (space >= 29)
        {
            return 270;
        }
        if (space >= 17)
        {
            return 180;
        }
        if (space >= 9)
        {
            return 90;
        }
        return 0;
    }

}
