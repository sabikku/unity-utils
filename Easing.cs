using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * Simple easing functions. Written with Unity in mind to serialize the easing type as an enum.
 */

[System.Serializable]
public enum EaseType {

	Lerp,

	InQuad,
	OutQuad,
	InOutQuad,

	InCubic,
	OutCubic,
	InOutCubic,

	InQuart,
	OutQuart,
	InOutQuart,

	InQuint,
	OutQuint,
	InOutQuint,

	InSine,
	OutSine,
	InOutSine,

	InExpo,
	OutExpo,
	InOutExpo,

	InCirc,
	OutCirc,
	InOutCirc,

	InElastic,
	OutElastic,
	InOutElastic,

	InBack,
	OutBack,
	InOutBack,

	InBounce,
	OutBounce,
	InOutBounce

}

public static class Easing {

	public static float Ease( EaseType easeType, float from, float to, float value ) {
		return Ease( easeType, from, to, value, true, 1, 1 );
	}

	public static float Ease( EaseType easeType, float from, float to, float value, bool clamp ) {
		return Ease( easeType, from, to, value, clamp, 1, 1 );
	}

	public static float Ease( EaseType easeType, float from, float to, float value, bool clamp, float amplitude, float amplitudeDuration ) {
		if ( clamp ) {
			value = Mathf.Clamp( value, 0, 1 );
		}
		if ( value == 0 ) {
			return from;
		}
		if ( value == 1 ) {
			return to;
		}
		to = to - from;
		SimpleEaseFunction function = null;
		if ( simpleEaseFunctions.TryGetValue( easeType, out function ) ) {
			return function( from, to, value );
		} else {
			AmplitudeEaseFunction amplitudeFunction = null;
			if ( amplitudeEaseFunctions.TryGetValue( easeType, out amplitudeFunction ) ) {
				return amplitudeFunction( from, to, value, amplitude, amplitudeDuration );
			} else {
				return to;
			}
		}
	}
	
	public static Vector2 EaseVector2( EaseType easeType, Vector2 from, Vector2 to, float value, bool clamp = true, float amplitude = 1, float amplitudeDuration = 1 ) {
		Vector2 result = new Vector2();
		result.x = Ease( easeType, from.x, to.x, value, clamp, amplitude, amplitudeDuration );
		result.y = Ease( easeType, from.y, to.y, value, clamp, amplitude, amplitudeDuration );
		return result;
	}
	
	public static Vector3 EaseVector3( EaseType easeType, Vector3 from, Vector3 to, float value, bool clamp = true, float amplitude = 1, float amplitudeDuration = 1 ) {
		Vector3 result = new Vector3();
		result.x = Ease( easeType, from.x, to.x, value, clamp, amplitude, amplitudeDuration );
		result.y = Ease( easeType, from.y, to.y, value, clamp, amplitude, amplitudeDuration );
		result.z = Ease( easeType, from.z, to.z, value, clamp, amplitude, amplitudeDuration );
		return result;
	}
	
	public static Color EaseColor( EaseType easeType, Color from, Color to, float value, bool clamp = true, float amplitude = 1, float amplitudeDuration = 1 ) {
		Color result = new Color();
		result.r = Ease( easeType, from.r, to.r, value, clamp, amplitude, amplitudeDuration );
		result.g = Ease( easeType, from.g, to.g, value, clamp, amplitude, amplitudeDuration );
		result.b = Ease( easeType, from.b, to.b, value, clamp, amplitude, amplitudeDuration );
		result.a = Ease( easeType, from.a, to.a, value, clamp, amplitude, amplitudeDuration );
		return result;
	}

	private delegate float SimpleEaseFunction( float from, float to, float value );
	private static Dictionary<EaseType, SimpleEaseFunction> m_simpleEaseFunctions = null;
	private static Dictionary<EaseType, SimpleEaseFunction> simpleEaseFunctions {
		get {
			if ( m_simpleEaseFunctions == null ) {
				m_simpleEaseFunctions = new Dictionary<EaseType, SimpleEaseFunction>();
				DefineSimpleEaseFunctions();
			}
			return m_simpleEaseFunctions;
		}
	}

	private delegate float AmplitudeEaseFunction( float from, float to, float value, float amplitude, float duration );
	private static Dictionary<EaseType, AmplitudeEaseFunction> m_amplitudeEaseFunctions = null;
	private static Dictionary<EaseType, AmplitudeEaseFunction> amplitudeEaseFunctions {
		get {
			if ( m_amplitudeEaseFunctions == null ) {
				m_amplitudeEaseFunctions = new Dictionary<EaseType, AmplitudeEaseFunction>();
				DefineComplexEaseFunctions();
			}
			return m_amplitudeEaseFunctions;
		}
	}
	
	private static void DefineSimpleEaseFunctions() {

		m_simpleEaseFunctions.Add( EaseType.Lerp, ( b, c, t ) => {
			return b + t*c;
		} );
		
		m_simpleEaseFunctions.Add( EaseType.InQuad, ( b, c, t ) => {
			return c*t*t + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutQuad, ( b, c, t ) => {
			return -c *t*(t-2) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutQuad, ( b, c, t ) => {
			if ((t*=2) < 1) return c/2*t*t + b;
			return -c/2 * ((--t)*(t-2) - 1) + b;
		} );
		
		m_simpleEaseFunctions.Add( EaseType.InCubic, ( b, c, t ) => {
			return c*t*t*t + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutCubic, ( b, c, t ) => {
			return c*((t-=1)*t*t + 1) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutCubic, ( b, c, t ) => {
			if ((t*=2) < 1) return c/2*t*t*t + b;
			return c/2*((t-=2)*t*t + 2) + b;
		} );
		
		m_simpleEaseFunctions.Add( EaseType.InQuart, ( b, c, t ) => {
			return c*t*t*t + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutQuart, ( b, c, t ) => {
			return -c * ((t-=1)*t*t*t - 1) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutQuart, ( b, c, t ) => {
			if ((t*=2) < 1) return c/2*t*t*t*t + b;
			return -c/2 * ((t-=2)*t*t*t - 2) + b;
		} );
		
		m_simpleEaseFunctions.Add( EaseType.InQuint, ( b, c, t ) => {
			return c*t*t*t*t*t + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutQuint, ( b, c, t ) => {
			return c*((t-=1)*t*t*t*t + 1) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutQuint, ( b, c, t ) => {
			if ((t*=2) < 1) return c/2*t*t*t*t*t + b;
			return c/2*((t-=2)*t*t*t*t + 2) + b;
		} );
		
		m_simpleEaseFunctions.Add( EaseType.InSine, ( b, c, t ) => {
			return -c * Mathf.Cos(t * ((float)Math.PI/2)) + c + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutSine, ( b, c, t ) => {
			return c * Mathf.Sin(t * ((float)Math.PI/2)) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutSine, ( b, c, t ) => {
			return -c/2 * (Mathf.Cos((float)Math.PI*t) - 1) + b;
		} );
		
		m_simpleEaseFunctions.Add( EaseType.InExpo, ( b, c, t ) => {
			return (t==0) ? b : c * Mathf.Pow(2, 10 * (t - 1)) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutExpo, ( b, c, t ) => {
			return (t==1) ? b+c : c * (-Mathf.Pow(2, -10 * t) + 1) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutExpo, ( b, c, t ) => {
			if (t==0) return b;
			if (t==1) return b+c;
			if ((t/2) < 1) return c/2 * Mathf.Pow(2, 10 * (t - 1)) + b;
			return c/2 * (-Mathf.Pow(2, -10 * --t) + 2) + b;
		} );
		
		m_simpleEaseFunctions.Add( EaseType.InCirc, ( b, c, t ) => {
			return -c * (Mathf.Sqrt(1 - t*t) - 1) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutCirc, ( b, c, t ) => {
			return c * Mathf.Sqrt(1 - (t=t-1)*t) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutCirc, ( b, c, t ) => {
			if ((t*=2) < 1) return -c/2 * (Mathf.Sqrt(1 - t*t) - 1) + b;
			return c/2 * (Mathf.Sqrt(1 - (t-=2)*t) + 1) + b;
		} );

		m_simpleEaseFunctions.Add( EaseType.InElastic, ( b, c, t ) => {
			float p=.3f; float s=p/4; float a = c;
			if (t==0) return b; if (t==1) return b+c;
			if (a >= Mathf.Abs(c)) s = p/(2*(float)Math.PI) * Mathf.Asin(c/a);
			return -(a*Mathf.Pow(2,10*(t-=1)) * Mathf.Sin( (t-s)*(2*(float)Math.PI)/p )) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutElastic, ( b, c, t ) => {
			float p=.3f; float s=p/4; float a=c;
			if (t==0) return b;  if (t==1) return b+c;
			if (a >= Mathf.Abs(c)) s = p/(2*(float)Math.PI) * Mathf.Asin (c/a);
			return a*Mathf.Pow(2,-10*t) * Mathf.Sin( (t-s)*(2*(float)Math.PI)/p ) + c + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutElastic, ( b, c, t ) => {
			float p=.3f*1.5f; float s=p/4; float a=c;
			if (t==0) return b;  if ((t*=2)==1) return b+c;
			if (a >= Math.Abs(c)) s = p/(2*(float)Math.PI) * Mathf.Asin (c/a);
			if (t < 1) return -.5f*(a*Mathf.Pow(2,10*(t-=1)) * Mathf.Sin( (t-s)*(2*(float)Math.PI)/p )) + b;
			return a*Mathf.Pow(2,-10*(t-=1)) * Mathf.Sin( (t-s)*(2*(float)Math.PI)/p )*.5f + c + b;
		} );

		m_simpleEaseFunctions.Add( EaseType.InBack, ( b, c, t ) => {
			return c*t*t*(2.70158f*t - 1.70158f) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.OutBack, ( b, c, t ) => {
			return c*((t=t-1)*t*(2.70158f*t + 1.70158f) + 1) + b;
		} );
		m_simpleEaseFunctions.Add( EaseType.InOutBack, ( b, c, t ) => {
			float s = 1.70158f;
			if ((t*=2) < 1) return c/2*(t*t*(((s*=(1.525f))+1)*t - s)) + b;
			return c/2*((t-=2)*t*(((s*=(1.525f))+1)*t + s) + 2) + b;
		} );

	}
	
	private static void DefineComplexEaseFunctions() {

		m_amplitudeEaseFunctions.Add( EaseType.OutBounce, ( b, c, t, x, d ) => {
			if ((t/=d) < (1/2.75)) {
				return c*(7.5625f*t*t) + b;
			} else if (t < (2/2.75f)) {
				return c*(7.5625f*(t-=(1.5f/2.75f))*t + .75f) + b;
			} else if (t < (2.5f/2.75f)) {
				return c*(7.5625f*(t-=(2.25f/2.75f))*t + .9375f) + b;
			} else {
				return c*(7.5625f*(t-=(2.625f/2.75f))*t + .984375f) + b;
			}
		} );
		m_amplitudeEaseFunctions.Add( EaseType.InBounce, ( b, c, t, x, d ) => {
			return c - m_amplitudeEaseFunctions[EaseType.OutBounce](0, c, d-t, x, d) + b;
		} );
		m_amplitudeEaseFunctions.Add( EaseType.InOutBounce, ( b, c, t, x, d ) => {
			if (t < d/2) return m_amplitudeEaseFunctions[EaseType.InBounce](0, c, t*2, x, d) * .5f + b;
			return m_amplitudeEaseFunctions[EaseType.OutBounce](0, c, t*2-d, x, d) * .5f + c*.5f + b;
		} );

	}


}