using System.Collections;
using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class MoverDecision : Decision
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override float[] Decide(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        throw new System.NotImplementedException();
    }

    public override List<float> MakeMemory(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        throw new System.NotImplementedException();
    }
}
