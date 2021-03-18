using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;  // for stringbuilder
using UnityEngine;
using UnityEngine.Windows.Speech;   // grammar recogniser

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;
    private string valueString;
    public AudioSource _as;
    public Rigidbody2D rb;
    private GrammarRecognizer gr;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _as = GetComponent<AudioSource>();
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

        switch (valueString)
        {
            case "JUMP":
                Jump();
                break;
            case "MUTE THE GAME":
                Mute();
                gr.Stop();
                break;
            default:
                break;
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

    // Method called when 'jump' command is used.
    void Jump(){
        rb.velocity = Vector2.up*jumpForce;
    }
    
    // Mute the audiosource once the command has been called
    void Mute(){
        _as.mute = !_as.mute;
    }
}
