using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class uniGen : MonoBehaviour
{
    public static SpawnSettings SpawnSettings = new SpawnSettings();
    [SerializeField]
    public GalacticBody universePrefab = null;

    GalacticBody universeBody;

    public GameObject universe;

    private void Start()
    {

        universeBody = Instantiate(universePrefab);
        universeBody.SpawnChildren();
    }

    void GetGalacticBodyByType(EntityType type)
    {
        universe.GetComponent<GalacticBody>().GetGalacticBodyByType(type);

    }
}
public class SpawnSettings
{
    public int minU = 1;
    public int maxU = 1;

    public int minG = 1;
    public int maxG = 1;

    public int minS = 1;
    public int maxS = 10;

    public int minP = 1;
    public int maxP = 5;

    public int minM = 1;
    public int maxM = 3;

    public float maxDistGK = 2f;
    public float maxDistSK = 2f;
    public float maxDistPK = 3f * 15f;
    public float maxDistMK = 4f * 15f * 15f;

    public float maxDist = 100000f;
    public float minDist = 100f;
}
