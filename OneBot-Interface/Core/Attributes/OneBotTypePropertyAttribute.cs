using OneBot.Core.Event;

namespace OneBot.Core.Attributes;

public class OneBotTypePropertyAttribute : OneBotExtraPropertiesAttribute
{
    public OneBotTypePropertyAttribute(string type, string detailType, string subType) :
        base(OneBotEvent.Type.PropertyName, type, OneBotEvent.DetailType.PropertyName, detailType, OneBotEvent.SubType.PropertyName, subType)
    {

    }

    public OneBotTypePropertyAttribute(string type, string detailType) :
        base(OneBotEvent.Type.PropertyName, type, OneBotEvent.DetailType.PropertyName, detailType)
    {

    }

    public OneBotTypePropertyAttribute(string type) :
        base(OneBotEvent.Type.PropertyName, type)
    {

    }
}
