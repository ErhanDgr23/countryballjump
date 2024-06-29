using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectsnapper : MonoBehaviour
{
    [SerializeField] GameObject[] objects;

    [SerializeField] GameObject topObject;
    [SerializeField] GameObject rightObject;
    [SerializeField] GameObject leftObject;
    [SerializeField] Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        PlaceObjectsAtScreenEdges();
        ActivateRandomObject();
    }

    void ActivateRandomObject()
    {
        // Tüm objeleri deaktif et
        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        // Rasgele bir objeyi aktif et
        if (objects.Length > 0)
        {
            int randomIndex = Random.Range(0, objects.Length);
            objects[randomIndex].SetActive(true);
        }
    }

    void PlaceObjectsAtScreenEdges()
    {
        // Ekranın üst orta noktası
        Vector3 topCenter = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 1, mainCamera.nearClipPlane));
        // Ekranın sağ orta noktası
        Vector3 rightCenter = mainCamera.ViewportToWorldPoint(new Vector3(1, 0.5f, mainCamera.nearClipPlane));
        // Ekranın sol orta noktası
        Vector3 leftCenter = mainCamera.ViewportToWorldPoint(new Vector3(0, 0.5f, mainCamera.nearClipPlane));

        // Objelerin Z koordinatını ayarlamak için
        topCenter.z = topObject.transform.position.z;
        rightCenter.z = rightObject.transform.position.z;
        leftCenter.z = leftObject.transform.position.z;

        // Objeleri yeni pozisyonlarına yerleştirme
        topObject.transform.position = topCenter + new Vector3(0f, 1.35f, 0f);
        rightObject.transform.position = rightCenter + new Vector3(1.35f, 0f, 0f);
        leftObject.transform.position = leftCenter + new Vector3(-1.35f, 0f, 0f);
    }
}
