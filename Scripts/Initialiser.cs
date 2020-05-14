using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialiser : MonoBehaviour
{
    public Powerups powerUpController;

    public BirthControl birthControl;

    float sizeX = 50, sizeY = 50;

    public GameObject virusHolder;
    public UnityEngine.UI.Text current, kills,
        powerUpSoapNum, powerUpViroStatNum, powerUpBombNum;

    public Transform boundaries;

    void Awake()
    {
        Game.ClearAll();

        sizeX = Camera.main.orthographicSize;
        sizeY = Camera.main.aspect * Camera.main.orthographicSize;

        SetBoundaries(sizeX, sizeY);

        Game.Stats.abstinence = 40f / Game.Stats.difficulty;

        Game.controllers.powerUpController = powerUpController;

        Game.SceneObjects.powerBombNumber = powerUpBombNum;
        Game.SceneObjects.powerSoapNumber = powerUpSoapNum;
        Game.SceneObjects.powerViroStatNumber = powerUpViroStatNum;
        Game.SceneObjects.kills = kills;
        Game.SceneObjects.current = current;
        Game.SceneObjects.fieldDimensions = new Vector2(sizeX / 2, sizeY / 2);
        Game.SceneObjects.virusHolder = virusHolder;

        for (int i = 0; i < 3; i++)
        {
            Game.SceneObjects.PowerUpState(false, i);
        }

        InitialiseViruses();
    }
    private void Start()
    {
        birthControl.StartNaughtiness();
    }
    void InitialiseViruses()
    {
        int maxViruses = Mathf.RoundToInt(Mathf.Clamp(Game.Stats.difficulty * 0.5f, 2f, 15f));
        for (int i = 0; i < maxViruses; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-Game.SceneObjects.fieldDimensions.x, +Game.SceneObjects.fieldDimensions.x),
                Random.Range(-Game.SceneObjects.fieldDimensions.y, +Game.SceneObjects.fieldDimensions.y),
                0
                );
            Vector3 rotation = new Vector3(0, 0, Random.Range(0f, 360f));

            GameObject newVirus = Instantiate(Game.prefabs.virus,
                position, Quaternion.Euler(rotation), virusHolder.transform);
            newVirus.GetComponent<Virus>().GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.value, Random.value)
            * Game.prefabs.speed, ForceMode2D.Impulse);

            Game.SceneObjects.viruses.Add(newVirus.GetComponent<Virus>());
        }

        for (int i = 0; i < Game.SceneObjects.viruses.Count; i++)
        {
            Vector3 position = Game.SceneObjects.viruses[i].transform.position;
            if (position.x > sizeX + 20 || position.x < -sizeX - 20 || position.y > sizeY + 20 || position.y < -sizeY - 20)
            {
                Virus stray = Game.SceneObjects.viruses[i];
                Game.SceneObjects.viruses.Remove(stray);
                Destroy(stray.gameObject);
            }
        }
    }
    void SetBoundaries(float x, float y)
    {
        Transform top = boundaries.GetChild(0);
        Transform right = boundaries.GetChild(1);
        Transform bottom = boundaries.GetChild(2);
        Transform left = boundaries.GetChild(3);
        int a = 10;
        SetColliderValue(top, 0, x+a/2.0f, 2 * y, 1+a);
        SetColliderValue(right, -y-a/2.0f, 0, 1+a, 2 * x);
        SetColliderValue(left, y+a/2.0f, 0, 1+a, 2 * x);
        SetColliderValue(bottom, 0, -x-a/2.0f, 2 * y, 1+a);
    }
    void SetColliderValue(Transform quad, float x, float y, float sX, float sY)
    {
        BoxCollider2D collider = quad.GetComponent<BoxCollider2D>();

        quad.localPosition = new Vector3(x, y, 0);
        collider.size = new Vector2(sX, sY);
    }

}
