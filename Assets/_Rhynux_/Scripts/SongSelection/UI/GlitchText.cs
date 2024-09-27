using UnityEngine;

public class RandomTitle : MonoBehaviour {
	[SerializeField] private TMPro.TextMeshProUGUI[] m_TargetTextFields;

    private void FixedUpdate() {
		foreach (TMPro.TextMeshProUGUI target in m_TargetTextFields)
			target.text = RandomBase64();
    }

	private string RandomBase64() {
		// Generate GUID as a Base64 string
		System.Byte[] guidBytes = System.Guid.NewGuid().ToByteArray();
		string base64 = System.Convert.ToBase64String (guidBytes);

		// Remove last 3 characters '(QAgw)=='
		System.Text.RegularExpressions.Regex regex = new (".{3}$");
		string result = regex.Replace (base64, " ");

		return result;
	}
}
