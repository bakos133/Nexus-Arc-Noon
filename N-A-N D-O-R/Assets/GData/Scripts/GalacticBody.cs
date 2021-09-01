﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalacticBody : MonoBehaviour
{
    //Variable                          //Call command
    public float surfaceG;              //GalacticBody.surfaceG
    public Vector3 force;               //GalacticBody.force
    public float mass;                  //GalacticBody.mass
    public Vector3 velocity;            //GalacticBody.velocity
    public Vector3 initialVelocity;     //GalacticBody.initialVelocity
    public float radius;                //GalacticBody.radius

    private void Start()
    {
        radius = transform.localScale.magnitude;
        surfaceG = radius;
        mass = (surfaceG * radius * radius / GManager.gConstant) * Time.deltaTime;

        radius += Random.Range(1, 20) * mass * GManager.gConstant * Time.deltaTime;

        //initialVelocity += new Vector3(0, Random.Range(20 / mass, 40 / mass), 0);
        velocity = initialVelocity;
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

}
