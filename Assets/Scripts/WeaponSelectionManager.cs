using UnityEngine;
using UnityEngine.UI;

public class WeaponSelectionManager : MonoBehaviour
{
    [Header("UI References")]
    public Button[] weaponButtons;
    public Button[] ammoButtons;
    public Button confirmButton;

    [Header("Default Selections")]
    public int defaultWeaponIndex = 0;
    public int defaultAmmoIndex = 0;

    private int selectedWeaponIndex = 0;
    private int selectedAmmoIndex = 0;
    private bool isInitialized = false;

    private WeaponGenerator weaponGenerator;
    private HeroBehavior hero;

    private void Start()
    {
        weaponGenerator = FindObjectOfType<WeaponGenerator>();
        hero = FindObjectOfType<HeroBehavior>();

        // Initialize default selections
        selectedWeaponIndex = defaultWeaponIndex;
        selectedAmmoIndex = defaultAmmoIndex;
        
        // Set up weapon buttons
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            int index = i; // Capture index for closure
            weaponButtons[i].onClick.AddListener(() => SelectWeapon(index));
        }

        // Set up ammo buttons
        for (int i = 0; i < ammoButtons.Length; i++)
        {
            int index = i; // Capture index for closure
            ammoButtons[i].onClick.AddListener(() => SelectAmmo(index + 1)); // +1 because 0 is "not selected"
        }

        // Set up confirm button
        confirmButton.onClick.AddListener(ConfirmSelection);

        // Highlight default selections
        UpdateButtonVisuals();
        isInitialized = true;
    }

    private void SelectWeapon(int index)
    {
        selectedWeaponIndex = index + 1; // +1 because 0 is "not selected"
        UpdateButtonVisuals();
    }

    private void SelectAmmo(int index)
    {
        selectedAmmoIndex = index;
        UpdateButtonVisuals();
    }

    private void UpdateButtonVisuals()
    {
        // Update weapon buttons
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            var colors = weaponButtons[i].colors;
            colors.normalColor = (i == selectedWeaponIndex - 1) ? Color.green : Color.white;
            weaponButtons[i].colors = colors;
        }

        // Update ammo buttons
        for (int i = 0; i < ammoButtons.Length; i++)
        {
            var colors = ammoButtons[i].colors;
            colors.normalColor = (i == selectedAmmoIndex - 1) ? Color.green : Color.white;
            ammoButtons[i].colors = colors;
        }
    }

    private void ConfirmSelection()
    {
        if (!isInitialized) return;

        // Apply selections to weapon generator
        weaponGenerator.GunType = selectedWeaponIndex;
        weaponGenerator.BulletType = selectedAmmoIndex;

        // Generate and equip the new weapon
        GameObject newWeapon = weaponGenerator.GunGenerator();
        
        // Find and destroy the old weapon if exists
        Weapon oldWeapon = hero.GetComponentInChildren<Weapon>();
        if (oldWeapon != null)
        {
            Destroy(oldWeapon.gameObject);
        }

        // Attach new weapon to hero
        newWeapon.transform.SetParent(hero.transform);
        newWeapon.transform.localPosition = new Vector3(0.05f, 0.15f, 0);
        newWeapon.transform.localRotation = Quaternion.identity;
        
        // Update hero's weapon reference
        hero.mWeaponHandler = newWeapon.GetComponent<Weapon>();
    }
}