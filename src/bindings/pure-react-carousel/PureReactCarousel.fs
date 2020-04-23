// ts2fable 0.7.1
namespace Feliz.PureReactCarousel


open Feliz
open Fable.Core
open Fable.Core.JsInterop
open Fable.React

[<Erase>]
type PureReactCarousel =
    static member inline carouselProvider (props: IReactProperty list) =
        Interop.reactApi.createElement (import "CarouselProvider" "pure-react-carousel", createObj !!props)
    static member inline slider (props: IReactProperty list) =
        Interop.reactApi.createElement (import "Slider" "pure-react-carousel", createObj !!props)
    static member inline slide (props: IReactProperty list) =
        Interop.reactApi.createElement (import "Slide" "pure-react-carousel", createObj !!props)
    static member inline buttonBack (props: IReactProperty list) =
        Interop.reactApi.createElement (import "ButtonBack" "pure-react-carousel", createObj !!props)
    static member inline buttonNext (props: IReactProperty list) =
        Interop.reactApi.createElement (import "ButtonNext" "pure-react-carousel", createObj !!props)

[<Erase>]
type carouselProvider =
    static member inline naturalSlideHeight (value: int) = Interop.mkAttr "naturalSlideHeight" value
    static member inline naturalSlideWidth (value: int) = Interop.mkAttr "naturalSlideWidth" value
    static member inline totalSlides (value: int) = Interop.mkAttr "totalSlides" value
    static member inline visibleSlides (value: int) = Interop.mkAttr "visibleSlides" value
    static member inline isIntrinsicHeight (value: bool) = Interop.mkAttr "isIntrinsicHeight" value
    static member inline children (elements: ReactElement seq) = prop.children elements

[<Erase>]
type slider =
    static member inline children (elements: ReactElement seq) = prop.children elements

[<Erase>]
type slide =
    static member inline index (value: int) = Interop.mkAttr "index" value
    static member inline children (elements: ReactElement seq) = prop.children elements

[<Erase>]
type buttonBack =
    static member inline children (elements: ReactElement seq) = prop.children elements

[<Erase>]
type buttonNext =
    static member inline children (elements: ReactElement seq) = prop.children elements


//     abstract children: ReactElement
//     abstract className: string option
//     abstract currentSlide: CarouselState option
//     abstract disableAnimation: CarouselState option
//     abstract disableKeyboard: CarouselState option
//     abstract hasMasterSpinner: CarouselState option
//     abstract interval: float option
//     abstract isPlaying: bool option
//     abstract lockOnWindowScroll: CarouselState option
//     abstract playDirection: CarouselProviderPropsPlayDirection option
//     abstract orientation: CarouselState option
//     abstract step: CarouselState option
//     abstract dragStep: CarouselState option
//     abstract tag: string option
//     abstract touchEnabled: CarouselState option
//     abstract dragEnabled: CarouselState option
//     abstract visibleSlides: CarouselState option
//     abstract infinite: CarouselState option
//     abstract isIntrinsicHeight: CarouselState option


// [<Import("Slider", "src")>]
// let Slider: SliderInterface = jsNative

// [<Import("Slide", "src")>]
// let Slide: SlideInterface = jsNative

// [<Import("ImageWithZoom", "src")>]
// let ImageWithZoom: ImageWithZoomInterface = jsNative

// [<Import("Image", "src")>]
// let Image: ImageInterface = jsNative

// [<Import("DotGroup", "src")>]
// let DotGroup: DotGroupInterface = jsNative

// [<Import("Dot", "src")>]
// let Dot: DotInterface = jsNative

// [<Import("ButtonNext", "src")>]
// let ButtonNext: ButtonNextInterface = jsNative

// [<Import("ButtonBack", "src")>]
// let ButtonBack: ButtonBackInterface = jsNative

// [<Import("ButtonLast", "src")>]
// let ButtonLast: ButtonLastInterface = jsNative

// [<Import("ButtonFirst", "src")>]
// let ButtonFirst: ButtonLastInterface = jsNative

// [<Import("ButtonPlay", "src")>]
// let ButtonPlay: ButtonPlayInterface = jsNative

// [<AllowNullLiteral>]
// type SliderProps =
//     abstract children: ReactElement
//     abstract className: string option
//     abstract classNameAnimation: string option
//     abstract classNameTray: string option
//     abstract classNameTrayWrap: string option
//     abstract moveThreshold: float option
//     abstract onMasterSpinner: (unit -> unit) option
//     abstract style: SliderPropsStyle option
//     abstract spinner: (unit -> unit) option
//     abstract trayTag: string option

// type SliderInterface = ReactElementType<SliderProps>

// [<AllowNullLiteral>]
// type SlideProps =
//     abstract className: string option
//     abstract classNameHidden: string option
//     abstract classNameVisible: string option
//     abstract index: float
//     abstract innerClassName: string option
//     abstract innerTag: string option
//     abstract onBlur: (unit -> unit) option
//     abstract onFocus: (unit -> unit) option
//     abstract tabIndex: float option
//     abstract tag: string option
//     abstract style: SliderPropsStyle option

// type SlideInterface = ReactElementType<SlideProps>

// [<AllowNullLiteral>]
// type ImageWithZoomProps =
//     abstract className: string option
//     abstract imageClassName: string option
//     abstract overlayClassName: string option
//     abstract src: string
//     abstract srcZoomed: string option
//     abstract tag: string option
//     abstract isPinchZoomEnabled: bool option

// type ImageWithZoomInterface = ReactElementType<ImageWithZoomProps>

// [<AllowNullLiteral>]
// type ImageProps =
//     abstract alt: string option
//     abstract children: ReactElement option
//     abstract className: string option
//     abstract hasMasterSpinner: bool
//     abstract isBgImage: bool option
//     abstract onError: (unit -> unit) option
//     abstract onLoad: (unit -> unit) option
//     abstract renderError: (unit -> unit) option
//     abstract renderLoading: (unit -> unit) option
//     abstract src: string
//     abstract style: ImagePropsStyle option
//     abstract tag: string option

// type ImageInterface = ReactElementType<ImageProps>

// [<AllowNullLiteral>]
// type RenderDotsProps =
//     abstract currentSlide: float option
//     abstract totalSlides: float option
//     abstract visibleSlides: float option
//     abstract disableActiveDots: bool option
//     abstract showAsSelectedForCurrentSlideOnly: bool option

// [<AllowNullLiteral>]
// type RenderDotsFunction =
//     [<Emit "$0($1...)">]
//     abstract Invoke: props:RenderDotsProps -> unit

// [<AllowNullLiteral>]
// type DotGroupProps =
//     abstract children: ReactElement option
//     abstract className: string option
//     abstract dotNumbers: bool option
//     abstract currentSlide: float option
//     abstract totalSlides: float option
//     abstract visibleSlides: float option
//     abstract disableActiveDots: bool option
//     abstract showAsSelectedForCurrentSlideOnly: bool option
//     abstract renderDots: RenderDotsFunction option

// type DotGroupInterface = ReactElementType<DotGroupProps>

// [<AllowNullLiteral>]
// type DotProps =
//     abstract children: ReactElement
//     abstract className: string option
//     abstract disabled: bool option
//     abstract onClick: (unit -> unit) option
//     abstract slide: float

// type DotInterface = ReactElementType<DotProps>

// [<AllowNullLiteral>]
// type ButtonNextProps =
//     abstract children: ReactElement
//     abstract className: string option
//     abstract disabled: bool option
//     abstract onClick: (Event<HTMLButtonElement> -> unit) option

// type ButtonNextInterface = ReactElementType<ButtonNextProps>

// [<AllowNullLiteral>]
// type ButtonBackProps =
//     abstract children: ReactElement
//     abstract className: string option
//     abstract disabled: bool option
//     abstract onClick: (Event<HTMLButtonElement> -> unit) option

// type ButtonBackInterface = ReactElementType<ButtonBackProps>

// [<AllowNullLiteral>]
// type ButtonLastProps =
//     abstract children: ReactElement
//     abstract className: string option
//     abstract disabled: bool option
//     abstract onClick: (Event<HTMLButtonElement> -> unit) option

// type ButtonLastInterface = ReactElementType<ButtonLastProps>

// [<AllowNullLiteral>]
// type ButtonFirstProps =
//     abstract children: ReactElement
//     abstract className: string option
//     abstract disabled: bool option
//     abstract onClick: (Event<HTMLButtonElement> -> unit) option

// type ButtonFirstInterface = ReactElementType<ButtonFirstProps>

// [<AllowNullLiteral>]
// type ButtonPlayProps =
//     abstract childrenPaused: ReactElement option
//     abstract childrenPlaying: ReactElement option
//     abstract disabled: bool option
//     abstract onClick: (Event<HTMLButtonElement> -> unit) option

// type ButtonPlayInterface = ReactElementType<ButtonPlayProps>

// [<AllowNullLiteral>]
// type SliderPropsStyle =
//     interface
//     end

// [<AllowNullLiteral>]
// type ImagePropsStyle =
//     [<Emit "$0[$1]{{=$2}}">]
//     abstract Item: key:string -> string

// [<Import("CarouselContext", "src")>]
// let CarouselContext: IContext<CarouselStoreInterface> = jsNative

// [<Import("CarouselProvider", "src")>]
// let CarouselProvider: CarouselProviderInterface = jsNative

// // [<Import("WithStore", "src")>]
// // let WithStore: WithStoreInterface = jsNative

// [<AllowNullLiteral>]
// type CarouselState =
//     abstract currentSlide: float
//     abstract disableAnimation: bool
//     abstract disableKeyboard: bool
//     abstract hasMasterSpinner: bool
//     abstract imageErrorCount: float
//     abstract imageSuccessCount: float
//     abstract lockOnWindowScroll: bool
//     abstract masterSpinnerFinished: bool
//     abstract masterSpinnerThreshold: float
//     abstract naturalSlideHeight: float
//     abstract naturalSlideWidth: float
//     abstract orientation: CarouselStateOrientation
//     abstract slideSize: float
//     abstract slideTraySize: float
//     abstract step: float
//     abstract dragStep: float
//     abstract totalSlides: float
//     abstract touchEnabled: bool
//     abstract dragEnabled: bool
//     abstract visibleSlides: float
//     abstract infinite: bool
//     abstract isIntrinsicHeight: bool

// [<AllowNullLiteral>]
// type CarouselStoreInterface =
//     abstract state: CarouselState
//     abstract setStoreState: (obj -> unit) with get
//     abstract getStoreState: (unit -> CarouselState) with get
//     abstract subscribe: ((unit -> unit) -> unit) with get
//     abstract unsubscribe: ((unit -> unit) -> unit) with get
//     abstract updateSubscribers: ((CarouselState -> unit) -> unit) with get
//     abstract subscribeMasterSpinner: (string -> unit) with get
//     abstract unsubscribeMasterSpinner: (string -> obj) with get
//     abstract unsubscribeAllMasterSpinner: (unit -> unit) with get
//     abstract masterSpinnerSuccess: (string -> unit) with get
//     abstract masterSpinnerError: (string -> unit) with get
//     abstract setMasterSpinnerFinished: (unit -> unit) with get
//     abstract isMasterSpinnerFinished: (unit -> bool) with get

// [<AllowNullLiteral>]
// type CarouselProviderProps =
//     abstract children: ReactElement
//     abstract className: string option
//     abstract currentSlide: CarouselState option
//     abstract disableAnimation: CarouselState option
//     abstract disableKeyboard: CarouselState option
//     abstract hasMasterSpinner: CarouselState option
//     abstract interval: float option
//     abstract isPlaying: bool option
//     abstract lockOnWindowScroll: CarouselState option
//     abstract naturalSlideHeight: CarouselState
//     abstract naturalSlideWidth: CarouselState
//     abstract playDirection: CarouselProviderPropsPlayDirection option
//     abstract orientation: CarouselState option
//     abstract step: CarouselState option
//     abstract dragStep: CarouselState option
//     abstract tag: string option
//     abstract totalSlides: CarouselState
//     abstract touchEnabled: CarouselState option
//     abstract dragEnabled: CarouselState option
//     abstract visibleSlides: CarouselState option
//     abstract infinite: CarouselState option
//     abstract isIntrinsicHeight: CarouselState option

// type CarouselProviderInterface = ReactElementType<CarouselProviderProps>

// [<AllowNullLiteral>]
// type CarouselInjectedProps =
//     abstract carouselStore: CarouselStoreInterface

// [<AllowNullLiteral>]
// type Diff<'T, 'U> =
//     interface
//     end

// // type Omit<'T, 'K> = obj

// [<AllowNullLiteral>]
// type MapStateToProps<'TStateProps> =
//     [<Emit "$0($1...)">]
//     abstract Invoke: state:CarouselState -> 'TStateProps

// // [<AllowNullLiteral>]
// // type WithStoreInterface =

// //     [<Emit "$0($1...)">]
// //     abstract Invoke: ``component``:ReactElementType<CustomProps> -> obj

// //     [<Emit "$0($1...)">]
// //     abstract Invoke: ``component``:ReactElementType<obj> * state:MapStateToProps<CustomStateProps> -> obj

// [<StringEnum>]
// [<RequireQualifiedAccess>]
// type CarouselStateOrientation =
//     | Horizontal
//     | Vertical

// [<StringEnum>]
// [<RequireQualifiedAccess>]
// type CarouselProviderPropsPlayDirection =
//     | Forward
//     | Backward
