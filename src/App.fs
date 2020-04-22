module App

open Elmish
open Elmish.React
open Feliz
open Feliz.PureReactCarousel
open Fable.Core.JsInterop

importSideEffects "pure-react-carousel/dist/react-carousel.es.css"
importSideEffects "./styles.css"

[<RequireQualifiedAccess>]
type Page =
 | MainMenu

type State =
    { MainMenu: MainMenu.State
      CurrentPage: Page }

type Msg =
    | MainMenuMsg of MainMenu.Msg

let init() =
    let (mainMenuState, mainMenuCmds) = MainMenu.init()
    { MainMenu = mainMenuState
      CurrentPage = Page.MainMenu }, Cmd.ofMsg (mainMenuCmds)

let update (msg: Msg) (state: State): State * Cmd<Msg> =
    match msg with
    | MainMenuMsg msg ->
        let (newState, cmd) = MainMenu.update msg state.MainMenu
        { state with MainMenu = newState }, Cmd.map MainMenuMsg cmd


let render (state: State) (dispatch: Msg -> unit) =
    let content =
        match state.CurrentPage with
        | Page.MainMenu ->
            MainMenu.render state.MainMenu (MainMenuMsg >> dispatch)
    Html.div [ 
        prop.style [ 
            style.padding 20
            style.backgroundColor "#202020" 
        ]
        prop.children [ 
            content
        ] 
    ]

#if DEBUG
open Elmish.HMR
#endif

Program.mkProgram MainMenu.init MainMenu.update MainMenu.render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
