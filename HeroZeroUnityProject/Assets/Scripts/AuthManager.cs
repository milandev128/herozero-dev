using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;
using TMPro;
using Utils;

namespace HeroZero
{
    public class AuthManager : MonoBehaviour
    {
        public static AuthManager Instance;

        // Login 
        public TMP_InputField loginUsername;
        public TMP_InputField loginPassword;
        public TextMeshProUGUI loginErrorMessage;

        // Register
        public TMP_InputField registerUsername;
        public TMP_InputField registerEmail;
        public TMP_InputField registerPassword;
        public TMP_InputField registerCPassword;
        public TextMeshProUGUI registerErrorMsg;
        public Image skin;
        public Sprite[] skins;

        public GameObject loginPanel;
        public GameObject registerPanel;

        public GameObject loadingIcon;

        public int currentSkinIndex = 0;
        
        private void Awake()
        {
            Instance = this;
        }

        public void showLoginPanel()
        {
            loginUsername.text = "";
            loginPassword.text = "";
            loginErrorMessage.text = "";

            loadingIcon.SetActive(false);
            registerPanel.gameObject.SetActive(false);
            loginPanel.gameObject.SetActive(true);
        }

        public void showRegisterPanel()
        {
            registerUsername.text = "";
            registerEmail.text = "";
            registerPassword.text = "";
            registerCPassword.text = "";
            registerErrorMsg.text = "";
            skin.sprite = skins[0];

            loadingIcon.SetActive(false);
            loginPanel.gameObject.SetActive(false);
            registerPanel.gameObject.SetActive(true);
        }

        public void OnLogin()
        {
            Debug.Log("Login Called");
            loadingIcon.SetActive(true);
            loginErrorMessage.text = "";
            loginErrorMessage.gameObject.SetActive(false);
            StartCoroutine(SendLogInRequest());
        }

        public void OnRegister()
        {
            loadingIcon.SetActive(true);
            registerErrorMsg.text = "";
            registerErrorMsg.gameObject.SetActive(false);
            StartCoroutine(SendRegisterRequest());
        }

        IEnumerator SendLogInRequest()
        {
            PlayerPrefs.DeleteAll();
            WWWForm form = new WWWForm();
            LoginRequest request = new LoginRequest();
            request.name = loginUsername.text;
            request.password = loginPassword.text;
            UnityWebRequest uwr = UnityWebRequest.Post(Constants.baseUrl + "/login", form);

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
                LoginResponse response = JsonConvert.DeserializeObject<LoginResponse>(uwr.downloadHandler.text);
            
                if (response.status == "success")
                {
                    Debug.Log("Login Successful");
                    PlayerPrefs.SetString("token", response.token);
                    PlayerPrefs.SetString("username", response.user.name);
                    PlayerPrefs.SetInt("coins", response.user.coins);
                    PlayerPrefs.SetInt("level", response.user.level);
                    PlayerPrefs.SetInt("strength", response.user.strength);
                    PlayerPrefs.SetInt("dodge", response.user.dodge);
                    PlayerPrefs.SetInt("skin", response.user.skin);
                    PlayerPrefs.SetString("items", string.Join(',', response.user.items));

                    SceneManager.LoadScene("main");
                }
                else
                {
                    loginUsername.text = "";
                    loginPassword.text = "";
                    loginErrorMessage.text = response.message;
                    loginErrorMessage.gameObject.SetActive(true);
                }
            }
            loadingIcon.SetActive(false);
        }

        IEnumerator SendRegisterRequest()
        {
            WWWForm form = new WWWForm();

            RegisterRequest request = new RegisterRequest();
            request.name = registerUsername.text;
            request.email = registerEmail.text;
            request.password = registerPassword.text;
            request.skin = currentSkinIndex;

            string jsonData = JsonConvert.SerializeObject(request);
            UnityWebRequest uwr = UnityWebRequest.Post(Constants.baseUrl + "/register", form);
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
                    Debug.Log("Signup Successful");
                    showLoginPanel();
                }
                else
                {
                    registerErrorMsg.gameObject.SetActive(true);
                    registerErrorMsg.text = response.message;
                }
            }
            loadingIcon.SetActive(false);
        }

        public void nextSkin()
        {
            currentSkinIndex++;
            if(currentSkinIndex >= skins.Length)
            {
                currentSkinIndex = 0;
                skin.sprite = skins[currentSkinIndex];
            }
            else
            {
                skin.sprite = skins[currentSkinIndex];
            }
        }

        public void previousSkin()
        {
            currentSkinIndex--;
            if (currentSkinIndex < 0)
            {
                currentSkinIndex = skins.Length - 1;
                skin.sprite = skins[currentSkinIndex];
            }
            else
            {
                skin.sprite = skins[currentSkinIndex];
            }
        }
    }
}