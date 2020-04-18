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

type DeferredCategories = DeferredResult<Map<string, Category>>

type State =
    { categories: DeferredCategories }

type Msg =
    | LoadCategories of AsyncOperationStatus<Result<Category list, string>>
    | LoadedCategoryPrograms of string * Result<Program list, string>

let init() = { categories = HasNotStartedYet }, Cmd.ofMsg (LoadCategories Started)

let startLoading (state: State) = { state with categories = InProgress }

let loadCategories =
    async {
        let! categories = Api.categoriesQuery()
        match categories with
        | Ok(categoryList) ->
            let categories =
                categoryList.Category.categories
                |> List.map (fun c ->
                    // TODO: Don't use .value!
                    { slug = c.slug |> Option.defaultValue ""
                      title = c.title |> Option.defaultValue ""
                      programs = HasNotStartedYet })
            return LoadCategories(Finished(Ok categories))

        | Error(error) -> return LoadCategories(Finished(Error error))
    }

let loadCategoryPrograms categorySlug =
    async {
        match! (Api.categoryPrograms categorySlug) with
        | Ok(programs) ->
            let categoryPrograms =
                programs
                |> List.map (fun p ->
                    { id = p.id
                      title = p.title |> Option.defaultValue ""
                      description = p.description |> Option.defaultValue ""
                      slug = p.slug |> Option.defaultValue ""
                      image = p.image |> Option.defaultValue "" })
            return LoadedCategoryPrograms((categorySlug, Ok categoryPrograms))
        | Error(error) -> return LoadedCategoryPrograms((categorySlug, Error error))
    }

let update (msg: Msg) (state: State) =
    match msg with
    | LoadCategories Started ->
        let nextState = startLoading state
        let nextCmd = Cmd.fromAsync (loadCategories)
        nextState, nextCmd

    | LoadCategories(Finished(Ok(categories))) ->
        let categoryMap =
            [ for c in categories -> c.slug, c ]
            |> Map.ofSeq

        let nextState = { state with categories = Resolved(Ok categoryMap) }
        let categorySlugs = categories |> List.map (fun c -> c.slug)
        nextState,
        Cmd.batch [ for category in categorySlugs -> Cmd.fromAsync (loadCategoryPrograms category) ]

    | LoadCategories(Finished(Error(error))) ->
        let nextState = { state with categories = Resolved(Error error) }
        nextState, Cmd.none

    | LoadedCategoryPrograms(category, Ok(programs)) ->
        match state.categories with
        | Resolved(Ok categories) ->
            let oldCategory = categories.[category]
            let newCategory = { oldCategory with programs = Resolved(Ok programs) }

            let modifiedCategoriesMap =
                categories
                |> Map.remove category
                |> Map.add category newCategory

            let nextState = { state with categories = Resolved(Ok modifiedCategoriesMap) }
            nextState, Cmd.none
        | _ -> state, Cmd.none

    | LoadedCategoryPrograms(category, Error(error)) ->
        let nextState = { state with categories = Resolved(Error error) }
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

let renderCategoryPrograms (programs: DeferredCategoryPrograms) =
    let renderedPrograms =
        match programs with
        | HasNotStartedYet -> [ Html.none ]
        | InProgress -> [ spinner ]
        | Resolved(Ok programs) ->
            [ for p in programs -> Html.h2 [ prop.text p.title ] ]
        | Resolved(Error error) -> [ renderError error ]

    Html.div [ prop.children renderedPrograms ]

let renderCategoryContent (category: Category) =
    Html.div
        [ prop.children
            [ Html.h1
                [ prop.className "title"
                  prop.text category.title ]
              renderCategoryPrograms category.programs ] ]


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
        |> Map.toList
        |> List.map (fun (slug, category) -> renderCategory category)
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
          prop.children [ renderCategories state.categories ] ]

#if DEBUG
open Elmish.HMR
#endif

Program.mkProgram init update render
|> Program.withReactSynchronous "elmish-app"
|> Program.run
