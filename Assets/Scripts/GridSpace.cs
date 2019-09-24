using UnityEngine;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {
	public Button button;
	public Text buttonText;
	private GameController gameController;

	public void SetSpace() {
		buttonText.text = gameController.GetPlayerSide();
		button.interactable = false;
		gameController.EndTurn();
	}

	public void SetGameControllerRef(GameController controller) {
		gameController = controller;
	}
}