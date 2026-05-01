using Content.Server.CrewRecords.Systems;
using Content.Shared.Cargo.BUI;
using Content.Shared.CrewAssignments;
using Content.Shared.CrewAssignments.Components;
using Content.Shared.CrewAssignments.Systems;
using Content.Shared.DoAfter;
using Content.Shared.Implants.Components;
using Content.Shared.Interaction;
using Content.Shared.MessageBoard.Components;
using Content.Shared.MessageBoard.Systems;
using Content.Shared.UserInterface;
using Robust.Server.GameObjects;
using Robust.Shared.Physics.Dynamics;
using System;
using System.Collections.Generic;
using System.Text;
using static Content.Shared.CrewAssignments.Systems.CodexEuiMsg;
using static Content.Shared.CrewAssignments.Systems.SharedJobNetSystem;
using static System.Collections.Specialized.BitVector32;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Content.Server.MessageBoard.Systems;

public sealed partial class MessageBoardSystem : SharedMessageBoardSystem
{
    [Dependency] private readonly UserInterfaceSystem _uiSystem = default!;
    [Dependency] private readonly CrewMetaRecordsSystem _crewMetaRecordsSystem = default!;
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MessageBoardComponent, BoundUIOpenedEvent>(OnUIOpened);
        SubscribeLocalEvent<MessageBoardComponent, MessageBoardCreateEntryPublicMessage>(OnCreateEntryPublic);
    }

    private void OnUIOpened(Entity<MessageBoardComponent> ent, ref BoundUIOpenedEvent args)
    {
        UpdateUserInterface(ent.Owner, ent.Comp);
    }

    private void OnCreateEntryPublic(Entity<MessageBoardComponent> ent, ref MessageBoardCreateEntryPublicMessage args)
    {
        var metaRecord = _crewMetaRecordsSystem.MetaRecords;
        if (metaRecord == null) return;
        MessageBoardEntry newEntry = new(args.Title, Name(args.Actor), args.Body);
        metaRecord.MessageBoardEntries.Add(newEntry);
        UpdateUserInterface(ent.Owner, ent.Comp);
    }

    private void UpdateUserInterface(EntityUid uid, MessageBoardComponent component)
    {
        _uiSystem.SetUiState(uid, MessageBoardUiKey.Main, new MessageBoardInterfaceState());
    }
}
