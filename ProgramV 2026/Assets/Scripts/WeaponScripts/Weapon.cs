using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private int damage = 10;
    [SerializeField] private int ammoMagazine = 30;
    [SerializeField] private float fireRate = 0.12f;
    [SerializeField] private float reloadRate = 1.2f;
    [SerializeField] private float bulletSpeed = 15f;
    [SerializeField] private TextMeshProUGUI ReloadText;

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;  

    [Header("References")]
    public Transform MuzzlePos;
    [SerializeField] private Transform bulletsParent;

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;

    private void Start()
    {
        ReloadText = GameObject.Find("Canvas").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (isReloading == true)
        {
            ReloadText.text = "(Reloading)";
        } else
        {
            ReloadText.text = "";
        }
    }

    private void OnEnable()
    {
        currentAmmo = ammoMagazine;
        isReloading = false;

        // Auto-find Bullets parent
        if (bulletsParent == null)
        {
            GameObject bullets = GameObject.Find("Bullets");
            if (bullets != null)
                bulletsParent = bullets.transform;
        }
    }

    public void TryFire()
    {
        if (Time.time < nextFireTime || isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        Fire();
        nextFireTime = Time.time + fireRate;
    }

    private void Fire()
    {
        Vector2 mouseWorld =
            Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 direction =
            (mouseWorld - (Vector2)MuzzlePos.position).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab,
            MuzzlePos.position,
            Quaternion.identity,
            bulletsParent
        );

        // Pass damage to bullet
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.SetDamage(damage);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;

        currentAmmo--;
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadRate);
        currentAmmo = ammoMagazine;
        isReloading = false;
    }

    #region Getters (optional for UI)
    public int GetCurrentAmmo() => currentAmmo;
    public int GetAmmoMagazine() => ammoMagazine;
    public int GetDamage() => damage;
    #endregion
}
