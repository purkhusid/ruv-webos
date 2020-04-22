module ProgramMenu

open Elmish
open Feliz
open Feliz.Bulma
open Feliz.Router

[<RequireQualifiedAccess>]
type Url =
    | Index of programId:int
    | NotFound

let parseUrl (url: string list) =
    match url with
    | [ Route.Int programId ] -> Url.Index programId
    | _ -> Url.NotFound 

type Episode = 
    { id: string 
      title: string 
      description: string
      file: string 
      image: string }

type Program =
    { id: int
      title: string
      description: string
      episodes: Episode list }

type DeferredProgram = DeferredResult<Program>

type State =
    { Program: DeferredProgram 
      CurrentUrl: Url}

type Msg =
    | LoadProgram of programId:int * AsyncOperationStatus<Result<Program, string>>

let init (url: Url) =
    match url with
    | Url.Index programId ->
        { Program = HasNotStartedYet; CurrentUrl=url }, Cmd.ofMsg (LoadProgram (programId, Started))
    | Url.NotFound -> 
        { Program = HasNotStartedYet; CurrentUrl=url }, Cmd.ofMsg (LoadProgram (0, Finished(Error "No programId in path")))

let startLoading (state: State) = { state with Program = InProgress }

let mapEpisodes (episodesResult: Option<Api.Episode list>): Episode list =
    match episodesResult with
    | Some(episodes) ->
        episodes
        |> List.map (fun e -> 
            { id = e.id
              title = e.title |> Option.defaultValue ""
              description = e.description |> Option.defaultValue ""
              file = e.file |> Option.defaultValue "" 
              image = e.image |> Option.defaultValue "" })
    | None -> List.empty              


let loadProgram programId =
    async {
        let! programResult = Api.program(programId)
        match programResult with
        | Ok(p) ->
            let episodes = mapEpisodes p.episodes
            let program =
                { id = p.id
                  title = p.title |> Option.defaultValue ""
                  description = p.description |> Option.defaultValue ""
                  episodes = episodes }
            return LoadProgram(programId, Finished(Ok program))
        | Error(error) -> return LoadProgram(programId, Finished(Error error))
    }

let update (msg: Msg) (state: State) =
    match msg with
    | LoadProgram (programId, Started) ->
        let nextState = startLoading state
        let nextCmd = Cmd.fromAsync (loadProgram programId)
        nextState, nextCmd
    | LoadProgram(programId, Finished(Ok(program))) ->
        let nextState = { state with Program = Resolved(Ok program) }
        nextState, Cmd.none
    | LoadProgram(programId, Finished(Error(error))) ->
        let nextState = { state with Program = Resolved(Error error) }
        nextState, Cmd.none

let renderError (errorMsg: string) =
    Html.h1 [ 
        prop.style [ 
            style.color.red 
        ]
        prop.text errorMsg 
   ]

let spinner =
    Html.div [ 
        prop.style [ 
            style.textAlign.center
            style.marginTop 20 
        ]
        prop.children [ 
            Html.i [ 
                prop.className "fa fa-cog fa-spin fa-2x"
            ] 
        ] 
    ]

let renderProgram (program: Program) =
    Bulma.columns [
        columns.isMultiline
        prop.children [
            Bulma.column [
                column.isOneQuarter
                prop.children [
                    Bulma.card [
                        Bulma.cardImage [
                            Bulma.image [
                                image.is3by2
                                prop.children [
                                    Html.img [
                                        prop.src (program.episodes.[0].image.Replace("$$IMAGESIZE$$","1980"))
                                    ]
                                ]
                            ]
                        ]
                    ]
                ]
            ]
        ]
    ]
    // Html.div [
    //     prop.children [
    //         Bulma.tile [
    //             tile.isAncestor
    //             prop.children [
    //                 Bulma.tile [
    //                     prop.children [
    //                         Bulma.box [
    //                             prop.children [
    //                                 Html.p [
    //                                     prop.className "title"
    //                                     prop.text "Yosafat"
    //                                 ]
    //                                 Html.p [
    //                                     prop.text "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."
    //                                 ]
    //                             ]
    //                         ]
    //                     ]
    //                 ]
    //                 Bulma.tile [
    //                     prop.children [
    //                         Bulma.box [
    //                             prop.children [
    //                                 Html.p [
    //                                     prop.className "title"
    //                                     prop.text "Yosafat2"
    //                                 ]
    //                                 Html.p [
    //                                     prop.text "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."
    //                                 ]
    //                             ]
    //                         ]
    //                     ]
    //                 ]
    //                 Bulma.tile [
    //                     prop.children [
    //                         Bulma.box [
    //                             prop.children [
    //                                 Html.p [
    //                                     prop.className "title"
    //                                     prop.text "Yosafat2"
    //                                 ]
    //                                 Html.p [
    //                                     prop.text "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."
    //                                 ]
    //                             ]
    //                         ]
    //                     ]
    //                 ]
    //                 Bulma.tile [
    //                     prop.children [
    //                         Bulma.box [
    //                             prop.children [
    //                                 Html.p [
    //                                     prop.className "title"
    //                                     prop.text "Yosafat2"
    //                                 ]
    //                                 Html.p [
    //                                     prop.text "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."
    //                                 ]
    //                             ]
    //                         ]
    //                     ]
    //                 ]
    //                 Bulma.tile [
    //                     prop.children [
    //                         Bulma.box [
    //                             prop.children [
    //                                 Html.p [
    //                                     prop.className "title"
    //                                     prop.text "Yosafat2"
    //                                 ]
    //                                 Html.p [
    //                                     prop.text "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin ornare magna eros, eu pellentesque tortor vestibulum ut. Maecenas non massa sem. Etiam finibus odio quis feugiat facilisis."
    //                                 ]
    //                             ]
    //                         ]
    //                     ]
    //                 ]

    //             ]
    //         ]
    //     ]
    // ]

let renderDeferredProgram (program: DeferredProgram) =
    match program with
    | HasNotStartedYet -> Html.none
    | InProgress -> spinner
    | Resolved(Ok program) ->
        renderProgram program
    | Resolved(Error error) -> renderError error

let render (state: State) (dispatch: Msg -> unit) =
    Html.div [ 
        prop.children [ 
            renderDeferredProgram state.Program 
        ] 
    ]
