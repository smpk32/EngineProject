using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EngineCtrl : MonoBehaviour
{
    private Transform tr;
    private Animator anim;

    private Ray ray;
    private RaycastHit hit;
    private Camera arCam;
    private GameObject panel;
    public GameObject text;
    private TextMeshProUGUI txt;

    private GameObject[] parts;
    private Transform[] subParts;
    //private BoxCollider partsCol;
    private Enginei4 instance;

    private AudioSource aSource;
    private AudioClip clip;
    private GameObject[] button;



    private MeshRenderer rd;

    private int rdParts;

    private float turnSpeed;

    private bool click;
    // Start is called before the first frame update
    void Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        parts = GameObject.FindGameObjectsWithTag("PARTS");
        arCam = Camera.main.GetComponent<Camera>();
        panel = GameObject.Find("Canvas").transform.Find("Panel").gameObject;
        button = GameObject.FindGameObjectsWithTag("BUTTON");
        instance = GetComponent<Enginei4>();

        aSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        Turn();
        Clicked();
        ChoiceParts();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void Warning()
    {

        rdParts = Random.Range(0, parts.Length - 1);
        Debug.Log("파츠" + parts.Length);
        parts[rdParts].gameObject.layer = 8;
        subParts = parts[rdParts].GetComponentsInChildren<Transform>(true);
        //partsCol = parts[rdParts].GetComponent<BoxCollider>();
        //partsCol.enabled = true;
        for (int i = 1; i < subParts.Length; i++)
        {
            rd = subParts[i].GetComponent<MeshRenderer>();
            rd.materials[rd.materials.Length - 1].SetColor("_OutlineColor", new Color(1, 0, 0, 0.5f));
        }
    }

    void Clicked()
    {
        if (Input.touchCount == 0) return;
        if (Input.touchCount >= 2 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (click == false)
            {
                anim.SetBool("EngineTouch", true);
                click = true;
                for (int i = 0; i < button.Length; i++)
                {
                    button[i].SetActive(false);
                }
            }
            else if (click == true)
            {
                anim.SetBool("EngineTouch", false);
                click = false;
                for (int i = 0; i < button.Length; i++)
                {
                    button[i].SetActive(true);
                }
                for (int i = 1; i < subParts.Length; i++)
                {
                    rd = subParts[i].GetComponent<MeshRenderer>();
                    rd.materials[rd.materials.Length - 1].SetColor("_OutlineColor", new Color(0, 0, 0, 0));
                }
            }
        }
    }

    void Turn()
    {
        if (Input.touchCount == 0) return;
        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            float speedX = Input.GetTouch(0).deltaPosition.x / Input.GetTouch(0).deltaTime;
            turnSpeed += speedX;
            tr.transform.rotation = Quaternion.Euler(0, turnSpeed * Time.deltaTime, 0);
        }
    }

    void ChoiceParts()
    {
        if (Input.touchCount == 0) return;

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit, 100.0f, 1 << 8))
            {
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i].SetActive(false);
                }
                parts[rdParts].SetActive(true);
                parts[rdParts].transform.localScale = new Vector3(5, 5, 5);
                parts[rdParts].transform.Translate(0, -8, 0);
                panel.SetActive(true);
                txt = text.GetComponent<TextMeshProUGUI>();
                txt.text = parts[rdParts].name + "가 고장났습니다.";
            }
        }
    }

    public void FixButton()
    {
        parts[rdParts].gameObject.layer = 0;
        parts[rdParts].transform.localScale = new Vector3(1, 1, 1);
        anim.SetBool("EngineTouch", false);
        click = false;
        panel.SetActive(false);
        StartCoroutine(Fixed());
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i].SetActive(true);
        }
        for (int i = 1; i < subParts.Length; i++)
        {
            rd = subParts[i].GetComponent<MeshRenderer>();
            rd.materials[rd.materials.Length - 1].SetColor("_OutlineColor", new Color(0, 0, 0, 0));
        }
    }

    IEnumerator Fixed()
    {
        clip = Resources.Load("congratulation") as AudioClip;
        aSource.clip = clip;
        aSource.Play();
        for (int i = 0; i < button.Length; i++)
        {
            button[i].SetActive(true);
        }
        while (aSource.isPlaying)
        {
            yield return 0;
        }
        StartCoroutine(MoveEngine());

    }
    IEnumerator MoveEngine()
    {
        clip = Resources.Load("engine") as AudioClip;
        aSource.clip = clip;
        aSource.Play();
        instance.enabled = true;
        yield return new WaitForSeconds(5);
        aSource.Stop();
        instance.enabled = false;
    }

    public void ButtonClick(int i)
    {
        clip = Resources.Load("Button-4") as AudioClip;
        aSource.clip = clip;
        aSource.Play();
        instance.SetVariation(i);
    }
}