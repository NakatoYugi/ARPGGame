using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIPanelType
{
    DialogPanel,
}

[Serializable]
public class UIPanelInfo : ISerializationCallbackReceiver
{
    [NonSerialized]
    public UIPanelType uiPanelType;
    public string uiPanelTypeString;
    public string uiPanelPath;

    public void OnAfterDeserialize()
    {
        uiPanelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), uiPanelTypeString);
    }

    public void OnBeforeSerialize()
    {
        
    }
}

public class UIManager
{
    private Dictionary<UIPanelType, string> panelType2Path;
    private Dictionary<UIPanelType, BasePanel> panelType2Panel;
    private Stack<BasePanel> panelStack;
    private Transform canvasTransform;

    private static UIManager mInstance;
    public static UIManager instance { 
        get {
            if (mInstance == null)
                mInstance = new UIManager();

            return mInstance;
        }
        private set { }
    }

    public void Init(TextAsset jsonText)
    {
        GetPathFormJson(jsonText);
    }
    private UIManager()
    {
        canvasTransform = GameObject.Find("Canvas").transform;
    }

    private class UIPanelInfoList
    {
        public List<UIPanelInfo> uiPanelInfoList;
    }

    private void GetPathFormJson(TextAsset jsonText)
    {
        if (panelType2Path == null)
        {
            panelType2Path = new Dictionary<UIPanelType, string>();
        }

        UIPanelInfoList _uIPanelInfoList = JsonUtility.FromJson<UIPanelInfoList>(jsonText.text);
        foreach (var item in _uIPanelInfoList.uiPanelInfoList)
        {
            panelType2Path.Add(item.uiPanelType, item.uiPanelPath);
        }
    }

    private BasePanel GetPanel(UIPanelType uiPanel)
    {
        BasePanel panel = null;

        string path = null;

        panelType2Path.TryGetValue(uiPanel, out path);

        if (path == null)
        {
            Debug.LogWarning("There are no " + uiPanel.ToString() + "'panelPath in Json File, please add it in jsonFile.");
            return panel;
        }

        GameObject panelPrefab = Resources.Load<GameObject>(path);
        GameObject panelGo = GameObject.Instantiate(panelPrefab, canvasTransform);
        panel = panelGo.GetComponent<BasePanel>();

        if (panel == null)
        {
            Debug.LogWarning("GameObjectPrefab:" + panelPrefab.name + " not have BasePanel component");
        }

        return panel;
    }

    public void PushPanel(UIPanelType uiPanel)
    {
        BasePanel panel = null;

        if (panelType2Panel == null)
            panelType2Panel = new Dictionary<UIPanelType, BasePanel>();

        panelType2Panel.TryGetValue(uiPanel, out panel);

        if (panel == null)
        {
            panel = GetPanel(uiPanel);
        }

        if (!panelType2Panel.ContainsKey(uiPanel))
        {
            panelType2Panel.Add(uiPanel, panel);
        }

        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        if (panelStack.Count > 0)
            panelStack.Peek().Pause();

        panelStack.Push(panel);
        panelStack.Peek().Enter();
    }

    public void PopPanel()
    {
        if (panelStack == null)
            panelStack = new Stack<BasePanel>();

        BasePanel topPanel = panelStack.Peek();
        if (topPanel == null)
        {
            Debug.LogWarning("There are no panel in stack, but still want to pop up panel in stack");
        }
        else
        {
            topPanel.Exit();
        }

        if (panelStack.Count < 0)
            return;

        topPanel = panelStack.Peek();
        if (topPanel != null)
        {
            topPanel.Resume();
        }
    }
}
