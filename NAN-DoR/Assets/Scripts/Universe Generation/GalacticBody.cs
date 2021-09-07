using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticBody : MonoBehaviour
{
    Gravity Gravity = new Gravity();

    public float surfaceG;
    public Vector3 force;
    public float mass;
    public Vector3 velocity;
    public Vector3 initialVelocity;

    [Range(0.1f,1000f)]
    public float pRadius;
    public float gRadius;
    const float MAX_RADIUS = 1000f;

    public bool collided = false;

    Rigidbody rb;
    GalacticBody rbGal;

    [SerializeField]
    EntityType EntityType;

    [SerializeField]
    GalacticBody childPrefab = null;

    Vector3 rotationVector => new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));

    List<GalacticBody> children = new List<GalacticBody>();

    private void Start()
    {
        transform.localScale = myScale(transform.localScale);
        mass = myMass(mass);
        rb.mass = mass;
        velocity = initialVelocity;
    }
    public void SpawnChildren()
    {
        childPrefab = this;
        var n = 0;
        var distQuotient = 1f;
        switch (EntityType)
        {
            case EntityType.Universe:
                pRadius = Random.Range(100f, 1000f);
                n = Random.Range(uniGen.SpawnSettings.minG, uniGen.SpawnSettings.maxG);
                break;
            case EntityType.Galaxy:
                n = Random.Range(uniGen.SpawnSettings.minS, uniGen.SpawnSettings.maxS);
                distQuotient = uniGen.SpawnSettings.maxDistGK;
                break;
            case EntityType.SolarSystem:
                n = Random.Range(uniGen.SpawnSettings.minP, uniGen.SpawnSettings.maxP);
                distQuotient = uniGen.SpawnSettings.maxDistSK;
                break;
            case EntityType.Planet:
                n = Random.Range(uniGen.SpawnSettings.minM, uniGen.SpawnSettings.maxM);
                distQuotient = uniGen.SpawnSettings.maxDistPK;
                break;
            case EntityType.Moon:
                distQuotient = uniGen.SpawnSettings.maxDistMK;
                break;
        }

        for (int i = 0; i < n; i++)
        {
            var tmpObj = Instantiate(childPrefab, transform.position + GetRandomVector3(uniGen.SpawnSettings.minDist, uniGen.SpawnSettings.maxDist / distQuotient), Quaternion.Euler(rotationVector));//Change this so the system decides which child prefab it wants for itself. Want system to know which comes next.
            var gb = tmpObj.GetComponent<GalacticBody>();
            gb.pRadius = Random.Range(pRadius*0.8f, pRadius*0.3f);
            gb.EntityType = gb.EntityType + 1;
            gb.name = EntityType.ToString();
            children.Add(gb);
            //gb.transform.parent = GameObject.Find("Universe").transform;
            gb.initialVelocity = gb.transform.right + gb.force * gb.mass * Time.deltaTime;
            if (gb.EntityType != EntityType.Moon)
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

    private void Update()
    {

        //For testing
        transform.localScale = myScale(transform.localScale);
        mass = myMass(mass);
        rb.mass = mass;

        foreach (GalacticBody galacticBody in Gravity.galacticBodies)
        {
            Vector3 dir = (this.transform.position - galacticBody.transform.position);
            float distance = dir.magnitude;
            if (galacticBody != this)
            {
                Gravity.Attract(galacticBody, this);
            }
        }


    }



    private void OnEnable()
    {
        if (Gravity.galacticBodies == null)
        {
            Gravity.galacticBodies = new List<GalacticBody>();
        }

        rb = GetComponent<Rigidbody>();
        Gravity.galacticBodies.Add(this);
    }

    private void OnDisable()
    {
        Gravity.galacticBodies.Remove(this);
    }

    public Vector3 myScale(Vector3 rad)
    {
        rad = new Vector3(pRadius, pRadius, pRadius);

        return rad;
    }

    public float myGRad(float pRad)
    {
        surfaceG = pRad * pRad / Gravity.gConstant;
        gRadius = surfaceG * 2f;

        return surfaceG;
    }

    public float myMass(float mM)
    {
        mM = myGRad(pRadius) / Gravity.gConstant;

        return mM;
    }

    public void OnCollisionStay(Collision collision)
    {
        GalacticBody tempG = collision.transform.GetComponent<GalacticBody>();
        if (mass > 0 && tempG.mass > mass)
        {
            tempG.mass += mass / Gravity.gConstant * Time.deltaTime;
            mass -= mass / Gravity.gConstant * Time.deltaTime;
        } else if (mass <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    public void UpdateMass()
    {
        AddForce();
    }

    public void AddForce()
    {
        velocity -= force / mass * Gravity.gConstant * 0.00001f;
        rb.AddForce(velocity);
    }

}
