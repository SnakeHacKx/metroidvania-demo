using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDScreen : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] List<Image> heartImages;

    [Header("Coins")]
    [SerializeField] TMP_Text coinsAmountText;
    
    [Header("Power Ups")]
    [SerializeField] Image powerUpIcon;
    [SerializeField] TMP_Text powerUpAmountText;

    private static HUDScreen _sharedInstance;

    public static HUDScreen SharedInstance
    {
        get
        {
            if (_sharedInstance == null)
            {
                _sharedInstance = FindObjectOfType<HUDScreen>();

                // Si sigue siendo nulo, lo cargamos desde los recursos
                if (_sharedInstance == null)
                {
                    var gameObj = Resources.Load("UI/HUD") as GameObject;
                    gameObj = Instantiate(gameObj, Vector3.zero, Quaternion.identity);
                    _sharedInstance = gameObj.GetComponent<HUDScreen>();
                }
            }

            return _sharedInstance;
        }
    }

    public void UpdateHealth(int health)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if ((i + 1) <= health)
            {
                // Color(r, g, b, alpha)
                heartImages[i].color = new Color(1, 1, 1, 1);
            }
            else
            {
                heartImages[i].color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void UpdateCoins(int coins)
    {
        coinsAmountText.text = "x" + coins.ToString();
    }

    public void UpdatePowerUp(int powerUpAmount)
    {
        //print(powerUpAmount);
        if (powerUpAmount <= 0)
        {
            powerUpIcon.color = new Color(1, 1, 1, 0);
        }
        powerUpAmountText.text = "x" + powerUpAmount.ToString();
    }

    public void UpdatePowerUp(Sprite icon, int powerUpAmount)
    {
        powerUpIcon.color = new Color(1, 1, 1, 1);
        powerUpIcon.sprite = icon;
        powerUpAmountText.text = "x" + powerUpAmount.ToString();
    }
}
