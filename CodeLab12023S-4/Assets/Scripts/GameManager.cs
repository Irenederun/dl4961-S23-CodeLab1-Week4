using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using TMPro;
using Unity.Profiling;
using UnityEditor.SceneManagement;
using UnityEditor.U2D.Path.GUIFramework;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int score = 0;//initial score
    private int level = 0;//level No.
    private int targetScore = 4;//target
  
    public TextMeshProUGUI beginText;
    public TextMeshProUGUI textMeshPro;
    public TextMeshProUGUI recordListText;
    public TextMeshProUGUI names;

    public int Score
    {
        get
        {
            return score;//allow other places to access score through Score
        }
        set
        {
            score = value;//allow score to be set to diff. values
        }
    }

    public List<int> records = new List<int>();//make a list that will hold all the record scores

    private string FIlE_PATH;//create a string that will hold file path
    private const string FILE_DIR = "/Data/";//directory string
    private const string FILE_NAME = "records.txt";//save file name

    public List<string> playerNames = new List<string>();//a list that will hold all player names
    private string NAME_FILE_PATH;
    private const string NAME_FILE_NAME = "names.txt";
    
    private void Awake()
    {
        if (Instance == null)
            //if no other instance of this game object in scene
        {
            DontDestroyOnLoad(gameObject);
            //don't destroy
            Instance = this;
            //set instance to this one
        }
        else
            //if there is
        {
            Destroy(gameObject);
            //destroy self
        }
    }

    void Start()
    {
        FIlE_PATH = Application.dataPath + FILE_DIR + FILE_NAME;//initialize paths
        NAME_FILE_PATH = Application.dataPath + FILE_DIR + NAME_FILE_NAME;

        if (File.Exists(FIlE_PATH) == false)//if no file exists,create one so there's something to read later
        {
            File.WriteAllText(FIlE_PATH, "Highest Records:\n" +
                                                "5" + "\n" +
                                                "4" + "\n" +
                                                "3" + "\n" +
                                                "2" + "\n" +
                                                "0" + "\n");
        }

        if (File.Exists(NAME_FILE_PATH) == false)
        {
            File.WriteAllText(NAME_FILE_PATH, "Player Names:\n" 
                                              + "a" + "\n"
                                              + "b" + "\n"
                                              + "c" + "\n"
                                              + "d" + "\n"
                                              + "e" + "\n");
        }
    }

    // Update is called once per frame
    void Update()
    {
        textMeshPro.text =
            ("Level: " + (1 + level) + "\n" +
             "Score: " + Score + "\n");//set displayed info text

        if (GameOver.gameOver == true)//if game over. boolean from Game over Script bc public static.
        {
            GameOver.gameOver = false;
            SceneManager.LoadScene("EndScene");
            GameObject[] destroyOnGO = GameObject.FindGameObjectsWithTag("GODestroy");
                //is there a less expensive way of doing this?
            foreach (GameObject GO in destroyOnGO)
            {
                GameObject.Destroy(GO);
            }//somehow for loop does not work - it doesn't accept destroyOnGO[i].
            
            beginText.text = "Too Bad! You did not make it to the ranking. Try Again!";
            
            UpdateHighScores();//call function
        }

        if (Score == targetScore)
            //if reaching target score
        {
            targetScore = targetScore *2;
            //reset targetScore
            level++;
            //increase level
            SceneManager.LoadScene(level);
            //load level
        }
    }

    void UpdateHighScores()
    {
        if (records.Count == 0) //if the record list is empty, which it should be every time upon restart
        {
            string fileContents = File.ReadAllText(FIlE_PATH);//read from existing file
            string[] fileSplit = fileContents.Split("\n");//split 
            
            string nameFileContents = File.ReadAllText(NAME_FILE_PATH);
            string[] nameFileSplit = nameFileContents.Split("\n");

            for (int i = 1; i < (fileSplit.Length - 1); i++)//starting from second line 
            {
                records.Add(Int32.Parse(fileSplit[i]));//turn string into integers and add to list
                playerNames.Add(nameFileSplit[i]);
            }
        }

        for (int i = 0; i < records.Count; i++)//for every time in the list
        {
            if (records[i] < Score)//compare it to current score
            {
                records.Insert(i, Score);//insert into list
                playerNames.Insert(i, ButtonPres.nickName);//insert corresponding player name
                beginText.text = "Congratulations! You've created a high score!";//change display text
                break;//avoid infinite loop
            }
        }

        if (records.Count > 5)//cut at top 5
        {
            records.RemoveRange(5, records.Count - 5);
            playerNames.RemoveRange(5, playerNames.Count - 5);
        }
        
        string recordsStr = "Highest Records:\n";//create a string to hold the scores 
        string nameStr = "Player Name:\n";//and names

        for (int i = 0; i < records.Count; i++)//for every item in the records list
        {
            recordsStr += records[i] + "\n";//add to the string
            nameStr += playerNames[i] + "\n";
        }
        
        recordListText.text = recordsStr;//display string
        names.text = nameStr;

        File.WriteAllText(FIlE_PATH, recordsStr);//write into txt
        File.WriteAllText(NAME_FILE_PATH, nameStr);
    }
}
