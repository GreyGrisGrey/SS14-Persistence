using Content.Shared.CrewAccesses.Components;
using Content.Shared.CrewAssignments.Components;
using Content.Shared.CrewAssignments.Prototypes;
using Content.Shared.Radio;
using Content.Shared.Station.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.MessageBoard.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class MessageBoardComponent : Component
{
}


[DataDefinition]
[Serializable]
[Virtual]
public partial class MessageBoardEntry
{
    public string Title;
    public string Author;
    public string Body;
    public DateTime CreationTime;
    public List<MessageBoardComment> Comments = new();

    public MessageBoardEntry(string title, string author, string body)
    {
        Title = title;
        Author = author;
        Body = body;
        CreationTime = DateTime.Now;
    }

}

[DataDefinition]
[Serializable]
[Virtual]
public partial class MessageBoardComment
{
    public string Author;
    public string Body;
    public DateTime CreationTime;

    public MessageBoardComment(string author, string body)
    {
        Author = author;
        Body = body;
        CreationTime = DateTime.Now;
    }

}

[NetSerializable, Serializable]
public sealed class MessageBoardInterfaceState : BoundUserInterfaceState
{

}

[Serializable, NetSerializable]
public sealed class MessageBoardCreateEntryPublicMessage : BoundUserInterfaceMessage
{
    public string Title;
    public string Body;

    public MessageBoardCreateEntryPublicMessage(string title, string body)
    {
        Title = title;
        Body = body;
    }
}
