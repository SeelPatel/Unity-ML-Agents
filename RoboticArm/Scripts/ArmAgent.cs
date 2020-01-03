using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using MLAgents;
using UnityEngine;

public class ArmAgent : Agent
{
    
    public GameObject a1, a2, target, endpoint;
    private TargetBall target_detector;

    private float dist_to_target;

    public float rotationSpeed, angleLimit;
    
    public override void InitializeAgent()
    {
        target_detector = target.GetComponent<TargetBall>();
        dist_to_target = Vector3.Distance(target.transform.position, endpoint.transform.position);
    }

    public override void CollectObservations()
    {
        AddVectorObs(a1.transform.rotation.y);
        AddVectorObs(a1.transform.rotation.z);
        
        AddVectorObs(a2.transform.rotation.y);
        AddVectorObs(a2.transform.rotation.x);

        AddVectorObs((target.transform.position - endpoint.transform.position).normalized);
        
    }

    public int get_sign(float num)
    {
        if (num > 0)
        {
            return 1;
        }

        return -1;
    }

    public Vector3 get_vector_signs(Vector3 input)
    {
        return new Vector3(get_sign(input.x), get_sign(input.y), get_sign(input.z));
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 a1_rotation = new Vector3(0,0,0);
        Vector3 a2_rotation = new Vector3(0,0,0);
        if (vectorAction[0] > 0.5)
        {
            a1_rotation += new Vector3(0,vectorAction[0],0);
        }
        if (vectorAction[1] > 0.5)
        {
            a1_rotation += new Vector3(0,0, vectorAction[1]);
        }
        if (vectorAction[2] > 0.5)
        {
            a1_rotation += new Vector3(0,-vectorAction[2],0);
        }
        if (vectorAction[3] > 0.5)
        {
            a1_rotation += new Vector3(0,0, -vectorAction[3]);
        }
        if (vectorAction[4] > 0.5)
        {
            a2_rotation += new Vector3(0, vectorAction[4], 0);
        }
        if (vectorAction[5] > 0.5)
        {
            a2_rotation += new Vector3(vectorAction[5], 0, 0);
        }
        if (vectorAction[6] > 0.5)
        {
            a2_rotation += new Vector3(0, -vectorAction[6], 0);
        }
        if (vectorAction[7] > 0.5)
        {
            a2_rotation += new Vector3(-vectorAction[7], 0, 0);
        }


        a1.transform.Rotate(a1_rotation * rotationSpeed);
        a2.transform.Rotate(a2_rotation * rotationSpeed);


        if (target_detector.hit)
        {
            SetReward(5000.0f);
            Done();
        }
        else
        {
            
            float new_dist = Vector3.Distance(target.transform.position, endpoint.transform.position);

            if (new_dist < dist_to_target)
            {
                SetReward(5.0f);
                dist_to_target = new_dist;
            }
        }
    }

    public override void AgentReset()
    {
        float targetX = Random.Range(-0.25f, 0.25f);
        float targetY = Random.Range(0.1f, 0.3f);
        float targetZ = Random.Range(-0.25f, 0.25f);

        target.transform.localPosition = new Vector3(targetX, targetY, targetZ);
        target_detector.hit = false;
    }
}
