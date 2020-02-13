using UnityEngine;

public class Well : MonoBehaviour
{
    public float BaseScale = 20;

    public SpriteRenderer DotSprite;
    public SpriteRenderer StickSprite;

    private Ray RayDown = new Ray(Vector3.zero, -Vector3.up);
    private Vector3 RayOffset = new Vector3(0, 1000, 0);

    private RaycastHit[] CastResult = new RaycastHit[5];

    private Camera Cam;
    private Color defaultC;

    private void Awake()
    {
        defaultC = DotSprite.color;
        Cam = Camera.main;
    }

    public void SetNewColor(Color newColor )
    {
        if (DotSprite != null)
        {
            DotSprite.color = newColor;
        }
        if(StickSprite != null)
        {
            StickSprite.color = newColor;
        }
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        RayDown.origin = pos + RayOffset;

        int resultCount = Physics.RaycastNonAlloc(RayDown, CastResult, RayOffset.y * 2, 1 << LayerMask.NameToLayer("Ground"));

        if (resultCount != 0 && Mathf.Abs(CastResult[0].point.y + 7 - pos.y) > 2)
        {
            pos.y = CastResult[0].point.y + 7;
        }

        transform.position = pos;
    }

    internal void SetDefault()
    {
        SetNewColor(defaultC);
    }

    private void LateUpdate()
    {
        Vector3 pos = Cam.transform.position;
        pos.y = transform.position.y;

        transform.LookAt(pos);

        DotSprite.transform.LookAt(Cam.transform);
    }

    public void SetNewScale(float multiplier)
    {
        if (DotSprite != null)
        {      
            float scale = BaseScale * multiplier;
            DotSprite.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
