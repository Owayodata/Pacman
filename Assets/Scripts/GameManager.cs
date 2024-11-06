using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    public Ghost[] ghosts;
    public Pacman pacman;
    public Transform pellets;
    public GameObject scoreText;
    public GameObject hiscoreText;
    public GameObject livesText;
    public GameObject pausePanel;
    public GameObject muteButton;
    public GameObject GameOverPanel;
    public AudioSource death;
    public AudioSource pelletEaten;
    public AudioSource ghostEaten;
    public bool isGameMuted = false;
    public int score { get; private set; }
    public int highscore { get; private set; }
    public int lives { get; private set; }

    public int ghostMultiplier { get; private set; } = 1;

    private void Start()
    {
        NewGame();
        // Load high score from PlayerPrefs
        highscore = PlayerPrefs.GetInt("HighScore", 0);
        hiscoreText.GetComponent<TMP_Text>().text = "" + highscore; // Display the high score at game start
    }

    private void UpdateHighScore()
    {
        // Update high score if current score exceeds it
        highscore = score;
        hiscoreText.GetComponent<TMP_Text>().text = "" + highscore;

        // Save the high score to PlayerPrefs
        PlayerPrefs.SetInt("HighScore", highscore);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        // Restart game if lives are 0 and any key is pressed

        if(lives <= 0)
        {
            GameOverPanel.SetActive(true);
        }

        // Update the score and lives text in the UI
        scoreText.GetComponent<TMP_Text>().text = "" + score;
        livesText.GetComponent<TMP_Text>().text = "" + lives;

        // Check if the score beats the high score, and update if necessary
        if (score > highscore)
        {
            UpdateHighScore();
        }
    }

    public void NewGame()
    {
        // Start a new game by resetting score, lives, and starting a new round
        if(GameOverPanel.gameObject.activeSelf)
            GameOverPanel.SetActive(false);

        SetScore(0);
        SetLives(3);
        hiscoreText.GetComponent<TMP_Text>().text = "" + highscore; // Ensure high score is shown at game start
        NewRound();

        foreach (Ghost ghost in ghosts)
        {
            ghost.movement.speed = 6f;
        }

    }

    private void NewRound()
    {
        // Reactivate all pellets for the new round
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }

        foreach (Ghost ghost in ghosts)
        {
            if(ghost.movement.speed <= 9)
            ghost.movement.speed += 1f;
        }

        ResetState();
    }

    private void ResetState()
    {
        // Reset ghost multiplier and ghosts/Pacman's states
        ResetGhostMultiplier();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void GameOver()
    {
        // Deactivate ghosts and Pacman at game over
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }
        foreach (Ghost ghost in ghosts)
        {
            ghost.movement.speed = 6f;
        }
        death.Play();
        this.pacman.gameObject.SetActive(false);

        
    }

    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void GhostEaten(Ghost ghost)
    {
        // Add points based on ghost multiplier and increment the multiplier
        int points = ghost.points * this.ghostMultiplier;
        SetScore(this.score + points);
        this.ghostMultiplier++;
        ghostEaten.Play();
    }

    public void PacmanEaten()
    {
        // Handle Pacman getting eaten, reduce lives, and check for game over
        this.pacman.gameObject.SetActive(false);
        SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3.0f); // Reset game state if lives remain
        }
        else
        {
            GameOver(); // End game if no lives are left
        }
    }

    public void PelletEaten(Pellet pellet)
    {
        // Disable pellet and add points to the score
        pellet.gameObject.SetActive(false);
        SetScore(this.score + pellet.points);
        pelletEaten.Play();

        // If no pellets are left, start a new round
        if (!HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        // Enable frightened state for all ghosts when Pacman eats a power pellet
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            ghosts[i].frightened.Enable(pellet.duration);
        }

        PelletEaten(pellet); // Call PelletEaten logic as well
        CancelInvoke(); // Cancel previous invokes
        Invoke(nameof(ResetGhostMultiplier), pellet.duration); // Reset multiplier after power pellet duration
    }

    private bool HasRemainingPellets()
    {
        // Check if there are any active pellets remaining in the scene
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }

    private void ResetGhostMultiplier()
    {
        // Reset the ghost multiplier back to 1
        this.ghostMultiplier = 1;
    }


    public void PauseGame()
    {
        if (Time.timeScale == 1)
        {
            pausePanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            pausePanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Mute()
    {
        if(isGameMuted == false)
        {
            AudioListener.volume = 0;
            isGameMuted = true;
        }
        
        else
        {
            AudioListener.volume = 1;
            isGameMuted= false;
        }
    }

}
