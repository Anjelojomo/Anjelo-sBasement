using Content.Server.Xeno.Actions.Components;
using Content.Shared.Actions;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Content.Shared.Xeno;
using Content.Shared.Xeno.Components;
using Robust.Shared.Physics.Systems;

namespace Content.Server.Xeno;

public sealed class XenoSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly DamageableSystem _damageable = default!;
    [Dependency] private readonly SharedPhysicsSystem _physics = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<XenoComponent, ComponentStartup>(OnStartup);
    }

    private void OnStartup(EntityUid uid, XenoComponent component, ComponentStartup args)
    {
        _actions.AddAction(uid, component.ActionNightVision);
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<XenoComponent>();
        while (query.MoveNext(out var uid, out var xeno))
        {
            // Update resting
            xeno.OnResting = HasComp<XenoRestingComponent>(uid);

            // Update weeds
            xeno.OnWeeds = false;
            foreach (var contact in _physics.GetContactingEntities(uid))
            {
                if (HasComp<XenoWeedsComponent>(contact))
                {
                    xeno.OnWeeds = true;
                    break;
                }
            }

            // Update regen
            var healthRegen = xeno.HealthRegen;
            if (xeno.OnResting)
            {
                healthRegen = xeno.OnWeeds ? xeno.HealthRegenOnWeeds : xeno.HealthRegenOnRest;
            }

            if (TryComp<DamageableComponent>(uid, out var damageable))
                HealDamage((uid, damageable), healthRegen * frameTime);

            Dirty(uid, xeno);
        }
    }

    public void HealDamage(Entity<DamageableComponent?> ent, float amount)
    {
        if (!Resolve(ent, ref ent.Comp))
            return;

        var heal = new DamageSpecifier();
        foreach (var (type, typeAmount) in ent.Comp.Damage.DamageDict)
        {
            var total = heal.GetTotal();
            if (typeAmount + total >= amount)
            {
                var change = -FixedPoint2.Min(typeAmount, amount - total);
                if (!heal.DamageDict.TryAdd(type, change))
                    heal.DamageDict[type] += change;

                break;
            }

            if (!heal.DamageDict.TryAdd(type, -typeAmount))
                heal.DamageDict[type] += -typeAmount;
        }

        if (heal.GetTotal() < FixedPoint2.Zero)
            _damageable.TryChangeDamage(ent, heal);
    }
}
