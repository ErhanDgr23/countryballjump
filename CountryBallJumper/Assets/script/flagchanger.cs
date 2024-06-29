using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flagchanger : MonoBehaviour
{
    private Sprite[] flags;

    void Start()
    {
        // "Resource/flags" klasöründeki tüm spriteları yükler
        flags = Resources.LoadAll<Sprite>("flags");

        if (flags.Length > 0)
        {
            // Rastgele bir index seç
            int randomIndex = Random.Range(0, flags.Length);

            // Seçilen spritenı kullan
            Sprite selectedFlag = flags[randomIndex];

            // Seçilen spritenı bir yere atamak için örnek kullanım
            GetComponent<SpriteRenderer>().sprite = selectedFlag;
        }
        else
        {
            Debug.LogWarning("No sprites found in Resource/flags");
        }
    }
}
