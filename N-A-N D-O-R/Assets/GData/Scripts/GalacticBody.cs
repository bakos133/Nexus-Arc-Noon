using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticBody : MonoBehaviour
{
    Gravity Gravity = new Gravity();

    //Variable                          //Call command
    public float surfaceG;              //GalacticBody.surfaceG
    public Vector3 force;               //GalacticBody.force
    public float mass;                  //GalacticBody.mass
    public Vector3 velocity;            //GalacticBody.velocity
    public Vector3 initialVelocity;     //GalacticBody.initialVelocity
    public float radius;                //GalacticBody.radius

    GalacticBody rb;


    #region noob addition
    [SerializeField]
    EntityType EntityType;

    [SerializeField]
    GalacticBody ChildPrefab = null;

    Vector3 rotationVector => new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));

    List<GalacticBody> children = new List<GalacticBody>();

    #endregion

    private void Start()
    {
        radius = transform.localScale.magnitude;
        surfaceG = radius;
        mass = (surfaceG * radius * radius / GManager.gConstant) ;

        radius += Random.Range(1, 20) * mass * GManager.gConstant ;

        //initialVelocity += new Vector3(0, Random.Range(20 / mass, 40 / mass), 0);
        velocity = initialVelocity;
    }
    private void Update()
    {
        foreach (GalacticBody galacticBody in GManager.galacticBodies)
        {
            if (galacticBody != this)
            {
                Gravity.Attract(this, galacticBody);
            }

        }

    }

    private void OnEnable()
    {
        rb = this.GetComponent<GalacticBody>();
        if (GManager.galacticBodies == null)
        {
            GManager.galacticBodies = new List<GalacticBody>();
        }

        GManager.galacticBodies.Add(rb);
    }

    private void OnDestroy()
    {
        GManager.galacticBodies.Remove(this);
    }

    public void UpdateMass()
    {
        //mass = (radius * radius / GManager.gConstant) * Time.deltaTime;
        AddForce();
        transform.localPosition -= velocity * Time.deltaTime;
    }

    public void AddForce()
    {
        velocity += (force * mass) / GManager.gConstant * Time.deltaTime;
    }

    #region noob addition
    public void SpawnChildren()
    {
        var n = 0;
        var distQuotient = 1f;
        switch (EntityType)
        {
            case EntityType.Universe:
                n = Random.Range(GManager.SpawnSettings.minGalaxy, GManager.SpawnSettings.maxGalaxy);
                break;
            case EntityType.Galaxy:
                n = Random.Range(GManager.SpawnSettings.minSolarSystem, GManager.SpawnSettings.maxSolarSystem);
                distQuotient = GManager.SpawnSettings.maxDistanceGalaxyK;
                break;
            case EntityType.SolarSystem:
                n = Random.Range(GManager.SpawnSettings.minPlanet, GManager.SpawnSettings.maxPlanet);
                distQuotient = GManager.SpawnSettings.maxDistanceSolarSystemK;
                break;
            case EntityType.Planet:
                n = Random.Range(
                    Random.Range(GManager.SpawnSettings.minMoonLow, GManager.SpawnSettings.minMoonHigh),
                    Random.Range(GManager.SpawnSettings.maxMoonLow, GManager.SpawnSettings.maxMoonHigh)
                    );
                distQuotient = GManager.SpawnSettings.maxDistancePlanetK;
                break;
            case EntityType.Moon:
                distQuotient = GManager.SpawnSettings.maxDistanceMoonK;
                break;

        }


        for (int i = 0; i < n; i++)
        {
            var tmpObj = Instantiate(ChildPrefab);
            tmpObj.transform.SetParent(transform);
            tmpObj.transform.SetPositionAndRotation(GetRandomVector3(GManager.SpawnSettings.minDistacne, GManager.SpawnSettings.maxDistance / distQuotient), Quaternion.Euler(rotationVector));
            var gb = tmpObj.GetComponent<GalacticBody>();
            children.Add(gb);
            if (EntityType != EntityType.Moon)
            {
                gb.SpawnChildren();
            }
        }
    }

    public List<GalacticBody> GetGalacticBodyByType(EntityType type)
    {
        if (children.Count > 0)
        {
            if (children[0].EntityType == type)
            {
                return children;

            }

        }
        return null;

    }


    public Vector3 GetRandomVector3(float min, float max)
    {
        return new Vector3(GetRandomNormal(min, max), GetRandomNormal(min, max), GetRandomNormal(min, max));
    }

    //kind of normal distribution between -max and max and delta between min and -min
    public float GetRandomNormal(float min, float max)
    {
        return Random.Range(Random.Range(-max, -min), Random.Range(min, max));

    }


    #endregion

}
