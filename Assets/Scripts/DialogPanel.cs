using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : BasePanel
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
    public RectTransform selectChoicePanel;
    public event Action<int> onMakeChoice;

    private const string SPEAKER_TAG = "speaker";
    private List<GameObject> choiceButtons; 
    private GameObject choiceButtonPrefab;
    private bool canContinueStory;

    private Coroutine typingCharacterCoroutine;

    private void OnEnable()
    {
        DialogManager.instance.OnDialogContinue += ChangeDialogText;
        DialogManager.instance.OnMeetChoices += InstantiateChoiceButtons;
    }

    private void OnDisable()
    {
        DialogManager.instance.OnDialogContinue -= ChangeDialogText;
        DialogManager.instance.OnMeetChoices -= InstantiateChoiceButtons;
    }

    private void Update()
    {
        if (canContinueStory && selectChoicePanel.childCount <= 0 && InputHandler.instance.GetContinuePressed())
        {
            DialogManager.instance.Continue();
        }

        if (canContinueStory)
        {
            DisplayChoiceButton();
        }
    }

    public override void Enter()
    {
        Debug.Log(this.gameObject.name);
        this.gameObject.SetActive(true);
    }

    public override void Exit()
    {
        this.gameObject.SetActive(false);
    }

    private void InstantiateChoiceButtons(List<string> choiceTexts)
    {
        choiceButtonPrefab = Resources.Load("ChoiceButton") as GameObject;

        if (choiceButtonPrefab == null)
        {
            Debug.LogWarning("Resources directory not contains \"ChoiceButton\"");
            return;
        }

        choiceButtons = new List<GameObject>(choiceTexts.Count);
        for (int i = 0; i < choiceTexts.Count; i++)
        {
            string choiceText = choiceTexts[i];
            GameObject ChoiceButton = GameObject.Instantiate(choiceButtonPrefab);
            choiceButtons.Add(ChoiceButton);
            ChoiceButton.transform.SetParent(selectChoicePanel);
            ChoiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choiceText;

            int k = i;
            ChoiceButton.GetComponent<Button>().onClick.AddListener(() => {
                onMakeChoice?.Invoke(k); 
            });
        }
        HideChoiceButton();
    }

    private void ChangeDialogText(string dialogText)
    {
        //如果上一句对话有选项，这一句对话需要先清空选项
        if (choiceButtons != null)
        {
            choiceButtons.Clear();
        }
       
        foreach (var transform in selectChoicePanel.GetComponentsInChildren<Transform>())
        {
            if (transform.name == selectChoicePanel.name) continue;
            Destroy(transform.gameObject);
        }

        if (typingCharacterCoroutine != null)
        {
            StopCoroutine(typingCharacterCoroutine);
        }
        typingCharacterCoroutine =  StartCoroutine(TypingCharacter(dialogText));

        //处理对话带有的标签
        HandleTags();
    }

    private void HandleTags()
    {
        List<string> tags = DialogManager.instance.GetCurrentDialogTags();
        foreach (var tag in tags)
        {
            string[] namePair = tag.Split(':');
            if (namePair.Length != 2)
            {
                Debug.LogWarning("Tag could not be appropriately parsed: " + tag);
                continue;
            }

            string key = namePair[0].Trim();
            string value = namePair[1].Trim();

            switch (key)
            {
                case SPEAKER_TAG:
                    this.nameText.text = value;
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently being handled: " + tag);
                    break;
            }
        }
    }

    private IEnumerator TypingCharacter(string Text)
    {
        //InputHandler.instance.jump_Input在被按下后一直到LateUpdate的期间内都是true的，
        //这就导致在下面循环体内判断按键是否按下与在Update中判断的结果是一样的，即使携程在Update后面执行
        //yield return new WaitForEndOfFrame();

        canContinueStory = false;
        this.dialogText.text = "";

        bool _isAddingRichText = false;
        foreach (var character in Text)
        {
            if (InputHandler.instance.GetContinuePressed())
            {
                this.dialogText.text = Text;
                break;
            }

            if (character == '<' || _isAddingRichText)
            {
                _isAddingRichText = true;
                dialogText.text += character;
                if (character == '>')
                {
                    _isAddingRichText = false;
                }
            }
            else
            {
                this.dialogText.text += character;
                yield return new WaitForSeconds(0.04f);
            }
        }

        canContinueStory = true;
    }

    private void HideChoiceButton()
    {
        foreach (var choiceButton in choiceButtons)
        {
            choiceButton.SetActive(false);
        }
    }

    private void DisplayChoiceButton()
    {
        if (choiceButtons == null) return;
        foreach (var choiceButton in choiceButtons)
        {
            choiceButton.SetActive(true);
        }
    }
}