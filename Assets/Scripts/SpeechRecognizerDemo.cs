using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class SpeechRecognizerDemo : MonoBehaviour, IPocketSphinxEvents {
 /* Named searches allow to quickly reconfigure the decoder */
    private const String ACTIONS_SEARCH = "actions";

    [SerializeField]
    private GameObject _pocketSphinxPrefab;
    [SerializeField]
    private Text _infoText;
    [SerializeField]
    private Text _SpeechResult;
    [SerializeField]
    private string[] progressTexts;

    private UnityPocketSphinx.PocketSphinx _pocketSphinx;

    private Dictionary<string, string> infoTextDict;

    private void SubscribeToPocketSphinxEvents() {
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

    private void UnsubscribeFromPocketSphinxEvents() {
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


    /*
    private void switchSearch(string searchKey) {
        _pocketSphinx.StopRecognizer();
        if (searchKey.Equals(KWS_SEARCH))
        {
            _pocketSphinx.StartListening(searchKey);
        }
        else
        {
            _pocketSphinx.StartListening(searchKey, 10000);
        }

        string text;
        infoTextDict.TryGetValue(searchKey, out text);

        _infoText.text = text;
        _SpeechResult.text = "Say something!";
    }
    */

	/// <summary>
	/// Ge an instance of PocketSphinx and initialize event subscriber
	/// </summary>
    void Awake() {
        UnityEngine.Assertions.Assert.IsNotNull(_pocketSphinxPrefab, "No PocketSphinx prefab assigned.");
        var obj = Instantiate(_pocketSphinxPrefab, this.transform) as GameObject;
        _pocketSphinx = obj.GetComponent<UnityPocketSphinx.PocketSphinx>();

        if (_pocketSphinx == null) {
            Debug.LogError("[SpeechRecognizerDemo] No PocketSphinx component found. Did you assign the right prefab???");
        }

        SubscribeToPocketSphinxEvents();

		/// TODO: Put a load page
        _infoText.text = "Please wait for Speech Recognition engine to load.";
        _SpeechResult.text = "Loading human dictionary...";
    }

    void Start() {
        _pocketSphinx.SetAcousticModelPath("en-us-ptm");
		Debug.Log("[SpeechRecognizerDemo] " + Application.streamingAssetsPath + "/cmusphinx-es-5.2/");
		_pocketSphinx.SetDictionaryPath("cmusphinx-es-5.2/etc/voxforge_es_sphinx.dic");
        _pocketSphinx.SetKeywordThreshold(1e-45f);
		// Unknow option
        // _pocketSphinx.AddBoolean("-allphone_ci", true);

        // These one are optional
        _pocketSphinx.AddGrammarSearchPath(ACTIONS_SEARCH, "actions.gram");
        _pocketSphinx.SetupRecognizer();
    }
    

    void OnDestroy() {
        if (_pocketSphinx != null) {
            UnsubscribeFromPocketSphinxEvents();
            _pocketSphinx.DestroyRecognizer();
        }
    }

    public void OnPartialResult(string hypothesis) {
        _SpeechResult.text = hypothesis;

    }

    public void OnResult(string hypothesis) {
        _SpeechResult.text = hypothesis;
    }

    /// <summary>
    /// TODO: Put something like listening
    /// </summary>
    public void OnBeginningOfSpeech() {
        
    }

    public void OnEndOfSpeech() {
    }

    public void OnError(string error) {
        Debug.LogError("[SpeechRecognizerDemo] An error ocurred at OnError()");
        Debug.LogError("[SpeechRecognizerDemo] error = " + error);
    }

    public void OnTimeout() {
        Debug.Log("[SpeechRecognizerDemo] Speech Recognition timed out");
    }

    public void OnInitializeSuccess()
    {
        _pocketSphinx.AddKeyphraseSearch(ACTIONS_SEARCH, "");
    }

    public void OnInitializeFailed(string error)
    {
        Debug.LogError("[SpeechRecognizerDemo] An error ocurred on Initialization PocketSphinx.");
        Debug.LogError("[SpeechRecognizerDemo] error = " + error);
    }

    public void OnPocketSphinxError(string error)
    {
        Debug.LogError("[SpeechRecognizerDemo] An error ocurred on OnPocketSphinxError().");
        Debug.LogError("[SpeechRecognizerDemo] error = " + error);
    }
}
