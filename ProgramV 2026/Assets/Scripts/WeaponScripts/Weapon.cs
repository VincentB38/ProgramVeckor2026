using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

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
    private Vector2 startMuzzlePos;
    public SoundManager SoundThing;
    


    private int currentAmmo;
    private float nextFireTime;
    private bool isReloading;

    private void Start()
    {
        ReloadText = UIProvider.Instance.reloadText;
        startMuzzlePos = MuzzlePos.localPosition;

        SoundThing = FindAnyObjectByType<SoundManager>();

        if (currentAmmo <= 0)
        {
            currentAmmo = ammoMagazine;
        }
    }

    private void Update()
    {
       

        MuzzlePos.localPosition = startMuzzlePos;

        if (Keyboard.current.rKey.wasPressedThisFrame && currentAmmo < ammoMagazine)
        {
            StartCoroutine(Reload());
        }
    }

    private void OnEnable()
    {
        //currentAmmo = ammoMagazine;
        isReloading = false;

        if (ReloadText == null && UIProvider.Instance != null)
            ReloadText = UIProvider.Instance.reloadText;

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

        if (currentAmmo <= 0 && !isReloading)
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
        SoundThing.PlaySound(0, bullet.transform);

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
        if (ReloadText != null)
            ReloadText.text = "(Reloading)";
        yield return new WaitForSeconds(reloadRate);
        currentAmmo = ammoMagazine;
        isReloading = false;
        if (ReloadText != null)
            ReloadText.text = "";
    }

    #region Getters (optional for UI)
    public int GetCurrentAmmo() => currentAmmo;
    public int GetAmmoMagazine() => ammoMagazine;
    public int GetDamage() => damage;
    #endregion
}
