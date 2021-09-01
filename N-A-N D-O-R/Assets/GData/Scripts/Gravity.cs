using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity
{

    public void Attract(GalacticBody gBody, GalacticBody gRB)
    {

        Vector3 direction = (gBody.transform.position - gRB.transform.position).normalized;
        float distance = direction.magnitude;

        float forceMag = GManager.gConstant * (gBody.mass * gRB.mass) / Mathf.Pow(distance, 2);
        gBody.force = direction.normalized * forceMag * Time.deltaTime;

        gRB.UpdateMass();
    }


}
