using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider slider;

    void Start()
    {        
        SetDifficulty();
    }
    public void SetDifficulty()
    {
        Game.Stats.difficulty = Mathf.RoundToInt(slider.value);
    }    

    

    /*Immunosuppressed!
Overwhelmed!
Bioterrorised!
Chronic-ised!
OUTBREAK!
Influxed!
Innundated!
ANNEXED!
Viraided!
Virushed!
Incapacitated!*/
}
