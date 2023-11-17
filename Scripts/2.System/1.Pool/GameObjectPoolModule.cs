using JKFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GameObjectPoolModule
{
    #region GameObjectPoolModule���е����ݼ���ʼ��
    //���ڵ�
    private Transform poolRootTransform;
    ///<summary>
    ///GameObject��������
    ///</summary>
    public Dictionary<string, GameObjectPoolData> poolDic { get; private set; } = new Dictionary<string, GameObjectPoolData>();
    public void Init(Transform poolRootTransform)
    {
        this.poolRootTransform = poolRootTransform;
    }

    /// <summary>
    /// ��ʼ������ز���������
    /// </summary>
    /// <param name="keyName">��Դ����</param>
    /// <param name="maxCapacity">�������ƣ�����ʱ�����ٶ����ǽ������أ�-1��������</param>
    /// <param name="defaultQuantity">Ĭ����������д��������з����Ӧ�����Ķ���0����Ԥ�ȷ���</param>
    /// <param name="prefab">��дĬ������ʱԤ�ȷ���Ķ���</param>
    public void InitObjectPool(string keyName,int maxCapacity = -1,GameObject prefab = null,int defaultQuantity = 0)
    {
        if (defaultQuantity > maxCapacity && maxCapacity != -1)
        {
            JKLog.Error("Ĭ���������������������");
            return;
        }
        //���ö�����Ѿ�����
        if(poolDic.TryGetValue(keyName,out GameObjectPoolData poolData))
        {
            //������������
            poolData.maxCapacity = maxCapacity;
            //�ײ�Queue�Զ��������ﲻ��

            //��ָ��Ĭ��������Ĭ�϶���ʱ��������
            if (defaultQuantity > 0)
            {
                if(prefab.IsNull() == false)
                {
                    int nowCapacity = poolData.PoolQueue.Count;
                    //���ɲ�ֵ���������������������
                    for(int i = 0; i < defaultQuantity - nowCapacity; i++)
                    {
                        GameObject go = GameObject.Instantiate(prefab);
                        go.name = prefab.name;
                        poolData.PushObj(go);
                    }

                }
                else
                {
                    JKLog.Error("Ĭ�϶���δָ��");
                }
            }
        }
        //���ö���ز�����
        else
        {
            //���������
            poolData = CreateGameObjectPoolData(keyName, maxCapacity);

            //��ָ��Ĭ��������Ĭ�϶���ʱ��������
            if (defaultQuantity != 0)
            {
                if(prefab.IsNull() == false)
                {
                    //�������������������������
                    for(int i = 0; i < defaultQuantity; i++)
                    {
                        GameObject go = GameObject.Instantiate(prefab);
                        go.name = prefab.name;
                        poolData.PushObj(go);
                    }
                }
                else
                {
                    JKLog.Error("Ĭ��������Ĭ�϶���δָ��");
                }
            }
        }
    }


    /// <summary>
    /// ��ʼ������ز���������
    /// </summary>
    /// <param name="maxCapacity">�������ƣ�����ʱ�����ٶ����ǽ������أ�-1��������</param>
    /// <param name="defaultQuantity">Ĭ����������д��������з����Ӧ�����Ķ���0����Ԥ�ȷ���</param>
    /// <param name="prefab">��дĬ������ʱԤ�ȷ���Ķ���</param>

    public void InitObjectPool(GameObject prefab, int maxCapacity = -1, int defaultQuantity = 0)
    {
        InitObjectPool(prefab.name, maxCapacity, prefab, defaultQuantity);
    }

    /// <summary>
    /// ��ʼ�������
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="maxCapacity">���������-1��������</param>
    /// <param name="gameObjects">Ĭ��Ҫ�Ž����Ķ�������</param>
    public void InitObjectPool(string keyName,int maxCapacity = -1, GameObject[] gameObjects = null)
    {
        if(gameObjects.Length>maxCapacity && maxCapacity != -1)
        {
            JKLog.Error("Ĭ���������������������");
            return;
        }

        //���ö�����Ѿ�����
        if(poolDic.TryGetValue(keyName,out GameObjectPoolData poolData))
        {
            //������������
            poolData.maxCapacity = maxCapacity;
        }
        //���õĶ���ز�����
        else
        {
            //���������
            poolData = CreateGameObjectPoolData(keyName, maxCapacity);
        }

        //��ָ��Ĭ��������Ĭ�϶���ʱ��������
        if (gameObjects.Length > 0)
        {
            int nowCapacity = poolData.PoolQueue.Count;
            //���ɲ�ֵ���������������������
            for(int i=0;i<gameObjects.Length;i++)
            {
                if (i < gameObjects.Length - nowCapacity)
                {
                    gameObjects[i].gameObject.name = keyName;
                    poolData.PushObj(gameObjects[i].gameObject);
                }
                else
                {
                    GameObject.Destroy(gameObjects[i].gameObject);
                }
            }
        }
    }


    private GameObjectPoolData CreateGameObjectPoolData(string keyName, int maxCapacity)
    {
        throw new NotImplementedException();
    }

    #endregion
}
