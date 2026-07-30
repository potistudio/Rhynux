using UnityEngine;

public class GlitchText : MonoBehaviour {
	[SerializeField] private TMPro.TextMeshProUGUI[] m_TargetTextFields;

    private void FixedUpdate() {
		foreach (TMPro.TextMeshProUGUI target in m_TargetTextFields)
			target.text = RandomBase64();
    }

	private static string RandomBase64() {
		// Generate GUID as a Base64 string
		System.Byte[] guidBytes = System.Guid.NewGuid().ToByteArray();
		string base64 = System.Convert.ToBase64String (guidBytes);

		// Remove last 3 characters '(QAgw)=='.
		// A GUID is always 24 Base64 characters, so slicing beats compiling a Regex per call.
		return base64[..^3] + " ";
	}
}
