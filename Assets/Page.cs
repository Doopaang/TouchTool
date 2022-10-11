using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Page : MonoBehaviour
{
    [HideInInspector]
    public Contents Parent { private get; set; }
    public GameObject Based { private get; set; }
    [HideInInspector]
    public List<Choice> child = new List<Choice>();
    [SerializeField]
    private GameObject choPrefab = null;

    public Text page;
    [SerializeField]
    private Dropdown type;
    [SerializeField]
    private InputField message;
    public InputField flag;

    public Pg Me { get; set; }

    public void Add()
    {
        Cho cho = new Cho()
        {
            text = "",
            page = 0
        };
        Me.choice.Add(cho);

        GameObject obj = Instantiate(choPrefab, Based.transform);

        Choice choice = obj.GetComponent<Choice>();
        choice.Parent = this;
        choice.Me = cho;

        for (int index = 0; index < Parent.child.Count; index++)
        {
            choice.page.options.Add(new Dropdown.OptionData(index.ToString()));
        }

        child.Add(choice);
        Manager.Instance.ScreenUpdate();
    }

    public void Destroy()
    {
        for (int index = child.Count; index > 0; index--)
        {
            Choice choice = child[index - 1];
            choice.Destroy();
        }
        Me.choice.Clear();

        foreach (Page page in Parent.child)
        {
            foreach (Choice choice in page.child)
            {
                if (choice.page.options.Count > 0)
                {
                    choice.page.options.RemoveAt(choice.page.options.Count - 1);
                }
            }
        }

        Parent.child.Remove(this);
        Parent.SortPage();
        Parent.Me.contents.Remove(Me);
        Destroy(gameObject);
        Manager.Instance.ScreenUpdate();
    }

    public float ScreenUpdate(float y)
    {
        RectTransform rectTrans = GetComponent<RectTransform>();
        y -= Manager.Instance.distance;
        rectTrans.localPosition = Vector3.right * rectTrans.localPosition.x + Vector3.up * y;
        y -= rectTrans.rect.height;

        foreach (Choice choice in child)
        {
            y = choice.ScreenUpdate(y);
        }
        return y;
    }

    public void Load(Pg page)
    {
        type.value = page.type;
        message.text = page.message;
        flag.text = page.flag.ToString();

        for (int index = 0; index < page.choice.Count; index++)
        {
            GameObject obj = Instantiate(choPrefab, Based.transform);

            Choice choice = obj.GetComponent<Choice>();
            choice.Parent = this;
            choice.Me = page.choice[index];

            child.Add(choice);

            for (int indexT = 0; indexT < Parent.child.Count; indexT++)
            {
                choice.page.options.Add(new Dropdown.OptionData(indexT.ToString()));
            }
        }
        for (int index = 0; index < page.choice.Count; index++)
        {
            Choice choice = child[index].GetComponent<Choice>();
            choice.Load(page.choice[index]);
        }
    }

    public void Up()
    {
        int index = Parent.child.IndexOf(this);

        if (index <= 0)
        {
            return;
        }

        Parent.Me.contents.Remove(Me);
        Parent.Me.contents.Insert(index - 1, Me);
        Parent.child.Remove(this);
        Parent.child.Insert(index - 1, this);

        Parent.SortPage();

        Manager.Instance.ScreenUpdate();
    }

    public void Down()
    {
        int index = Parent.child.IndexOf(this);

        if (index == Parent.child.Count - 1)
        {
            return;
        }

        Parent.Me.contents.Remove(Me);
        Parent.Me.contents.Insert(index + 1, Me);
        Parent.child.Remove(this);
        Parent.child.Insert(index + 1, this);

        Parent.SortPage();

        Manager.Instance.ScreenUpdate();
    }

    public void Type()
    {
        Pg temp = Me;
        temp.type = type.value;
        Me = temp;
    }

    public void Message()
    {
        Pg temp = Me;
        temp.message = message.text;
        Me = temp;
    }
    
    public void Flag()
    {
        Pg temp = Me;
        temp.flag = int.Parse(flag.text);
        Me = temp;
    }
}
