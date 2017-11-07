using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class QuestionManager : MonoBehaviour {

    public Transform[] shelfs;

    public GameObject carrotPre;
    public GameObject eggplantPre;
    public GameObject greenApplePre;
    public GameObject greenBananaPre;
    public GameObject lemonPre;
    public GameObject melonPre;
    public GameObject orangePre;

    public Text wordballoonText;
    [HideInInspector]
    public List<GameObject> things = new List<GameObject>();    //物品列表，便于管理
    public static RandomManager rm = RandomManager.Instance();  
    private JsonManager jm;         //最终选定的物品预设赋给它
    private GameObject prefab;
    [HideInInspector]
    public string[] fruitsArr = new string[] { "萝卜", "茄子", "绿苹果", "绿香蕉", "柠檬", "哈密瓜", "橙子" };
    [HideInInspector]
    public string[] sizeArr = new string[] { "最大的", "中等的", "最小的" };
    [HideInInspector]
    public string[] whoArr = new string[] { "前者", "后者" };
    [HideInInspector]
    public List<string> fruitStr = new List<string>();   //水果名，用作提问题，可能一个，可能两个
    [HideInInspector]
    public List<string> sizeStr = new List<string>();   //水果大小，用作提问题
    [HideInInspector]
    public List<string> whoStr = new List<string>();    //指定选谁，用作提问题
    [HideInInspector]
    public string[] finalyFruitStr = new string[2];    //最终确定指定的水果名
    [HideInInspector]
    public int questionCount = 0;   //出题数

    void Awake()
    {
        jm = GameObject.Find("jsonManager").GetComponent<JsonManager>();
        shelfs = new Transform[4];
        for(int i = 0; i < 4; i++)
        {
            shelfs[i] = GameObject.Find("Canvas").transform.FindChild("shelfs").GetChild(i);
        }
    }

    //生成题目
    public IEnumerator CreateQuestion(int level)
    {
        //选择出题形式，题库or随机，内有题号增加
        chooseQuestionStyle(level);

        if (questionCount % 2 != 0) //轮流出架子
            EnterShelf01(); //奇数题号出01架子
        else
            EnterShelf23(); //偶数题号出23架子
        //生成水果
        CreateFruits(fruitStr[0], fruitStr[1]);
        //生成提问问题
        yield return new WaitForSeconds(2.3f);  //等待架子进入完毕后,再提问
        if (whoStr.Count == 1)
            CreateProblemByTest1(sizeStr[0], fruitStr[0], fruitStr[1], whoStr[0]);
        else if(sizeStr.Count==2)
            CreateProblemByTest2(sizeStr[0], sizeStr[1], fruitStr[0], fruitStr[1]);

    }

    /// <summary>
    /// 只负责生成两种水果，上下架放的顺序打乱
    /// </summary>
    /// <param name="fruit1">传入的是中文字符串</param>
    /// <param name="fruit2">传入的是中文字符串</param>
    void CreateFruits(string fruit1, string fruit2)
    {
        GameObject pre1 = GetFruitsPrefabByString(fruit1);
        GameObject pre2 = GetFruitsPrefabByString(fruit2);
        
        Vector3 go1Scale = pre1.transform.localScale;
        Vector3 go2Scale = pre2.transform.localScale;
        float k = 1.0f;
        //分别生成三个，并且逐渐增大
        for(int i = 0; i < 3; i++)
        {
            things.Add(Instantiate(pre1));
            iTween.ScaleTo(things[things.Count - 1], iTween.Hash("time", 0, "scale",
                new Vector3(go1Scale.x * k, go1Scale.y * k, go1Scale.z * k),
                "easetype", iTween.EaseType.linear));

            things.Add(Instantiate(pre2));
            iTween.ScaleTo(things[things.Count - 1], iTween.Hash("time", 0, "scale",
                new Vector3(go2Scale.x * k, go2Scale.y * k, go2Scale.z * k),
                "easetype", iTween.EaseType.linear));

            k *= 1.4f;
        }
        //对物品的大小做自适应，位置自适应在架子排序那里处理
        for(int i = 0; i < 6; i++)
        {
            Vector2 temp = things[i].GetComponent<RectTransform>().sizeDelta;
            things[i].GetComponent<RectTransform>().sizeDelta = new Vector2(temp.x * UIAdaptive.k.x, temp.y * UIAdaptive.k.y);
        }
        //在架子上排好位置，乱序
        SortOnShelfs();
    }
#region 架子上排序专用
    //在架子上摆好位置
    void SortOnShelfs()
    {
        List<GameObject> temp1 = new List<GameObject>();    //临时列表
        List<GameObject> temp2 = new List<GameObject>();
        System.Random r = new System.Random();
        int r1; //随机数，决定上下架
        int[] a = new int[3];
        SortRandom(out a[0], out a[1], out a[2]);
        int[] b = new int[3];
        SortRandom(out b[0], out b[1], out b[2]);
        for (int i = 0; i < 6; i++)
        {
            r1 = r.Next(0, 2);
            if (r1 == 0)    //在上架
            {
                if (temp1.Count < 3)    //确保上下架的数目各为3
                {
                    if (questionCount % 2 != 0) //若是奇数题号，说明出了01架子，应放在其上面
                        things[i].transform.parent = shelfs[0].GetChild(0);
                    else
                        things[i].transform.parent = shelfs[2].GetChild(0);
                    temp1.Add(things[i]);
                }
                else
                {
                    if (questionCount % 2 != 0)
                        things[i].transform.parent = shelfs[1].GetChild(0);
                    else
                        things[i].transform.parent = shelfs[3].GetChild(0);
                    temp2.Add(things[i]);
                }
            }
            else //在下架
            {
                if (temp2.Count < 3)    //确保上下架的数目各为3
                {
                    if (questionCount % 2 != 0)
                        things[i].transform.parent = shelfs[1].GetChild(0);
                    else
                        things[i].transform.parent = shelfs[3].GetChild(0);
                    temp2.Add(things[i]);
                }
                else
                {
                    if (questionCount % 2 != 0)
                        things[i].transform.parent = shelfs[0].GetChild(0);
                    else
                        things[i].transform.parent = shelfs[2].GetChild(0);
                    temp1.Add(things[i]);
                }
            }
        }
        for(int i = 0; i < temp1.Count; i++)
        {
            float y = temp1[i].transform.GetComponent<RectTransform>().sizeDelta.y / 2;
            temp1[i].transform.GetComponent<RectTransform>().localPosition = SortPosition(a[i],y);
        }
        for (int i = 0; i < temp2.Count; i++)
        {
            float y = temp2[i].transform.GetComponent<RectTransform>().sizeDelta.y / 2;
            temp2[i].transform.GetComponent<RectTransform>().localPosition = SortPosition(b[i],y);
        }
        temp1.Clear();
        temp2.Clear();
    }
    //0,1,2其中之一，但不重复
    void SortRandom(out int x1,out int x2,out int x3)
    {
        System.Random r = new System.Random();
        x1 = r.Next(0, 3);
        while (true)
        {
            x2 = r.Next(0, 3);
            if (x1 != x2) break;
        }
        while (true)
        {
            x3 = r.Next(0, 3);
            if ((x3 != x2) && (x3 != x1)) break;
        }
    }
    //根据0,1,2决定最终位置
    Vector3 SortPosition(int x,float y)
    {
        Vector3 pos = new Vector3();
        
        switch (x)
        {
            case 0:
                pos = new Vector3(-165 * UIAdaptive.k.x, y * UIAdaptive.k.y, 0);
                break;
            case 1:
                pos = new Vector3(-5 * UIAdaptive.k.x, y * UIAdaptive.k.y, 0);
                break;
            case 2:
                pos = new Vector3(155 * UIAdaptive.k.x, y * UIAdaptive.k.y, 0);
                break;
        }
        return pos;
    }
    #endregion
    //生成提问问题，根据题库第一部分
    void CreateProblemByTest1(string size, string fruit1, string fruit2, string who)
    {
        wordballoonText.transform.parent.gameObject.SetActive(true);
        if (who.Equals("前者"))
            finalyFruitStr[0] = fruit1;
        else if (who.Equals("后者"))
            finalyFruitStr[0] = fruit2;

        wordballoonText.text = "请帮我找出" + size + finalyFruitStr[0];
    }

    //生成提问问题，根据题库第一部分
    void CreateProblemByTest2(string size1, string size2, string fruit1, string fruit2)
    {
        wordballoonText.transform.parent.gameObject.SetActive(true);
        finalyFruitStr[0] = fruit1;
        finalyFruitStr[1] = fruit2;
        wordballoonText.text = "请帮我找出" + size1 + finalyFruitStr[0] + "和" + size2 + finalyFruitStr[1];
    }

    public void EnterShelf01()
    {
        iTween.MoveBy(shelfs[0].gameObject, iTween.Hash("delay", 0.0f, "time", 2.0f, "x", -shelfs[0].GetComponent<RectTransform>().sizeDelta.x, "easetype", iTween.EaseType.linear));
        iTween.MoveBy(shelfs[1].gameObject, iTween.Hash("delay", 0.0f, "time", 2.0f, "x", -shelfs[1].GetComponent<RectTransform>().sizeDelta.x, "easetype", iTween.EaseType.linear));
    }

    public void EnterShelf23()
    {
        iTween.MoveBy(shelfs[2].gameObject, iTween.Hash("delay", 0.0f, "time", 2.0f, "x", -shelfs[2].GetComponent<RectTransform>().sizeDelta.x, "easetype", iTween.EaseType.linear));
        iTween.MoveBy(shelfs[3].gameObject, iTween.Hash("delay", 0.0f, "time", 2.0f, "x", -shelfs[3].GetComponent<RectTransform>().sizeDelta.x, "easetype", iTween.EaseType.linear));
    }

    //根据字符串获取水果种类预设
    GameObject GetFruitsPrefabByString(string fruit)
    {
        switch (fruit)
        {
            case "萝卜":
                prefab = carrotPre;
                break;
            case "茄子":
                prefab = eggplantPre;
                break;
            case "绿苹果":
                prefab = greenApplePre;
                break;
            case "绿香蕉":
                prefab = greenBananaPre;
                break;
            case "柠檬":
                prefab = lemonPre;
                break;
            case "哈密瓜":
                prefab = melonPre;
                break;
            case "橙子":
                prefab = orangePre;
                break;
        }
        return prefab;
    }

    void chooseQuestionStyle(int level)
    {
        if (level == 1)
        {
            //若有题库，优先使用题库出题
            if (jm.testFruits1.Count > 0)
            {
                questionCount = jm.testFruits1[0].ID;   //获取题号
                sizeStr.Add(ConvertString(jm.testFruits1[0].Size));    //获取size
                fruitStr.Add(ConvertString(jm.testFruits1[0].Fruit1)); //获取水果名
                fruitStr.Add(ConvertString(jm.testFruits1[0].Fruit2)); //获取水果名
                whoStr.Add(ConvertString(jm.testFruits1[0].Who));      //获取指定选谁
                jm.testFruits1.RemoveAt(0);
            }
            //否则,若题库用完了或没有题库，那么使用随机出题
            else
            {
                rm.createRandom(level);
                System.Random r = new System.Random();
                questionCount++;
                sizeStr.Add(sizeArr[r.Next(0, 3)]);       //获取size
                fruitStr.Add(fruitsArr[rm.fruitsRandom1]);   //获取水果名
                fruitStr.Add(fruitsArr[rm.fruitsRandom2]);   //获取水果名
                whoStr.Add(whoArr[r.Next(0, 2)]);         //获取指定选谁
            }
        }
        if(level == 2)
        {
            //若有题库，优先使用题库出题
            if (jm.testFruits2.Count > 0)
            {
                questionCount = jm.testFruits2[0].ID;   //获取题号
                sizeStr.Add(ConvertString(jm.testFruits2[0].Size1));    //获取size1
                sizeStr.Add(ConvertString(jm.testFruits2[0].Size2));    //获取size2
                fruitStr.Add(ConvertString(jm.testFruits2[0].Fruit1)); //获取水果名1
                fruitStr.Add(ConvertString(jm.testFruits2[0].Fruit2)); //获取水果名2
                jm.testFruits2.RemoveAt(0);
            }
            //否则,若题库用完了或没有题库，那么使用随机出题
            else
            {
                rm.createRandom(level);
                System.Random r = new System.Random();
                questionCount++;
                sizeStr.Add(sizeArr[r.Next(0, 3)]);       //获取size1
                sizeStr.Add(sizeArr[r.Next(0, 3)]);       //获取size2
                fruitStr.Add(fruitsArr[rm.fruitsRandom1]);   //获取水果名
                fruitStr.Add(fruitsArr[rm.fruitsRandom2]);   //获取水果名
            }
        }
    }
    //将json的英文数据转化为中文数据
    string ConvertString(string str)
    {
        switch (str)
        {
            case "carrot":
                return "萝卜";
            case "eggplant":
                return "茄子";
            case "greenApple":
                return "绿苹果";
            case "greenBanana":
                return "绿香蕉";
            case "lemon":
                return "柠檬";
            case "melon":
                return "哈密瓜";
            case "orange":
                return "橙子";
            case "first":
                return "前者";
            case "second":
                return "后者";
            case "the biggest":
                return "最大的";
            case "the smallest":
                return "最小的";
            case "the middle":
                return "中等的";
            default:
                return null;
        }
    }

    //   public GameObject chickenPrefab1;
    //   public GameObject dogPrefab1;
    //   public GameObject lionPrefab1;
    //   public GameObject pigPrefab1;
    //   public GameObject sheepPrefab1;
    //   public GameObject wolfPrefab1;
    //   [HideInInspector]
    //   public GameObject prefab;   //最终选定的动物预设赋给它
    //   [HideInInspector]
    //   public Transform leftCreatePoint;   //左出生点
    //   [HideInInspector]
    //   public float houseSize_width;   //房屋的宽度
    //   [HideInInspector]
    //   public float houseCapcity;      //房屋的容量的宽度
    //   [HideInInspector]
    //   public List<GameObject> animals = new List<GameObject>();   //动物列表，方便管理
    //   [HideInInspector]
    //   public bool existRanCount = false;
    //   [HideInInspector]
    //   public int initCount;   //初始数量
    //   [HideInInspector]
    //   public int addCount;    //补进数量
    //   [HideInInspector]
    //   public int ranCount;    //逃跑数量

    //   float lastTime;         //最后一个动物进屋所需时间

    //   public static RandomManager rm = RandomManager.Instance();
    //   private JsonManager jm;
    //   private Transform canvas;   //画布的引用
    //   private Text TextID;        //显示的题号
    //   private int testID = 0;

    //   private List<Transform> createPointObjs = new List<Transform>();
    //   private RectTransform[] ct; //获取出生点下物体的RectTransform

    //   void Start () {
    //       jm = GameObject.Find("jsonManager").GetComponent<JsonManager>();
    //       canvas = GameObject.Find("Canvas").transform;
    //       TextID = canvas.FindChild("PanelUI").FindChild("TestID").GetComponent<Text>();
    //       //找到左出生点的引用
    //       leftCreatePoint = canvas.FindChild("leftCreatePoint");
    //   }

    //   public IEnumerator createQuestion(int level)
    //   {
    //       rm.createRandom(level);

    //       chooseQuestionStyle();
    //       if (testID > 0)
    //           TextID.text = "第" + testID + "题";
    //       yield return createAnimalsByCount(initCount, randomKindsPrefab());
    //       //等待最后一个动物进屋后再...
    //       lastTime = 5.6f;
    //       yield return addInOrder(addCount);
    //       yield return ranInOrder(ranCount);

    //       sortInHouse();
    //   }
    //   //根据输入数量生成相应数量的动物
    //   public IEnumerator createAnimalsByCount(int count, GameObject prefab)
    //   {
    //       for (int i = 0; i < count; i++)
    //       {
    //           animals.Add(Instantiate(prefab));
    //           animals[animals.Count - 1].transform.parent = leftCreatePoint;
    //           //--让动物自适应大小--//
    //           Vector3 size = animals[animals.Count - 1].GetComponent<RectTransform>().sizeDelta;
    //           animals[animals.Count - 1].GetComponent<RectTransform>().sizeDelta = new Vector3(size.x * UIAdaptive.k.x, size.y * UIAdaptive.k.y, 1);

    //           findAllChildren(animals[animals.Count - 1].transform);
    //           initUI();
    //           createPointObjs.Clear();    //每生出一个 自适应一个，并清空表，供下一个使用
    //           //--让动物自适应大小--//
    //           animals[animals.Count - 1].transform.localPosition = new Vector3(0, 0, 0);
    //           yield return new WaitForSeconds(1.2f);
    //       }
    //   }
    //   //让动物有序的有间隔时间地逃跑
    //   public IEnumerator ranInOrder(int count)
    //   {
    //       if (count > 0)
    //       {
    //           //若有逃跑者，让它跑！跑到屏幕外就自动销毁
    //           yield return new WaitForSeconds(lastTime);
    //           //等完后，有逃跑的再逃跑
    //           existRanCount = true;
    //           for (int i = 0; i < count; i++)
    //           {
    //               //让相应动物逃跑
    //               animals[animals.Count - 1].GetComponent<AnimalsBehaviourScript>().canRan = true;
    //               animals[animals.Count - 1].GetComponent<AnimalsBehaviourScript>().changeVoice = true;
    //               yield return new WaitForSeconds(1.2f);
    //               //在列表中移除
    //               animals.RemoveAt(animals.Count - 1);
    //               //销毁相应动物在动物脚本内已经实现
    //           }
    //           existRanCount = false;//关闭逃跑
    //       }
    //       //再等逃跑的逃跑完后，在生成答案中调用时再出现答案
    //       yield return new WaitForSeconds(lastTime);
    //   }
    //   //让动物有序的有间隔时间地补进
    //   public IEnumerator addInOrder(int count)
    //   {
    //       if (count > 0)
    //       {
    //           yield return new WaitForSeconds(lastTime);
    //           //等完后，有补进的再补进
    //           yield return createAnimalsByCount(count, randomKindsPrefab());
    //       }
    //   }
    //   //根据随机数获取随机物种
    //   GameObject randomKindsPrefab()
    //   {

    //       switch (rm.kindsRandom)
    //       {
    //           case 0:
    //               prefab = chickenPrefab1;
    //               break;
    //           case 1:
    //               prefab = dogPrefab1;
    //               break;
    //           case 2:
    //               prefab = lionPrefab1;
    //               break;
    //           case 3:
    //               prefab = pigPrefab1;
    //               break;
    //           case 4:
    //               prefab = sheepPrefab1;
    //               break;
    //           case 5:
    //               prefab = wolfPrefab1;
    //               break;
    //       }
    //       return prefab;
    //   }

    //   //选择出题方式，题库or随机
    //   void chooseQuestionStyle()
    //   {
    //       //若有题库，优先使用题库出题
    //       if (jm.tests.Count > 0)
    //       {
    //           testID = jm.tests[0].ID;
    //           initCount = jm.tests[0].initCount;
    //           addCount = jm.tests[0].addCount;
    //           ranCount = jm.tests[0].ranCount;
    //           jm.tests.RemoveAt(0);
    //       }
    //       //否则,若题库用完了或没有题库，那么使用随机出题
    //       else
    //       {
    //           testID++;
    //           initCount = rm.initCountRandom;
    //           addCount = rm.addCountRandom;
    //           ranCount = rm.ranCountRandom;
    //       }
    //   }
    //   //最终留下的动物在房屋内排好序，方便数数
    //   void sortInHouse()
    //   {
    //       Vector3 destination;
    //       if (animals.Count <= 0) return;
    //       if (animals.Count == 1) //若只有一个，那就放在正中间咯
    //       {
    //           destination = new Vector3(Screen.width / 2, 0, 0);
    //           animals[0].transform.GetComponent<AnimalsBehaviourScript>().destination = destination;
    //           return;
    //       }

    //       houseSize_width = GameObject.Find("house").GetComponent<RectTransform>().rect.width * 0.90f;//计算得
    //       houseCapcity = houseSize_width - animals[0].transform.GetComponent<RectTransform>().sizeDelta.x;//减一个身位长度
    //       float interval = houseCapcity / (animals.Count - 1);    //动物间隔宽度
    //       float halfSize_X = animals[0].transform.GetComponent<RectTransform>().sizeDelta.x / 2;  //半个身位长度
    //       Debug.Log("houseSize_width = " + houseSize_width);
    //       Debug.Log("houseCapcity = " + houseCapcity);
    //       Debug.Log("interval = " + interval);
    //       Debug.Log("halfSize_X = " + halfSize_X);
    //       for (int i = 0; i < animals.Count; i++)
    //       {
    //           //计算分析得
    //           destination = new Vector3(Screen.width / 2.0f - houseCapcity / 2.0f + i * interval + halfSize_X, 0, 0);
    //           Debug.Log("第" + i + "个动物坐标 = " + destination);
    //           animals[i].transform.GetComponent<AnimalsBehaviourScript>().destination = destination;
    //       }
    //       Debug.Log(animals[animals.Count - 1].transform.GetComponent<AnimalsBehaviourScript>().destination
    //           - animals[0].transform.GetComponent<AnimalsBehaviourScript>().destination);
    //   }

    //   void findAllChildren(Transform uiObject)
    //   {
    //       if (uiObject.childCount <= 0) return;   //若没孩子就别往下了
    //       for (int i = 0; i < uiObject.childCount; i++)
    //       {
    //           createPointObjs.Add(uiObject.GetChild(i));
    //           findAllChildren(uiObject.GetChild(i));  //递归查找
    //       }
    //   }
    //   void initUI()
    //   {
    //       ct = new RectTransform[createPointObjs.Count];
    //       for (int i = 0; i < createPointObjs.Count; i++)
    //       {
    //           ct[i] = createPointObjs[i].GetComponent<RectTransform>(); //获得每个UI元素的RectTransform
    //       }

    //       for (int i = 0; i < createPointObjs.Count; i++)  //必须是计算以锚点位置的大小
    //       {
    //           ct[i].anchoredPosition = new Vector3(ct[i].anchoredPosition.x * UIAdaptive.k.x, ct[i].anchoredPosition.y * UIAdaptive.k.y, 0);
    //           ct[i].sizeDelta = new Vector3(ct[i].sizeDelta.x * UIAdaptive.k.x, ct[i].sizeDelta.y * UIAdaptive.k.y, 1);
    //       }
    //   }

    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(100, 300, 800, 800), "houseSize_width = " + houseSize_width.ToString());
    //    GUI.Label(new Rect(100, 350, 800, 800), "houseCapcity = " + houseCapcity.ToString());
    //}
}
