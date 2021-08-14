using UnityEngine;

[CreateAssetMenu(fileName = "New CardBundles", menuName = "Card Bundles", order = 10)]
public class CardBundles : ScriptableObject
{
    [SerializeField]
    private CardBundleData[] _bundles;

    public CardBundleData[] Bundles => _bundles;
}
