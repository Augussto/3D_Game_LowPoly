using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSelectedScene : MonoBehaviour
{
    public string selectedScene;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
    }
    
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(selectedScene);
    }
}
