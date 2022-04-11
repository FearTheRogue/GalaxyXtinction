using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private List<Weapon> weapons = new List<Weapon>();
    //[SerializeField] private int selectedWeapon = 0;

    private void Start()
    {
        SelectWeapon();
    }

    private void SelectWeapon()
    {
        foreach (Weapon w in weapons)
        {

        }
    }
}
