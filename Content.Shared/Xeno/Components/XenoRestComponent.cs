﻿using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Xeno.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class XenoRestComponent : Component
{
    [DataField]
    public EntProtoId Action = "ActionXenoRest";

    [DataField]
    public int? OriginalDrawDepth;
}
