module Api

open Fable.Core
open Fable.SimpleHttp
open Thoth.Json


let apiUrl = "https://graphqladdi.spilari.ruv.is"

type GraphQLQuery =
    { operationName: string
      query: string
      variables: Map<string, string> }

type GraphQLResponse<'t> =
    { data: 't }

type Program =
    { id: int
      title: Option<string>
      description: Option<string>
      slug: Option<string>
      image: Option<string> }

type Category =
    { slug: Option<string>
      title: Option<string>
      programs: Option<Program list> }

type CategoryList =
    { categories: Category list }

type Categories =
    { Category: CategoryList }

let inline categoryListDecoder json = Decode.Auto.fromString<GraphQLResponse<Categories>> json

let inline executeQuery<'t> query decoder =
    async {
        let body = Encode.Auto.toString (4, query)
        let! response = Http.request apiUrl
                        |> Http.method POST
                        |> Http.content (BodyContent.Text body)
                        |> Http.header (Headers.contentType "application/json")
                        |> Http.header (Headers.referer "https://www.ruv.is/sjonvarp")
                        |> Http.header (Headers.origin "https://www.ruv.is")
                        |> Http.send
        match response.statusCode with
        | HttpOk ->
            match response.content with
            | ResponseContent.Text content ->
                match decoder content with
                | Ok(value) -> return Ok(value.data)
                | Error(e) -> return Error(e)
            | _ -> return Error("Unexpected response content")
        | HttpError -> return Error(response.responseText)
    }


let categoriesQuery() =
    let queryString = """{
            Category(station: tv) {
                categories {
                    slug
                    title
                }
            }
        }"""

    let query =
        { operationName = null
          query = queryString
          variables = Map.empty }
    executeQuery<CategoryList> query categoryListDecoder


let categoryPrograms category =
    async {
        let queryString = """query getCategoryPrograms($category: String!) {
            Category(station: tv, category: $category) {
                categories {
                    programs {
                        title
                        description: short_description
                        id
                        slug
                        image
                    }
                }
            }
        }"""
        let variables = Map.empty

        let query =
            { operationName = null
              query = queryString
              variables = variables.Add("category", category) }
        let! categoryList = executeQuery query categoryListDecoder
        match categoryList with
        | Ok(categoryList) ->
            if categoryList.Category.categories.Length <> 1 then
                return Error("Query for category programs did not return a single category")
            else
                match categoryList.Category.categories.[0].programs with
                | Some(programs) -> return Ok(programs)
                | None -> return Error("Category programs was null")
        | Error(error) -> return Error(error)
    }

let program programId =
    let queryString = """{
            Program(id: $programId) {
                slug
                title
                description
                foreign_title
                id
                image
                portrait_image
                episodes(limit: 1) {
                    title
                    id
                    description
                    firstrun
                    scope
                    rating
                    file_expires
                    file
                    clips {
                        time
                        text
                        slug
                    }
                    image
                    subtitles {
                        name
                        value
                    }
                }
                rest: episodes {
                    title
                    id
                    firstrun
                    description
                    image
                }
            }
        }"""
    let variables = Map.empty

    let query =
        { operationName = null
          query = queryString
          variables = variables.Add("episodeId", programId) }
    executeQuery query
