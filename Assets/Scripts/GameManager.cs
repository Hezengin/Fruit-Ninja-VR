using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> classicUIElements;
    public List<GameObject> timerUIElements;
    public FruitThrower fruitThrower1;
    public FruitThrower fruitThrower2;
    public GameObject uiPanel;
    public GameObject scorePanel;
    public GameObject livePanel;
    public GameObject timerPanel;
    public GameObject gameModePanel;
    public GameObject countDownPanel;
    public string gameMode;

    public int score = 0;
    public int lives = 3;

    public void AddScore()
    {
        score++;
        scorePanel.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = $"Score: {score}";
    }

    public void LoseLife()
    {
        lives--;
        livePanel.transform.Find("Live").GetComponent<TextMeshProUGUI>().text = $"Live: {lives}";

        if (lives <= 0)
        {
            Debug.Log("No lives left! Game Over.");
            GameModeFinished(false);
        }
    }

    IEnumerator CountDownPanel(int seconds)
    {
        countDownPanel.SetActive(true);

        if (countDownPanel != null)
        {
            for (int i = 0; i < seconds; i++)
            {
                countDownPanel.transform.Find("CountDown").GetComponent<TextMeshProUGUI>().text = $"{seconds - i}";
                yield return new WaitForSeconds(1);
            }
            countDownPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("COUNTDOWN ZERO?");
        }
    }

    public IEnumerator InitGameMode(string gameMode)
    {
        InitUIObjects(gameMode);
        score = 0;
        lives = 3;
        ResetScoreTableUI(); // extra reset because when the replay button is pressed it doesnt reload the ui components

        if (gameModePanel == null)
        {
            Debug.LogError("gameModePanel is not assigned in the Inspector!");
            yield break;
        }

        gameModePanel.transform.Find("GameMode").GetComponent<TextMeshProUGUI>().text = $"Game mode: {gameMode}";

        // Wait until CountDownPanel is finished
        yield return StartCoroutine(CountDownPanel(4));

        // After countdown, initialize game objects
        InitGameModeObjects();
    }

    # region Onclicks
    public void OnClassicClicked()
    {
        gameMode = "Classic";
        uiPanel.SetActive(false); // Hide UI
        StartCoroutine(InitGameMode(gameMode));
        Debug.Log("Classic button clicked");
    }

    public void OnTimerClicked()
    {
        gameMode = "Timer";
        uiPanel.SetActive(false); // Hide UI
        StartCoroutine(InitGameMode(gameMode));
        Debug.Log("Timer button clicked");
    }

    public void OnReturnMainClicked()
    {
        ResetUI();
    }

    public void OnReplayGameModeClicked()
    {
        if (gameMode == null)
        {
            Debug.Log("GameMode is NULL");
            return;
        }
        uiPanel.SetActive(false); // Hide UI
        StartCoroutine(InitGameMode(gameMode));
    }
    #endregion

    #region INIT/DEINIT
    public void InitUIObjects(string gameMode)
    {
        if (gameMode == "Classic")
        {
            foreach (GameObject go in classicUIElements)
            {
                
                go.SetActive(true);
                Debug.Log(go.name);
            }
        }
        else if (gameMode == "Timer")
        {
            foreach (GameObject go in timerUIElements)
            {
                go.SetActive(true);
                Debug.Log(go.name);
            }
        }
    }

    public void DeInitUIObjects(string gameMode)
    {
        if (gameMode == "Classic")
        {
            foreach (GameObject go in classicUIElements)
            {
                go.SetActive(false);
                Debug.Log($"DeIniT: {go.name}");
            }
        }
        else if (gameMode == "Timer")
        {
            foreach (GameObject go in timerUIElements)
            {
                go.SetActive(false);
                Debug.Log($"DeIniT: {go.name}");
            }
        }
    }

    public void InitGameModeObjects()
    {
        fruitThrower1.gameObject.SetActive(true);
        fruitThrower2.gameObject.SetActive(true);
        fruitThrower1.StartThrowingFruit();
        fruitThrower2.StartThrowingFruit();
    }

    public void DeInitGameModeObjects()
    {
        fruitThrower1.StopThrowingFruit();
        fruitThrower2.StopThrowingFruit();
        fruitThrower1.gameObject.SetActive(false);
        fruitThrower2.gameObject.SetActive(false);
    }
    #endregion

    public void GameModeFinished(bool gameOver)
    {
        uiPanel.SetActive(true);
        uiPanel.transform.Find("Buttons Main").gameObject.SetActive(false);
        uiPanel.transform.Find("Buttons Finish").gameObject.SetActive(true);
        if (gameOver)
        {
            uiPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"Game Over";
        }
        else
        {
            uiPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = $"Score: {score}";
        }

        score = 0;
        lives = 3;

        Debug.Log($"GameMode: {gameMode}");
        ResetScoreTableUI();

        DeInitUIObjects(gameMode);
        DeInitGameModeObjects();
    }

    void ResetScoreTableUI()
    {
        scorePanel.transform.Find("Score").GetComponent<TextMeshProUGUI>().text = $"Score: {score}";
        livePanel.transform.Find("Live").GetComponent<TextMeshProUGUI>().text = $"Lives: {lives}";
    }

    private void ResetUI()
    {
        uiPanel.SetActive(true);
        uiPanel.transform.Find("Buttons Main").gameObject.SetActive(true);
        uiPanel.transform.Find("Buttons Finish").gameObject.SetActive(false);
        uiPanel.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = "Select a game mode";
    }
}
