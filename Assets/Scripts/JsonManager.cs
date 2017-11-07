using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class JsonManager : MonoBehaviour {

#region 题库第一部分的属性
    [HideInInspector]
    public List<Test1Item> testFruits1;
    [Serializable]
    public class Test1Item
    {
        public int ID;          //题号
        public string Size;     //大小
        public string Fruit1;   //第一种水果
        public string Fruit2;   //第二种水果
        public string Who;      //指定是选谁？
    }
    [Serializable]
    public class Test1
    {
        public List<Test1Item> testFruits1;
    }
    #endregion

    #region 题库第二部分的属性
    [HideInInspector]
    public List<Test2Item> testFruits2;
    [Serializable]
    public class Test2Item
    {
        public int ID;          //题号
        public string Size1;    //第一种水果的大小
        public string Size2;    //第二种水果的大小
        public string Fruit1;   //第一种水果
        public string Fruit2;   //第二种水果
    }
    [Serializable]
    public class Test2
    {
        public List<Test2Item> testFruits2;
    }
    #endregion

    void Awake()
    {
        string json = File.ReadAllText("H:\\云孩科技/Json/testFruits.txt");  //读取json数据
        JsonTest1(json);
        JsonTest2(json);
    }

    void Start () {
        
	}

    public void JsonTest1(string json)
    {
        if (json != string.Empty)
        {
            Test1 item = JsonUtility.FromJson<Test1>(json);     //反序列化后存储到类或结构体
            testFruits1 = item.testFruits1;                     //获取类的对象拥有的属性列表
        }
    }

    public void JsonTest2(string json)
    {
        if (json != string.Empty)
        {
            Test2 item = JsonUtility.FromJson<Test2>(json);     //反序列化后存储到类或结构体
            testFruits2 = item.testFruits2;                     //获取类的对象拥有的属性列表
        }
    }
}

//{
//	"testFruits1":[
//		{"ID": 1,"Size": "the smallest","Fruit1": "greenApple","Fruit2": "eggplant","Who":"first"},
//		{"ID": 2,"Size": "the biggest","Fruit1": "greenBanana","Fruit2": "lemon","Who":"second"},
//		{"ID": 3,"Size": "the middle","Fruit1": "greenApple","Fruit2": "orange","Who":"second"},
//		{"ID": 4,"Size": "the biggest","Fruit1": "greenBanana","Fruit2": "eggplant","Who":"second"},
//		{"ID": 5,"Size": "the smallest","Fruit1": "melon","Fruit2": "carrot","Who":"first"}
//	]
//}


//{
//	"testFruits1":[
//		{"ID": 1,"Size": "the smallest","Fruit1": "greenApple","Fruit2": "eggplant","Who":"first"},
//		{"ID": 2,"Size": "the biggest","Fruit1": "greenBanana","Fruit2": "lemon","Who":"second"}
//	],
//	"testFruits2":[
//		{"ID": 1,"Size1": "the smallest","Size2": "the biggest","Fruit1": "greenApple","Fruit2": "eggplant"},
//		{"ID": 2,"Size1": "the biggest","Size2": "the smallest","Fruit1": "greenBanana","Fruit2": "lemon"}
//	]
//}

