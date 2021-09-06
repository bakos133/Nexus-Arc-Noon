using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity
{
    public static List<GalacticBody> galacticBodies = new List<GalacticBody>();
    public float gConstant = 6.67430f;

    public void Attract(GalacticBody gBody, GalacticBody gRB)
    {
        Vector3 direction = (gBody.transform.position - gRB.transform.position).normalized;
        float distance = direction.magnitude;

        float forceMag = gConstant * (gBody.mass * gRB.mass) / Mathf.Pow(distance, 2);
        gBody.force = direction.normalized * forceMag / gConstant;

        gBody.AddForce();
    }
}