using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity
{
    public static List<GalacticBody> galacticBodies = new List<GalacticBody>();
    public float gConstant = 6.67430f * Mathf.Pow(10, -11);

    void Start()
    {
        //gConstant += 6.67430f * Mathf.Pow(10, -11);
    }

    public void Attract(GalacticBody gBody, GalacticBody gRB)
    {
        Vector3 direction = (gBody.transform.position - gRB.transform.position).normalized;
        float distance = direction.magnitude;

        float forceMag = (gConstant * (gBody.mass * gRB.mass) / Mathf.Pow(distance, 2));
        gBody.force = direction.normalized * forceMag;


        float tempDist = Vector3.Distance(gBody.transform.position, gRB.transform.position);
        if (tempDist > 1f)
        {
            gBody.UpdateMass();
            
        }
        else
        {
            if (gBody.mass > gRB.mass)
            {
                gBody.radius += gRB.mass;
                gBody.velocity -= gRB.velocity;
                gRB.collided = true;
            }
            else
            {
                gRB.radius += gBody.mass;
                gRB.velocity -= gBody.velocity;
                gBody.collided = true;
            }
        }

    }
}
