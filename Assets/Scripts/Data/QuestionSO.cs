using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz/Question")]
public class QuestionSO : ScriptableObject
{
    [Header ("Type your question here")]
    public string QuestionText;

    [Header ("Create your Answer options")]
    public List<Option> Options;

    [Header ("Select Background image for your question")]
    public Sprite BackgroundImage;
}

[System.Serializable]
public class Option
{
    public string OptionText;
    public List<TraitChange> TraitChanges;
}

[System.Serializable] // This makes TraitChange visible in the inspector
public struct TraitChange
{
    public string TraitName;
    public int ScoreChange;
}