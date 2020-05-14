using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    
    void Update()
    {
        transform.localScale = Vector3.one * (1.2f + 0.05f*Mathf.Sin(0.15f * Time.frameCount));
    }
}
