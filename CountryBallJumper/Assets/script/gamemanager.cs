using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class customgamemode
{
    public string name;
    public string explanation;
    public UnityEvent fonksiyonaktif;
    public UnityEvent fonksiyondeaktif;
}

public class gamemanager : MonoBehaviour
{
    public static gamemanager managersc;

    public customgamemode selectedMode;

    [SerializeField] int[] leveller;
    [SerializeField] GameObject[] yildizlar;
    [SerializeField] GameObject[] sesler;
    [SerializeField] GameObject winpan;
    [SerializeField] AudioListener kameralistener;
    [SerializeField] Image soundimage;
    [SerializeField] Sprite soundon, soundoff;
    [SerializeField] customgamemode[] gamemode;
    [SerializeField] TextMeshProUGUI leveltext;
    [SerializeField] TextMeshProUGUI gamemodetext;
    [SerializeField] TextMeshProUGUI reachtext;
    [SerializeField] BallMovement ballscript;
    [SerializeField] int level;

    [HideInInspector] public GameObject gamemodepan;

    private AiryUIAnimatedElement animatedwinpan;
    int reachvalue;
    [SerializeField] bool levenexed, soundbool, soundchanged;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        managersc = this;
    }

    private void Start()
    {
        soundbool = true;
        gamemodepan = gamemodetext.transform.parent.gameObject;
        animatedwinpan = winpan.transform.GetChild(0).GetComponent<AiryUIAnimatedElement>();
        sesler[1] = GameObject.FindGameObjectWithTag("musicplayer");

        if (!PlayerPrefs.HasKey("level"))
        {
            level = 1;
            PlayerPrefs.SetInt("level", level);
        }
        else
            level = PlayerPrefs.GetInt("level");

        leveltext.text = "level " + level;

        float probability = UnityEngine.Random.value;

        if (probability <= 0.4f)
            ozellikaktif();

        reachvalue = nextlevelint();
        reachtext.text = "Reach " + reachvalue + " hit";
    }

    public void ses()
    {
        soundbool = !soundbool;
        soundchanged = false;
    }

    public int nextlevelint()
    {
        int index = Mathf.Clamp(level - 1, 0, leveller.Length - 1);

        if (level > leveller.Length)
            index = leveller.Length - 1;

        int value = leveller[index];
        return value;
    }

    public void kapatoyun()
    {
        Application.Quit();
    }

    private void LateUpdate()
    {
        if (soundchanged)
            return;

        if (!soundbool)
        {
            foreach (var item in sesler)
            {
                item.gameObject.SetActive(false);
            }

            soundimage.sprite = soundoff;
            soundchanged = true;
        }
        else
        {
            foreach (var item in sesler)
            {
                item.gameObject.SetActive(true);
            }

            soundimage.sprite = soundon;
            soundchanged = true;
        }
    }

    public void nextlevel()
    {
        if (!levenexed)
        {
            level++;
            PlayerPrefs.SetInt("level", level);
            StartCoroutine(beklenextload());
            levenexed = true;
        }
    }

    public void reslevel()
    {
        if (!levenexed)
        {
            StartCoroutine(beklenextload());
            levenexed = true;
        }
    }

    IEnumerator beklenextload()
    {
        yield return new WaitForSeconds(0.3f);
        Application.LoadLevel(0);
    }

    void ozellikaktif()
    {
        gamemodepan.gameObject.SetActive(true);

        // Gamemode dizisi boşsa veya null ise işlem yapma
        if (gamemode == null || gamemode.Length == 0)
        {
            Debug.LogWarning("Gamemode dizisi boş veya null.");
            return;
        }

        // Rasgele bir game mode seç
        int randomIndex = UnityEngine.Random.Range(0, gamemode.Length);
        selectedMode = gamemode[randomIndex];

        // Seçilen game mode ile ilgili işlemleri yapabilirsiniz
        selectedMode.fonksiyonaktif.Invoke();
        rasgeleyildizspawnla();
        gamemodetext.text = selectedMode.explanation;
    }

    public void rasgeleyildizspawnla()
    {
        // Dizi boşsa veya null ise işlem yapma
        if (yildizlar == null || yildizlar.Length == 0)
        {
            Debug.LogWarning("Yıldızlar dizisi boş veya null.");
            return;
        }

        // Rasgele bir yıldız seç
        int randomIndex = UnityEngine.Random.Range(0, yildizlar.Length);
        GameObject selectedStar = yildizlar[randomIndex];

        // Seçilen yıldızı etkinleştir
        selectedStar.SetActive(true);
    }

    public void restartgame()
    {
        Application.LoadLevel(level);
    }
}
