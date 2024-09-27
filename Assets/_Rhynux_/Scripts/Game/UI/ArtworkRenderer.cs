using UnityEngine;

public sealed class ArtworkRenderer : MonoBehaviour {
    [SerializeField] private UnityEngine.UI.Image m_Image;

    public void SetSprite (Sprite _sprite) {
        m_Image.sprite = _sprite;
    }
}
