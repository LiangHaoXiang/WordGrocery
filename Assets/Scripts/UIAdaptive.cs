using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIAdaptive : MonoBehaviour {

    private List<Transform> UIobjs = new List<Transform>();
    private RectTransform[] ct;
    private Transform canvas;
    public static Vector2 k;

    void Start () {
        if (GameObject.Find("Canvas"))
        {
            canvas = GameObject.Find("Canvas").transform;
            findAllChildren(canvas);    //找到canvas下的所有物体并添加到UIobjs列表

            initUI();
        }
    }
	
    void findAllChildren(Transform uiObject)
    {
        if (uiObject.childCount <= 0) return;   //若没孩子就别往下了
        for (int i = 0; i < uiObject.childCount; i++)
        {
            UIobjs.Add(uiObject.GetChild(i));   //陆续添加ui物体
            findAllChildren(uiObject.GetChild(i));  //递归查找
        }
    }

    //UI自适应...前提是必须将所有UI元素的锚点都设为左下角才能正确显示？No!
    void initUI()
    {
        Vector2 editScreen = new Vector2(853, 480);   //编辑窗口 高清宽高比，跟标清比值一样
        k = new Vector2(Screen.width / editScreen.x, Screen.height / editScreen.y);//比例值

        ct = new RectTransform[UIobjs.Count];
        for (int i = 0; i < UIobjs.Count; i++)
        {
            ct[i] = UIobjs[i].GetComponent<RectTransform>(); //获得每个UI元素的RectTransform
        }

        for (int i = 0; i < UIobjs.Count; i++)  //必须是计算以锚点位置的坐标
        {
            ct[i].anchoredPosition = new Vector3(ct[i].anchoredPosition.x * k.x, ct[i].anchoredPosition.y * k.y, 0);
            ct[i].sizeDelta = new Vector3(ct[i].sizeDelta.x * k.x, ct[i].sizeDelta.y * k.y, 1);
        }
    }


}
