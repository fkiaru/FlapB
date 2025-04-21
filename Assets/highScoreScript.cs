using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class highScoreScript : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI HighScoreText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        Debug.Log("enter Enable de HS Screen");
        HighScoreText.text = "High Score : "+LogicScript.Instance.bestScore.ToString();        
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LogicScript.Instance.ToggleHighScore();
        }
    }
}
