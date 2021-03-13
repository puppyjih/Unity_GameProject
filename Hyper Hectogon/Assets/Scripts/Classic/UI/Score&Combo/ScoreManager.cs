using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreManager : MonoBehaviour 
{
    // Serialize Fields
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Image fillImage;


    // Private Variables
    private int score = 0;
    private int combo = 0;
    private int maxCombo = 100;
    private int minCombo = 1;
    private int highScore;
    private float comboGauge = 0;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt(Constants.MYHIGHSCORE, 0);
    }

    /// <summary>
    /// Multiply combo and enemyScore. Adds result to player score
    /// </summary>
    /// <param name="enemyScore">Score of the Collided Enemy</param>
    public void UpdateScore(int enemyScore)
    {
        enemyScore *= combo;
        this.score += enemyScore;
        scoreText.text = this.score.ToString();

        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(Constants.MYHIGHSCORE, highScore);
        }
    }

    /// <summary>
    /// Adds deltaCombo to player combo gauge. If gauge >= 100, combo += 1
    /// </summary>
    /// <param name="deltaCombo">float value to change combo gauge</param>
    public void UpdateCombo(float deltaCombo)
    {
        comboGauge += deltaCombo;
        if(comboGauge >= 100)
        {
            comboGauge -= 100;
            combo += 1;
        }
        else if(comboGauge < 0)
        {
            comboGauge += 100;
            combo -= 1;
        }

        #region Clamp combo in range {minCombo, maxCombo}

        if (this.combo < minCombo)
        {
            this.combo = minCombo;
        }
        if(this.combo > maxCombo)
        {
            this.combo = maxCombo;
        }

        #endregion
        
        fillImage.fillAmount = comboGauge / 100;
        comboText.text = "x" + combo.ToString();
        // Combo Gauge UI Update
    }

    public void SetComboGauge100()
    {
        comboGauge = 100;
    }
}

    //public int GetScore() {
    //    //return int.Parse(_scoreText.text);
    //    return score;
    //}

    //public void SetScore(int score) {
    //    this.score = score;
    //    scoreText.text = score.ToString();
    //}

    //public int GetCombo() {
    //    return int.Parse(comboText.text);
    //}

    //public void SetCombo(int combo) {
    //    comboText.text = combo.ToString();
    //}
