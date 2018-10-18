using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {
    public EnemyAI enemy;
    public PlayerControl hero;
    public GameObject startButton;
    public GameObject attackButton;
    public CameraFollow cameraFollow;
    public GameObject UIRoot;
    private Text[] mainText;
    private int timeRemaining = 10;
    private GameObject[] UI;

    // Use this for initialization
    void Start()
    {
        enemy.GetComponentInChildren<Gun>().gunFired += SwapTurn;
        hero.GetComponentInChildren<Gun>().gunFired += SwapTurn;
        mainText = startButton.GetComponentsInChildren<Text>();
        UIRoot.SetActive(false);
    }

    public void StartGame()
    {
        startButton.GetComponent<Button>().enabled = false;
        enemy.hasTurn = true;
        InvokeRepeating("DecreaseTime", 0, 1);
        SwapTurn();
    }

    void SwapTurn()
    {
        StartCoroutine(SwapTurnCoroutine());
    }

    IEnumerator SwapTurnCoroutine()
    {
        timeRemaining = 10;
        hero.hasTurn = !hero.hasTurn;
        UIRoot.SetActive(hero.hasTurn);
        cameraFollow.SetPlayerToFollow(hero.hasTurn ? hero.transform : enemy.transform);
        if (!enemy.hasTurn)
        {
            yield return new WaitForSeconds(2f);
        }
        enemy.hasTurn = !enemy.hasTurn;
    }

    void DecreaseTime()
    {
        timeRemaining--;
        if (timeRemaining < 0) SwapTurn();
        foreach (Text t in mainText)
        {
            t.text = "" + timeRemaining;
        }
    }
}
