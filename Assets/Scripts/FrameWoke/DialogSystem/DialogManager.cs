using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance { get; private set; }
    
    public bool storyIsPlaying { get; private set; }
    private Story currentStory;

    //对话开始的事件，可供负责弹出对话框的脚本订阅
    public event Action OnStoryStart;
    public event Action OnStoryEnd;
    public event Action<string> OnDialogContinue;
    public event Action<List<string>> OnMeetChoices;

    private DialogPanel dialogPanel;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one DialogManager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        dialogPanel = FindObjectOfType<DialogPanel>(true);
        dialogPanel.onMakeChoice += MakeChoice;
    }

    private void OnDisable()
    {
        dialogPanel.onMakeChoice -= MakeChoice;
    }

    public void Continue()
    {
        StartOrContinueDilog();
    }

    private void Update()
    {
        if (!storyIsPlaying) return;
    }

    public void EnterDialogueMode(TextAsset inkJson)
    {
        currentStory = new Story(inkJson.text);
        storyIsPlaying = true;

        OnStoryStart?.Invoke();
        StartOrContinueDilog();
    }

    public void StartOrContinueDilog()
    {
        if (currentStory.canContinue)
        {
            string lineOfContent = currentStory.Continue();
            OnDialogContinue?.Invoke(lineOfContent);
            DisplayChoices();
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    public List<string> GetCurrentDialogTags()
    {
        return currentStory.currentTags;
    }

    private void DisplayChoices()
    {
        List<Choice> _choices = currentStory.currentChoices;

        if (_choices.Count <= 0) return;

        List<string> _choiceTexts = new List<string>();
        for (int i = 0; i < _choices.Count; i++)
        {
            _choiceTexts.Add(_choices[i].text);
        }
        
        OnMeetChoices?.Invoke(_choiceTexts);
    }

    private void MakeChoice(int idx)
    {
        currentStory.ChooseChoiceIndex(idx);
        StartOrContinueDilog();
    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);
        storyIsPlaying = false;
        OnStoryEnd?.Invoke();
    }
}