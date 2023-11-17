using JKFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class GameObjectPoolModule
{
    #region GameObjectPoolModule持有的数据及初始化
    //根节点
    private Transform poolRootTransform;
    ///<summary>
    ///GameObject对象容器
    ///</summary>
    public Dictionary<string, GameObjectPoolData> poolDic { get; private set; } = new Dictionary<string, GameObjectPoolData>();
    public void Init(Transform poolRootTransform)
    {
        this.poolRootTransform = poolRootTransform;
    }

    /// <summary>
    /// 初始化对象池并设置容量
    /// </summary>
    /// <param name="keyName">资源名称</param>
    /// <param name="maxCapacity">容量限制，超出时会销毁而不是进入对象池，-1代表无限</param>
    /// <param name="defaultQuantity">默认容量，填写会向池子中放入对应数量的对象，0代表不预先放入</param>
    /// <param name="prefab">填写默认容量时预先放入的对象</param>
    public void InitObjectPool(string keyName,int maxCapacity = -1,GameObject prefab = null,int defaultQuantity = 0)
    {
        if (defaultQuantity > maxCapacity && maxCapacity != -1)
        {
            JKLog.Error("默认容量超出最大容量限制");
            return;
        }
        //设置对象池已经存在
        if(poolDic.TryGetValue(keyName,out GameObjectPoolData poolData))
        {
            //更新容量限制
            poolData.maxCapacity = maxCapacity;
            //底层Queue自动扩容这里不管

            //在指定默认容量和默认对象时才有意义
            if (defaultQuantity > 0)
            {
                if(prefab.IsNull() == false)
                {
                    int nowCapacity = poolData.PoolQueue.Count;
                    //生成差值容量个数的物体放入对象池
                    for(int i = 0; i < defaultQuantity - nowCapacity; i++)
                    {
                        GameObject go = GameObject.Instantiate(prefab);
                        go.name = prefab.name;
                        poolData.PushObj(go);
                    }

                }
                else
                {
                    JKLog.Error("默认对象未指定");
                }
            }
        }
        //设置对象池不存在
        else
        {
            //创建对象池
            poolData = CreateGameObjectPoolData(keyName, maxCapacity);

            //在指定默认容量和默认对象时才有意义
            if (defaultQuantity != 0)
            {
                if(prefab.IsNull() == false)
                {
                    //生成容量个数的物体放入对象池
                    for(int i = 0; i < defaultQuantity; i++)
                    {
                        GameObject go = GameObject.Instantiate(prefab);
                        go.name = prefab.name;
                        poolData.PushObj(go);
                    }
                }
                else
                {
                    JKLog.Error("默认容量或默认对象未指定");
                }
            }
        }
    }


    /// <summary>
    /// 初始化对象池并设置容量
    /// </summary>
    /// <param name="maxCapacity">容量限制，超出时会销毁而不是进入对象池，-1代表无限</param>
    /// <param name="defaultQuantity">默认容量，填写会向池子中放入对应数量的对象，0代表不预先放入</param>
    /// <param name="prefab">填写默认容量时预先放入的对象</param>

    public void InitObjectPool(GameObject prefab, int maxCapacity = -1, int defaultQuantity = 0)
    {
        InitObjectPool(prefab.name, maxCapacity, prefab, defaultQuantity);
    }

    /// <summary>
    /// 初始化对象池
    /// </summary>
    /// <param name="keyName"></param>
    /// <param name="maxCapacity">最大容量，-1代表无限</param>
    /// <param name="gameObjects">默认要放进来的对象数组</param>
    public void InitObjectPool(string keyName,int maxCapacity = -1, GameObject[] gameObjects = null)
    {
        if(gameObjects.Length>maxCapacity && maxCapacity != -1)
        {
            JKLog.Error("默认容量超出最大容量限制");
            return;
        }

        //设置对象池已经存在
        if(poolDic.TryGetValue(keyName,out GameObjectPoolData poolData))
        {
            //更新容量限制
            poolData.maxCapacity = maxCapacity;
        }
        //设置的对象池不存在
        else
        {
            //创建对象池
            poolData = CreateGameObjectPoolData(keyName, maxCapacity);
        }

        //在指定默认容量和默认对象时才有意义
        if (gameObjects.Length > 0)
        {
            int nowCapacity = poolData.PoolQueue.Count;
            //生成差值容量个数的物体放入对象池
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
