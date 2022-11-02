using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using Utils;

public class ShopUIHandler : MonoBehaviour {

    public static ShopUIHandler Instance;
    public GameObject previewPanel;
    public TextMeshProUGUI descName, descStrength, descDodge;
    public GameObject btnBuy;
    private ItemModel currentSword;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameObject[] shopList = GameObject.FindGameObjectsWithTag("sword");
        foreach (GameObject shopItem in shopList)
        {
            ItemModel itemModel = shopItem.GetComponent<ItemModel>();
            Button btnSword = shopItem.GetComponent<Button>();
            btnSword.onClick.AddListener(() => showPreviewPanel(itemModel));
        }
    }

    public void closePreviewPanel() 
    {
        descName.text = "";
        descStrength.text = "";
        descDodge.text = "";
        currentSword = null;
        previewPanel.gameObject.SetActive(false);
    }

    public void showPreviewPanel(ItemModel sword)
    {
        descName.text = "Name: " + sword.name;
        descStrength.text = "Strength: +" + sword.strength;
        descDodge.text = "Dodge: +" + sword.dodge;
        currentSword = sword;
        
        var itemList = JsonConvert.DeserializeObject<List<ItemRequest>>(PlayerPrefs.GetString("items"));
        
        bool flag = false;
        foreach(ItemRequest itemRequest in itemList) {
            if(itemRequest.name == sword.name) {
                flag = true;
                break;
            }
        }
        if(flag) {
            btnBuy.gameObject.SetActive(false);
        } else {
            btnBuy.gameObject.SetActive(true);
        }
        
        previewPanel.gameObject.SetActive(true);
    }

    public void onBuyItem()
    {
        GameManager.Instance.loadingIcon.SetActive(true);
        StartCoroutine(SendBuyItemRequest());
    }

    IEnumerator SendBuyItemRequest()
    {
        WWWForm form = new WWWForm();
        ItemRequest request = new ItemRequest();
        request.name = currentSword.name;
        request.strength = currentSword.strength;
        request.dodge = currentSword.dodge;

        UnityWebRequest uwr = UnityWebRequest.Post(Constants.baseUrl + "/buyitem", form);

        string token = PlayerPrefs.GetString("token");
        uwr.SetRequestHeader("Content-Type", "application/json");
        uwr.SetRequestHeader("Authorization", "Bearer " + token);

        string jsonData = JsonConvert.SerializeObject(request);
        byte[] dataToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(dataToSend);
        uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        uwr.SetRequestHeader("Content-Type", "application/json");
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError("Error:" + uwr.error);
        }
        else
        {
            Debug.Log(uwr.downloadHandler.text);
            APIResponse response = JsonConvert.DeserializeObject<APIResponse>(uwr.downloadHandler.text);
        
            if (response.status == "success")
            {
                Debug.Log("Buy Item Successful");
                previewPanel.gameObject.SetActive(false);
                GameManager.Instance.RefreshUserData();
            }
            else
            {
                Debug.Log(response.message);
            }
        }
        GameManager.Instance.loadingIcon.SetActive(false);
    }
}