using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{
    public int hitcounter;

    [SerializeField] float speed;
    [SerializeField] GameObject respanel;
    [SerializeField] TextMeshPro hitcountertext;
    [SerializeField] TextMeshProUGUI finalcountertext;
    [SerializeField] TextMeshProUGUI recordtext;
    [SerializeField] TextMeshProUGUI hitcounterbestscoretext;
    [SerializeField] AudioSource sesci;
    [SerializeField] AudioClip hitclip;
    [SerializeField] Vector2 direction;

    [HideInInspector] public Rigidbody2D rb;

    private gamemanager managersc;
    private Animator anim, hitcounteranimator;
    private AiryUIAnimatedElement animatedrespan;
    private bool isOutOfView = false;
    private bool GameStart = false;
    private bool recordt = false;
    private float outOfViewTime = 0f;
    private float outOfViewDuration = 2f;

    void Start()
    {
        hitcounterbestscoretext.text = "Best " + PlayerPrefs.GetFloat("hit").ToString();
        managersc = gamemanager.managersc;
        anim = GetComponent<Animator>();
        hitcounteranimator = hitcountertext.transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void basla()
    {
        direction = Vector2.one.normalized;
        GameStart = true;
    }

    void Update()
    {
        if (!GameStart)
            return;

        if (direction.y == 0f || (direction.y < 0.2f && direction.y > -0.2f))
            direction = new Vector2(direction.x, direction.y + 0.5f);

        if (direction.x == 0f || (direction.x < 0.2f && direction.x > -0.2f))
            direction = new Vector2(direction.x + 0.5f, direction.y);

        direction = new Vector2(Mathf.Clamp(direction.x, -0.75f, 0.75f), Mathf.Clamp(direction.y, -0.75f, 0.75f));
        speed = Mathf.Clamp(speed, 0f, 14f);

        hitcountertext.text = hitcounter.ToString();
        rb.velocity = direction * speed;

        // Topun kamera görüş alanı dışında kalma süresini kontrol et
        if (IsOutOfView())
        {
            if (!isOutOfView)
            {
                isOutOfView = true;
                outOfViewTime = 0f;
            }
            else
            {
                outOfViewTime += Time.deltaTime;
                if (outOfViewTime >= outOfViewDuration)
                {
                    OnOutOfView();
                }
            }
        }
        else
        {
            isOutOfView = false;
            outOfViewTime = 0f;
        }
    }

    #region //gamemods
    public void gamemodespeed()
    {
        speed += 4f;
    }

    public void gamemodespeedfalse()
    {
        speed -= 4f;
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "star")
        {
            managersc.selectedMode.fonksiyondeaktif.Invoke();
            managersc.gamemodepan.gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "paddle")
        {
            speed += 0.333f;
            hitcounter++;

            if (hitcounter > PlayerPrefs.GetFloat("hit"))
            {
                PlayerPrefs.SetFloat("hit", hitcounter);
                recordt = true;
            }

            hitcounteranimator.Play("counter");
        }

        sesci.PlayOneShot(hitclip);
        direction = Vector2.Reflect(direction, collision.contacts[0].normal);
        anim.Play("hit");

        float randomTorque = UnityEngine.Random.Range(-10f, 10f);
        rb.AddTorque(randomTorque);
    }

    bool IsOutOfView()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1;
    }


    void OnOutOfView()
    {
        if(respanel.activeSelf == false)
        {
            respanel.gameObject.SetActive(true);
            finalcountertext.text = "Total hits " + hitcounter.ToString();

            if(recordt)
                recordtext.gameObject.SetActive(true);

            if (animatedrespan != null)
                animatedrespan.ShowElement();
        }
    }
}
