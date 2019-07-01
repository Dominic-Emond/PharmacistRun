using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantsEffects : MonoBehaviour
{
    public float GrabTime;
    public float EffectTime;
    public int Type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlantEffect()
    {
        if (Type == 1)
        {
            PlantEffectSpeedBoost();
        }
    }
    void PlantEffectSpeedBoost()
    {

    }
}
