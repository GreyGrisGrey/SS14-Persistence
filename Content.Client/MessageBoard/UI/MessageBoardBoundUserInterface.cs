using Content.Client.CrewAssignments.UI;
using Content.Shared.IdentityManagement;
using Content.Shared.MessageBoard.Components;
using JetBrains.Annotations;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Prototypes;
using Robust.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using static Robust.Client.UserInterface.Controls.MenuBar;

namespace Content.Client.MessageBoard.UI;

[UsedImplicitly]
public sealed class MessageBoardBoundUserInterface : BoundUserInterface
{
    [ViewVariables]
    private MessageBoard? _menu;
    private CreateEntry? _createEntry;

    public MessageBoardBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        var spriteSystem = EntMan.System<SpriteSystem>();
        var dependencies = IoCManager.Instance!;
        _menu = new MessageBoard(Owner, EntMan, dependencies.Resolve<IPrototypeManager>(), spriteSystem);
        var localPlayer = dependencies.Resolve<IPlayerManager>().LocalEntity;
        var description = new FormattedMessage();
        _menu.OnClose += Close;
        _menu.OpenCentered();
        _menu.CreateEntryPublicButton.OnPressed += CreateEntryPublic;
    }

    public void CreateEntryPublic(BaseButton.ButtonEventArgs args)
    {
        if(_createEntry != null)
        {
            _createEntry.Dispose();
        }
        _createEntry = new CreateEntry();
        _createEntry.OpenCentered();
        _createEntry.CurrentEntryType = CreateEntry.EntryType.Public;
        _createEntry.PostButton.OnPressed += FinalizeEntryPublic;
    }

    public void FinalizeEntryPublic(BaseButton.ButtonEventArgs args)
    {
        if (_createEntry == null)
            return;
        var title = _createEntry.MainTitleLabel.Text;
        var content = Rope.Collapse(_createEntry.DescriptionLabel.TextRope);
        if(title == string.Empty || content == string.Empty)
        {
            return;
        }
        SendMessage(new MessageBoardCreateEntryPublicMessage(title, content));
        _createEntry.Dispose();
        _createEntry = null;
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (!disposing)
            return;

        _menu?.Dispose();
    }

}
