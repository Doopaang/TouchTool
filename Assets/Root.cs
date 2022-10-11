using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField]
    private GameObject based = null;
    [HideInInspector]
    public List<Message> child = new List<Message>();
    [SerializeField]
    private GameObject msgPrefab = null;
    
    public void Add()
    {
        Msg msg = new Msg()
        {
            key = "",
            flags = new List<Flg>()
        };
        Manager.Instance.data.messages.Add(msg);

        GameObject obj = Instantiate(msgPrefab, based.transform);
        
        Message message = obj.GetComponent<Message>();
        message.Parent = this;
        message.Based = based;
        message.Me = msg;
        message.Add();

        child.Add(message);
        Manager.Instance.ScreenUpdate();
    }

    public float ScreenUpdate()
    {
        RectTransform rectTrans = GetComponent<RectTransform>();
        float y = rectTrans.localPosition.y;
        y -= rectTrans.rect.height;

        foreach (Message message in child)
        {
            y = message.ScreenUpdate(y);
        }
        return y;
    }

    public void Load(Data data)
    {
        if(data.messages.Count == 0)
        {
            return;
        }

        for (int index = 0; index < data.messages.Count; index++)
        {
            GameObject obj = Instantiate(msgPrefab, based.transform);

            Message message = obj.GetComponent<Message>();
            message.Parent = this;
            message.Based = based;
            message.Me = data.messages[index];
            message.Load(data.messages[index]);

            child.Add(message);
        }
    }
}
