using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public Image loadingbar;

    public TextMeshProUGUI TimerText;

    float MaxTime;
    float CurrentTime;
    float Interval = 7.0f;
    bool isHurry = false;
    bool isRunning = false;

    private void Awake()
    {
        Instance = this;
        StopTimer();
    }

    // Use this for initialization
    void Start()
    {
        isHurry = true;
        Invoke("InitHurry", 5f);
    }

    void TimerTickMS()
    {
        CurrentTime -= Interval;
        TimeOver();
    }

    void InitHurry()
    {
        isHurry = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning) {
            TimerText.text = GetTImeValueFormated();
            loadingbar.fillAmount = 1 - CurrentTime/MaxTime;
        } else {
            loadingbar.fillAmount = 0;
        }
    }

    public void ResetTimer()
    {
        CurrentTime = MaxTime;
    }

    string GetTImeValueFormated()
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(CurrentTime);
        string str = null;
        str = string.Format("{0:D2}:{1:D2}", t.Hours, t.Minutes);
        if (CurrentTime > 650 && CurrentTime <= 700 && !isHurry)
        {
            isHurry = true;            
        }
        else if (CurrentTime > 700 && isHurry)
        {
            HideHurry();
        }
        return str;
    }

    public void HideHurry()
    {
        
        isHurry = false;
    }

    public void StartTimer(float startTime)
    {
        if (isRunning == true)
            CancelInvoke("TimerTickMS");
        InvokeRepeating("TimerTickMS", Interval / 60.0f, Interval / 60.0f);
        if (IsInvoking("TimerTickMS"))
            isRunning = true;
        CurrentTime = startTime;
        MaxTime = startTime;
        loadingbar.fillAmount = 0;
        loadingbar.gameObject.SetActive(true);
        TimerText.text = GetTImeValueFormated();
    }

    public void StopTimer()
    {
        if (isRunning == true)
        {
            CurrentTime = MaxTime;
            isRunning = false;
            loadingbar.fillAmount = 0;
            loadingbar.gameObject.SetActive(false);
            CancelInvoke("TimerTickMS");
        }

    }
    public void TimeOver()
    {
        if (CurrentTime <= 0)
        {
            MissionUIHandler.Instance.CompleteMission();
            Debug.Log("Time's Up . . . . . . . . . . .");
            CancelInvoke("TimerTickMS");
        }
    }

    public void PauseTimer()
    {
        if (isRunning == true)
        {
            isRunning = false;
            CancelInvoke("TimerTickMS");
        }
    }

    public void ResumeTimer()
    {
        if (isRunning == false)
        {
            InvokeRepeating("TimerTickMS", Interval / 60.0f, Interval / 60.0f);
            isRunning = true;
        }
    }

}