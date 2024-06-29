using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public FixedTouchField touchfield;
    public float speed = 10f;
    private Vector3 targetPosition;
    private float speedstore;
    private float minX;
    private float maxX;
    private float fixedY;
    private Camera mainCamera;

    void Start()
    {
        // Ekran sınırlarını belirle
        mainCamera = Camera.main;
        Vector3 screenLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, transform.position.z - mainCamera.transform.position.z));
        Vector3 screenRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, transform.position.z - mainCamera.transform.position.z));

        speedstore = speed;
        targetPosition = Vector3.zero;
        minX = screenLeft.x + 0.85f;
        maxX = screenRight.x - 0.85f;

        // Objenin sabit Y pozisyonunu belirle
        fixedY = transform.position.y;
    }

    void FixedUpdate()
    {
        // Dokunma alanından gelen hareket miktarını ekle
        if (Input.GetButton("Fire1"))
        {
            speed = speedstore;
            targetPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetButtonUp("Fire1"))
            speed = 0f;

        // Hedef pozisyona doğru hareket et
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // X pozisyonunu belirli sınırlar içinde tut
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), fixedY, 1f);
    }

    #region //gamemods
    public void gamemodepaddlelengt()
    {
        transform.localScale = new Vector3(0.35f, 0.5f, 1f);
    }

    public void gamemodepaddlelengtfalse()
    {
        transform.localScale = new Vector3(0.55f, 0.6f, 1f);
    }
    #endregion
}
