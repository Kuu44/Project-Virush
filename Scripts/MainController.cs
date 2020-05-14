using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainController : MonoBehaviour
{
    public AudioSource bgm;
    public GameObject winBox, loseBox,
        soapButton, viroStatButton;
    public GameObject[] virusList, explosionList;

    public BirthControl birthControl;

    public Text loseText;

    public Text loseDes;
    public Text levelText;

    static string[] LoseWords = {"Immunosuppressed!", "Overwhelmed!",
        "Bioterrorised!", "Chronic-ised!", "OUTBREAK!", "Influxed!",
    "Innundated!", "ANNEXED!", "Viraided!", "Virushed!", "Incapacitated!"};

    void Awake()
    {
        Game.SceneObjects.loseDes = loseDes;
        Game.SceneObjects.loseText = loseText;
        Game.SceneObjects.winBox = winBox;
        Game.SceneObjects.loseBox = loseBox;

    }
    private void Start()
    {
        if (levelText != null)
            levelText.text = Game.Stats.currentLevel.ToString();

        if (PlayerPrefs.GetInt("MusicON", 0) == 1)
        {
            bgm.time = Game.Stats.audioTime;
        }
        else
        {
            if (bgm.isPlaying)
                bgm.Stop();
        }
    }
    
    #region Menu
    public void loadMainScene()
    {
        StartCoroutine(loadScene("title"));
    }
    public void loadLevelFresh()
    {
        Game.HardClear();
        loadNextLevel();
    }
    public void loadNextLevel()
    {
        Game.Stats.currentLevel++;

        int index = (Game.Stats.currentLevel - 1) % virusList.Length;
        Game.prefabs.virus = virusList[index];
        Game.prefabs.explosion = explosionList[index];

        float sizeX = Camera.main.orthographicSize;
        float sizeY = Camera.main.aspect * Camera.main.orthographicSize;

        Game.Stats.loseNumber = Mathf.RoundToInt(sizeX * sizeY * 0.02f);
        IncreaseDifficulty();

        print("This is Level: " + Game.Stats.currentLevel + ". Difficulty: " + Game.Stats.difficulty
            + "With virus no: " + index);

        StartCoroutine(loadScene("1"));
    }
    void IncreaseDifficulty()
    {
        if (Game.Stats.currentLevel < 7)
        {
            Game.Stats.difficulty += 4;
            return;
        }
        else if (Game.Stats.currentLevel < 13)
        {
            Game.Stats.difficulty += 3;
            return;
        }
        else if (Game.Stats.currentLevel < 19)
        {
            Game.Stats.difficulty += 2;
            return;
        }
        else
        {
            Game.Stats.difficulty++;
        }
    }
    IEnumerator loadScene(string scene)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        while (!load.isDone || Time.frameCount == 3000)
        {
            if ((Time.frameCount == 3000))
            {
                print("failed music");
            }
            Game.Stats.audioTime = bgm.time;
            yield return null;
        }
    }
    public void UpdateStatsUI()
    {
        UpdateStats();
    }
    public static void UpdateStats()
    {
        int kills = PlayerPrefs.GetInt("totalKills");
        PlayerPrefs.SetInt("totalKills", kills + Game.Stats.killCount);

        int virusesGen = PlayerPrefs.GetInt("virusesGenerated");
        PlayerPrefs.SetInt("virusesGenerated", virusesGen + Game.Stats.virusesGenerated);

        int clickCount = PlayerPrefs.GetInt("clickCount");
        PlayerPrefs.SetInt("clickCount", clickCount + Game.Stats.clicks);
    }
    #endregion
    #region GamePlay 
    public void Pause()
    {
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        Time.timeScale = 1f;
    }
    public static void WinGame()
    {
        Game.SceneObjects.winBox.SetActive(true);
    }
    public static void LoseGame()
    {
        Game.SceneObjects.loseDes.text = "YOU KILLED " + Game.Stats.killCount.ToString() + " VIRUSES! AND REACHED ROUND " + Game.Stats.currentLevel.ToString() + "!";
        Game.SceneObjects.loseText.text = LoseWords[Random.Range(0, LoseWords.Length)];
        Game.SceneObjects.loseBox.SetActive(true);
        UpdateStats();
    }
    public void SoapPowerUp()
    {
        int powerUp = PlayerPrefs.GetInt("powerUpSoap");
        if (powerUp > 0)
        {
            PlayerPrefs.SetInt("powerUpSoap", powerUp - 1);
            StartCoroutine(SoapPowerUpTimer());
        }
    }
    IEnumerator SoapPowerUpTimer()
    {
        Game.SceneObjects.PowerUpState(true, 1);
        Game.PowerUps.powerUpSoap = true;
        soapButton.GetComponent<UnityEngine.UI.Button>().enabled = false;

        yield return new WaitForSeconds(3f);

        Game.SceneObjects.PowerUpState(false, 1);
        soapButton.GetComponent<UnityEngine.UI.Button>().enabled = true;
        Game.PowerUps.powerUpSoap = false;
        StopCoroutine(SoapPowerUpTimer());
    }
    public void ViroStatPowerUp()
    {
        int powerUp = PlayerPrefs.GetInt("powerUpViroStat");
        if (powerUp > 0)
        {
            PlayerPrefs.SetInt("powerUpViroStat", powerUp - 1);
            StartCoroutine(ViroStatPowerUpTimer());
        }
    }
    IEnumerator ViroStatPowerUpTimer()
    {
        Game.SceneObjects.PowerUpState(true, 2);
        Game.PowerUps.powerUpViroStat = true;
        viroStatButton.GetComponent<UnityEngine.UI.Button>().enabled = false;
        birthControl.StopNaughtiness();

        yield return new WaitForSeconds(3f);

        Game.SceneObjects.PowerUpState(false, 2);
        birthControl.StartNaughtiness();
        viroStatButton.GetComponent<UnityEngine.UI.Button>().enabled = true;
        Game.PowerUps.powerUpViroStat = false;
        StopCoroutine(ViroStatPowerUpTimer());
    }
    public void BombPowerUp()
    {
        int powerUp = PlayerPrefs.GetInt("powerUpBomb");
        if (powerUp > 0)
        {
            Game.SceneObjects.PowerUpState(true, 0);
            Game.PowerUps.powerUpBomb = true;
        }
    }
    #endregion
}
