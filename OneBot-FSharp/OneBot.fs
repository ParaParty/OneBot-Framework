namespace Onebot.FSharp

open OneBot.CommandRoute.Configuration
open OneBot.Core.Context
open OneBot.Core.Util
open OneBot.Core.Model
open OneBot.Core
open OneBot.FSharp

module FBot =
    type Context = OneBotContext

    type FileId = string
    type UserId = string

    type Message = 
        | Complex of Message List
        | Text of string
        | Audio of FileId
        | File of FileId
        | Image of FileId
        | Location of double * double * string * string
        | Mention of UserId
        | MentionAll
        | Reply of UserId * string
        | Video of FileId
        | Voice of FileId

    type Reaction = 
        | Response of Message
        | Mute of int
        | WaitFor of string * int * (Context -> Reaction)
        | Reactions of Reaction List
        | Ignore

    type Handler = Context -> Reaction
    type OneBotCallback = delegate of OneBotContext -> unit

    type Rule =     
        | Group of Rule List
        | SubRule of string * Rule
        | Command of string * Handler
        | Fallback of Handler

    
    let bot (rule : Rule) : Unit = 

        let createMessage (msg : Message) : OneBot.Core.Model.Message.SimpleMessage =

            let messageModel (msg : Message) : Message.MessageSegmentRef = 
                let f = Message.MessageSegment in match msg with
                | Text x -> f(Segments.Text(x))
                | Audio x -> f(Segments.Audio(x))
                | File x -> f(Segments.File(x))
                | Image x -> f(Segments.Image(x))
                | Location (la,lo,t,c) -> f(Segments.Location(la,lo,t,c))
                | Mention u -> f(Segments.Mention(u))
                | MentionAll -> f(Segments.MentionAll())
                | Reply (u,s) -> f(Segments.Reply(u,s))
                | Video x -> f(Segments.Video(x))
                | Voice x -> f(Segments.Voice(x))

            match msg with
            | Complex xs -> List.map messageModel xs |> System.Collections.Generic.List |> Message.SimpleMessage
            | _ -> messageModel msg |> Message.SimpleMessage





        let callback (handler : Handler) (context : OneBotContext) : Unit = 
            match handler context with 
                | Response msg -> context.Reply(createMessage msg) |> ignore //暂时忽略ActionRespone
                

        let rec configure (builder : CommandRouteNodeBuilder) (rule : Rule) : Unit = 
            match rule with
            | Command (pattern,handler) -> builder.Command (pattern, OneBotCallback(callback handler))
            | SubRule (pattern,rule) -> builder.Group (pattern, fun newBuilder -> configure newBuilder rule) |> ignore
            | Fallback handler -> builder.Command (OneBotCallback(callback handler)) |> ignore
            | Group rules -> List.map (configure builder) rules |> ignore
            ()


        ()

  
