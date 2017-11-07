using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnThingsClick : MonoBehaviour
{
    [HideInInspector]
    public Vector3 size;
    [HideInInspector]
    public string Name;
    [HideInInspector]
    public string sizeStr;
    [HideInInspector]
    public Transform storePoint;
    [HideInInspector]
    public Vector3 storePosition;
    Vector3 orginPosition;
    Transform parent;

    private QuestionManager questionManager; //场景中的出题管理器
    private AnswerManager answerManager;

    void Awake () {
        questionManager = GameObject.Find("questionManager").GetComponent<QuestionManager>();
        answerManager = GameObject.Find("answerManager").GetComponent<AnswerManager>();
        Name = gameObject.name;
        storePoint = GameObject.Find("LeftDown").transform.FindChild("ShopingCart").FindChild("storePosition");
    }
    void Update()
    {
        storePosition = storePoint.position;
        size = transform.localScale;
    }

    void sizeMatch()
    {
        if (size == new Vector3(1, 1, 1))
            sizeStr = "最小的";
        if (size == new Vector3(1.4f, 1.4f, 1.4f))
            sizeStr = "中等的";
        if (size == new Vector3(1.96f, 1.96f, 1.96f))
            sizeStr = "最大的";
    }
    void nameMatch()
    {
        switch (Name)
        {
            case "carrot(Clone)":
                Name = "萝卜";
                break;
            case "eggplant(Clone)":
                Name = "茄子";
                break;
            case "greenApple(Clone)":
                Name = "绿苹果";
                break;
            case "greenBanana(Clone)":
                Name = "绿香蕉";
                break;
            case "lemon(Clone)":
                Name = "柠檬";
                break;
            case "melon(Clone)":
                Name = "哈密瓜";
                break;
            case "orange(Clone)":
                Name = "橙子";
                break;
        }
    }
    //物品点击事件
    public void Click()
    {
        sizeMatch();
        nameMatch();
        orginPosition = gameObject.transform.position;  //摆在架子上的原始位置
        parent = gameObject.transform.parent;           //原始父物体

        Moving();
        if (questionManager.whoStr.Count == 1)  //若是题库的第一部分，即只选一种
        {
            if ((sizeStr.Equals(questionManager.sizeStr[0])) && (Name.Equals(questionManager.finalyFruitStr[0])))
            {
                answerManager.isCorrects[0] = true;
            }
            else
            {
                answerManager.isWrong = true;
                wrongMangae();
            }
        }
        else if (questionManager.sizeStr.Count == 2)    //若是题库的第二部分，即需要选两种
        {
            //若本物品与第一个size以及name匹配或者与第二个size以及name匹配，则正确
            if (((sizeStr.Equals(questionManager.sizeStr[0])) && (Name.Equals(questionManager.finalyFruitStr[0])))
                || ((sizeStr.Equals(questionManager.sizeStr[1])) && (Name.Equals(questionManager.finalyFruitStr[1]))))
            {
                //若还没有正确的，就先让第一个正确，并且让此物品按钮失效，不给再次点击
                if (answerManager.isCorrects[0] == false)
                {
                    answerManager.isCorrects[0] = true;
                    gameObject.GetComponent<Button>().enabled = false;
                }
                else  //否则就是第二个正确的了
                {
                    answerManager.isCorrects[1] = true;
                }        
            }
            else
            {
                answerManager.isWrong = true;
                wrongMangae();
            }
        }
    }

    void Moving()
    {
        iTween.MoveTo(gameObject, iTween.Hash("time", 1.0f, "position", storePosition,
            "easetype", iTween.EaseType.easeInOutSine, "onstart", "parentChange"));
    }
    //答错处理
    void wrongMangae()
    {
        iTween.MoveTo(gameObject, iTween.Hash("time", 1.0f, "position", orginPosition,
            "delay", 2.0f, "easetype", iTween.EaseType.easeInOutSine, "onstart", "parentChange"));
    }
    //父级改变
    void parentChange()
    {
        if (gameObject.transform.parent == parent)
            gameObject.transform.parent = storePoint;
        else
            gameObject.transform.parent = parent;
    }
}
