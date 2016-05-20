using UnityEngine;
using System.Collections;
using Prime31;
using System.Collections.Generic;

public class AnalyticWrapper
{
	public static bool debugLog = false;
	public static bool isDisabled = false;

	public static void StartSession ()
	{
		if (isDisabled)
			return;

		LogMessage ("Start Session");

		#if UNITY_ANDROID
		FlurryAnalytics.startSession ("7DJ8D7NFSJQ3BGVRF246", true);
		#endif
	}

	public static void EndSession ()
	{
		if (isDisabled)
			return;

		LogMessage ("End Session");

		#if UNITY_ANDROID
		FlurryAnalytics.onEndSession ();
		#endif
	}

	public static void LogEvent (string name)
	{
		if (isDisabled)
			return;

		LogMessage ("Log Event: " + name);
		#if UNITY_ANDROID
		FlurryAnalytics.logEvent (name);
		#endif
	}

	public static void LogEvent (string name, params KeyValuePair<string,string>[] parameters)
	{
		if (isDisabled)
			return;

		var dictioanry = new Dictionary<string,string> ();
		foreach (var pair in parameters)
			dictioanry.Add (pair.Key, pair.Value);

		LogEvent (name, dictioanry);
	}

	public static void LogEvent (string name, Dictionary<string,string> parameters)
	{
		if (isDisabled)
			return;

		LogMessage ("Log Event: " + name);
		#if UNITY_ANDROID
		FlurryAnalytics.logEvent (name, parameters);
		#endif
	}

	public static void LogTimedEvent (string name)
	{
		if (isDisabled)
			return;

		LogMessage ("Start Timed Event: " + name);
		#if UNITY_ANDROID
		FlurryAnalytics.logEvent (name, true);
		#endif
	}

	public static void LogTimedEvent (string name, params KeyValuePair<string,string>[] parameters)
	{
		if (isDisabled)
			return;

		var dictionary = new Dictionary<string,string> ();
		foreach (var pair in parameters)
			dictionary.Add (pair.Key, pair.Value);

		LogTimedEvent (name, dictionary);
	}

	public static void LogTimedEvent (string name, Dictionary<string,string> parameters)
	{
		if (isDisabled)
			return;

		LogMessage ("Start Timed Event: " + name);
		#if UNITY_ANDROID
		FlurryAnalytics.logEvent (name, parameters);
		#endif
	}

	public static void EndTimedEvent (string name)
	{
		if (isDisabled)
			return;

		LogMessage ("End Timed Event: " + name);
		#if UNITY_ANDROID
		FlurryAnalytics.endTimedEvent (name);
		#endif
	}

	public static void EndTimedEvent (string name, params KeyValuePair<string,string>[] parameters)
	{
		if (isDisabled)
			return;

		var dictionary = new Dictionary<string,string> ();
		foreach (var pair in parameters)
			dictionary.Add (pair.Key, pair.Value);

		EndTimedEvent (name, dictionary);
	}

	public static void EndTimedEvent (string name, Dictionary<string,string> parameters)
	{
		if (isDisabled)
			return;

		LogMessage ("End Timed Event: " + name);
		#if UNITY_ANDROID
		FlurryAnalytics.endTimedEvent (name, parameters);
		#endif
	}

	static void LogMessage (string message)
	{
		if (debugLog)
			Debug.Log ("Analytic: " + message);
	}
}
