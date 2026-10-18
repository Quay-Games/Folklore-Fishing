using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODManager : Singleton<FMODManager>
{
	[Header("MenuVoiceOver")]

	public EventReference MenuGreeting;
	public EventReference ReplayVoiceLine;
	public EventReference SkipVoiceLine;
	public EventReference MenuPlay;
	public EventReference MenuSettings;
	public EventReference MainMenu;

	[Header("LevelVoiceOver")]

	public EventReference[] IntroCutscenes;
	public EventReference[] LeaveBoatPrompts;

	[Header("ShopVoiceOver")]

	public EventReference[] ShoreIntros;
	public EventReference[] LeaveShorePrompts;
	public EventReference[] BaitShopIntros;
	public EventReference[] BaitNames;
	public EventReference[] FishBoardFish;
    public EventReference BaitShopBoughtBait;
    public EventReference BaitShopSoldFish;


    //captain
    public EventReference YouHave;

	//shop keeper
	public EventReference Left;


	[Header("Prices")]

	public EventReference Gold;
	public EventReference And;
	public EventReference SoldOut;
	public EventReference MadeByCombining;
	public EventReference[] Numbers;
	public List<EventReference> GetNumber (int number) {
		List<EventReference> numbers = new List<EventReference>();
		if(number <= 20) {
			numbers.Add(this.Numbers[number]);
		} else if(number <= 99) {
			int firstDigit = int.Parse(number.ToString()[0].ToString());
			int secondDigit = int.Parse(number.ToString()[1].ToString());
			numbers.Add(this.Numbers[firstDigit + 18]);
			numbers.Add(this.Numbers[secondDigit]);
		} else if (number >= 100 && number <= 999) {
			int firstDigit = int.Parse(number.ToString()[0].ToString());
			numbers.Add(this.Numbers[firstDigit + 28]);
			numbers.Add(this.And);
			int lastTwo = number % 100;
			if (lastTwo <= 20) {
				numbers.Add(this.Numbers[lastTwo]);
			} else {
			int secondDigit = int.Parse(number.ToString()[1].ToString());
			int thirdDigit = int.Parse(number.ToString()[2].ToString());
			if (secondDigit != 0) {
				numbers.Add(this.Numbers[secondDigit + 19]);
				}
			numbers.Add(this.Numbers[thirdDigit]);
			}
		}
		return numbers;
	}

	[Header("LevelSFX")]

	public EventReference ThrowRod;
	public EventReference LandRod;
	public EventReference ReelSound;
	public EventReference ActivityLevelSplash;
	public EventReference BaitBoxOpen;
	public EventReference[] BaitSounds;
	public EventReference[] AttatchBaitSounds;
	public EventReference CorrectSFX;
	public EventReference CatchSplash;
	public EventReference FishCatch;
	public EventReference BossCatch;
	public EventReference Unspool;
	public EventReference SeagullWarning;
	public EventReference SeagullAttack;
	public EventReference SeagullMiss;
	public EventReference FliesWarning;
	public EventReference FliesAttack;
	public EventReference FliesMiss;
	public EventReference Duck;

	[Header("ShopSFX")]

	public EventReference MoneyEarnt;
	public EventReference ItemBuy;
	public EventReference NavigationError;
	public EventReference ClickError;
	public EventReference ShopEnter;
	public EventReference BaitBoardOpenClose;

	[Header("ShoreMenuButtons")]
	public EventReference BaitShopButton;
}
