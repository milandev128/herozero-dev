using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using Utils;

public class MissionUIHandler : MonoBehaviour {

    public static MissionUIHandler Instance;

    public GameObject missionStartButton;
    public GameObject missionCancelButton;
    public GameObject timerAndLoadingBar;

    public GameObject allCompletePanel;
    public GameObject missionPanel;
    public GameObject levelCompletePanel;
    public GameObject levelFailedPanel;

    private void Awake()
    {
        Instance = this;
    }
    
    public void StartMission()
    {
        timerAndLoadingBar.SetActive(true);
        missionStartButton.SetActive(false);
        missionCancelButton.SetActive(true);
        Timer.Instance.StartTimer(1800);
    }

    public void initPanel()
    {
        allCompletePanel.SetActive(false);
        levelCompletePanel.SetActive(false);
        levelFailedPanel.SetActive(false);
        missionCancelButton.gameObject.SetActive(false);
        missionStartButton.gameObject.SetActive(true);
        missionPanel.SetActive(false);
    }

    public void showLevelPanel()
    {
        initPanel();
        missionPanel.SetActive(true);
    }

    public void showAllPanel()
    {
        initPanel();
        allCompletePanel.SetActive(true);
    }

    public void LevelFailed()
    {
        Timer.Instance.StopTimer();
        initPanel();
        levelFailedPanel.SetActive(true);
    }

    public void LevelComplete()
    {
        initPanel();
        levelCompletePanel.SetActive(true);
    }
    
    public void CompleteMission()
    {
        GameManager.Instance.loadingIcon.SetActive(true);
        StartCoroutine(SendCompleteMissionRequest());
    }

    IEnumerator SendCompleteMissionRequest()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Constants.baseUrl + "/completemission"))
        {
            string token = PlayerPrefs.GetString("token");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.Send();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
                LevelFailed();
            }
            else
            {
                APIResponse response = JsonConvert.DeserializeObject<APIResponse>(request.downloadHandler.text);

                if(response.status == "success") {
                    Debug.Log("Complete Mission Successful");
                    GameManager.Instance.RefreshUserData();
                    LevelComplete();
                } else {
                    LevelFailed();
                }
            }
        }
        GameManager.Instance.loadingIcon.SetActive(false);
    }
}