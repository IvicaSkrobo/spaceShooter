using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Text _gameOverText;
    [SerializeField]
    Text _scoreText;
    [SerializeField]
    Text _ammoText;
    [SerializeField]
    Text _waweText;

    [SerializeField]
    GameObject ammoUI;
    Transform[] ammoImages;

    [SerializeField]
    Text _restartText;
    [SerializeField]
    Sprite[] _livesSprites;
    [SerializeField]
    Image livesImg;


    [SerializeField]
    Image _boostImg;

    GameManager _gameManager;

    public void Start()
    {
        _scoreText.text = "Score: 0";
        livesImg.sprite = _livesSprites[3];
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _boostImg.fillAmount = 0;
       _gameManager = FindObjectOfType<GameManager>();

        ammoImages = ammoUI.GetComponentsInChildren<Transform>();
        _waweText.gameObject.SetActive(false);

    }

    public void UpdateAmmo(int ammo,int ammoMax)
    {
        _ammoText.text =  ammo.ToString() + "/" + ammoMax.ToString();

        if (ammoImages == null)
        {
            ammoImages = ammoUI.GetComponentsInChildren<Transform>();
        }

        for (int i= ammoImages.Length-1; i > 0; i--)
        {
            if (i > ammo)
            {
                ammoImages[i].gameObject.SetActive(false);

            }
            else
            {
                ammoImages[i].gameObject.SetActive(true);

            }
        }
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
    public void UpdateWaweText(int waweNumb)
    {
        StartCoroutine(ShowWaweText(waweNumb));
    }

    private IEnumerator ShowWaweText(int _waweNumb)
    {
        _waweText.gameObject.SetActive(true);
        _waweText.text = "WAWE "+ _waweNumb.ToString() +"\n"+ "Are you ready?";

        yield return new WaitForSeconds(4f);
        _waweText.gameObject.SetActive(false);

    }

    public void Update()
    {
        
    }

    public void UpdateBooster(float boostFillAmount)
    {

        _boostImg.fillAmount = boostFillAmount;

    }
}
