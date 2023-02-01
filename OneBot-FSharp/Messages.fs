namespace OneBot.FSharp

module Segments =
    
    type Audio (fileId : string) = 
        interface Core.Model.Message.Audio with
            member FileId with get() = fileId

    type File (fieldId : string) = 
        interface Core.Model.Message.Audio with
            member FileId with get() = fileId

    type Image (fieldId : string) =
        interface Core.Model.Message.Image with
            member FileId with get() = fieldId

    type Location (latitude : double, longitude : double, title : string, content : string) = 
        interface Core.Model.Message.Location with
            member Latitude with get() = latitude
            member Longtitude with get() = longitude
            member Title with get() = title
            member Content with get() = content
    
    type Mention (userId : string) =
        interface Core.Model.Message.Location with
            member UserId with get() = userId
    
    type MentionAll () = 
        interface Core.Model.Message.MentionAll

    type Reply (messageId : int, userId : string) with
        interface Core.Model.Message.Reply with
            member MessageId with get() = messageId
            member UserId with get() = userId

    type Text (text : string) =
        interface Core.Model.Message.Text with
            member Text with get() = text

    type Video (fileId : string) =
        interface Core.Model.Message.Text with
            member FileId with get() = fieldId

    type Voice (fileId : string) = 
        interface Core.Model.Message.Voice with
            member FileId with get()

    //double Latitude { get; }

    //double Longitude { get; }

    //string Title { get; }

    //string Content { get; }
