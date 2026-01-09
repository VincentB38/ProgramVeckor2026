using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    string name;

    int damage;
    int ammoMagazine;

    public Weapon(string name, int damage, int ammoMagazine)
    {
        this.name = name;
        this.damage = damage;
        this.ammoMagazine = ammoMagazine;
    }

    public string GetName()
    {
        return name;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetAmmo()
    {
        return ammoMagazine;
    }
}
