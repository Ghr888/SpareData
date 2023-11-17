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
/// �����ϵͳ
/// </summary>
public static class PoolSystem
{
    #region �����ϵͳ���ݼ���̬���췽��
    ///<summary>
    ///����ز��������Ϸ�������ƣ����ڽ�������Ҳ�Ž������
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

    #region GameObject��������API(��ʼ����ȡ�������롢���)
    ///<summary>
    ///��ʼ��һ��GameObject���͵Ķ��������
    ///</summary>
    /// <param name="keyName">��Դ����</param>
    /// <param name="maxCapacity">�������ƣ�����ʱ�����ٶ����ǽ������أ�-1��������</param>
    /// <param name="defaultQuantity">Ĭ����������д��������з����Ӧ�����Ķ���0����Ԥ�ȷ���</param>
    /// <param name="prefab">��дĬ������ʱԤ�ȷ���Ķ���</param>
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
    /// ��ʼ�������
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="maxCapacity">���������-1��������</param>
    /// <param name="gameObjects">Ĭ��Ҫ�Ž����Ķ�������</param>
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
    /// ��ʼ������ز���������
    /// </summary>
    /// <param name="maxCapacity">�������ƣ�����ʱ�����ٶ����ǽ������أ�-1��������</param>
    /// <param name="defaultQuantity">Ĭ����������д��������з����Ӧ�����Ķ���0����Ԥ�ȷ���</param>
    /// <param name="prefab">��дĬ������ʱԤ�ȷ���Ķ���</param>
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
