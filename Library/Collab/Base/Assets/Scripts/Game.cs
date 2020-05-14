using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Game
{
    public static class prefabs
    {
        public static GameObject virus;
        public static GameObject explosion;
        public static float speed
        {
            get
            {
                return Random.Range(20f, 200f);
            }
        }
    }

    public static class controllers
    {
        public static Powerups powerUpController;
        public static SoundController soundController;
    }
    public static class toggles
    {
        public static bool muted = false;
    }
    public static class Stats
    {
        public static bool notWon = true;
        public static float abstinence;
        public static int difficulty = 10;
        public static int minDifficulty = 3;
        public static int maxDifficulty = 15;
        public static int currentLevel;

        public static int loseNumber = 70;

        public static int killCount;
        public static int virusesGenerated;
        public static int clicks;
    }
    public static class SceneObjects
    {
        public static List<Virus> viruses;
        public static UnityEngine.UI.Text current;
        public static UnityEngine.UI.Text kills;
        public static UnityEngine.UI.Text powerSoapNumber;
        public static UnityEngine.UI.Text powerBombNumber;
        public static UnityEngine.UI.Text powerViroStatNumber;
        public static UnityEngine.UI.Text loseText;
        public static Vector2 fieldDimensions = new Vector2();
        public static GameObject virusHolder;
        
        public static GameObject winBox, loseBox;

        public static void PowerUpState(bool state, int index)
        {
            if (state)
            {
                controllers.powerUpController.flashIconFor(index);
            }
            else
            {
                controllers.powerUpController.stopFlashIconFor(index);
            }
            {
                int powerUpBombNum = PlayerPrefs.GetInt("powerUpBomb");
                Game.SceneObjects.powerBombNumber.text = powerUpBombNum.ToString();

                int powerUpSoapNum = PlayerPrefs.GetInt("powerUpSoap");
                Game.SceneObjects.powerSoapNumber.text = powerUpSoapNum.ToString();

                int powerUpViroStatNum = PlayerPrefs.GetInt("powerUpViroStat");
                Game.SceneObjects.powerViroStatNumber.text = powerUpViroStatNum.ToString();
            }
        }
    }
    public static class PowerUps
    {
        public static bool powerUpSoap;
        public static bool powerUpBomb;
        public static bool powerUpViroStat;
        public static void CheckPowerUpCondition()
        {
            int powerUpBombNum = PlayerPrefs.GetInt("powerUpBomb");
            int powerUpSoapNum = PlayerPrefs.GetInt("powerUpSoap");
            int powerUpViroStatNum = PlayerPrefs.GetInt("powerUpViroStat");
            int totalKills = PlayerPrefs.GetInt("totalKills") + Game.Stats.killCount;

            if (totalKills % 10 == 0)
            {
                PlayerPrefs.SetInt("powerUpBomb", powerUpBombNum + 1);
            }
            if (totalKills % 20 == 0)
            {
                PlayerPrefs.SetInt("powerUpViroStat", powerUpViroStatNum + 1);
            }
            if (totalKills % 30 == 0)
            {
                PlayerPrefs.SetInt("powerUpSoap", powerUpSoapNum + 1);
            }
        }
    }
    public static void ClearAll()
    {
        Game.Stats.notWon = true;
        Game.SceneObjects.viruses = new List<Virus>();
        Game.SceneObjects.current = null;
        Game.SceneObjects.kills = null;
    }
    public static void HardClear()
    {
        Game.Stats.killCount = 0;
        Game.Stats.virusesGenerated = 0;
        Game.Stats.clicks = 0;
        Game.Stats.currentLevel = 1;
        Game.Stats.difficulty = Game.Stats.minDifficulty;
    }
}
