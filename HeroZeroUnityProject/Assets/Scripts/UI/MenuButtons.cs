using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuButtons : MonoBehaviour {

    public static MenuButtons Instance;
    public GameObject btnInfo, btnShop, btnMission;
    public GameObject pageInfo, pageShop, pageMission;
    public Sprite imgActive, imgNormal;
    public GameObject skin, itemlist;
    public GameObject btnStart, btnCancel;
    public TextMeshProUGUI timer;

    private void Awake()
    {
        Instance = this;        
    }

    public void initPanel() 
    {
        btnInfo.GetComponent<Image>().sprite = imgNormal;
        btnShop.GetComponent<Image>().sprite = imgNormal;
        btnMission.GetComponent<Image>().sprite = imgNormal;
        skin.gameObject.SetActive(true);
        itemlist.gameObject.SetActive(true);
        pageInfo.gameObject.SetActive(false);
        pageShop.gameObject.SetActive(false);
        pageMission.gameObject.SetActive(false);
    }

    public void showInformationPanel() 
    {
        initPanel();
        if(Timer.Instance) {
            Timer.Instance.StopTimer();
            timer.text = "00:30";
        }
        btnInfo.GetComponent<Image>().sprite = imgActive;
        pageInfo.gameObject.SetActive(true);
    }

    public void showShoppingPanel()
    {
        initPanel();
        if(Timer.Instance) {
            Timer.Instance.StopTimer();
            timer.text = "00:30";
        }
        btnShop.GetComponent<Image>().sprite = imgActive;
        pageShop.gameObject.SetActive(true);
        itemlist.gameObject.SetActive(false);
        GameObject panelPreview = pageShop.transform.Find("Preview Panel").gameObject;
        panelPreview.SetActive(false);
    }

    public void showMissionPanel()
    {
        initPanel();
        if(Timer.Instance) {
            Timer.Instance.StopTimer();
            timer.text = "00:30";
        }
        GameObject panelAll = pageMission.transform.Find("AllMissionCompleted").gameObject;
        GameObject panelMission = pageMission.transform.Find("MissionBox").gameObject;
        GameObject panelComplete = pageMission.transform.Find("LevelComplete").gameObject;
        GameObject panelFailed = pageMission.transform.Find("LevelFailed").gameObject;

        btnMission.GetComponent<Image>().sprite = imgActive;
        skin.gameObject.SetActive(false);
        itemlist.gameObject.SetActive(false);
        panelComplete.gameObject.SetActive(false);
        panelFailed.gameObject.SetActive(false);

        btnCancel.gameObject.SetActive(false);
        btnStart.gameObject.SetActive(true);
        pageMission.gameObject.SetActive(true);

        if(PlayerPrefs.GetInt("level") == 1) {
            panelAll.SetActive(false);
            panelMission.SetActive(true);
        } else {
            panelAll.SetActive(true);
            panelMission.SetActive(false);
        }
    }
}