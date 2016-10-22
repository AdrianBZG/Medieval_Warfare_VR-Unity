using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpeechRecognizerDemo : MonoBehaviour, IPocketSphinxEvents
{

    /* Named searches allow to quickly reconfigure the decoder */
    private const String DIGITS_SEARCH = "digits";

    /* Keyword we are looking for to activate menu */
    private const String KEYPHRASE = "oh mighty computer";

    #region Public serialized fields
    [SerializeField]
    private GameObject _pocketSphinxPrefab;
    [SerializeField]
    private Text _infoText;
    #endregion

    #region Private fields
    private UnityPocketSphinx.PocketSphinx _pocketSphinx;
    #endregion

    #region Private methods
    private void SubscribeToPocketSphinxEvents()
    {
        EM_UnityPocketsphinx em = _pocketSphinx.EventManager;

        em.OnBeginningOfSpeech += OnBeginningOfSpeech;
        em.OnEndOfSpeech += OnEndOfSpeech;
        em.OnError += OnError;
        em.OnInitializeFailed += OnInitializeFailed;
        em.OnInitializeSuccess += OnInitializeSuccess;
        em.OnPartialResult += OnPartialResult;
        em.OnPocketSphinxError += OnPocketSphinxError;
        em.OnResult += OnResult;
        em.OnTimeout += OnTimeout;
    }

    private void UnsubscribeFromPocketSphinxEvents()
    {
        EM_UnityPocketsphinx em = _pocketSphinx.EventManager;

        em.OnBeginningOfSpeech -= OnBeginningOfSpeech;
        em.OnEndOfSpeech -= OnEndOfSpeech;
        em.OnError -= OnError;
        em.OnInitializeFailed -= OnInitializeFailed;
        em.OnInitializeSuccess -= OnInitializeSuccess;
        em.OnPartialResult -= OnPartialResult;
        em.OnPocketSphinxError -= OnPocketSphinxError;
        em.OnResult -= OnResult;
        em.OnTimeout -= OnTimeout;
    }
    #endregion

    #region MonoBehaviour methods
    void Awake()
    {
        UnityEngine.Assertions.Assert.IsNotNull(_pocketSphinxPrefab, "No PocketSphinx prefab assigned.");
        var obj = Instantiate(_pocketSphinxPrefab, this.transform) as GameObject;
        _pocketSphinx = obj.GetComponent<UnityPocketSphinx.PocketSphinx>();

        if (_pocketSphinx == null)
        {
            Debug.LogError("[SpeechRecognizerDemo] No PocketSphinx component found. Did you assign the right prefab???");
        }

        SubscribeToPocketSphinxEvents();

        _infoText.text = "Loaded voice Recognition";
    }

    void Start()
    {
        _pocketSphinx.SetAcousticModelPath("en-us-ptm");
        //Debug.Log("[SpeechRecognizerDemo] " + Application.streamingAssetsPath + "cmudict-en-us.dict");
        _pocketSphinx.SetDictionaryPath("cmudict-en-us.dict");
        _pocketSphinx.SetKeywordThreshold(1e-45f);
        _pocketSphinx.AddBoolean("-allphone_ci", true);

        // These one are optional
        _pocketSphinx.AddGrammarSearchPath(DIGITS_SEARCH, "actions.gram");

        _pocketSphinx.SetupRecognizer();
    }
    

    void OnDestroy()
    {
        if (_pocketSphinx != null)
        {
            UnsubscribeFromPocketSphinxEvents();
            _pocketSphinx.DestroyRecognizer();
        }
    }
    #endregion

    #region PocketSphinx event methods
    public void OnPartialResult(string hypothesis)
    {
        _infoText.text = hypothesis;
    }

    public void OnResult(string hypothesis)
    {
        _infoText.text = "Result" + hypothesis;
    }

    public void OnBeginningOfSpeech()
    {
        _infoText.text = "Listening";
    }

    public void OnEndOfSpeech()
    {
        _infoText.text = "Processing";
    }

    public void OnError(string error)
    {
        _infoText.text = "Error: " + error;
        Debug.LogError("[SpeechRecognizerDemo] An error ocurred at OnError()");
        Debug.LogError("[SpeechRecognizerDemo] error = " + error);
    }

    public void OnTimeout()
    {
        _infoText.text = "TimeOut";
        Debug.Log("[SpeechRecognizerDemo] Speech Recognition timed out");
    }

    public void OnInitializeSuccess()
    {
        _pocketSphinx.StartListening("digits");
    }

    public void OnInitializeFailed(string error)
    {
        _infoText.text = "FAIL Initialize" + error;
        Debug.LogError("[SpeechRecognizerDemo] An error ocurred on Initialization PocketSphinx.");
        Debug.LogError("[SpeechRecognizerDemo] error = " + error);
    }

    public void OnPocketSphinxError(string error)
    {
        _infoText.text = "FAIL Initialize SPHINX" + error;
        Debug.LogError("[SpeechRecognizerDemo] An error ocurred on OnPocketSphinxError().");
        Debug.LogError("[SpeechRecognizerDemo] error = " + error);
    }
    #endregion
}
