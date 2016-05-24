using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Trigger {
    public TriggerType Type { get; internal set; }
}

[Serializable]
public enum TriggerType {
    Immediate,
    StatusPhase,
    EnterCatacombs,
    ExitCatacombs,
    TakeDamage,
    SearchRoom,
    ExitRoom,
    ExploreRoom,
    GainCurrency,
    GainItem
}
