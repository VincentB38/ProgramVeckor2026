using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    // Variables for Weapon Description
    [SerializeField] private string name;
    [SerializeField] private Image icon;

    // Variables for Weapon Stats
    [SerializeField] private int damage;
    [SerializeField] private int ammoMagazine;
    [SerializeField] private int currentAmmo;

    [SerializeField] private float fireRate;
    [SerializeField] private float reloadRate;
    [SerializeField] private float bulletSpeed;

    [SerializeField] private float range;

    // Variables for Weapon Components
    public Transform MuzzlePos;
    public Transform BulletParent;

    public void Fire(GameObject bullet)
    {
        Vector3 targetDirection = GetMousePosition();

        GameObject newBullet = Instantiate(bullet, MuzzlePos);
        newBullet.transform.parent = BulletParent;
        newBullet.GetComponent<Rigidbody2D>().linearVelocity = targetDirection * bulletSpeed;

        currentAmmo--; // Reload type, Press to reload, or auto reload?

        Debug.Log("Fired");
    }

    // Gets mouse position and converts to world. Slightly inaccurate (Will try to fix)
    private Vector3 GetMousePosition()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouseScreen);
        mousePos.z = 0;
        return mousePos;
    }

    protected virtual void Reload()
    {
        // Virtual to make it possible to change animation or any other variables in subclasses

        WaitForSeconds(reloadRate);
        currentAmmo = ammoMagazine;
    }

    IEnumerator WaitForSeconds(float time)
    {
        yield return WaitForSeconds(time);
    }

    #region GetVariables
    public string GetName()
    {
        return name;
    }

    public int GetDamage()
    {
        return damage;
    }

    public int GetAmmoMagazine()
    {
        return ammoMagazine;
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public float GetReloadRate()
    {
        return reloadRate;
    }

    public float GetRange()
    {
        return range;
    }
    #endregion
}
