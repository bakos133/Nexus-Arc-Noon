using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    GalacticBody rb;

    private void OnEnable()
    {
        rb = this.GetComponent<GalacticBody>();
        if (GManager.galacticBodies == null)
        {
            GManager.galacticBodies = new List<Gravity>();
        }

        GManager.galacticBodies.Add(this);
    }

    void CollisionEnter(Collision collision)
    {
        GalacticBody tempG = collision.transform.GetComponent<GalacticBody>();
        if (tempG.mass > this.GetComponent<GalacticBody>().mass)
        {
            tempG.mass += this.GetComponent<GalacticBody>().mass / GManager.gConstant * Time.deltaTime;
            tempG.velocity -= this.GetComponent<GalacticBody>().velocity / GManager.gConstant * Time.deltaTime;
            Destroy(tempG.gameObject);
        }
        else
        {
            this.GetComponent<GalacticBody>().mass += tempG.mass / GManager.gConstant * Time.deltaTime;
            this.GetComponent<GalacticBody>().velocity -= tempG.velocity / GManager.gConstant * Time.deltaTime;
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        GManager.galacticBodies.Remove(this);
    }

    private void Update()
    {
        foreach (Gravity galacticBody in GManager.galacticBodies)
        {
            if (galacticBody != this)
                Attract(galacticBody);

        }

    }

    void Attract(Gravity gBody)
    {
        GalacticBody gRB = gBody.GetComponent<GalacticBody>();

        Vector3 direction = (rb.transform.position - gRB.transform.position).normalized;
        float distance = direction.magnitude;

        float forceMag = GManager.gConstant * (rb.mass * gRB.mass) / Mathf.Pow(distance, 2);
        rb.force = direction.normalized * forceMag * Time.deltaTime;

        gRB.UpdateMass();
    }


}
