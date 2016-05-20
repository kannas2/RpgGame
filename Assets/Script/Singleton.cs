using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;
    private static object _lock = new object();

    public static T Instance
    {
        get
        {
            //다른 오브젝트 인스턴스/
            _instance = GameObject.FindObjectOfType<T>() as T;

            lock (_lock)
            {
                if (!_instance)
                {
                    _instance = GameObject.Find(typeof(T).ToString()).GetComponent<T>();
                }
                return _instance;
            }
        }
    }
}