using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusExplosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(selfDestroy());
    }

    IEnumerator selfDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
