using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Scroll : MonoBehaviour
{
    private Renderer myRenderer;
    private Material myMaterial;

    private float offset;

    [SerializeField] private float increase = 1f; // unidades por segundo
    [SerializeField] private float speed = 0.05f; // ajuste fino da velocidade

    [Header("Shader")]
    [SerializeField] private string textureProperty = "_MainTex";

    [Header("Sorting (opcional)")]
    [SerializeField] private string sortingLayer;
    [SerializeField] private int sortingOrder;

    private void Awake()
    {
        SetupMaterial();
    }

    private void OnEnable()
    {
        SetupMaterial();
        ResetOffset();
    }

    private void Start()
    {
        SetupMaterial();
        ResetOffset();
    }

    private void Update()
    {
        if (myMaterial == null) return;

        // movimento independente do tempo de jogo (funciona mesmo pausado)
        offset += increase * Time.unscaledDeltaTime;

        // velocidade final controlada só pelo "speed"
        SetTexOffset(new Vector2(offset * speed, 0f));
    }

    private void SetupMaterial()
    {
        if (myRenderer == null)
        {
            myRenderer = GetComponent<Renderer>();
            if (!string.IsNullOrEmpty(sortingLayer))
                myRenderer.sortingLayerName = sortingLayer;
            myRenderer.sortingOrder = sortingOrder;
        }

        if (myMaterial == null && myRenderer != null)
        {
            myMaterial = myRenderer.material;

            if (!myMaterial.HasProperty(textureProperty))
            {
                if (myMaterial.HasProperty("_BaseMap")) textureProperty = "_BaseMap";
                else if (myMaterial.HasProperty("_MainTex")) textureProperty = "_MainTex";
            }
        }
    }

    private void ResetOffset()
    {
        offset = 0f;
        if (myMaterial != null) SetTexOffset(Vector2.zero);
    }

    private void SetTexOffset(Vector2 o)
    {
        myMaterial.SetTextureOffset(textureProperty, o);
    }
}
