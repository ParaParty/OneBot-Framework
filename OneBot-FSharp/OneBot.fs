namespace Onebot.FSharp

open OneBot.CommandRoute.Configuration
open OneBot.Core.Context
open OneBot.Core.Util
open OneBot.Core.Model


module FBot =
    type Context = OneBotContext

    type FileId = string
    type UserId = int

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
        | Message of Message
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

        let callback (handler : Handler) (context : OneBotContext) : Unit = 
            match handler context with 
                | Reply text -> context.Reply (new Message.SimpleMessage(text)) |> ignore //暂时忽略ActionRespone
                

        let rec configure (builder : CommandRouteNodeBuilder) (rule : Rule) : Unit = 
            match rule with
            | Command (pattern,handler) -> builder.Command (pattern, OneBotCallback(callback handler))
            | SubRule (pattern,rule) -> builder.Group (pattern, fun newBuilder -> configure newBuilder rule) |> ignore
            | Fallback handler -> builder.Command (OneBotCallback(callback handler)) |> ignore
            | Group rules -> List.map (configure builder) rules |> ignore
            ()


        ()

  
