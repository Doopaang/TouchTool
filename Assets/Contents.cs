using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Contents : MonoBehaviour
{
    [HideInInspector]
    public Message Parent { get; set; }
    public GameObject Based { private get; set; }
    [HideInInspector]
    public List<Page> child = new List<Page>();
    [SerializeField]
    private GameObject pgPrefab = null;

    public Text flag;

    public Flg Me { get; set; }

    public void Add()
    {
        Pg pg = new Pg()
        {
            type = 0,
            message = "",
            choice = new List<Cho>(),
            flag = -1
        };
        Me.contents.Add(pg);

        GameObject obj = Instantiate(pgPrefab, Based.transform);

        Page page = obj.GetComponent<Page>();
        page.Parent = this;
        page.Based = Based;
        page.Me = pg;
        page.page.text = child.Count.ToString();
        
        child.Add(page);

        foreach (Page pgTemp in child)
        {
            foreach (Choice choTemp in pgTemp.child)
            {
                choTemp.page.options.Add(new Dropdown.OptionData((child.Count - 1).ToString()));
            }
        }

        Manager.Instance.ScreenUpdate();
    }

    public void Destroy()
    {
        for (int index = child.Count; index > 0; index--)
        {
            Page page = child[index - 1];
            page.Destroy();
        }
        Me.contents.Clear();

        Parent.child.Remove(this);
        Parent.SortContents();
        Parent.Me.flags.Remove(Me);
        Destroy(gameObject);
        Manager.Instance.ScreenUpdate();
    }

    public float ScreenUpdate(float y)
    {
        RectTransform rectTrans = GetComponent<RectTransform>();
        y -= Manager.Instance.distance;
        rectTrans.localPosition = Vector3.right * rectTrans.localPosition.x + Vector3.up * y;
        y -= rectTrans.rect.height;

        foreach (Page page in child)
        {
            y = page.ScreenUpdate(y);
        }
        return y;
    }

    public void Load(Flg flag)
    {
        for (int index = 0; index < flag.contents.Count; index++)
        {
            GameObject obj = Instantiate(pgPrefab, Based.transform);

            Page page = obj.GetComponent<Page>();
            page.Parent = this;
            page.Based = Based;
            page.Me = flag.contents[index];
            page.page.text = child.Count.ToString();
            
            child.Add(page);

            foreach (Page pgTemp in child)
            {
                foreach (Choice choTemp in pgTemp.child)
                {
                    choTemp.page.options.Add(new Dropdown.OptionData((child.Count - 1).ToString()));
                }
            }
        }
        for (int index = 0; index < flag.contents.Count; index++)
        {
            Page page = child[index].GetComponent<Page>();
            page.Load(flag.contents[index]);
        }
    }

    public void SortPage()
    {
        for (int index = 0; index < child.Count; index++)
        {
            child[index].page.text = index.ToString();
        }
    }

    public void Up()
    {
        int index = Parent.child.IndexOf(this);

        if (index <= 0)
        {
            return;
        }

        Parent.Me.flags.Remove(Me);
        Parent.Me.flags.Insert(index - 1, Me);
        Parent.child.Remove(this);
        Parent.child.Insert(index - 1, this);

        Parent.SortContents();

        Manager.Instance.ScreenUpdate();
    }

    public void Down()
    {
        int index = Parent.child.IndexOf(this);

        if (index == Parent.child.Count - 1)
        {
            return;
        }

        Parent.Me.flags.Remove(Me);
        Parent.Me.flags.Insert(index + 1, Me);
        Parent.child.Remove(this);
        Parent.child.Insert(index + 1, this);

        Parent.SortContents();

        Manager.Instance.ScreenUpdate();
    }
}
