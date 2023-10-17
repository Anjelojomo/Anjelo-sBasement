using Content.Server.Weapons.Ranged.Systems;
using Content.Server.Xeno.Actions.Components;
using Content.Shared.Actions;
using Content.Shared.Damage;
using Content.Shared.Movement.Components;
using Content.Shared.Stealth;
using Content.Shared.Stealth.Components;
using Content.Shared.Xeno;
using Robust.Server.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Timing;

namespace Content.Server.Xeno.Actions.Systems;

public sealed class XenoRageSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actionsSystem = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<XenoRageComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<XenoRageComponent, XenoRageEvent>(OnToggle);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<XenoRageComponent, DamageableComponent>();
        while (query.MoveNext(out var uid, out var comp, out var damageable))
        {
            if (!comp.Enabled)
                continue;

            if (_timing.CurTime <= comp.TimeUsed)
                continue;

            comp.Enabled = false;
            Dirty(uid, comp);
            damageable.DamageModifierSetId = comp.PassiveModifierSet;
        }
    }

    private void OnStartup(EntityUid uid, XenoRageComponent component, ComponentStartup args)
    {
        _actionsSystem.AddAction(uid, component.Action);
    }

    private void OnToggle(EntityUid uid, XenoRageComponent component, XenoRageEvent args)
    {
        if (!TryComp<DamageableComponent>(uid, out var damageable))
            return;

        component.Enabled = true;
        Dirty(uid, component);
        component.TimeUsed = _timing.CurTime + TimeSpan.FromSeconds(30f);
        damageable.DamageModifierSetId = component.ModifierSet;
    }
}
