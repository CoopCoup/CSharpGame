using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammo : MonoBehaviour
{
    public Sprite fullAmmo, emptyAmmo;
    Image ammoImage;

    private void Awake()
    {
        ammoImage = GetComponent<Image>();
    }

    public void SetAmmoImage(AmmoStatus status)
    {
        switch (status)
        {
            case AmmoStatus.Empty:
                   ammoImage.sprite = emptyAmmo;
                break;
            case AmmoStatus.Full:
                ammoImage.sprite = fullAmmo;
                break;
        }
    }
}

public enum AmmoStatus
{
    Empty = 0,
    Full = 1
}