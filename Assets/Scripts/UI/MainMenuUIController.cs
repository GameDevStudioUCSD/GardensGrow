using UnityEngine;
using System.Collections;

public class MainMenuUIController : MonoBehaviour {
	public GameObject titleCard;
	public GameObject mainMenu;
	public GameObject loadMenu;
	public GameObject optionMenu;

	private enum UIState {Title, Menu, LoadMenu, OptionMenu};
	private UIState state;

	// Use this for initialization
	void Start () {
		if (titleCard != null) {
			titleCard.SetActive(true);
		}
		if (mainMenu != null) {
			mainMenu.SetActive(false);
		}
		if (loadMenu != null) {
			loadMenu.SetActive(false);
		}
		if (optionMenu != null) {
			optionMenu.SetActive(false);
		}


		state = UIState.Title;
	}
	
	// Update is called once per frame
	void Update () {
		if (state == UIState.Title) {
			if (Input.anyKeyDown) {
				state = UIState.Menu;
				if (titleCard != null) {
					titleCard.SetActive(false);
				}
				if (mainMenu != null) {
					mainMenu.SetActive(true);
				}
			}
		}
		if (state == UIState.Menu) {
			if (Input.GetKeyDown(KeyCode.Escape)) {
				state = UIState.Title;
				if (titleCard != null) {
					titleCard.SetActive(true);
				}
				if (mainMenu != null) {
					mainMenu.SetActive(false);
				}
			}
		}
	}

	public void StartNewGame () {
		Application.LoadLevel(1);
	}

	public void LoadGame () {
		state = UIState.LoadMenu;
		if (mainMenu != null) {
			mainMenu.SetActive(false);
		}
		if (loadMenu != null) {
			loadMenu.SetActive(true);
		}
	}

	public void Options () {
		state = UIState.OptionMenu;
		if (mainMenu != null) {
			mainMenu.SetActive(false);
		}
		if (optionMenu != null) {
			optionMenu.SetActive(true);
		}
	}

	public void Back () {
		state = UIState.Menu;

		if (loadMenu != null) {
			loadMenu.SetActive(false);
		}
		if (optionMenu != null) {
			optionMenu.SetActive(false);
		}
		if (mainMenu != null) {
			mainMenu.SetActive(true);
		}
	}

	public void Quit () {
		Application.Quit();
	}
}
