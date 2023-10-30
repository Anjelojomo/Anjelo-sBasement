using Content.Shared.Actions;
using Content.Shared.DoAfter;
using Robust.Shared.GameStates;
using Robust.Shared.Map;
using Robust.Shared.Serialization;
using System.Numerics;

namespace Content.Shared.Xeno;

[RegisterComponent, NetworkedComponent]
public sealed partial class SharedXenoStunAbilitiesComponent : Component { }
public sealed partial class XenoStunEvent : EntityTargetActionEvent { }
public sealed partial class XenoExplosiveEvent : InstantActionEvent { }
public sealed partial class XenoSpitEvent : WorldTargetActionEvent { }
public sealed partial class XenoSpit2Event : WorldTargetActionEvent { }
public sealed partial class XenoSpitRejuvenateEvent : WorldTargetActionEvent { }
public sealed partial class XenoCrushDashEvent : WorldTargetActionEvent { }
public sealed partial class XenoVinesEvent : InstantActionEvent { }
public sealed partial class XenoToggleStealthEvent : InstantActionEvent { }
public sealed partial class XenoBuildWallEvent : WorldTargetActionEvent { }
[Serializable, NetSerializable]
public sealed partial class XenoBuildWallDoAfterEvent : SimpleDoAfterEvent
{
    public readonly MapCoordinates Coordinates;

    public XenoBuildWallDoAfterEvent(MapCoordinates coordinates)
    {
        Coordinates = coordinates;
    }
}
public sealed partial class XenoPsychicCureEvent : EntityTargetActionEvent { }
[Serializable, NetSerializable]
public sealed partial class XenoPsychicCureDoAfterEvent : SimpleDoAfterEvent { }
public sealed partial class XenoPsychicJumpEvent : WorldTargetActionEvent { }
public sealed partial class XenoEndureEvent : InstantActionEvent { }
public sealed partial class XenoRageEvent : InstantActionEvent { }
public sealed partial class XenoScreechEvent : InstantActionEvent { }
public sealed partial class XenoLayEggEvent : InstantActionEvent { }
