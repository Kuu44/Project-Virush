using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Powerups : MonoBehaviour
{
    public Image[] pimage = new Image[3];
    public GameObject[] screenIndicator = new GameObject[3];
    bool[] flashing = new bool[3];
    public void flashIconFor(int index)
    {
        flashing[index] = true;
        screenIndicator[index].SetActive(true);
    }

    public void stopFlashIconFor(int index)
    {
        flashing[index] = false;
        screenIndicator[index].SetActive(false);
    }

    private void Update()
    {
        for (int i =0; i < 3; i++)
        {
            if (flashing[i])
            {
                Color t = pimage[i].color;
                pimage[i].color = new Color(t.r,t.g,t.b, Mathf.Sin(0.15f*Time.frameCount) * 0.5f + 1);
            }
        }
    }
}
