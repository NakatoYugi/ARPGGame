using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogComponent : MonoBehaviour
{
    public TextAsset inkJson;

    private void OnEnable()
    {
        DialogManager.instance.OnStoryStart += () => {
            UIManager.instance.PushPanel(UIPanelType.DialogPanel);
            //CameraHandler.instance.enabled = false;
            FindObjectOfType<PlayerManager>().enabled = false;
        };
        DialogManager.instance.OnStoryEnd += () => {
            UIManager.instance.PushPanel(UIPanelType.DialogPanel);
            //CameraHandler.instance.enabled = true;
            FindObjectOfType<PlayerManager>().enabled = true;
        };
    }

    private void OnDisable()
    {
        DialogManager.instance.OnStoryStart -= () => { UIManager.instance.PushPanel(UIPanelType.DialogPanel); };
        DialogManager.instance.OnStoryEnd -= () => { UIManager.instance.PushPanel(UIPanelType.DialogPanel); };
    }

    private void Update()
    {
        if (!DialogManager.instance.storyIsPlaying && InputHandler.instance.GetContinuePressed())
        {
            DialogManager.instance.EnterDialogueMode(inkJson);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player") return;

        
    }
}
