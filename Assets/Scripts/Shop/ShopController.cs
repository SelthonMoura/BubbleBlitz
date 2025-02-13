using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject shopSpawnerLeft;
    public GameObject shopSpawnerRight;
    [SerializeField] private GameEventListener<CustomEvent<object, Bullet>> bulletHit;
    [SerializeField] private GameEvent _removeBullet;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite _right;
    [SerializeField] private Sprite _left;

    public float tweenDuration;
    public Ease easeType;

    // Start is called before the first frame update
    void OnEnable()
    {
        _spriteRenderer.sprite = _right;
        bulletHit.AddListener<object, Bullet>(ShopHit);

        transform.DOLocalMoveX(shopSpawnerRight.transform.position.x, tweenDuration)
            .SetEase(easeType)
            .SetLoops(2, LoopType.Yoyo)
            .OnStepComplete(() =>
            {
                _spriteRenderer.sprite = _left;
            })
            .OnComplete(ShopFinishAnimation);
    }

    private void OnDestroy()
    {
        bulletHit.RemoveListener<object, Bullet>(ShopHit);
    }

    void ShopFinishAnimation()
    {
        gameObject.SetActive(false);
    }

    void ShopHit(object hit, Bullet bullet)
    {
        if ((object)transform != hit) return;

        transform.position = shopSpawnerLeft.transform.position;
        shopPanel.SetActive(true);
        shopPanel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
        gameObject.SetActive(false);
    }
}
