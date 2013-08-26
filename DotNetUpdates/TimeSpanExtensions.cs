using System;

public static class TimeSpanExtensions
{
	public static string ToString(this TimeSpan timeSpan, string format)
	{
		if (format == "mm\\:ss")
			return string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);

		throw new NotImplementedException();
	}
}
