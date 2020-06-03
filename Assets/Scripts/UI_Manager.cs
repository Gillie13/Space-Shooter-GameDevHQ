using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _ammoCount;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelPrompt;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;

    private GameManager _gameManager;



    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
        _ammoCount.text = "Ammo: " + 15 + "/15";
        _gameOverText.gameObject.SetActive(false);
        _restartLevelPrompt.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

    }


    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateAmmo(int ammoCount)
    {
        _ammoCount.text = "Ammo: " + ammoCount.ToString() + "/15";
    }

    public void UpdateLives(int currentLives)
    {
        if (currentLives >= 1)
        {
            _livesImg.sprite = _liveSprites[currentLives];
        }
        else { 
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartLevelPrompt.gameObject.SetActive(true);
        StartCoroutine(FlashingGameOver());

    }

    IEnumerator FlashingGameOver()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }

}
