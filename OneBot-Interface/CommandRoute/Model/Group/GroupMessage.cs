namespace OneBot.CommandRoute.Model;

public interface GroupMessage
{
    string DetailType { get;  }
    
    string MessageId { get;  } 
    
    string Message { get;  }
    
    string AltMessage { get; }
    
    string GroupId { get;  }
    
    string UserId { get; } 
}
