using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _gameOverText;
    [SerializeField]
    Text _scoreText;
    // Start is called before the first frame update

    [SerializeField]
    Text _restartText;
    [SerializeField]
    Sprite[] _livesSprites;
    [SerializeField]
    Image livesImg;


    GameManager _gameManager;

    public void Start()
    {
        _scoreText.text = "Score: 0";
        livesImg.sprite = _livesSprites[3];
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);

        _gameManager = FindObjectOfType<GameManager>();
    }


    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        livesImg.sprite = _livesSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _restartText.gameObject.SetActive(true);

        if(_gameManager!=null)
        {
            _gameManager.GameOver();
        }

        StartCoroutine(FlickerEffect());
    }

    private IEnumerator FlickerEffect()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);

        }
    }
}
