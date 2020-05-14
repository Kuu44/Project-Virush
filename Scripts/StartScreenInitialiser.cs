using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreenInitialiser : MonoBehaviour
{
    public Text killCount, virusGenerated, clickCount;
    public Transform boundaries;

    public GameObject startButton,
        statsOnButton, statsOffButton,
        helpOnButton, helpOffButton,
        creditsOnButton, creditsOffButton;
    public Sprite[] virusSprites;

    public GameObject[] virusList;

    float sizeX, sizeY;

    
    
    void Awake()
    {
        Game.ClearAll();

        Game.Stats.currentLevel = 0;
        sizeX = Camera.main.orthographicSize;
        sizeY = Camera.main.aspect * Camera.main.orthographicSize;

        if (boundaries)
        {
            SetBoundaries(sizeX, sizeY);
        }
        RandomizeButtonAppearance();
        InitialiseViruses();
        UpdateBoxes();
    }
    void SetBoundaries(float x, float y)
    {
        Transform top = boundaries.GetChild(0);
        Transform right = boundaries.GetChild(1);
        Transform bottom = boundaries.GetChild(2);
        Transform left = boundaries.GetChild(3);
        int a = 10;
        SetColliderValue(top, 0, x + a / 2.0f, 2 * y, 1 + a);
        SetColliderValue(right, -y - a / 2.0f, 0, 1 + a, 2 * x);
        SetColliderValue(left, y + a / 2.0f, 0, 1 + a, 2 * x);
        SetColliderValue(bottom, 0, -x - a / 2.0f, 2 * y, 1 + a);
    }
    void SetColliderValue(Transform quad, float x, float y, float sX, float sY)
    {
        BoxCollider2D collider = quad.GetComponent<BoxCollider2D>();

        quad.localPosition = new Vector3(x, y, 0);
        collider.size = new Vector2(sX, sY);
    }

    List<int> notTakens;
    void RandomizeButtonAppearance()
    {
        notTakens = new List<int>();
        for (int i = 0; i < virusSprites.Length; i++) 
        {
            notTakens.Add(i);
        }

        Sprite startbuttonsprite = virusSprites[GetRandomIndex()];
        Sprite statbuttonsprite = virusSprites[GetRandomIndex()];
        Sprite helpbuttonsprite = virusSprites[GetRandomIndex()];
        Sprite creditsbuttonsprite = virusSprites[GetRandomIndex()];

        startButton.GetComponent<Image>().sprite = startbuttonsprite;
        statsOnButton.GetComponent<Image>().sprite = statbuttonsprite;
        statsOffButton.GetComponent<Image>().sprite = statbuttonsprite;
        helpOnButton.GetComponent<Image>().sprite = helpbuttonsprite;
        helpOffButton.GetComponent<Image>().sprite = helpbuttonsprite;
        creditsOnButton.GetComponent<Image>().sprite = creditsbuttonsprite;
        creditsOffButton.GetComponent<Image>().sprite = creditsbuttonsprite;
    }
    int GetRandomIndex()
    {
        int index = notTakens[Random.Range(0, notTakens.Count)];
        notTakens.Remove(index);
        return index;
    }
    
    public void UpdateBoxes()
    {
        if (!PlayerPrefs.HasKey("totalKills") ||
            !PlayerPrefs.HasKey("virusesGenerated") ||
            !PlayerPrefs.HasKey("clickCount"))
        {
            ClearPlayerPrefs();
        }

        killCount.text = PlayerPrefs.GetInt("totalKills").ToString();
        virusGenerated.text = PlayerPrefs.GetInt("virusesGenerated").ToString();
        clickCount.text = PlayerPrefs.GetInt("clickCount").ToString();

        PlayerPrefs.Save();
    }
    void InitialiseViruses()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject virus = virusList[Random.Range(0, virusList.Length)];

            Vector3 position = new Vector3(
                Random.Range(-sizeX * 0.5f, +sizeX * 0.5f),
                Random.Range(-sizeY * 0.5f, +sizeY * 0.5f),
                0
                );
            Vector3 rotation = new Vector3(0, 0, Random.Range(0f, 360f));

            Virus newVirus = Instantiate(virus,
                position, Quaternion.Euler(rotation)).GetComponent<Virus>();
            newVirus.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.value, Random.value).normalized
            * Game.prefabs.speed, ForceMode2D.Impulse);

            Game.SceneObjects.viruses.Add(newVirus);
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
    public void ClearPlayerPrefs()
    {
        //PowerUps
        PlayerPrefs.SetInt("powerUpBomb", 1);
        PlayerPrefs.SetInt("powerUpViroStat", 1);
        PlayerPrefs.SetInt("powerUpSoap", 1);
        PlayerPrefs.SetInt("SoundON", 1);
        PlayerPrefs.SetInt("MusicON", 0);
        //Stats
        PlayerPrefs.SetInt("totalKills", 0);
        PlayerPrefs.SetInt("virusesGenerated", 0);
        PlayerPrefs.SetInt("clickCount", 0);
    }
}
