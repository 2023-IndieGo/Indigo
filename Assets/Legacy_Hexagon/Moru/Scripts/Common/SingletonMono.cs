using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
{
    protected static T m_instance;
    public static T instance { get { if (m_instance == null) m_instance = FindObjectOfType<T>(); return m_instance; } }

    /// <summary>
    /// DonDestroyOnLoad는 해당 메서드를 재정의 후 추가해주세요.
    /// </summary>
    protected virtual void Awake()
    {
        if (m_instance == null)
        {
            m_instance = (T)this;
        }
        else if (m_instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
    }
}

public class SingleTone<T> where T : class, new()
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            // 만약 instance가 존재하지 않을 경우 새로 생성한다.
            if (_instance == null)
            {
                _instance = new T();

            }
            // _instance를 반환한다.
            return _instance;
        }
    }
}