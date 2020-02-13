using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{

    private GameObject startCv;
    private GameObject secondCv;
    // Start is called before the first frame update
    void Start()
    {
        startCv = GameObject.Find("StartCanvas");
        secondCv = GameObject.Find("Second").transform.Find("SecondCanvas").gameObject;
        StartCoroutine(Fade());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(3);
        startCv.SetActive(false);
        secondCv.SetActive(true);
    }

    public void NextScene()
    {
        SceneManager.LoadScene("MainScene");
    }
}
