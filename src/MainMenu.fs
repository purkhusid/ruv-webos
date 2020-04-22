module MainMenu

open Elmish
open Elmish.React
open Feliz
open Feliz.PureReactCarousel
open Fable.Core.JsInterop
open Feliz.Router

importSideEffects "pure-react-carousel/dist/react-carousel.es.css"
importSideEffects "./styles.css"

[<RequireQualifiedAccess>]
type Url =
    | Index

let parseUrl (url: string list) =
    match url with
    | _ -> Url.Index

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
    | NavigateToProgram of int

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
    | NavigateToProgram programId -> state, Router.navigate(sprintf "/program/%i"  programId)


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


let renderCategoryPrograms (programs: DeferredCategoryPrograms) =
    let renderedPrograms =
        match programs with
        | HasNotStartedYet -> [ Html.none ]
        | InProgress -> [ spinner ]
        | Resolved(Ok programs) ->
            [ for p in programs -> Html.h2 [ prop.text p.title ] ]
        | Resolved(Error error) -> [ renderError error ]

    Html.div [ 
        prop.children renderedPrograms 
    ]

let renderSlide program index dispatch =
    PureReactCarousel.slide [ 
        slide.index index
        slide.children [ 
             Html.div [
                prop.children [
                    Html.a [
                        prop.onClick(fun _ -> dispatch (NavigateToProgram program.id))
                        prop.children [
                            Html.img [
                                prop.src program.image
                                prop.width 399
                                prop.height 225
                            ]
                        ]
                    ]

                ]
            ]
        ] 
    ]


let renderCategoryCarousel (programs: Program list) dispatch =
    let slideCount = programs.Length
    let visibleSlides = if slideCount >= 3 then 3 else slideCount
    let slides = 
        programs
        |> List.indexed 
        |> List.map (fun (i, p) -> renderSlide p i dispatch)

    Html.div [ 
        prop.children [ 
            PureReactCarousel.carouselProvider [ 
                carouselProvider.isIntrinsicHeight true
                carouselProvider.naturalSlideWidth 399
                carouselProvider.naturalSlideHeight 225
                carouselProvider.totalSlides slideCount
                carouselProvider.visibleSlides visibleSlides
                carouselProvider.children [ 
                    Html.div [
                        prop.className "container"
                        prop.children [

                            PureReactCarousel.slider [
                                slider.children [
                                    Html.div [
                                        prop.children slides
                                    ]                                
                                ] 
                            ]

                            PureReactCarousel.buttonBack [ 
                                prop.className "buttonBack"
                                buttonBack.children [
                                    Html.text "Back"
                                ] 
                            ]

                            PureReactCarousel.buttonNext [ 
                                prop.className "buttonNext"
                                buttonNext.children [ 
                                    Html.text "Next"
                                ] 
                            ] 
                            
                        ]                    
                    ]
                ] 
            ] 
        ] 
    ]

let renderCategoryContent (category: Category) dispatch =
    let programs =
        match category.programs with
        | HasNotStartedYet -> Html.none
        | InProgress -> spinner
        | Resolved(Ok programs) ->
            renderCategoryCarousel programs dispatch
        | Resolved(Error error) -> renderError error

    Html.div [ 
        prop.children [ 
            Html.h1 [ 
                prop.className "title"
                prop.text category.title
                prop.style [
                    style.color "#FFF"
                ]
            ]
            programs 
        ] 
    ]


let renderCategories (categories: DeferredCategories) dispatch=
    match categories with
    | HasNotStartedYet -> Html.none
    | InProgress -> spinner
    | Resolved(Error errorMsg) -> renderError errorMsg
    | Resolved(Ok categories) ->
        categories
        |> Map.toList
        |> List.map (fun (slug, category) -> renderCategoryContent category dispatch)
        |> Html.div

let render (state: State) (dispatch: Msg -> unit) =
    Html.div [ 
        prop.children [ 
            renderCategories state.categories dispatch
        ] 
    ]
