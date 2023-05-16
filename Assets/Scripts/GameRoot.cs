using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot instance { get; private set; }
    public TextAsset panelType2PathJsonText;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one GameRoot in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        //在游戏最开始先将所有Panel实例化好，防止其它脚本在游戏开始时引用Panel的脚本失效。
        UIManager.instance.Init(panelType2PathJsonText);
        foreach (var item in System.Enum.GetNames(typeof(UIPanelType)).Select(name => (UIPanelType)System.Enum.Parse(typeof(UIPanelType), name)))
        {
            UIManager.instance.PushPanel(item);
            UIManager.instance.PopPanel();
        }
    }

    private void Start()
    {
       
    }

}
