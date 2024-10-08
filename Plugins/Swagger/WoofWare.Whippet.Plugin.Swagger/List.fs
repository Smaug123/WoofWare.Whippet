namespace WoofWare.Whippet.Plugin.Swagger

[<RequireQualifiedAccess>]
module internal List =

    let allSome<'a> (l : 'a option list) : 'a list option =
        let rec go acc (l : 'a option list) =
            match l with
            | [] -> Some (List.rev acc)
            | None :: _ -> None
            | Some head :: tail -> go (head :: acc) tail

        go [] l
