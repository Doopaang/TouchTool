using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Manager : MonoBehaviour
{
    [HideInInspector]
    public static Manager Instance { get; private set; }

    [HideInInspector]
    public Data data = new Data();

    [SerializeField]
    private InputField inputField;
    private string fileName;

    [SerializeField]
    private GameObject root;
    [SerializeField]
    private GameObject content;

    [HideInInspector]
    public float distance = 40.0f;

    private string path;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        data.messages = new List<Msg>();

        path = Application.dataPath + "/../";

        Screen.SetResolution(Screen.width, Screen.width / 16 * 9, Screen.fullScreen);
    }

    public void ScreenUpdate()
    {
        Root rt = root.GetComponent<Root>();
        float y = rt.ScreenUpdate();

        RectTransform rtTrans = rt.GetComponent<RectTransform>();
        RectTransform contTrans = content.GetComponent<RectTransform>();
        float height = -rtTrans.localPosition.y * 2.0f - y + rtTrans.rect.height;

        if (height > Screen.height)
        {
            contTrans.sizeDelta = Vector2.right * contTrans.sizeDelta.x + Vector2.up * height;
        }
        else
        {
            contTrans.sizeDelta = Vector2.right * contTrans.sizeDelta.x + Vector2.up * Screen.height;
        }
    }

    public void Name()
    {
        fileName = inputField.text;
    }

    public void Save()
    {
        string str = JsonUtility.ToJson(data, true);
        str.Replace("\n", "\r\n");
        File.WriteAllText(path + fileName + ".json", str);
        Debug.Log(str);
    }

    public void Load()
    {
        Root rt = root.GetComponent<Root>();
        for (int index = rt.child.Count; index > 0; index--)
        {
            rt.child[index - 1].Destroy();
        }

        string str = File.ReadAllText(path + fileName + ".json", System.Text.Encoding.UTF8);
        data = JsonUtility.FromJson<Data>(str);

        rt.Load(data);
        Manager.Instance.ScreenUpdate();
    }
}

[System.Serializable]
public class Data
{
    public List<Msg> messages;

    public Msg Find(string key)
    {
        foreach (Msg msg in messages)
        {
            if (msg.key == key)
            {
                return msg;
            }
        }
        return null;
    }
}

[System.Serializable]
public class Msg
{
    public string key;
    public int flag;
    public List<Flg> flags;
}
[System.Serializable]
public class Flg
{
    public List<Pg> contents;
}
[System.Serializable]
public class Pg
{
    public int type;
    public string message;
    public List<Cho> choice;
    public int flag;
}
[System.Serializable]
public class Cho
{
    public int page;
    public string text;
}