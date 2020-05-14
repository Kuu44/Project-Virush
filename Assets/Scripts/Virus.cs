using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : MonoBehaviour
{
    public Rigidbody2D self;
    public Transform body;
    void Awake()
    {
        body.localScale = Vector3.zero;
        self = GetComponent<Rigidbody2D>();
        StartCoroutine(grow());
        Game.Stats.virusesGenerated++;    
    } 

    IEnumerator grow()
    {
        for(int i = 1; i<=10; i++)
        {
            yield return new WaitForSeconds(0.1f/(i*1.0f));
            body.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }
    }
}
