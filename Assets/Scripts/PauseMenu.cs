using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;  // for stringbuilder
using UnityEngine;
using UnityEngine.Windows.Speech;   // grammar recogniser
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused=false;
    public GameObject pauseMenuUI;
     private GrammarRecognizer gr;
    private string valueString;

    private void Start()
    {
        pauseMenuUI.SetActive(false);
        gr = new GrammarRecognizer(Path.Combine(Application.streamingAssetsPath, 
                                                "SimpleGrammar.xml"), 
                                    ConfidenceLevel.Low);
        Debug.Log("Grammar loaded!");
        gr.OnPhraseRecognized += GR_OnPhraseRecognized;
        gr.Start();
        if (gr.IsRunning) Debug.Log("Recogniser running");
    }

    private void GR_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        StringBuilder message = new StringBuilder();
        Debug.Log("Recognised a phrase");
        // read the semantic meanings from the args passed in.
        SemanticMeaning[] meanings = args.semanticMeanings;
        // Move pawn from C2 to C4 - Piece, Start, Finish
        // semantic meanings are returned as key/value pairs
        // Piece/"pawn", Start/"C2", Finish/"C4"
        // use foreach to get all the meanings.
        foreach(SemanticMeaning meaning in meanings)
        {
            string keyString = meaning.key.Trim();
            valueString = meaning.values[0].Trim();
            message.Append("Key: " + keyString + ", Value: " + valueString + " ");
        }

        
        // use a string builder to create the string and out put to the user
        Debug.Log(message);
    }

    private void OnApplicationQuit()
    {
        if (gr != null && gr.IsRunning)
        {
            gr.OnPhraseRecognized -= GR_OnPhraseRecognized;
            gr.Stop();
        }
    }


    // Update is called once per frame
    void Update()
    {
        switch (valueString)
        {
            case "pause":
                PauseGame();
                break;
            case "resume":
                ResumeGame();
                break;
            case "quit":
                Quit();
                break;
            default:
                break;
        }

        // if(Input.GetKeyDown(KeyCode.Escape)){
        //     if(isPaused){
        //         ResumeGame();
        //     }
        //     else{
        //         PauseGame();
        //     }
        // }
    }

    // Update is called once per frame
    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale=0f;
        isPaused = true;

    }

    // Update is called once per frame
    void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale=1f;
        isPaused = false;
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale=1f;
        isPaused = false;
    }

    public void Quit(){
        SceneManager.LoadScene("MainMenu");
    }
}
