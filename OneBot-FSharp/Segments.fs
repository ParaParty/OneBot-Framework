namespace OneBot.FSharp

open OneBot.Core.Model.Message

module Segments =
    
    type Audio (fileId : string) = 
        interface MessageSegmentData.Audio with
            member this.FileId with get() = fileId

    type File (fileId : string) = 
        interface MessageSegmentData.Audio with
            member this.FileId with get() = fileId

    type Image (fileId : string) =
        interface MessageSegmentData.Image with
            member this.FileId with get() = fileId

    type Location (latitude : double, longitude : double, title : string, content : string) = 
        interface MessageSegmentData.Location with
            member _.Latitude with get() = latitude
            member _.Longitude with get() = longitude
            member _.Title with get() = title
            member _.Content with get() = content
    
    type Mention (userId : string) =
        interface MessageSegmentData.Mention with
            member _.UserId with get() = userId
    
    type MentionAll () = 
        interface MessageSegmentData.MentionAll with

    type Reply (messageId : string, userId : string) =
        interface MessageSegmentData.Reply with
            member _.MessageId with get() = messageId
            member _.UserId with get() = userId

    type Text (text : string) =
        interface MessageSegmentData.Text with
            member _.Text with get() = text

    type Video (fileId : string) =
        interface MessageSegmentData.Video with
            member _.FileId with get() = fileId

    type Voice (fileId : string) = 
        interface MessageSegmentData.Voice with
            member _.FileId with get() = fileId
