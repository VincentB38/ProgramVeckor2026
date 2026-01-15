using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform weaponRoot;// Player -> WeaponRoot
    private Transform playerTransform;
    private bool isWalking;
    public float yOffset;

    [Header("Weapon Slots (max 2)")]
    public GameObject weaponSlot1Prefab;
    public GameObject weaponSlot2Prefab;
    public TextMeshProUGUI WeaponEquipped;

    private Vector3 weaponRootPos;

    private Weapon[] weapons = new Weapon[2];
    private int activeWeaponIndex = 0;

    private void Start()
    {
        EquipWeapons();
        SwitchWeapon(0);

        weaponRootPos = weaponRoot.localPosition;

        playerTransform = gameObject.transform;
    }

    public void ChangeWeapon(GameObject newWeaponPrefab)
    {

        if (weaponSlot2Prefab == null)
        {
            weaponSlot2Prefab = newWeaponPrefab;

            // Spawn the new weapon in slot 2
            weapons[1] = SpawnWeapon(newWeaponPrefab);

            // Equip slot 2
            SwitchWeapon(1);
            activeWeaponIndex = 1;

            print("Weapon assigned to slot 2 and equipped: " + newWeaponPrefab.name);
            return;
        }

        // Destroy the old weapon in the active slot
        if (weapons[activeWeaponIndex] != null)
        {
            Destroy(weapons[activeWeaponIndex].gameObject);
            weapons[activeWeaponIndex] = null;
        }

        // Assign the new prefab to the correct slot
        if (activeWeaponIndex == 0)
        {
            weaponSlot1Prefab = newWeaponPrefab;
        }
        else if (activeWeaponIndex == 1)
        {
            weaponSlot2Prefab = newWeaponPrefab;
        }

        // Spawn the new weapon
        Weapon newWeapon = SpawnWeapon(newWeaponPrefab);
        weapons[activeWeaponIndex] = newWeapon;

        // Activate the new weapon
        SwitchWeapon(activeWeaponIndex);
    }

    private void Update()
    {
        HandleSwitchInput();
        HandleShooting();

        weaponRoot.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);

        isWalking = playerTransform.GetComponent<Animator>().GetBool("IsWalking");
        // When walking, pull the gun down to follow animation
        if (isWalking)
        {
            weaponRoot.transform.localPosition = new Vector3(weaponRootPos.x, -yOffset, weaponRootPos.z);
        }
        else
        {
            weaponRoot.transform.localPosition = weaponRootPos;
        }
    }

    private void HandleShooting()
    {
        Weapon weapon = weapons[activeWeaponIndex];
        if (weapon == null) return;

        if (Mouse.current.leftButton.isPressed)
        {
            weapon.TryFire();
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
    
    // Spawns Weapon on Player
    private Weapon SpawnWeapon(GameObject weaponPrefab)
    {
        GameObject weaponObj = Instantiate(weaponPrefab, weaponRoot);
        weaponObj.transform.localPosition = Vector3.zero;
        weaponObj.transform.localRotation = Quaternion.identity;
        weaponObj.transform.localScale = Vector3.one;

        return weaponObj.GetComponent<Weapon>();
    }

    // Switches Weapons
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

        if (index == 0 && weaponSlot1Prefab != null)
            WeaponEquipped.text = "Weapon Slot 1: " + weaponSlot1Prefab.name;
        else if (index == 1 && weaponSlot2Prefab != null)
            WeaponEquipped.text = "Weapon Slot 2: " + weaponSlot2Prefab.name;
    }
}
