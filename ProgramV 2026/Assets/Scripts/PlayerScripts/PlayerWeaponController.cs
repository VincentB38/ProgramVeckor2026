using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    // Variables for Weapons

    // Access to two weapons at a time
    List<GameObject> equippedWeapons = new List<GameObject>();
    public GameObject currentWeapon;
    public GameObject bullet;

    // Variables for UI

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            currentWeapon.GetComponent<Weapon>().Fire(bullet);
        }
    }

    
}
