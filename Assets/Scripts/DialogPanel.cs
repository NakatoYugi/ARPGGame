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
        //�����һ��Ի���ѡ���һ��Ի���Ҫ�����ѡ��
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

        //����Ի����еı�ǩ
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
        //InputHandler.instance.jump_Input�ڱ����º�һֱ��LateUpdate���ڼ��ڶ���true�ģ�
        //��͵���������ѭ�������жϰ����Ƿ�������Update���жϵĽ����һ���ģ���ʹЯ����Update����ִ��
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