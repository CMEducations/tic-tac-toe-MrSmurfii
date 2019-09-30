using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public Text[,] buttonList2D = new Text[3, 3];
	public Text[] buttonList;
	public GameObject gameOverPanel;
	public Text gameOverText;
	public GameObject restartButton;
	public bool playAgainstComputer = true;
	public int rows = 3;
	public int columns = 3;


	private int moveCount;
	private string playerSide;
	private string computerSide;

	private ComputerPlayer computerPlayer;
	private bool playerTurn;


	private void Awake() {
		buttonList2D = MakeBoard2D(buttonList, rows, columns);
		SetGameControllerRefOnButtons();
		playerSide = "X";
		computerSide = "O";
		gameOverPanel.SetActive(false);
		moveCount = 0;
		restartButton.SetActive(false);
		computerPlayer = GetComponent<ComputerPlayer>();
		playerTurn = true;
	}

	void SetGameControllerRefOnButtons() {
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				buttonList2D[i, j].GetComponentInParent<GridSpace>().SetGameControllerRef(this);
			}
		}
	}

	public string GetPlayerSide() {
		return playerSide;
	}

	public void EndTurn() {
		moveCount++;
		if (IsGameWonBy(playerSide))
			GameOver(playerSide);
		else if (IsGameWonBy(computerSide))
			GameOver(computerSide);
		else if (moveCount >= rows*columns)
			GameOver("draw");
		else
			ChangeSides();
	}

	void GameOver(string winningPlayer) {
		if (winningPlayer == "draw")
			SetGameOverText("It's a draw!");
		else
			SetGameOverText(winningPlayer + " Wins!");
		SetBoardInteractable(false);
		gameOverPanel.SetActive(true);
		restartButton.SetActive(true);
	}

	void ChangeSides() {
		if (!playAgainstComputer) {
			playerSide = (playerSide == "X") ? "O" : "X";
		}

		if (playAgainstComputer) {
			playerTurn = !playerTurn;
			if (!playerTurn)
				ComputerTurn();
		}
	}

	void SetGameOverText(string value) {
		gameOverPanel.SetActive(true);
		gameOverText.text = value;
	}

	public void RestartGame() {
		playerSide = "X";
		moveCount = 0;
		gameOverPanel.SetActive(false);
		restartButton.SetActive(false);
		playerTurn = true;
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				buttonList2D[i, j].text = "";
			}
		}

		SetBoardInteractable(true);
	}

	void SetBoardInteractable(bool toggle) {
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				buttonList2D[i, j].GetComponentInParent<Button>().interactable = toggle;
			}
		}
	}

	private bool IsGameWonBy(string side) {
		int counter = 0;
		for (int row = 0; row < rows; row++) {
			counter = 0;
			for (int col = 0; col < columns; col++) {
				if (buttonList2D[row, col].text == side)
					counter++;
			}

			if (counter == rows)
				return true;
		}

		for (int col = 0; col < columns; col++) {
			counter = 0;
			for (int row = 0; row < rows; row++) {
				if (buttonList2D[row, col].text == side)
					counter++;
			}

			if (counter == columns)
				return true;
		}

		counter = 0;
		for (int row = 0; row < rows; row++) {
			if (buttonList2D[row, row].text == side)
				counter++;
			if (counter == rows)
				return true;
		}

		counter = 0;
		for (int row = 0; row < rows; row++) {
			if (buttonList2D[row, (rows - 1) - row].text == side)
				counter++;
			if (counter == rows)
				return true;
		}
		return false;
	}

	void ComputerTurn() {
		ComputerPlayer.Move compMove = computerPlayer.FindBestMove(buttonList2D);
		buttonList2D[compMove.row, compMove.col].text = computerSide;
		buttonList2D[compMove.row, compMove.col].GetComponentInParent<Button>().interactable = false;

		EndTurn();
	}

	private Text[,] MakeBoard2D(Text[] board, int rows, int columns) {
		Text[,] output = new Text[rows, columns];
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				output[i, j] = board[i * columns + j];
			}
		}

		return output;
	}
}