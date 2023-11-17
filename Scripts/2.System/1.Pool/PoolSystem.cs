using System.Collections;
using UnityEngine;
using System.Collections.Generic;
//using static JKFrame.GameObjectPoolModule;
using Unity.Collections;
using JKFrame;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 对象池系统
/// </summary>
public static class PoolSystem
{
    #region 对象池系统数据及静态构造方法
    ///<summary>
    ///对象池层物体的游戏物体名称，用于将层物体也放进对象池
    ///</summary>
    public const string PoolLayerGameObjectName = "PoolLayerGameObjectName";
    private static GameObjectPoolModule GameObjectPoolModule;
    private static ObjectPoolModule ObjectPoolModule;
    private static Transform poolRootTransform;
    public static void Init()
    {
        GameObjectPoolModule = new GameObjectPoolModule();
        ObjectPoolModule = new ObjectPoolModule();
        poolRootTransform = new GameObject("PoolRoot").transform;
        poolRootTransform.position = Vector3.zero;
        poolRootTransform.SetParent(JKFrameRoot.RootTransform);
        GameObjectPoolModule.Init(poolRootTransform);
    }

    #endregion

    #region GameObject对象池相关API(初始化、取出、放入、清空)
    ///<summary>
    ///初始化一个GameObject类型的对象池类型
    ///</summary>
    /// <param name="keyName">资源名称</param>
    /// <param name="maxCapacity">容量限制，超出时会销毁而不是进入对象池，-1代表无限</param>
    /// <param name="defaultQuantity">默认容量，填写会向池子中放入对应数量的对象，0代表不预先放入</param>
    /// <param name="prefab">填写默认容量时预先放入的对象</param>
    public static void InitGameObjectPool(string keyName,int maxCapacity=-1,GameObject prefab = null,int defaultQuantity = 0)
    {
        GameObjectPoolModule.InitObjectPool(keyName, maxCapacity, prefab, defaultQuantity);
#if UNITY_EDITOR
        if (JKFrameRoot.EditorEventModule != null)
        {
            JKFrameRoot.EditorEventModule.EventTrigger<string, int>("OnInitGameObjectPool", keyName, defaultQuantity);
        }
#endif
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="maxCapacity">最大容量，-1代表无限</param>
    /// <param name="gameObjects">默认要放进来的对象数组</param>
    public static void InitGameObjectPool(string keyName, int maxCapacity, GameObject[] gameObjects = null)
    {
        GameObjectPoolModule.InitObjectPool(keyName, maxCapacity, gameObjects);
#if UNITY_EDITOR
        if (JKFrameRoot.EditorEventModule != null)
        {
            JKFrameRoot.EditorEventModule.EventTrigger<string, int>("OnInitGameObjectPool", keyName, gameObjects.Length);
        }
#endif
    }

    /// <summary>
    /// 初始化对象池并设置容量
    /// </summary>
    /// <param name="maxCapacity">容量限制，超出时会销毁而不是进入对象池，-1代表无限</param>
    /// <param name="defaultQuantity">默认容量，填写会向池子中放入对应数量的对象，0代表不预先放入</param>
    /// <param name="prefab">填写默认容量时预先放入的对象</param>
    public static GameObject GetGameObject(string keyName,Transform parent = null)
    {
        GameObject go = GameObjectPoolModule.GetObject(keyName, parent);
#if UNITY_EDITOR
        if (go != null && JKFrameRoot.EditorEventModule != null) JKFrameRoot.EditorEventModule.EventTrigger<string, int>("OnGetGameObject", keyName, 1);
#endif
        return go;
    }



    #endregion

}
