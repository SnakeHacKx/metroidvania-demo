using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private List<GameObject> weapons;
    public int activeWeapon;

    public List<GameObject> GetAllWeapons() { return weapons; }

    public SwordController GetWeaponAt(int index) { return weapons[index].GetComponent<SwordController>(); }

    void Start()
    {
        weapons = new List<GameObject>();

        // Cuando se hace un bucle a transform, se consiguen los hijos del gameObject
        foreach (Transform weapon in transform) 
        {
            weapons.Add(weapon.gameObject);
        }

        for (int i = 0; i < weapons.Count; i++)
        {
            // Si i == activeWeapon sera true y justo ese arma no se desactivara (ya que al inicio debe haber una activa)
            weapons[i].SetActive(i == activeWeapon);
        }
    }

    public void ChangeWeapon(int newWeapon)
    {
        weapons[activeWeapon].SetActive(false);
        weapons[newWeapon].SetActive(true);
        activeWeapon = newWeapon;
    }

    
    void Update()
    {
        
    }
}
