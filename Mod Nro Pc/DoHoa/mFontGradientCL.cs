using UnityEngine;

namespace DoHoa;

public class mFontGradientCL
{
	public enum HAlign
	{
		Left,
		Center,
		Right
	}

	private static GUIStyle cachedStyle;

	private static Font cachedFont;

	private static void EnsureStyle(int fontSize)
	{
		if (cachedFont == null)
		{
			cachedFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
		}
		if (cachedStyle == null || cachedStyle.fontSize != fontSize)
		{
			cachedStyle = new GUIStyle(GUI.skin.label)
			{
				font = cachedFont,
				fontSize = fontSize * mGraphics.zoomLevel / 2,
				fontStyle = FontStyle.Bold,
				alignment = TextAnchor.UpperLeft,
				richText = false
			};
		}
	}

	public static void Draw(string text, int x, int y, int fontSize = 48, HAlign hAlign = HAlign.Center, bool useOutline = true, int outlineSize = 2, bool useRainbow = false, Color[] gradientColors = null, float gradientSpeed = 0.3f, bool useShine = true, float shineSpeed = 50f, float shineWidthFactor = 0.1f, float shineAlpha = 0.7f)
	{
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		EnsureStyle(fontSize);
		GUIStyle gUIStyle = cachedStyle;
		if (gradientColors == null || gradientColors.Length < 2)
		{
			gradientColors = new Color[2]
			{
				new Color(1f, 0.5f, 0f),
				new Color(1f, 1f, 0.2f)
			};
		}
		Vector2 vector = gUIStyle.CalcSize(new GUIContent(text));
		float x2 = vector.x;
		float y2 = vector.y;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		if (1 == 0)
		{
		}
		float num = hAlign switch
		{
			HAlign.Left => x, 
			HAlign.Center => (float)x - x2 / 2f, 
			HAlign.Right => (float)x - x2, 
			_ => (float)x - x2 / 2f, 
		};
		if (1 == 0)
		{
		}
		float num2 = num;
		bool flag8 = false;
		float num3 = num2;
		bool flag9 = false;
		float num4 = num3;
		bool flag10 = false;
		float num5 = num4;
		bool flag11 = false;
		float num6 = num5;
		bool flag12 = false;
		float num7 = num6;
		bool flag13 = false;
		float num8 = num7;
		bool flag14 = false;
		float x3 = num8;
		Rect position = new Rect(x3, y, x2, y2);
		if (useOutline && outlineSize > 0)
		{
			gUIStyle.normal.textColor = Color.black;
			for (int i = -outlineSize; i <= outlineSize; i++)
			{
				for (int j = -outlineSize; j <= outlineSize; j++)
				{
					if (i != 0 || j != 0)
					{
						GUI.Label(new Rect(position.x + (float)i, position.y + (float)j, position.width, position.height), text, gUIStyle);
					}
				}
			}
		}
		Color textColor;
		if (useRainbow)
		{
			float h = Time.time * gradientSpeed % 1f;
			textColor = Color.HSVToRGB(h, 1f, 1f);
		}
		else
		{
			int num9 = gradientColors.Length;
			float num10 = Mathf.PingPong(Time.time * gradientSpeed * 0.5f, (float)num9 - 1f);
			int num11 = Mathf.FloorToInt(num10);
			float t = num10 - (float)num11;
			Color a = gradientColors[num11];
			Color b = gradientColors[Mathf.Min(num11 + 1, num9 - 1)];
			textColor = Color.Lerp(a, b, t);
		}
		gUIStyle.normal.textColor = textColor;
		GUI.Label(position, text, gUIStyle);
		if (useShine && x2 > 0f)
		{
			float min = 10f;
			float max = 30f;
			float num12 = Mathf.Clamp(x2 * shineWidthFactor, min, max);
			float num13 = Time.time * shineSpeed % (x2 + num12) - num12;
			Rect position2 = new Rect(position.x + num13, position.y, num12, position.height);
			GUI.BeginGroup(position2);
			int num14 = 3;
			for (int k = -num14; k <= num14; k++)
			{
				float a2 = shineAlpha * Mathf.Exp((float)(-Mathf.Abs(k)) * 0.5f);
				gUIStyle.normal.textColor = new Color(1f, 1f, 1f, a2);
				GUI.Label(new Rect(0f - num13 + (float)k, 0f, position.width, position.height), text, gUIStyle);
			}
			GUI.EndGroup();
		}
	}
}
