using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Inventory : MonoBehaviour {

    public Image powerUpImage;

	public void AddPowerUp(Sprite powerUpSprite){
		powerUpImage.sprite = powerUpSprite;
		powerUpImage.enabled = true;
	}

	public void ConsumePowerUp(){
		powerUpImage.sprite = null;
		powerUpImage.enabled = false;
	}
}
