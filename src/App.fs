module App

open Elmish
open Elmish.React
open Feliz
open Feliz.Router

[<RequireQualifiedAccess>]
type Url =
    | MainMenu
    | ProgramMenu of ProgramMenu.Url

let parseUrl (url: string list) =
    match url with
    | "program" :: programSegments -> Url.ProgramMenu (ProgramMenu.parseUrl programSegments)
    | _ -> Url.MainMenu

[<RequireQualifiedAccess>]
type Page =
 | MainMenu of MainMenu.State
 | ProgramMenu of ProgramMenu.State

type State =
    { CurrentPage: Page
      CurrentUrl: Url }

type Msg =
    | MainMenuMsg of MainMenu.Msg
    | ProgramMenuMsg of ProgramMenu.Msg
    | UrlChanged of string list

let init() =
    let (mainMenuState, mainMenuCmds) = MainMenu.init()
    { CurrentPage = Page.MainMenu mainMenuState
      CurrentUrl = parseUrl (Router.currentUrl()) }, (Cmd.map MainMenuMsg mainMenuCmds)

let update (msg: Msg) (state: State): State * Cmd<Msg> =
    match state.CurrentPage, msg with
    | Page.MainMenu mainMenuState, MainMenuMsg msg ->
        let (newState, cmd) = MainMenu.update msg mainMenuState
        { state with CurrentPage = Page.MainMenu newState }, Cmd.map MainMenuMsg cmd
    | Page.ProgramMenu programMenuState, ProgramMenuMsg msg ->
        let (newState, cmd) = ProgramMenu.update msg programMenuState
        { state with CurrentPage = Page.ProgramMenu newState }, Cmd.map ProgramMenuMsg cmd
    | _, UrlChanged url ->
        match parseUrl url with
        | Url.MainMenu ->
            let (newState, cmd) = MainMenu.init ()
            { state with CurrentPage = Page.MainMenu newState }, Cmd.map MainMenuMsg cmd
        | Url.ProgramMenu programId ->
            let (newState, cmd) = ProgramMenu.init programId
            { state with CurrentPage = Page.ProgramMenu newState }, Cmd.map ProgramMenuMsg cmd

let render (state: State) (dispatch: Msg -> unit) =
    let content =
        match state.CurrentPage with
        | Page.MainMenu mainMenuState ->
            MainMenu.render mainMenuState (MainMenuMsg >> dispatch)
        | Page.ProgramMenu programMenuState ->
            ProgramMenu.render programMenuState (ProgramMenuMsg >> dispatch)     
    
    let page =
        Html.div [ 
            prop.style [ 
                style.padding 20
                // style.backgroundColor "#202020" 
                style.backgroundColor "#FFF" 
            ]
            prop.children [ 
                content
            ] 
        ]

    Router.router [
      Router.onUrlChanged (UrlChanged >> dispatch)
      Router.application [ page ]
    ]

#if DEBUG
open Elmish.HMR
#endif

Program.mkProgram init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
