using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponRoot; // Player -> WeaponRoot

    [Header("Weapon Slots (max 2)")]
    [SerializeField] private GameObject weaponSlot1Prefab;
    [SerializeField] private GameObject weaponSlot2Prefab;

    private Weapon[] weapons = new Weapon[2];
    private int activeWeaponIndex = 0;

    private void Start()
    {
        EquipWeapons();
        SwitchWeapon(0);
    }

    private void Update()
    {
        HandleSwitchInput();
        HandleShooting();
    }

    private void HandleShooting()
    {
        Weapon weapon = weapons[activeWeaponIndex];
        if (weapon == null) return;

        if (Keyboard.current.spaceKey.isPressed)
        {
            weapon.TryFire(); // ✅ CORRECT
        }
    }

    private void HandleSwitchInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
            SwitchWeapon(0);

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
            SwitchWeapon(1);
    }

    private void EquipWeapons()
    {
        if (weaponSlot1Prefab != null)
            weapons[0] = SpawnWeapon(weaponSlot1Prefab);

        if (weaponSlot2Prefab != null)
            weapons[1] = SpawnWeapon(weaponSlot2Prefab);
    }

    private Weapon SpawnWeapon(GameObject weaponPrefab)
    {
        GameObject weaponObj = Instantiate(weaponPrefab, weaponRoot);
        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;
        weaponObj.transform.localScale = Vector3.one;

        return weaponObj.GetComponent<Weapon>();
    }

    private void SwitchWeapon(int index)
    {
        if (index < 0 || index >= weapons.Length) return;
        if (weapons[index] == null) return;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] != null)
                weapons[i].gameObject.SetActive(i == index);
        }

        activeWeaponIndex = index;
    }
}
