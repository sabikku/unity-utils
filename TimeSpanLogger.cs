using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

/**
 * Logging utility to measure time.
 */

public static class TimeSpanLogger {

	static Stack<float> loggingStarts = new Stack<float>();

	public static void Begin() {
		loggingStarts.Push( Time.realtimeSinceStartup );
	}

	public static void End( string whatWasMeasured ) {
		float start = loggingStarts.Pop();
		float end = Time.realtimeSinceStartup;
		float diff = end - start;
		System.TimeSpan timeSpan = new System.TimeSpan( (long)(System.TimeSpan.TicksPerSecond * diff) );
		if ( timeSpan.Milliseconds == 0 ) {
			Debug.Log( "TimeSpanLogger: " + 0 + "ms" );
			return;
		}
		Debug.Log( "TimeSpanLogger: " + whatWasMeasured + " took "
			+ timeSpan.Minutes + "m " + timeSpan.Seconds + "s " + timeSpan.Milliseconds + "ms" );
	}

}
