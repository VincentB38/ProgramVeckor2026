using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Transform weaponRoot;     // Body/WeaponRoot
    [SerializeField] private GameObject weaponPrefab;  // AssaultRifle
    [SerializeField] private GameObject bulletPrefab;

    private Weapon currentWeapon;

    private void Start()
    {
        EquipWeapon();
    }

    private void Update()
    {
        if (currentWeapon == null) return;

        if (Keyboard.current.spaceKey.isPressed)
        {
            currentWeapon.TryFire(bulletPrefab);
        }
    }

    private void EquipWeapon()
    {
        GameObject weapon = Instantiate(weaponPrefab, weaponRoot);
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.identity;
        weapon.transform.localScale = Vector3.one;

        currentWeapon = weapon.GetComponent<Weapon>();
    }
}
