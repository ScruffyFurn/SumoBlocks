using UnityEngine;
#if UNITY_WINRT
using System;
using System.Collections;
using System.Threading;
using System.IO;

/// <summary>
/// Windows specific and interop between Unity and Windows Store or Windows Phone 8
/// </summary>
public static class WindowsGateway
{
	
	static WindowsGateway()
	{
		
		#if UNITY_METRO
		
		// unity now supports handling size changed in 4.3
		//UnityEngine.WSA.Application.windowSizeChanged += WindowSizeChanged;


		
		#endif
		
		// create blank implementations to avoid errors within editor
		UnityLoaded = delegate {};

		//Our Share function
		ShareHighScore = delegate {};
		
	}
	
	/// <summary>
	/// Called from Unity when the app is responsive and ready for play
	/// </summary>
	public static Action UnityLoaded;

	public static Action ShareHighScore;
	
	#if UNITY_METRO
	
	/// <summary>
	/// Deal with windows resizing
	/// </summary>
	public static void WindowSizeChanged(int width, int height) 
	{

	} 


	
	#endif
	
}

#endif
