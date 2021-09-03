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
    public bool collided = false;

    Rigidbody rb;
    GalacticBody rbGal;

    [SerializeField]
    EntityType EntityType;

    [SerializeField]
    GalacticBody childPrefab = null;

    Vector3 rotationVector => new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15));

    List<GalacticBody> children = new List<GalacticBody>();


    public void SpawnChildren()
    {
        var n = 0;
        var distQuotient = 1f;
        switch (EntityType)
        {
            case EntityType.Universe:
                n = Random.Range(GManager.SpawnSettings.minG, GManager.SpawnSettings.maxG);
                break;
            case EntityType.Galaxy:
                n = Random.Range(GManager.SpawnSettings.minS, GManager.SpawnSettings.maxS);
                distQuotient = GManager.SpawnSettings.maxDistGK;
                break;
            case EntityType.SolarSystem:
                n = Random.Range(GManager.SpawnSettings.minP, GManager.SpawnSettings.maxP);
                distQuotient = GManager.SpawnSettings.maxDistSK;
                break;
            case EntityType.Planet:
                n = Random.Range(GManager.SpawnSettings.minM, GManager.SpawnSettings.maxM);
                distQuotient = GManager.SpawnSettings.maxDistPK;
                break;
            case EntityType.Moon:
                distQuotient = GManager.SpawnSettings.maxDistMK;
                break;
        }

        for (int i = 0; i < n; i++)
        {
            var tmpObj = Instantiate(childPrefab);
            tmpObj.transform.SetParent(transform);
            tmpObj.transform.SetPositionAndRotation(GetRandomVector3(GManager.SpawnSettings.minDist, GManager.SpawnSettings.maxDist / distQuotient), Quaternion.Euler(rotationVector));
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

    private void Update()
    {
        while (!collided)
        {
            foreach (GalacticBody galacticBody in Gravity.galacticBodies)
            {
                if (galacticBody != this)
                {
                    Gravity.Attract(this, galacticBody);
                    return;
                }
            }
        } Destroy(this.gameObject);

    }

    private void OnEnable()
    {
        radius = transform.localScale.sqrMagnitude;
        surfaceG = radius * 2;
        mass = surfaceG * radius * radius;

        //initialVelocity = new Vector3(Random.Range(0, Gravity.gConstant *mass), Random.Range(0, Gravity.gConstant * mass), Random.Range(0, Gravity.gConstant * mass));

        velocity = initialVelocity;

        rb = GetComponent<Rigidbody>();
        rbGal = this.GetComponent<GalacticBody>();
        if (Gravity.galacticBodies == null)
        {
            Gravity.galacticBodies = new List<GalacticBody>();
        }

        Gravity.galacticBodies.Add(rbGal);
    }

    private void OnDisable()
    {
        Gravity.galacticBodies.Remove(this);
    }

    public void UpdateMass()
    {
        mass = surfaceG * radius * radius;
        AddForce();
    }

    public void AddForce()
    {
        velocity += force / mass;
        transform.Rotate(Vector3.up * velocity.magnitude * mass * Time.deltaTime);
        transform.position -= velocity;
    }

}
