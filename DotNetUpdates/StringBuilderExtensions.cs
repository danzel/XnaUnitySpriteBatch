using System.Text;

public static class StringBuilderExtensions
{
	public static void Clear(this StringBuilder stringBuilder)
	{
		stringBuilder.Length = 0;
	}
}
