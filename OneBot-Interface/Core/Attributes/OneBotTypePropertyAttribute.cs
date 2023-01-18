namespace OneBot.Core.Attributes;

public class OneBotTypePropertyAttribute : OneBotExtraPropertiesAttribute
{
    public OneBotTypePropertyAttribute(string type, string detailType, string subType) :
        base("type", type, "detail_type", detailType, "sub_type", subType)
    {

    }

    public OneBotTypePropertyAttribute(string type, string detailType) :
        base("type", type, "detail_type", detailType)
    {

    }

    public OneBotTypePropertyAttribute(string type) :
        base("type", type)
    {

    }
}
