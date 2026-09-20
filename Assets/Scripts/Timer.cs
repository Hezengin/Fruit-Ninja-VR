using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime = 35;

    [SerializeField] private GameManager gameManager;

    //TODO fix -min timer 

    void Update()
    {
        //Debug.Log("Remaining Time: " + remainingTime);
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("Time Left: {0:00}:{1:00}", minutes, seconds);
            //Debug.Log(timerText.text);
        }
        else
        {
            gameManager.GameModeFinished(false);
            remainingTime = 15;
        }
    }
}
