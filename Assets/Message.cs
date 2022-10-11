using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    [HideInInspector]
    public Root Parent { private get; set; }
    public GameObject Based { private get; set; }
    [HideInInspector]
    public List<Contents> child = new List<Contents>();
    [SerializeField]
    private GameObject flgPrefab = null;

    [SerializeField]
    private InputField key;

    public Msg Me { get; set; }

    public void Add()
    {
        Flg flg = new Flg()
        {
            contents = new List<Pg>()
        };
        Me.flags.Add(flg);

        GameObject obj = Instantiate(flgPrefab, Based.transform);

        Contents contents = obj.GetComponent<Contents>();
        contents.Parent = this;
        contents.Based = Based;
        contents.Me = flg;
        contents.flag.text = child.Count.ToString();
        contents.Add();

        child.Add(contents);
        
        Manager.Instance.ScreenUpdate();
    }

    public void Destroy()
    {
        for (int index = child.Count; index > 0; index--)
        {
            Contents contents = child[index - 1];
            contents.Destroy();
        }
        Me.flags.Clear();

        Parent.child.Remove(this);
        Manager.Instance.data.messages.Remove(Me);
        Destroy(gameObject);
        Manager.Instance.ScreenUpdate();
    }

    public float ScreenUpdate(float y)
    {
        RectTransform rectTrans = GetComponent<RectTransform>();
        y -= Manager.Instance.distance;
        rectTrans.localPosition = Vector3.right * rectTrans.localPosition.x + Vector3.up * y;
        y -= rectTrans.rect.height;

        foreach (Contents contents in child)
        {
            y = contents.ScreenUpdate(y);
        }
        return y;
    }

    public void Load(Msg message)
    {
        key.text = message.key;

        for (int index = 0; index < message.flags.Count; index++)
        {
            GameObject obj = Instantiate(flgPrefab, Based.transform);

            Contents contents = obj.GetComponent<Contents>();
            contents.Parent = this;
            contents.Based = Based;
            contents.Me = message.flags[index];
            contents.flag.text = child.Count.ToString();

            child.Add(contents);
        }
        for (int index = 0; index < message.flags.Count; index++)
        {
            Contents contents = child[index].GetComponent<Contents>();
            contents.Load(message.flags[index]);
        }
    }

    public void SortContents()
    {
        for (int index = 0; index < child.Count; index++)
        {
            child[index].flag.text = index.ToString();
        }
    }

    public void Key()
    {
        Me.key = key.text;
    }
}
