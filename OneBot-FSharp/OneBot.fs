namespace Onebot_FSharp

open OneBot.CommandRoute.Configuration
open OneBot.Core.Context

module OneBot =
    type Context = OneBotContext
    
    type Reaction = 
        | Reply of string
        | Message of string
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


    let callback (handler : Handler) (context : OneBotContext) : Unit = 
        match handler context with 
            | Reply text -> ()



    let rec configure (builder : CommandRouteNodeBuilder) (rule : Rule) : Unit = 
        match rule with
        | Command (pattern,handler) -> builder.Command (pattern, OneBotCallback(callback handler))
        | SubRule (pattern,rule) -> builder.Group (pattern, fun newBuilder -> configure newBuilder rule); ()
        | Fallback handler -> builder.Command (OneBotCallback(callback handler)); ()
        | Group rules -> List.map (configure builder) rules; ()
        ()
        

    
    let openBot (rule : Rule) : Unit = 
        ()

  
