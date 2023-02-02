namespace Onebot.FSharp

open Microsoft.FSharp.Core
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
        | QuickReply of string
        | Response of Message
        | WaitFor of string * int * (Context -> Reaction)
        | Reactions of Reaction List
        | Ignore

    type Handler = Context -> Reaction
    type OneBotCallback = delegate of OneBotContext -> unit
    
    type Condition = Context -> bool
    
    type Rule =
        | Group of Rule List
        | SubRule of string * Rule
        | Command of string * Handler
        | EnableWhen of Condition * Rule 
        | Fallback of Handler * Rule

    let konst a b = a
    
    let deploy (rule : Rule) : Unit = 
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


        let callback (hdl : Handler) (cond : Condition) (fallback : Handler) (ctx : OneBotContext) : Unit =
            let response = if cond ctx then hdl ctx else fallback ctx
            let rec process r = match r with 
                                | Response msg -> ctx.Reply(createMessage msg) |> ignore //暂时忽略ActionResponse
                                | Reactions xs -> List.map process xs |> ignore
                                | Ignore -> ()
            process response
        
        
        let rec configure (builder : CommandRouteNodeBuilder) fallback condition rule =
            match rule with
            | Group rs -> let _ = List.map (configure builder fallback condition) rs
                          builder.Command(OneBotCallback(callback fallback (konst true) fallback)) |> ignore
            | SubRule (p,r) -> builder.Group (p, fun b -> configure b fallback condition r) |> ignore
            | Command (p,h) -> builder.Command (p,OneBotCallback(callback h condition fallback))
            | EnableWhen (c,r) -> configure builder fallback c r
            | Fallback (f,r) -> configure builder f condition r
        
    
        let builder = new CommandRouteNodeBuilder()
        let defaultCondition = konst true
        let defaultFallback = konst Ignore
        configure builder defaultFallback defaultCondition rule
        
        ()
        
    // 示例：
    // let exampleBot =
    //     let always = fun a _ -> a
    //     let refuse = QuickReply "您已进入+1机器人黑名单"
    //     let blackList = ["114514"]
    //     let mutable queryMode = false
    //     let mutable enabled = true
    //     EnableWhen ((fun _ -> enabled),
    //         Fallback (always refuse,
    //             EnableWhen ((fun ctx -> not (List.contains ctx.Event.Id blackList)),
    //                 Fallback ((fun _ -> QuickReply "+1"),
    //                     Group [
    //                         Command ("open", always (QuickReply "Bot已开启"));
    //                         Command ("close", fun _ -> (queryMode <- true; QuickReply "确认关闭(Y/N)?"));
    //                         EnableWhen ((fun _ -> queryMode),
    //                             Group [
    //                                 Command ("Y", fun _ -> (QuickReply "已关闭"))
    //                                 Command ("N", fun _ -> (queryMode <- false; QuickReply "已取消"))
    //                             ]
    //                         ) 
    //                     ]
    //                 )
    //             )
    //         )                
    //     )
    //
    // deploy exampleBot
    //
    // 一样的功能但使用let分解功能点，减少嵌套：
    // let exampleBot2 =
    //     let always = fun a _ -> a
    //     let refuse = QuickReply "您已进入+1机器人黑名单"
    //     let blackList = ["114514"]
    //     let mutable enabled = true
    //     let isEnabled = fun _ -> enabled
    //     let isNotBlocked = fun (ctx : Context) -> not (List.contains ctx.Event.Id blackList)
    //     let replyPlusOne = fun _ -> QuickReply "+1"
    //
    //     let shutdownFeature =
    //         let mutable queryMode = false
    //         let isQueryMode = fun _ -> queryMode
    //         let shutdownQuery =
    //              Group [
    //                  Command ("Y", fun _ -> (QuickReply "已关闭"))
    //                  Command ("N", fun _ -> (queryMode <- false; QuickReply "已取消"))
    //              ]
    //         Group [
    //             Command ("open", always (QuickReply "Bot已开启"));
    //             Command ("close", fun _ -> (queryMode <- true; QuickReply "确认关闭(Y/N)?"));
    //             EnableWhen (isQueryMode,shutdownQuery)
    //         ]
    //              
    //     EnableWhen (isEnabled,
    //         Fallback (always refuse,
    //             EnableWhen (isNotBlocked,
    //                 Fallback (replyPlusOne,shutdownFeature)
    //             )
    //         )                
    //     )
    //
    // deploy exampleBot2
        
  
