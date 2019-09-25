using System;
using UnityEngine;
using UnityEngine.UI;
public class ComputerPlayer : MonoBehaviour {

	public struct Move {
		public int row, col;
	}
	
	private string player = "X";
	private string computer = "O";
	private GameController controller;
	private int rows;
	private int columns;
	
	private void Awake() {
		controller = GetComponentInParent<GameController>();
		rows = controller.rows;
		columns = controller.columns;
	}

	bool IsMovesLeft(Text[,] board) {
		for (int i = 0; i < rows; i++)
			for (int j = 0; j < columns; j++)
				if (board[i, j].text == "")
					return true;
		return false;
	}

	int Evaluate(Text[,] board) {
		for (int row = 0; row < rows; row++) {
			if (board[row, 0].text == board[row, 1].text && board[row, 1].text == board[row, 2].text) {
				if (board[row, 0].text == computer)
					return 10;
				if (board[row, 0].text == player)
					return -10;
			}
		}

		for (int col = 0; col < columns; col++) {
			if (board[0, col].text == board[1, col].text && board[1, col].text == board[2, col].text) {
				if (board[0, col].text == computer)
					return 10;
				else if (board[0, col].text == player)
					return -10;
			}
		}

		if (board[0, 0].text == board[1, 1].text && board[1, 1].text == board[2, 2].text) {
			if (board[0, 0].text == computer)
				return 10;
			else if (board[0, 0].text == player)
				return -10;
		}

		if (board[0, 2].text == board[1, 1].text && board[1, 1].text == board[2, 0].text) {
			if (board[0, 2].text == computer)
				return 10;
			else if (board[0, 2].text == player)
				return -10;
		}

		return 0;
	}

	int MinMax(Text[,] board, int depth, bool isMax) {
		int score = Evaluate(board);

		if (score == 10)
			return score;
		if (score == -10)
			return score;
		if (IsMovesLeft(board) == false)
			return 0;

		if (isMax) {
			int best = -1000;

			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < columns; j++) {
					
					if (board[i, j].text == "") {
						board[i, j].text = computer;

						best = Math.Max(best, MinMax(board, depth + 1, !isMax));

						board[i, j].text = "";
					}
				}
			}

			return best;
		}
		else {
			int best = 1000;

			for (int i = 0; i < rows; i++) {
				for (int j = 0; j < columns; j++) {
					
					if (board[i, j].text == "") {
						board[i, j].text = player;

						best = Math.Min(best, MinMax(board, depth + 1, !isMax));

						board[i, j].text = "";
					}
				}
			}

			return best;
		}
	}

	public Move FindBestMove(Text[,] board) {
		int bestVal = -1000;
		Move bestMove;
		bestMove.row = -1;
		bestMove.col = -1;
		
		
		for (int i = 0; i < rows; i++) {
			for (int j = 0; j < columns; j++) {
				if (board[i, j].text == "") {
					board[i, j].text = computer;

					int moveVal = MinMax(board, 0, false);

					board[i, j].text = "";

					if (moveVal > bestVal) {
						bestMove.row = i;
						bestMove.col = j;
						bestVal = moveVal;
					}
				}
			}
		}
		return bestMove;
	}
}
