using JKFrame;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPoolData
{
    #region GameObjectPoolData���е����ݼ���ʼ������
    //��һ������� ���ڵ�
    public Transform RootTransform;
    //��������
    public Queue<GameObject> PoolQueue;
    //�������� -1��������
    public int maxCapacity = -1;
    public GameObjectPoolData(int capacity = -1)
    {
        if(capacity == -1)
        {
            PoolQueue = new Queue<GameObject>();
        }
        else
        {
            PoolQueue = new Queue<GameObject>(capacity);    
        }
    }
    public void Init(string assetPath,Transform poolRootObj,int capacity = -1)
    {
        //�������ڵ� �����õ�����ظ��ڵ��·�
        GameObject go = PoolSystem.GetGameObject(PoolSystem.PoolLayerGameObjectName, poolRootObj);
        if (go.IsNull())
        {
            go = new GameObject(PoolSystem.PoolLayerGameObjectName);
            go.transform.SetParent(poolRootObj);
        }
        RootTransform = go.transform;
        RootTransform.name = assetPath;
        maxCapacity = capacity;
    }
    #endregion

    #region GameObjectPool������ز���
    ///<summary>
    ///������Ž������
    ///</summary>
    public bool PushObj(GameObject obj)
    {
        //����ǲ��ǳ�������
        if (maxCapacity != -1 && PoolQueue.Count >= maxCapacity)
        {
            GameObject.Destroy(obj);
            return false;
        }
        //���������
        PoolQueue.Enqueue(obj);
        //���ø�����
        obj.transform.SetParent (RootTransform);
        //��������
        obj.SetActive(false);
        return true;
    }

    ///<summary>
    ///�Ӷ�����л�ȡ����
    ///</summary>
    ///<returns></returns>
    public GameObject GetObj(Transform parent = null)
    {
        GameObject obj = PoolQueue.Dequeue();
        //��ʾ����
        obj.SetActive(true);
        //���ø�����
        obj.transform.SetParent(parent);
        if(parent == null)
        {
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(obj, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
        return obj;
    }

    ///<summary>
    ///���ٲ�����
    ///</summary>
    /// <param name="pushThisToPool">������ز㼶�ҽӵ�Ҳ���ͽ������</param>
    public void Desotry(bool pushThisToPool = false)
    {
        maxCapacity = -1;
        if (!pushThisToPool)
        {
            //��ʵ���� ��������ɾ���㼶������ �ᵼ���·����ж��󶼱�ɾ��������
        }
    }
    #endregion
}