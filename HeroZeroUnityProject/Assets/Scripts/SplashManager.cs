using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashManager : MonoBehaviour
{
    
    void Start()
    {
        StartCoroutine(loadScene("auth"));
    }

    IEnumerator loadScene(string scene)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scene);
    }
}
