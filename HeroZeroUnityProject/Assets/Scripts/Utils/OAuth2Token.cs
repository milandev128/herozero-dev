using System;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class OAuth2Token
{
    public string access_token, refresh_token, token_type, session_state, scope;
     public int expires_in, refresh_expires_in;

    

    public void StoreInPlayerPrefs()
    {
        Debug.Log($"{nameof(OAuth2Token)}: will save all non-whitespace token values to PlayerPrefs");

        if (!string.IsNullOrWhiteSpace(access_token))
        {
            PlayerPrefs.SetString(nameof(access_token), access_token);
        }
        if (!string.IsNullOrWhiteSpace(refresh_token))
        {
            PlayerPrefs.SetString(nameof(refresh_token), refresh_token);
        }
        if (!string.IsNullOrWhiteSpace(token_type))
        {
            PlayerPrefs.SetString(nameof(token_type), token_type);
        }
        if (!string.IsNullOrWhiteSpace(session_state))
        {
            PlayerPrefs.SetString(nameof(session_state), session_state);
        }
        if (!string.IsNullOrWhiteSpace(scope))
        {
            PlayerPrefs.SetString(nameof(scope), scope);
        }
        PlayerPrefs.SetInt(nameof(expires_in), expires_in);
        PlayerPrefs.SetInt(nameof(refresh_expires_in), refresh_expires_in);
    }

    public static bool ExistsInPlayerPrefs()
    {
        bool exists = PlayerPrefs.HasKey(nameof(access_token)) && PlayerPrefs.HasKey(nameof(refresh_token));
        // those two should be the minimum requirements for usage/refresh
        Debug.Log($"{nameof(OAuth2Token)}: checking if token exists in PlayerPrefs: {exists}");
        return exists;
    }

    public static OAuth2Token LoadFromPlayerPrefs()
    {
        Debug.Log($"{nameof(OAuth2Token)}: will try to load token from PlayerPrefs");

        OAuth2Token token = new OAuth2Token();
        if (PlayerPrefs.HasKey(nameof(access_token)))
        {
            token.access_token = PlayerPrefs.GetString(nameof(access_token));
        }
        if (PlayerPrefs.HasKey(nameof(refresh_token)))
        {
            token.refresh_token = PlayerPrefs.GetString(nameof(refresh_token));
        }
        if (PlayerPrefs.HasKey(nameof(token_type)))
        {
            token.token_type = PlayerPrefs.GetString(nameof(token_type));
        }
        if (PlayerPrefs.HasKey(nameof(session_state)))
        {
            token.session_state = PlayerPrefs.GetString(nameof(session_state));
        }
        if (PlayerPrefs.HasKey(nameof(scope)))
        {
            token.scope = PlayerPrefs.GetString(nameof(scope));
        }
        if (PlayerPrefs.HasKey(nameof(expires_in)))
        {
            token.expires_in = PlayerPrefs.GetInt(nameof(expires_in));
        }
        if (PlayerPrefs.HasKey(nameof(refresh_expires_in)))
        {
            token.refresh_expires_in = PlayerPrefs.GetInt(nameof(refresh_expires_in));
        }
        return token;
    }

    public string AccessToken() => access_token;

    public static void DeleteFromPlayerPrefs()
    {
        Debug.Log($"{nameof(OAuth2Token)}: will try to delete all token-related keys from PlayerPrefs");

        PlayerPrefs.DeleteKey(nameof(access_token));
        PlayerPrefs.DeleteKey(nameof(refresh_token));
        PlayerPrefs.DeleteKey(nameof(token_type));
        PlayerPrefs.DeleteKey(nameof(session_state));
        PlayerPrefs.DeleteKey(nameof(scope));
        PlayerPrefs.DeleteKey(nameof(expires_in));
        PlayerPrefs.DeleteKey(nameof(refresh_expires_in));
    }


    [Serializable] public class NewTokenAcquired : UnityEvent<OAuth2Token> { }

    [Serializable] public class TokenAcquisitionFailed : UnityEvent<string> { }

    [Serializable] public class TokenRefreshFailed : UnityEvent<string> { }

    [Serializable] public class TokenValidity : UnityEvent<bool> { }
}
