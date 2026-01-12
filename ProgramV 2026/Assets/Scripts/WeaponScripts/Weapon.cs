using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Weapon : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int ammoMagazine = 30;
    [SerializeField] private float fireRate = 0.12f;
    [SerializeField] private float reloadRate = 1.2f;
    [SerializeField] private float bulletSpeed = 15f;

    [Header("References")]
    public Transform MuzzlePos;
    [SerializeField] private Transform bulletsParent; // ← Bullets object

    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;

    private void Awake()
    {
        currentAmmo = ammoMagazine;

        // Auto-find Bullets object if not set
        if (bulletsParent == null)
        {
            GameObject bullets = GameObject.Find("Bullets");
            if (bullets != null)
                bulletsParent = bullets.transform;
        }
    }

    public void TryFire(GameObject bulletPrefab)
    {
        if (Time.time < nextFireTime || isReloading) return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        Fire(bulletPrefab);
        nextFireTime = Time.time + fireRate;
    }

    private void Fire(GameObject bulletPrefab)
    {
        Vector2 mouseWorld = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        Vector2 direction =
            (mouseWorld - (Vector2)MuzzlePos.position).normalized;

        GameObject bullet = Instantiate(
            bulletPrefab,
            MuzzlePos.position,
            Quaternion.identity,
            bulletsParent        // ✅ PARENTED HERE
        );

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
}
