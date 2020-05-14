using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    public GameObject unitySphere;
    public MainController mainController;
    public Initialiser initial;

    
    void Update()
    {
        if (Game.Stats.notWon)
        {
            Game.SceneObjects.current.text = Game.SceneObjects.viruses.Count.ToString();
            Game.SceneObjects.kills.text = Game.Stats.killCount.ToString();

            //Bomb PowerUp
            if (Input.GetMouseButtonDown(0) && Game.PowerUps.powerUpBomb)
            {
                Game.Stats.clicks++;

                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,10f));
                
                int powerUp = PlayerPrefs.GetInt("powerUpBomb");
                PlayerPrefs.SetInt("powerUpBomb", powerUp - 1);
                Game.PowerUps.powerUpBomb = false;

                Game.SceneObjects.PowerUpState(false, 0);
                
                Collider[] hits = Physics.OverlapSphere(pos, Game.SceneObjects.fieldDimensions.x*1.333f, LayerMask.GetMask("Virus"), QueryTriggerInteraction.Collide);
                StartCoroutine(AreaEffect(pos));

                for (int i = 0; i < hits.Length; i++)
                {
                    GameObject virus = hits[i].transform.parent.parent.gameObject;
                    DestroyVirus(virus);
                }               
            }else

            //Normal Gameplay
            /*if (Input.GetMouseButtonDown(0))
            {
                Game.Stats.clicks++;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit, 20, LayerMask.GetMask("Virus"), QueryTriggerInteraction.Collide))
                {
                    
                    GameObject virus = hit.transform.parent.parent.gameObject;
                    DestroyVirus(virus);
                }
            }*/
            if (Input.GetMouseButtonDown(0))
            {
                Game.Stats.clicks++;

                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));

                Collider[] hits = Physics.OverlapSphere(pos, 8f, LayerMask.GetMask("Virus"), QueryTriggerInteraction.Collide);

                for (int i = 0; i < hits.Length; i++)
                {
                    GameObject virus = hits[i].transform.parent.parent.gameObject;
                    DestroyVirus(virus);
                }
            }

            //TouchVersion
            /*if (Input.touchCount > 0 && Game.PowerUps.powerUpSoap)
            {
                Game.Stats.clicks++;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    Ray ray = Camera.main.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out RaycastHit hit, 70, LayerMask.GetMask("Virus"), QueryTriggerInteraction.Collide))
                    {
                        GameObject virus = hit.transform.parent.parent.gameObject;
                        DestroyVirus(virus);
                    }
                }
            }*/

            if (Input.touchCount > 0 && Game.PowerUps.powerUpSoap)
            {
                Game.Stats.clicks++;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));


                    Collider[] hits = Physics.OverlapSphere(pos, 7f, LayerMask.GetMask("Virus"), QueryTriggerInteraction.Collide);

                    for (int ii = 0; ii < hits.Length; ii++)
                    {
                        GameObject virus = hits[ii].transform.parent.parent.gameObject;
                        DestroyVirus(virus);
                    }
                }
            }
            else if (Input.touchCount > 1)
            {
                Game.Stats.clicks++;
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    if (touch.phase == TouchPhase.Began)
                    {
                        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 10f));


                        Collider[] hits = Physics.OverlapSphere(pos, 7f, LayerMask.GetMask("Virus"), QueryTriggerInteraction.Collide);

                        for (int ii = 0; ii < hits.Length; ii++)
                        {
                            GameObject virus = hits[ii].transform.parent.parent.gameObject;
                            DestroyVirus(virus);
                        }
                    }
                }
            }


            //EndGame
            if (Game.SceneObjects.viruses.Count > Game.Stats.loseNumber)
            {
                GameFinished();
                FindObjectOfType<BirthControl>().StopNaughtiness();

                MainController.LoseGame();
            }
            if (Game.SceneObjects.viruses.Count == 0)
            {
                GameFinished();
                FindObjectOfType<BirthControl>().StopNaughtiness();

                MainController.WinGame();
                StartCoroutine(WinScreen());
            }
        }
    }
    IEnumerator WinScreen()
    {
        yield return new WaitForSeconds(2f);
        mainController.loadNextLevel();
        StopCoroutine(WinScreen());
    }
    void GameFinished()
    {
        Game.Stats.notWon = false;
    }
    void DestroyVirus(GameObject virus)
    {
        Game.controllers.soundController.splatSound();
        Game.SceneObjects.viruses.Remove(virus.GetComponent<Virus>());

        Instantiate(Game.prefabs.explosion, virus.transform.position, Quaternion.identity);

        Destroy(virus);

        Game.Stats.killCount++;

        Game.PowerUps.CheckPowerUpCondition();
    }
    IEnumerator AreaEffect(Vector3 position)
    {
        GameObject areaEffect = Instantiate(unitySphere, position, Quaternion.identity);
        areaEffect.transform.localScale = Vector3.one * Game.SceneObjects.fieldDimensions.x*0.66f;
        print(areaEffect.transform.localScale);
        yield return new WaitForSeconds(0.25f);

        Destroy(areaEffect);
        StopCoroutine(AreaEffect(position));
    }

}
