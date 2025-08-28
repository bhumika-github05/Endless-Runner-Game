using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button[] claimButtons;

    private int totalDays = 8;
    private float actualTimeInHours = 24f; // Real-time hours display

    private const double testTimeInSeconds = 60f; // 1 min test time
    private const double resetTimeInSeconds = testTimeInSeconds * 2f; // Reset if user delays

    private int currentDay = 0;
    private DateTime startTime;

    private void Start()
    {
        if (PlayerPrefs.HasKey("TimerStart"))
        {
            PlayerPrefs.DeleteKey("TimerStart");
            PlayerPrefs.DeleteKey("CurrentDay");

        }
        LoadState();            // Load saved data
        UpdateAllButtons();     // Setup claim buttons

        if (PlayerPrefs.HasKey("TimerStart"))
        {
            startTime = DateTime.Parse(PlayerPrefs.GetString("TimerStart"));
        }
        else
        {
            timerText.text = "00:00 HRS"; // Default display
        }
    }

    private void Update()
    {
        CheckTimer(); // Continuously update timer logic
    }

    public void OnClaimButtonClicked(int dayIndex)
    {
        if (dayIndex != currentDay)
            return;

        claimButtons[dayIndex].interactable = false;
        Start24HourTimer();
        UpdateAllButtons();
    }

    public void Start24HourTimer()
    {
        startTime = DateTime.UtcNow;
        PlayerPrefs.SetString("TimerStart", startTime.ToString());
        PlayerPrefs.Save();
    }

    public void CheckTimer()
    {
        if (!PlayerPrefs.HasKey("TimerStart"))
            return;

        DateTime savedStartTime = DateTime.Parse(PlayerPrefs.GetString("TimerStart"));
        TimeSpan passedTime = DateTime.UtcNow - savedStartTime;

        // If time exceeds 2 minutes (reset threshold)
        if (passedTime.TotalSeconds >= resetTimeInSeconds)
        {
            RestartFromDay1();
            return;
        }

        // If 1 min passed, proceed to next day
        if (passedTime.TotalSeconds >= testTimeInSeconds)
        {
            ProceedToNextDay();
            return;
        }

        // Else, show remaining time
        float scaledHours = (float)(passedTime.TotalSeconds / testTimeInSeconds) * actualTimeInHours;
        double remainingHours = actualTimeInHours - scaledHours;

        TimeSpan timeRemaining = TimeSpan.FromHours(remainingHours);
        string countdown = string.Format("{0:D2}:{1:D2}", timeRemaining.Hours, timeRemaining.Minutes);
        timerText.text = countdown + " HRS";
    }

    public void ProceedToNextDay()
    {
        currentDay++;

        if (currentDay >= totalDays)
        {
            currentDay = totalDays - 1; // Stay at last day
        }

        PlayerPrefs.SetInt("CurrentDay", currentDay);
        RestartTimer();
        UpdateAllButtons();
        timerText.text = "00:00 HRS";
    }

    public void RestartFromDay1()
    {
        currentDay = 0;
        PlayerPrefs.SetInt("CurrentDay", currentDay);
        RestartTimer();
        UpdateAllButtons();
        timerText.text = "00:00 HRS";
    }

    public void RestartTimer()
    {
        if (PlayerPrefs.HasKey("TimerStart"))
        {
            PlayerPrefs.DeleteKey("TimerStart");
        }
    }

    public void LoadState()
    {
        currentDay = PlayerPrefs.GetInt("CurrentDay", 0);

        if (PlayerPrefs.HasKey("TimerStart"))
        {
            startTime = DateTime.Parse(PlayerPrefs.GetString("TimerStart"));
        }
    }

    public void UpdateAllButtons()
    {
        for (int i = 0; i < claimButtons.Length; i++)
        {
            // Enable only the current day's button if timer hasn't started
            claimButtons[i].interactable = (i == currentDay && !PlayerPrefs.HasKey("TimerStart"));
        }
    }
}