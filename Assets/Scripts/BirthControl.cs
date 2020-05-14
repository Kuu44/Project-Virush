using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthControl : MonoBehaviour
{
    public Initialiser initial;
    // Start is called before the first frame update

    public void StartNaughtiness()
    {
        StartCoroutine(CheckNaughtiness());
    }
    public void StopNaughtiness()
    {
        StopAllCoroutines();
    }
    public IEnumerator CheckNaughtiness()
    {
        float sizeX = Game.SceneObjects.fieldDimensions.x * 2;
        float sizeY = Game.SceneObjects.fieldDimensions.y * 2;
        for (; ; )
        {
            yield return new WaitForSeconds(Game.Stats.abstinence);

            int n = GetExponentialFactor();
            int vn = Game.SceneObjects.viruses.Count;
            for (int i = 0; i < vn; i += n)
            {
                Vector3 position = new Vector3(
                    Random.Range(-Game.SceneObjects.fieldDimensions.x, +Game.SceneObjects.fieldDimensions.x),
                    Random.Range(-Game.SceneObjects.fieldDimensions.y, +Game.SceneObjects.fieldDimensions.y),
                    0
                    );
                Vector3 rotation = new Vector3(0, 0, Random.Range(0f, 360f));

                GameObject newVirus = Instantiate(Game.prefabs.virus,
                    position, Quaternion.Euler(rotation), Game.SceneObjects.virusHolder.transform);
                newVirus.GetComponent<Virus>().GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.value, Random.value)
                * Game.prefabs.speed, ForceMode2D.Impulse);

                Game.SceneObjects.viruses.Add(newVirus.GetComponent<Virus>());
            }
            for (int i = 0; i < Game.SceneObjects.viruses.Count; i++)
            {
                if (Game.SceneObjects.viruses[i].transform.position.x > sizeX + 100 ||
                    Game.SceneObjects.viruses[i].transform.position.x < -sizeX - 100 ||
                    Game.SceneObjects.viruses[i].transform.position.y > sizeY + 100 ||
                    Game.SceneObjects.viruses[i].transform.position.y < -sizeY - 100)
                {
                    Virus stray = Game.SceneObjects.viruses[i];
                    Game.SceneObjects.viruses.Remove(stray);
                    Destroy(stray.gameObject);
                }
            }
        }
    }
    int GetExponentialFactor()
    {
        float difficulty = Game.Stats.difficulty * 0.5f;
        int n = Mathf.RoundToInt(Mathf.Log(Game.SceneObjects.viruses.Count, difficulty));
        if (n < 1) n = 1;
        return n;
    }
}
