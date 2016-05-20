using UnityEngine;
using System.Collections;

public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
	protected static T _instance;

	public static T Instance {
		get
		{
			if (_instance == null)
				_instance = (T)FindObjectOfType(typeof(T));
			
			if (_instance == null)
				Debug.LogError("Unity cannot find a type of " + typeof(T).Name + " in the scene");
			
			return _instance;
		}
	}

	public static T InstanceDirect{ get { return _instance; } }
}

public class Singleton<T> where T:new()
{
	protected static T _instance;

	public static T Instance {
		get
		{
			if (_instance == null)
				_instance = new T();

			return _instance;
		}
	}
}