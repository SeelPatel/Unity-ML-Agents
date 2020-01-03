using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RoboticArm : MonoBehaviour
{
    [NotNull] public GameObject A1_END, A2_PIVOT;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        A2_PIVOT.transform.position = A1_END.transform.position;
    }
}
