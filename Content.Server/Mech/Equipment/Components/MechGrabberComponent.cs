﻿using System.Numerics;
using System.Threading;
using System.Collections.Generic;
using Content.Shared.DoAfter;
using Robust.Shared.Audio;
using Robust.Shared.Containers;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.List;

namespace Content.Server.Mech.Equipment.Components;

/// <summary>
/// A piece of mech equipment that grabs entities and stores them
/// inside of a container so large objects can be moved.
/// </summary>
[RegisterComponent]
public sealed partial class MechGrabberComponent : Component
{
    /// <summary>
    /// The change in energy after each grab.
    /// </summary>
    [DataField("grabEnergyDelta")]
    public float GrabEnergyDelta = -30;

    /// <summary>
    /// How long does it take to grab something?
    /// </summary>
    [DataField("grabDelay")]
    public float GrabDelay = 2.5f;

    /// <summary>
    /// The offset from the mech when an item is dropped.
    /// This is here for things like lockers and vendors
    /// </summary>
    [DataField("depositOffset")]
    public Vector2 DepositOffset = new(0, -1);

    /// <summary>
    /// The maximum amount of items that can be fit in this grabber
    /// </summary>
    [DataField("maxContents")]
    public int MaxContents = 10;

    /// <summary>
    /// The sound played when a mech is grabbing something
    /// </summary>
    [DataField("grabSound")]
    public SoundSpecifier GrabSound = new SoundPathSpecifier("/Audio/Mecha/sound_mecha_hydraulic.ogg");

    public EntityUid? AudioStream;

    [ViewVariables(VVAccess.ReadWrite)]
    public Container ItemContainer = default!;

    [DataField, ViewVariables(VVAccess.ReadOnly)]
    public DoAfterId? DoAfter;

    /// <summary>
    /// List of entity prototype IDs that are blacklisted from being grabbed by this component.
    /// </summary>
    [DataField("blacklist", customTypeSerializer: typeof(PrototypeIdListSerializer<EntityPrototype>))]
    public List<string> BlacklistPrototypes = new();
}