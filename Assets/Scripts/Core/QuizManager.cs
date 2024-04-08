using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuizManager : MonoBehaviour
{
    [Header("Questions for Players")]
    public List<QuestionSO> Questions;
    
    [Header("Answer options for players")]
    public List<Button> OptionButtons;

    [Header("Quiz UI Parameters")]
    public TextMeshProUGUI QuestionTextUI;
    public Image Background;
    public TextTyper TextTyper;

    private int _currentQuestionIndex = -1;

    public static QuizManager Instance;

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

    public void NextQuestion()
    {
        _currentQuestionIndex++;
        if (_currentQuestionIndex < Questions.Count)
        { 
            QuestionSO currentQuestion = Questions[_currentQuestionIndex];
            TextTyper.TypingCompleted += RevealOptionButtons;
            TextTyper.DisplayText(currentQuestion.QuestionText, QuestionTextUI);
            Background.sprite = currentQuestion.BackgroundImage;

            foreach(var button in OptionButtons)
            {
                button.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Quiz Finished");
            UIManager.Instance.GoToResultScreen();
        }
    }

    private void RevealOptionButtons()
    {
        QuestionSO currentQuestion = Questions[_currentQuestionIndex];
        for (int i = 0; i < OptionButtons.Count; i++)
        {
            if (i < currentQuestion.Options.Count)
            {
                OptionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentQuestion.Options[i].OptionText;
                OptionButtons[i].onClick.RemoveAllListeners();
                int index = i; // Capture the current index
                OptionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
                OptionButtons[i].gameObject.SetActive(true); // Now activate it
            }
        }

        TextTyper.TypingCompleted -= RevealOptionButtons; // Unsubscribe to ensure this runs once per question
    }

    public void OnOptionSelected(int optionIndex)
    {
        if (_currentQuestionIndex < Questions.Count && optionIndex < Questions[_currentQuestionIndex].Options.Count)
        {
            Option selectedOption = Questions[_currentQuestionIndex].Options[optionIndex];

            foreach (var change in selectedOption.TraitChanges)
            {
                DataManager.Instance.AddTraitScore(change.TraitName, change.ScoreChange);
            }
        }

        ChangeSlide();
    }

    public void ChangeSlide()
    {
        StartCoroutine(UIManager.Instance.Transition(() =>
        {
            NextQuestion();
        }));
    }
    private void OnDisable()
    {
        TextTyper.TypingCompleted -= RevealOptionButtons;
    }
}