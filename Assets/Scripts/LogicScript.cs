using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using System.IO;
using System;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UIElements;
public class LogicScript : MonoBehaviour
{
    [SerializeField]
    public enum GameState
    {
        MainMenu,
        GameRun,
        GameOver
    }
    [SerializeField]
    public GameObject currentScore;
    public GameObject objectsDuJeu;
    [HideInInspector]
    public GameState state;
    public int playerScore;
    public TextMeshProUGUI scoreText;
    public GameObject gameOverScreen;
    public GameObject mainMenu;
    public GameObject highScoreScreen;
    public GameObject highScoreAnimation;
    public GameObject FeuHighScore;
    [SerializeField]
    private BirdScript bird;
    public GameObject pauseScreen;
    public GameObject pauseButton;
    public AudioSource dingSFX;
    public AudioSource aieSFX;
    public bool isPaused = false;
    public int bestScore = 0;
    private bool newHighScoreReach = false;
    [SerializeField]
    public bool invincible = false;
    
    // Création du singleton
    private static LogicScript instance = null;
    public static LogicScript Instance => instance; 
    private void Awake()    
    {
        if (instance != null && instance != this)
        {
            Debug.Log("destruction d'un objet"+this.gameObject);
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
            Debug.Log(" celui-ci est conservé "+ this.gameObject);
        }
        //DontDestroyOnLoad(this.gameObject);

        // Initialisation du Game Manager...
        state= GameState.MainMenu;
        playerScore = 0;
        mainMenu.SetActive(true);
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
        pauseButton.SetActive(false);
        highScoreScreen.SetActive(false);
        highScoreAnimation.SetActive(false);
        FeuHighScore.SetActive(false);
        bestScore = ScoreManager.LaunchInitializeScore();

        Debug.Log("BestScores (initialisé dans le GM = " + bestScore);
    }
    public void OnNewHighScore()
    {
        highScoreAnimation.SetActive(true);
        FeuHighScore.SetActive(true);
    }
    public void startGame() // démarre le jeu
    {
        Debug.Log("hit Start button");
        if (state == GameState.MainMenu)
        {
            newHighScoreReach = false;
            Debug.Log("enter if mainMenu");
            state = GameState.GameRun;
            objectsDuJeu.SetActive(true);
            currentScore.SetActive(true);
            mainMenu.SetActive(false);
            pauseScreen.SetActive(true);
            pauseButton.SetActive(true);
            //bird.setActive(true);           
            
        }
    }
    public void ToggleHighScore()
    {
        if (state == GameState.MainMenu)
        {
            bool isHSScreenActive = highScoreScreen.gameObject.activeSelf;
            highScoreScreen.SetActive(!isHSScreenActive);
            mainMenu.SetActive(isHSScreenActive);
        }
    }
            

    public void TogglePause()
    {
        if (state == GameState.GameRun)
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }
    public void doExitGame()
    {
        Debug.Log("Log exit Game!");
        Application.Quit();
    }
    void Start()
    {
        //bird = GameObject.FindGameObjectWithTag("Bird").GetComponent<BirdScript>();
        //GameObject.FindGameObjectWithTag("Bird").SetActive(false);
    }
    public void resetScores()
    {
        Debug.Log("enter reset scores");
        playerScore = 0;
        
        ScoreManager.ResetHighScore();
        ToggleHighScore();
        bestScore = ScoreManager.LaunchInitializeScore();
        Debug.Log("New scores = "+ playerScore+" "+bestScore);  
    }
    [ContextMenu("Increase Score")]
    public void addScore(int scoreToAdd)
    {
        if (bird.getAlive()) {            
            
            
            playerScore += scoreToAdd;            
            scoreText.text = playerScore.ToString();
            dingSFX.Play();
            bird.avale();
            // check if score > bestScore
            if (playerScore> bestScore)
            {
                bestScore = playerScore;
                ScoreManager.SaveScoreIfBetter(bestScore);
                if (!newHighScoreReach)
                {
                    newHighScoreReach = true;
                    OnNewHighScore();
                }

            }
            //if (playerScore > 2)
            //{
            //    Debug.Log("quti");
            //    Application.Quit();
            //}
        }
    }
    public void restartGame()
    {
        state = GameState.MainMenu;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);        
    }
    public void gameOver()
    {
        if (bird.getAlive()) { aieSFX.Play(); }
        state = GameState.GameOver;
        //if (playerScore> bestScore)
        //{
        //    bestScore = playerScore;
        //    ScoreManager.SaveScoreIfBetter(bestScore);
        //}
        pauseScreen.SetActive(false);
        pauseButton.SetActive(false);
        gameOverScreen.SetActive(true);        
    }
};
//[System.Serializable]
//public class  Scores
//{
//    public int bestScore = 0;
//};
//public class JsonDemo
//{
//    static Scores m_score;
//    static string chemin, jsonString;
//    static public Scores  ReadScores()
//    {
//        chemin = Application.streamingAssetsPath + "/bestScores.json";
//        jsonString = File.ReadAllText(chemin);
//        Scores score = JsonUtility.FromJson<Scores>(jsonString);
//        Debug.Log("readBestScore" + score.bestScore);
//        m_score = score;
//        return score;
//    }
//    static public void WriteScores(int bestscore)
//    {
//        m_score.bestScore = bestscore;
//        chemin = Application.streamingAssetsPath + "/bestScores.json";
//        jsonString = JsonUtility.ToJson(m_score);
//        File.WriteAllText(chemin, jsonString);
//        Debug.Log("WriteBestScore" + m_score);       

//    }
//}

[System.Serializable]
public class ScoreData
{
    public int bestScore;
}

public static class ScoreManager
{
    private static string fileName = "bestscore.json";
    private static string savePath => Path.Combine(Application.persistentDataPath, fileName);

    private static ScoreData data;

    public static int LaunchInitializeScore()
    {
        Debug.Log("start launchInitializeScore");
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("Fichier de score inexistant. Création d'un fichier vide.");
            data = new ScoreData(); // Score par défaut
            Save(); // Écrit le fichier par défaut
        }
        else
        {
            string json = File.ReadAllText(savePath);
            data = JsonUtility.FromJson<ScoreData>(json);
            Debug.Log("lecture du fichier de SCORE " + data);
        }

        return data.bestScore;
    }
        public static void SaveScoreIfBetter(int newScore)
    {
        
        Debug.Log("enter saveIfBetter "+newScore+" > ?"+data.bestScore);
        if (newScore > data.bestScore)
        {
            data.bestScore = newScore;
            Save();
        }
    }
        private static void Save()
    {
        string json = JsonUtility.ToJson(data, true);
        Debug.Log("Ecriture du fichier : " + savePath+" avec score = "+data.bestScore);
        File.WriteAllText(savePath, json);
    }
    public static void ResetHighScore()
    {
        if (data == null) data = new ScoreData();
        data.bestScore = 0;
        Save();
    }

    public static int GetBestScore() => data?.bestScore ?? 0;
}



//public class BestScoreManager : MonoBehaviour
//{
//    private string fileName = "bestscore.json";
//    private string persistentPath;
//    private string streamingPath;
//    public ScoreData scoreData;

//    void Start()
//    {
//        persistentPath = Path.Combine(Application.persistentDataPath, fileName);
//        streamingPath = Path.Combine(Application.streamingAssetsPath, fileName);

//        StartCoroutine(InitializeScore());
//    }

//    IEnumerator InitializeScore()
//    {
//        if (File.Exists(persistentPath))
//        {
//            // Lire directement depuis le dossier persistent
//            string json = File.ReadAllText(persistentPath);
//            scoreData = JsonUtility.FromJson<ScoreData>(json);
//            Debug.Log("Best score chargé depuis persistentDataPath : " + scoreData.bestScore);
//        }
//        else
//        {
//            // Lire depuis StreamingAssets (Android => WebRequest requis)
//            string json = null;

//            if (Application.platform == RuntimePlatform.Android)
//            {
//                UnityWebRequest request = UnityWebRequest.Get(streamingPath);
//                yield return request.SendWebRequest();

//                if (request.result != UnityWebRequest.Result.Success)
//                {
//                    Debug.LogError("Erreur lecture streamingAssets : " + request.error);
//                    yield break;
//                }
//                json = request.downloadHandler.text;
//            }
//            else
//            {
//                json = File.ReadAllText(streamingPath);
//            }

//            // Parser et sauvegarder dans persistent
//            scoreData = JsonUtility.FromJson<ScoreData>(json);
//            File.WriteAllText(persistentPath, json);
//            Debug.Log("Fichier copié depuis StreamingAssets vers persistentDataPath. BestScore = " + scoreData.bestScore);
//        }
//    }

//    public void SaveBestScore(int newScore)
//    {
//        if (scoreData == null)
//            scoreData = new ScoreData();

//        if (newScore > scoreData.bestScore)
//        {
//            scoreData.bestScore = newScore;
//            string json = JsonUtility.ToJson(scoreData, true);
//            File.WriteAllText(persistentPath, json);
//            Debug.Log("Nouveau meilleur score sauvegardé : " + scoreData.bestScore);
//        }
//    }

//    public int GetBestScore()
//    {
//        return scoreData != null ? scoreData.bestScore : 0;
//    }
//}

