using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class SpellsInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textSomeInfo;

    private int activeSpellNum = -1;
    private int activeSpellSlot = 0;

    private int[] spellNumFromSave = new int[8];

    private int[] runeArray = new int[3];

    string[] title =
    {
        "None",
        "MMM",
        "MMP",
        "MMS",
        "MMU",
        "MMI",
        "PPM",
        "PPP",
        "PPS",
        "PPU",
        "PPI",
        "SSM",
        "SSP",
        "SSS",
        "SSU",
        "SSI",
        "UUM",
        "UUP",
        "UUS",
        "UUU",
        "UUI",
        "IIM",
        "IIP",
        "IIS",
        "IIU",
        "III",
        "PUM",
        "SIP",
        "UMS",
        "IPU",
        "MSI",
        "MPI",
        "PSM",
        "SUP",
        "UIS",
        "IMU"
    };

    public void SetValues(int[] spellNumFromSave)
    {
        this.spellNumFromSave = spellNumFromSave;
        GameObject.Find("CharacterGirl").GetComponent<SpellManager>().SetValues(spellNumFromSave);
    }

    public int[] GetValues()
    {
        return spellNumFromSave;
    }

    public void PressOnSpell(int spellNum)
    {
        textSomeInfo.text = "Some info about " + title[spellNum];
        if(activeSpellSlot != 0)
        {
            spellNumFromSave[activeSpellSlot - 1] = spellNum;

            string objectPath = "Main Camera/InventoryCanvas/Spells/Slot" + activeSpellSlot.ToString();
            string imagePath = "SpellIcon/Icon" + spellNumFromSave[activeSpellSlot - 1].ToString();
            GameObject.Find(objectPath).GetComponent<Button>().GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);

            for(int i = 0; i < spellNumFromSave.Length; i++)
            {
                if (i == activeSpellSlot - 1) continue;

                if(spellNumFromSave[i] == spellNum)
                {
                    spellNumFromSave[i] = 0;
                    objectPath = "Main Camera/InventoryCanvas/Spells/Slot" + (i + 1).ToString();
                    imagePath = "SpellIcon/Icon" + spellNumFromSave[i].ToString();
                    GameObject.Find(objectPath).GetComponent<Button>().GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
                }
            }

            activeSpellSlot = 0;
        }
        else
        {
            activeSpellNum = spellNum;
        }
    }

    public void PressOnRune(int runeNum)
    {

    }

    public void ChangeSlot(int slot)
    {
        textSomeInfo.text = "Some info about " + title[spellNumFromSave[slot - 1]];
        if (activeSpellNum != -1)
        {
            spellNumFromSave[slot - 1] = activeSpellNum;
            string objectPath = "Main Camera/InventoryCanvas/Spells/Slot" + slot.ToString();
            string imagePath = "SpellIcon/Icon" + spellNumFromSave[slot - 1].ToString();
            GameObject.Find(objectPath).GetComponent<Button>().GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);

            for (int i = 0; i < spellNumFromSave.Length; i++)
            {
                if (i == slot - 1) continue;
                if (spellNumFromSave[i] == activeSpellNum)
                {
                    spellNumFromSave[i] = 0;
                    objectPath = "Main Camera/InventoryCanvas/Spells/Slot" + (i + 1).ToString();
                    imagePath = "SpellIcon/Icon" + spellNumFromSave[i].ToString();
                    GameObject.Find(objectPath).GetComponent<Button>().GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
                }
            }

            activeSpellNum = -1;
        }
        else if(activeSpellSlot != 0 && slot != activeSpellSlot)
        {
            int value = spellNumFromSave[activeSpellSlot - 1];
            spellNumFromSave[activeSpellSlot - 1] = spellNumFromSave[slot - 1];
            spellNumFromSave[slot - 1] = value;

            string objectPath = "Main Camera/InventoryCanvas/Spells/Slot" + slot.ToString();
            string imagePath = "SpellIcon/Icon" + spellNumFromSave[slot - 1].ToString();
            GameObject.Find(objectPath).GetComponent<Button>().GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);

            objectPath = "Main Camera/InventoryCanvas/Spells/Slot" + activeSpellSlot.ToString();
            imagePath = "SpellIcon/Icon" + spellNumFromSave[activeSpellSlot - 1].ToString();
            GameObject.Find(objectPath).GetComponent<Button>().GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);

            activeSpellSlot = 0;
        }
        else
        {
            activeSpellSlot = slot;
        }
    }

    public void NewInfoInInventory()
    {
        for (int i = 0; i < spellNumFromSave.Length; i++)
        {
            string objectPath = "Main Camera/InventoryCanvas/Spells/Slot" + (i + 1).ToString();
            string imagePath = "SpellIcon/Icon" + spellNumFromSave[i].ToString();
            GameObject.Find(objectPath).GetComponent<Button>().GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
        }
    }

    public void FillSpellManager()
    {
        GameObject.Find("CharacterGirl").GetComponent<SpellManager>().SetValues(spellNumFromSave);
    }
    enum SpellName
    {
        None,
        MMM,
        MMP,
        MMS,
        MMU,
        MMI,
        PPM,
        PPP,
        PPS,
        PPU,
        PPI,
        SSM,
        SSP,
        SSS,
        SSU,
        SSI,
        UUM,
        UUP,
        UUS,
        UUU,
        UUI,
        IIM,
        IIP,
        IIS,
        IIU,
        III,
        PUM,
        SIP,
        UMS,
        IPU,
        MSI,
        MPI,
        PSM,
        SUP,
        UIS,
        IMU
    }
}
