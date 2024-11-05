using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class UITimer : MonoBehaviour
{
    // Public variables for easy setup in the Inspector
    public Text timerText;  // Reference to a UI Text element
    public float countdownTime = 10f;  // 10 minutes in seconds (10 * 60 = 600 seconds)
    public LevelFinishScreen levelFinishedPanel;

    // Private variables
    private float remainingTime;
    private bool timerIsRunning = false;

    void Start()
    {
        // Initialize the timer with the countdown time
        remainingTime = countdownTime;
        timerIsRunning = true;  // Start the timer
    }

    void Update()
    {
        // Check if the timer is running
        if (timerIsRunning)
        {
            if (remainingTime > 0)
            {
                // Reduce the remaining time
                remainingTime -= Time.deltaTime;

                // Update the UI to show the countdown
                UpdateTimerText(remainingTime);
            }
            else
            {
                // Stop the timer when the time runs out
                remainingTime = 0;
                timerIsRunning = false;

                // Optional: Trigger actions when the timer reaches zero
                TimerFinished();
            }
        }
    }

    // Update the text component to display the remaining time in "MM:SS" format
    void UpdateTimerText(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        timerText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    // Optional: Function that gets called when the timer reaches zero
    void TimerFinished()
    {
        Debug.Log("Timer has finished!");
        // Add additional logic here for when the timer finishes
        if (levelFinishedPanel != null) {
            levelFinishedPanel.FinishLevel();
        }
    }

    public float GetTimeRemaining() {
        return remainingTime;
    }
}
