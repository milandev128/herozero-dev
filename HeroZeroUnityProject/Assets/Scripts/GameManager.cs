using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Utils;

public class GameManager : PersistentSingleton<GameManager>
{

    public TextMeshProUGUI level, strength, dodge, coins, username;
    public Image skin;
    public Sprite[] skinSprites;
    public GameObject[] items;
    public GameObject mainComponent;
    public GameObject loadingIcon;

    private void Start()
    {
        MenuButtons.Instance.showInformationPanel();
        StartCoroutine(DisplayUserInfo());
    }

    IEnumerator DisplayUserInfo()
    {
        loadingIcon.gameObject.SetActive(true);
        mainComponent.gameObject.SetActive(false);

        yield return StartCoroutine(SendUserDataRequest());

        loadingIcon.gameObject.SetActive(false);
        mainComponent.gameObject.SetActive(true);
    }

    public void RefreshUserData()
    {
        StartCoroutine(SendUserDataRequest());
    }

    IEnumerator SendUserDataRequest()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Constants.baseUrl + "/getuserdata"))
        {
            string token = PlayerPrefs.GetString("token");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + token);

            yield return request.Send();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.Log(request.error);
            }
            else
            {
                GetUserInfoResponse response = JsonConvert.DeserializeObject<GetUserInfoResponse>(request.downloadHandler.text);
                Debug.Log("Get user information successfully");

                PlayerPrefs.SetString("username", response.user.name);
                PlayerPrefs.SetInt("coins", response.user.coins);
                PlayerPrefs.SetInt("level", response.user.level);
                PlayerPrefs.SetInt("strength", response.user.strength);
                PlayerPrefs.SetInt("dodge", response.user.dodge);
                PlayerPrefs.SetInt("skin", response.user.skin);
                PlayerPrefs.SetString("items", JsonConvert.SerializeObject(response.user.items));

                SetUserInformation();
            }
        }
    }

    public void SetUserInformation() 
    {
        username.text = PlayerPrefs.GetString("username");
        coins.text = PlayerPrefs.GetInt("coins") + "";
        level.text = PlayerPrefs.GetInt("level") + "";
        skin.sprite = skinSprites[PlayerPrefs.GetInt("skin")];

        int tmpstrength = PlayerPrefs.GetInt("strength");
        int tmpdodge = PlayerPrefs.GetInt("dodge");
        var itemList = JsonConvert.DeserializeObject<List<ItemRequest>>(PlayerPrefs.GetString("items"));

        foreach(ItemRequest itemRequest in itemList) {
            tmpstrength += itemRequest.strength;
            tmpdodge += itemRequest.dodge;
        }

        foreach(GameObject item in items) {
            bool flag = false;
            foreach(ItemRequest itemRequest in itemList) {
                if(itemRequest.name == item.GetComponent<ItemModel>().name) {
                    flag = true;
                    break;
                }
            }
            if(flag) {
                item.SetActive(true);
            } else {
                item.SetActive(false);
            }
        }

        strength.text = tmpstrength.ToString();
        dodge.text = tmpdodge.ToString();
    }
}
