using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("----------GameObjects----------")]
    public GameObject BlockedLevel2;
    public GameObject BlockedMrGuest;
    public GameObject unlockLevel2Button;
    public GameObject unlockMrGuestButton;
    public GameObject Level2Button;
    public GameObject MrGuestButton;

    [Header("----------Skins----------")]
    public GameObject sebas;
    public GameObject mrGuest;

    [Header("----------Coins----------")]
    public TextMeshProUGUI coinsText;
    private int _numberOfCoins;

    [Header("----------Bool----------")]
    private bool level2IsUnlocked;
    private bool mrGuestIsUnlocked;
    public bool mrGuestIsSelected;
    public bool sebasIsSelected;

    // Start is called before the first frame update
    void Start()
    {
        //Gets the aready existing number of coins from PlayerMovement script and sets it to _numberOfCoins
        //Displays the number of coins on start
        _numberOfCoins = PlayerPrefs.GetInt("numberOfCoins", 0);

        //Updates on start if level 2 is unlocked or if mrguest is unlocked
        level2IsUnlocked = (PlayerPrefs.GetInt("level2IsUnlocked") != 0);
        mrGuestIsUnlocked = (PlayerPrefs.GetInt("mrGuestIsUnlocked") != 0);
        MrGuestIsUnlocked();
        Level2IsUnlocked();

        sebasIsSelected = (PlayerPrefs.GetInt("sebasIsSelected") != 0);
        mrGuestIsSelected = (PlayerPrefs.GetInt("mrGuestIsSelected") != 0);

        if(sebasIsSelected)
        {
            SelectSebas();
        }
        else if(mrGuestIsSelected)
        {
            SelectMrGuest();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Displays the number of coins when its value updates
        coinsText.text = Mathf.FloorToInt(_numberOfCoins).ToString("D3");

        //Updates if level 2 is unlocked or if mrguest is unlocked
        PlayerPrefs.GetInt("level2IsUnlocked", (level2IsUnlocked ? 1 : 0));
        PlayerPrefs.GetInt("mrGuestIsUnlocked", (mrGuestIsUnlocked ? 1 : 0));
    }

    public void BuyLevel2()
    {
        if(_numberOfCoins >= 10) //If the number of coins is greater or equal to "10"
        {
            BlockedLevel2.SetActive(false);
            unlockLevel2Button.SetActive(false);
            Level2Button.SetActive(true);
            _numberOfCoins = _numberOfCoins-10;
            level2IsUnlocked = true;
            PlayerPrefs.SetInt("numberOfCoins", _numberOfCoins); //The value of PlayerPrefs "numberOfCoins" is updated with "_numberOfCoins"
        }
        Level2IsUnlocked();
    }

    public void Level2IsUnlocked()
    {
        if(level2IsUnlocked == true)
        {
            BlockedLevel2.SetActive(false);
            unlockLevel2Button.SetActive(false);
            Level2Button.SetActive(true);
        }
        PlayerPrefs.SetInt("level2IsUnlocked", (level2IsUnlocked ? 1 : 0));
    }

    public void BuyMrGuest()
    {
        if(_numberOfCoins >= 10) //If the number of coins is greater or equal to "5"
        {
            BlockedMrGuest.SetActive(false);
            unlockMrGuestButton.SetActive(false);
            MrGuestButton.SetActive(true);
            _numberOfCoins = _numberOfCoins-10;
            mrGuestIsUnlocked = true;
            PlayerPrefs.SetInt("numberOfCoins", _numberOfCoins); //The value of PlayerPrefs "numberOfCoins" is updated with "numberOfCoins"
        }
        MrGuestIsUnlocked();
    }
    
    public void MrGuestIsUnlocked()
    {
        if(mrGuestIsUnlocked == true)
        {
            BlockedMrGuest.SetActive(false);
            unlockMrGuestButton.SetActive(false);
            MrGuestButton.SetActive(true);
        }
        PlayerPrefs.SetInt("mrGuestIsUnlocked", (mrGuestIsUnlocked ? 1 : 0));
    }

    public void SelectSebas()
    {
        sebas.SetActive(true);
        mrGuest.SetActive(false);
        sebasIsSelected = true;
        mrGuestIsSelected = false;
        PlayerPrefs.SetInt("sebasIsSelected", (sebasIsSelected ? 1 : 0));
        PlayerPrefs.SetInt("sebasIsSelected", (sebasIsSelected ? 1 : 0));
    }

    public void SelectMrGuest()
    {
        sebas.SetActive(false);
        mrGuest.SetActive(true);
        sebasIsSelected = false;
        mrGuestIsSelected = true;
        PlayerPrefs.SetInt("mrGuestIsSelected", (mrGuestIsSelected ? 1 : 0));
        PlayerPrefs.SetInt("sebasIsSelected", (sebasIsSelected ? 1 : 0));
    }
}