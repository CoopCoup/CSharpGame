using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    public GameObject ammoPrefab;
    public TopDownCharacterController playerScript;
    List<Ammo> ammunition = new List<Ammo>();

    private void Start()
    {
        DrawAmmo();
    }

    private void Update()
    {
        DrawAmmo();
    }

    private void OnEnable()
    {
        TopDownCharacterController.OnAmmoChange += DrawAmmo;
    }

    private void OnDisable()
    {
        TopDownCharacterController.OnAmmoChange -= DrawAmmo;
    }

    public void DrawAmmo()
    {
        ClearAmmo();

        for(int i = 0; i < 5; i++) 
        {
            CreateEmptyAmmo();
        }

        for(int i = 0; i < ammunition.Count; i++)
        {
            int ammoStatusRemainder = (int)Mathf.Clamp(playerScript.ammoCount - i, 0, 1);
            ammunition[i].SetAmmoImage((AmmoStatus)ammoStatusRemainder);
        }
    }

    public void CreateEmptyAmmo()
    {
        GameObject newAmmo = Instantiate(ammoPrefab);
        newAmmo.transform.SetParent(transform);

        Ammo ammoComponent = newAmmo.GetComponent<Ammo>();
        ammoComponent.SetAmmoImage(AmmoStatus.Empty);
        ammunition.Add(ammoComponent);
    }

    public void ClearAmmo()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        ammunition = new List<Ammo>();
    }
}
