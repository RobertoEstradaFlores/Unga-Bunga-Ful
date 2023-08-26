using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cambioescena : MonoBehaviour
{
    public int SceneBuildIndex;

    public string SceneToLoad;
    private void OnTriggerEnter2D(Collider2D Otro)
    {
        if (Otro.tag =="player")
        {
            SceneManager.LoadScene(SceneBuildIndex,LoadSceneMode.Single);
        }
    }
}
