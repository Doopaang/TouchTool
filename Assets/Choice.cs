using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    [HideInInspector]
    public Page Parent { private get; set; }
    
    public Dropdown page;
    [SerializeField]
    private InputField text;

    [HideInInspector]
    public Cho Me { get; set; }

    public void Destroy()
    {
        Parent.child.Remove(this);
        Parent.Me.choice.Remove(Me);
        Destroy(gameObject);
        Manager.Instance.ScreenUpdate();
    }

    public float ScreenUpdate(float y)
    {
        RectTransform rectTrans = GetComponent<RectTransform>();
        y -= Manager.Instance.distance;
        rectTrans.localPosition = Vector3.right * rectTrans.localPosition.x + Vector3.up * y;
        y -= rectTrans.rect.height;

        return y;
    }

    public void Load(Cho choice)
    {
        page.value = choice.page;
        text.text = choice.text;
    }

    public void Up()
    {
        int index = Parent.child.IndexOf(this);

        if (index <= 0)
        {
            return;
        }

        Parent.Me.choice.Remove(Me);
        Parent.Me.choice.Insert(index - 1, Me);
        Parent.child.Remove(this);
        Parent.child.Insert(index - 1, this);

        Manager.Instance.ScreenUpdate();
    }

    public void Down()
    {
        int index = Parent.child.IndexOf(this);

        if (index == Parent.child.Count - 1)
        {
            return;
        }

        Parent.Me.choice.Remove(Me);
        Parent.Me.choice.Insert(index + 1, Me);
        Parent.child.Remove(this);
        Parent.child.Insert(index + 1, this);

        Manager.Instance.ScreenUpdate();
    }

    public void Page()
    {
        Cho temp = Me;
        temp.page = page.value;
        Me = temp;
    }
    
    public void Text()
    {
        Cho temp = Me;
        temp.text = text.text;
        Me = temp;
    }
}
