using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private Text _liveText;
    [SerializeField] private Text _scoreText;
    [SerializeField] private GameObject _gameoverScreen;

    private readonly string _gameoverAnimationTrigger = "gameover";
    private int _availableLive;

    public void UpdateUI(int availableLive)
    {
        _availableLive = availableLive;
        _liveText.text = availableLive + "/" + availableLive;
        _scoreText.text = 0.ToString();
    }
    public void UpdateUI(int live, int score)
    {
        _liveText.text = _availableLive + "/" + live;
        _scoreText.text = score.ToString();
    }

    public void GameOverScreen()
    {
        _gameoverScreen.SetActive(true);
        _gameoverScreen.GetComponent<Animator>().SetBool(_gameoverAnimationTrigger, true);
    }
}
