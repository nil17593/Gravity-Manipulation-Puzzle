using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Parodystudios.GravityModificationPuzzle
{

    /// <summary>
    /// GameManager:
    /// - Manages game-related functionality such as the timer, game over conditions, and UI elements.
    /// - Controls the timer display, game-over messages, and restarting the game.
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private float timeInSeconds;
        [SerializeField] private Transform endGameCanvas;
        [SerializeField] private Text messageText;
        [SerializeField] private Text timerText;
        [SerializeField] private Transform pointCubesParent;
        [SerializeField] private CharacterMovement characterMovementController;

        private bool canRunTheTimer;

        void Start()
        {
            canRunTheTimer = true;
        }

        void Update()
        {
            if (canRunTheTimer)
            {
                timeInSeconds -= Time.deltaTime;
                DisplayTimeLeft(timeInSeconds);
                if (timeInSeconds <= 0)
                {
                    timeInSeconds = 0;
                    canRunTheTimer = false;
                    DisplayMessagePanel("Game Over!!");
                }
                else
                {
                    CheckGameOver();
                }
            }
        }

        // Display the time left on the UI timer.
        private void DisplayTimeLeft(float timeToDisplay)
        {
            float minutes = Mathf.FloorToInt(timeToDisplay / 60);
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Display the end-game message panel.
        private void DisplayMessagePanel(string message)
        {
            if (canRunTheTimer)
            {
                canRunTheTimer = false;
            }
            endGameCanvas.gameObject.SetActive(true);
            messageText.text = message;
        }

        // Restart the game by reloading the current scene.
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Check for game-over conditions.
        private void CheckGameOver()
        {
            bool gameOver = true;

            foreach (Transform pointCube in pointCubesParent)
            {
                if (pointCube.gameObject.activeInHierarchy)
                {
                    gameOver = false;
                    break;
                }
            }
            if (gameOver)
            {
                DisplayMessagePanel("All cubes Collected!");
            }
            if (!gameOver)
            {
                // Check if player is falling continuously
                gameOver = characterMovementController.CHeckForGameOver();
                Debug.Log("GAME over= " + gameOver);
                if (gameOver)
                {
                    DisplayMessagePanel("Game Over!");
                }
            }
        }
    }
}