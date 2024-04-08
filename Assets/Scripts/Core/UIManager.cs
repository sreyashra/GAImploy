using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure you have this namespace if you're using TextMeshPro

public class UIManager : MonoBehaviour
{
    [Header("Panel Parameters")]
    [SerializeField] private GameObject _playerIdPanel;
    [SerializeField] private GameObject _quizPanel;
    [SerializeField] private GameObject _resultsPanel;
    [SerializeField] private GameObject _transitionPanel;

    [Header("PlayerID Start Screen Parameters")]
    public TMP_InputField PlayerIdInputField;
    public Button SubmitButton;

    [Header("Results Panel Parameters")]
    public Button SaveResultButton;

    [Header("Transition Parameters")]
    public Animator TransitionAnimator;
    public float TransitionTime = 1f;

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        SubmitButton.onClick.AddListener(OnSubmitButtonClicked);
        SaveResultButton.onClick.AddListener(SaveResults);
    }

    private void OnSubmitButtonClicked()
    {
        string playerId = PlayerIdInputField.text.Trim();
        if (!string.IsNullOrEmpty(playerId))
        {
            DataManager.Instance.SetCurrentPlayerId(playerId);
            PlayerIdInputField.text = ""; // Optional: Clear field after setting
            StartCoroutine(Transition(() =>
            {
                _playerIdPanel.SetActive(false);
                _quizPanel.SetActive(true);
                QuizManager.Instance.NextQuestion();
            }));
            
        }
    }

    public IEnumerator Transition(Action onTransitionMiddle = null)
    {
        // Fade to black
        _transitionPanel.SetActive(true);
        TransitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(TransitionTime);

        // Call the action when the screen is covered
        onTransitionMiddle?.Invoke();

        // Fade back in
        TransitionAnimator.SetTrigger("EndTransition");
        yield return new WaitForSeconds(TransitionTime);
        _transitionPanel?.SetActive(false);
    }

    public void GoToResultScreen()
    {
        _quizPanel.SetActive(false);
        StartCoroutine(Transition(() =>
        {
            _resultsPanel.SetActive(true);
        }));
        
    }

    private void SaveResults()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"{DataManager.Instance.GetCurrentPlayerID()}_Results.csv");
        DataManager.Instance.ExportDataToCSV(filePath);
    }
}
