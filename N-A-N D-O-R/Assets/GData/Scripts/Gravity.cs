using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity
{
    public static List<GalacticBody> galacticBodies = new List<GalacticBody>();
    public float gConstant = 6.67430f;

    void Start()
    {
        //gConstant += 6.67430f * Mathf.Pow(10, -11);
    }

    public void Attract(GalacticBody gBody, GalacticBody gRB)
    {
        Vector3 direction = (gBody.transform.position - gRB.transform.position).normalized;
        float distance = direction.magnitude;

        float forceMag = gConstant * (gBody.mass * gRB.mass) / Mathf.Pow(distance, 2);
        gBody.force = direction.normalized * forceMag;

        gBody.UpdateMass();

        if(gBody.transform.position == gRB.transform.position)
        {
            if (gBody.mass > gRB.mass)
            {
                gBody.mass += gRB.mass / gConstant;
                gBody.velocity -= gRB.velocity / gConstant;
                gRB.collided = true;
            }
            else
            {
                gRB.mass += gBody.mass / gConstant;
                gRB.velocity -= gBody.velocity / gConstant;
                gBody.collided = true;
            }
        }

    }
}
