using System;
using System.Collections;
using System.Collections.Generic;
using MLAgents;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class MoverAgent : Agent
{
    
    [Header("Specific to Mover")]
    public GameObject cube;

    private ResetParameters m_ResetParams;

    private float[] raycast_directions = {-50.0f,-25.0f, 0.0f,  25.0f, 50.0f};

    private Rigidbody _Rigidbody;

    private bool done = false;

    private float dist_to_cube = 0.0f;

    public float rotateSpeed;
    public float moveSpeed;
    

    public override void InitializeAgent()
    {
        var academy = FindObjectOfType<Academy>();
        m_ResetParams = academy.resetParameters;
        SetResetParameters();

        _Rigidbody = gameObject.GetComponent<Rigidbody>();
        dist_to_cube = Vector3.Distance(transform.localPosition, cube.transform.localPosition);

    }

    private void Update()
    {

    }

    private void OnDrawGizmos()
    {
        Vector3 forward = transform.forward;
        for (int i = 0; i < raycast_directions.Length; i++)
        {
            Quaternion angle = Quaternion.AngleAxis(raycast_directions[i],new Vector3(0,1,0));
            Vector3 dir = angle * forward;
            Ray ray = new Ray(transform.position, dir);
            
            Gizmos.DrawRay(ray);
        }
    }

    public override void CollectObservations()
    {
        AddVectorObs(transform.position.x);
        AddVectorObs(transform.position.z);
        AddVectorObs(transform.rotation.y);
        
        Vector3 forward = transform.forward;
        for (int i = 0; i < raycast_directions.Length; i++)
        {
            Quaternion angle = Quaternion.AngleAxis(raycast_directions[i],new Vector3(0,1,0));
            Vector3 dir = angle * forward;

            Ray ray = new Ray(transform.position, dir);

            RaycastHit hit;
            Physics.Raycast(ray, out hit, 15.0f);
           

            if (hit.collider != null && hit.collider.CompareTag("MoverTarget"))
            {
                AddVectorObs(1);
                print(hit.distance);
            }
            else
            {
                
                AddVectorObs(0);
            }
         
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if (brain.brainParameters.vectorActionSpaceType == SpaceType.Continuous)
        {

            Vector3 rotateDir = Vector3.up * Mathf.Clamp(vectorAction[0], -1, 1);
            Vector3 moveDir = transform.forward * Mathf.Clamp(vectorAction[1],-1,1);

            

            transform.Rotate(rotateDir * rotateSpeed);
            _Rigidbody.AddForce(moveDir * moveSpeed);
        }

        Vector3 pos = transform.localPosition;

        if (done)
        {
            SetReward(50.0f);
            Done();
        }
        else if(Math.Abs(pos.x) > 0.47 || Math.Abs(pos.z) > 0.47)
        {
            SetReward(-25.0f);
            Done();
        }
        else
        {
            
            SetReward(-0.1f);
            
            float newDist = Vector3.Distance(transform.localPosition, cube.transform.localPosition);
            if (newDist < dist_to_cube)
            {
                AddReward(0.1f);
                dist_to_cube = newDist;
            }

            
        }
    }

    public override void AgentReset()
    {
        cube.transform.localPosition = new Vector3(Random.Range(-0.45f, 0.45f), 1.75f, Random.Range(-0.45f, 0.45f));
        this.transform.localPosition = new Vector3(Random.Range(-0.45f, 0.45f), 1.75f, Random.Range(-0.45f, 0.45f));
        
        
        done = false;
    }


    public void SetCube()
    {
        
    }

    public void SetResetParameters()
    {
        SetCube();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MoverTarget"))
        {
            done = true;
        }
    }
}
