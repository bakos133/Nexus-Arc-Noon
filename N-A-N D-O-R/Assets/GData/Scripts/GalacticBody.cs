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

    GalacticBody rb;

    private void Start()
    {
        radius = transform.localScale.x;
        surfaceG = radius * 2;
        mass = surfaceG * radius * radius / Gravity.gConstant;

        //radius += Random.Range(1, 20) * mass / Gravity.gConstant;

        initialVelocity = new Vector3(Random.Range(0.001f, 0.01f) * mass, Random.Range(0.001f, 0.01f) * mass, Random.Range(0.001f, 0.01f) * mass);
        velocity = initialVelocity;
    }
    private void Update()
    {
        if (!collided)
        {
            foreach (GalacticBody galacticBody in Gravity.galacticBodies)
            {
                if (galacticBody != this)
                {
                    Gravity.Attract(this, galacticBody);
                    return;
                }

            }
        }
        else
        {
            Destroy(this.gameObject);
        }


    }

    private void OnEnable()
    {
        rb = this.GetComponent<GalacticBody>();
        if (Gravity.galacticBodies == null)
        {
            Gravity.galacticBodies = new List<GalacticBody>();
        }

        Gravity.galacticBodies.Add(rb);
    }

    private void OnDestroy()
    {
        Gravity.galacticBodies.Remove(this);
    }

    public void UpdateMass()
    {
        mass = surfaceG * radius * radius / Gravity.gConstant;
        AddForce();
        transform.position -= velocity * Time.deltaTime;
    }

    public void AddForce()
    {
        velocity += force / (mass * Gravity.gConstant) * Time.deltaTime;
    }

}
