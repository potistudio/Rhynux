
/// <summary>
/// CopyHelper
/// </summary>
public static class CopyHelper {
	/// <summary>
	/// DeepCopy
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="src"></param>
	/// <returns></returns>
	public static T DeepCopy<T>(this T src) {
		//* Using Binary Formatter *//
		using System.IO.MemoryStream stream = new();

		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new();
		formatter.Serialize (stream, src);
		stream.Position = 0;

		return (T)formatter.Deserialize (stream);

		//* Using JSON Serializer (not working correctly) *//
		// var o = new System.Text.Json.JsonSerializerOptions(){ IncludeFields = true };
		// System.ReadOnlySpan<byte> b = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes<T>(src);
		// return System.Text.Json.JsonSerializer.Deserialize<T>(b);
	}
}
