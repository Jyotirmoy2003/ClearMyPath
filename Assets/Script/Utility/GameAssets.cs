using StarterAssets;
using UnityEngine;
using UnityEngine.UI;

public class GameAssets : MonoSingleton<GameAssets>
{
    public ObjectPool cannonPool;
    public StarterAssetsInputs starterAssetsInputs;
    [Header("Male Android Input")]
    public HoldButton maleLeftSideButton;
    public HoldButton maleRightSideButton;
    public HoldButton maleShootButton;

}
