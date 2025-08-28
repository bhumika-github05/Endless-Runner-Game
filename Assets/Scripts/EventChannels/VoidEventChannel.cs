using UnityEngine;

[CreateAssetMenu(fileName = "EventChannel", menuName = "Scriptable Object/EventChannel")]
public class VoidEventChannel : GenericEventChannel<NoParam>
{

}

public struct NoParam
{

}