using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommunicationArea : MonoBehaviour
{
    [SerializeField] private List<Sentence> sentences;
    private int sentenceIndex = -1;

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.GetComponent<Player>() != null)
        {
            sentenceIndex = 0;
            UI.instance.Speak(sentences[sentenceIndex]);
            GameManager.instance.SetPauseGame(true);
            ++sentenceIndex;
        }
    }

    void Update()
    {
        if(sentenceIndex > 0)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                if(sentenceIndex < sentences.Count)
                {
                    UI.instance.Speak(sentences[sentenceIndex]);
                    ++sentenceIndex;
                }
                else
                {
                    GameManager.instance.SetPauseGame(false);
                    UI.instance.SpeakDone();
                    Destroy(gameObject);
                }
            }
        }
    }
}
