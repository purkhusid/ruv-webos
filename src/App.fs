module App

open Elmish
open Elmish.React
open Feliz

type Program =
    { id: int
      title: string
      description: string
      slug: string
      image: string }

type DeferredCategoryPrograms = DeferredResult<Program list>

type Category =
    { slug: string
      title: string
      programs: DeferredCategoryPrograms }

type DeferredCategories = DeferredResult<Category list>

type State =
    { Categories: DeferredCategories }

type Msg = LoadCategories of AsyncOperationStatus<Result<Category list, string>>

let init() = { Categories = HasNotStartedYet }, Cmd.ofMsg (LoadCategories Started)

let startLoading (state: State) = { state with Categories = InProgress }

let loadCategories =
    async {
        let! categories = Api.categoriesQuery()
        match categories with
        | Ok(categoryList) ->
            let categories =
                categoryList.Category.categories
                |> List.map (fun c ->
                    { slug = c.slug
                      title = c.title
                      programs = HasNotStartedYet })
            return LoadCategories(Finished(Ok categories))

        | Error(error) -> return LoadCategories(Finished(Error error))
    }

// let loadCategory categorySlug = async {
//         let! categoryPrograms = Api.categoryPrograms categorySlug
//         match categoryPrograms with
//         | Ok(category) ->
//             return
//     }

let update (msg: Msg) (state: State) =
    match msg with
    | LoadCategories Started ->
        let nextState = startLoading state
        let nextCmd = Cmd.fromAsync (loadCategories)
        nextState, nextCmd

    | LoadCategories(Finished(Ok(categories))) ->
        let nextState = { state with Categories = Resolved(Ok categories) }
        let categorySlugs = categories |> List.map (fun c -> c.slug)
        // Replace Cmd.none with fetching of each program
        // nextState, Cmd.batch[ for category in categorySlugs]
        nextState, Cmd.none

    | LoadCategories(Finished(Error(error))) ->
        let nextState = { state with Categories = Resolved(Error error) }
        nextState, Cmd.none


let renderError (errorMsg: string) =
    Html.h1
        [ prop.style [ style.color.red ]
          prop.text errorMsg ]

let spinner =
    Html.div
        [ prop.style
            [ style.textAlign.center
              style.marginTop 20 ]
          prop.children [ Html.i [ prop.className "fa fa-cog fa-spin fa-2x" ] ] ]


let renderCategoryContent (category: Category) =
    Html.h1
        [ prop.className "title"
          prop.text category.title ]

let renderCategory category =
    Html.div
        [ prop.className "box"
          prop.style
              [ style.marginTop 15
                style.marginBottom 15 ]
          prop.children [ renderCategoryContent category ] ]


let renderCategories (categories: DeferredCategories) =
    match categories with
    | HasNotStartedYet -> Html.none
    | InProgress -> spinner
    | Resolved(Error errorMsg) -> renderError errorMsg
    | Resolved(Ok categories) ->
        categories
        |> List.map (renderCategory)
        |> Html.div

let title =
    Html.h1
        [ prop.className "title"
          prop.text "Whoot" ]

let render (state: State) (dispatch: Msg -> unit) =
    Html.div
        [ prop.style
            [ style.padding 20
              style.backgroundColor "#202020" ]
          prop.children [ renderCategories state.Categories ] ]

#if DEBUG
open Elmish.HMR
#endif

Program.mkProgram init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
