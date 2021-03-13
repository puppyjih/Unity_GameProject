using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : Selectable
{
    // Start is called before the first frame update
 

    private GameObject player;
    private Transform Hand;
    
    WeaponInfo weaponInfo;
    
    [SerializeField] private GameObject[] inventory;
    private Image[] weaponImage;
    private Toggle[] toggle;
    private int inventorySize = 0;
    //private PlayerStat playerStat;

    class WeaponInfo
    {
        public GameObject[] weaponInventory;
        public int selected = 0;
        public WeaponInfo() { }

        public void setSize(int inventorySize)
        {
            weaponInventory = new GameObject[inventorySize];
        }

        public void ChangeSelectedWeapon(Transform Hand)
        {
            weaponInventory[selected] = Hand.childCount > 0 ? Hand.GetChild(0).gameObject : null;
        }
    };

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Hand = player.transform.Find("Hand");
        inventorySize = inventory.Length;
        toggle = new Toggle[inventorySize];
        weaponImage = new Image[inventorySize];
        weaponInfo = new WeaponInfo();
        weaponInfo.setSize(inventorySize);

        for (int i = 0; i < inventorySize; i++)
        {
            toggle[i] = inventory[i].transform.Find("Toggle").GetComponent<Toggle>();
            weaponImage[i] = toggle[i].transform.GetChild(0).GetComponent<Image>();
            weaponImage[i].sprite = Hand.childCount > 0 ? Hand.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite : player.transform.Find("DefaultWeapon").GetChild(0).GetComponent<SpriteRenderer>().sprite;
            weaponInfo.weaponInventory[i] = Hand.childCount > 0 ? Hand.GetChild(0).gameObject : null;
            
        }
        toggle[0].isOn = true;
        //playerStat = player.GetComponent<PlayerStat>();
    }

    //public void CheckTogglePushed(bool isOn)
    //{
    //    if(isOn)
    //    {
    //        weaponImage
    //    }
    //}
    // Update is called once per frame
    public void UpdateWeapon()
    {
        weaponImage[weaponInfo.selected].sprite = Hand.childCount > 0 ? Hand.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite : player.transform.Find("DefaultWeapon").GetChild(0).GetComponent<SpriteRenderer>().sprite;
        weaponInfo.ChangeSelectedWeapon(Hand);
        toggle[weaponInfo.selected].GetComponent<ToggleManager>().weapon = weaponInfo.weaponInventory[weaponInfo.selected];
    }

    public void LoadWeapon()
    {
        //toggle[0].onValueChanged.
        OnDisable();
    }

}
