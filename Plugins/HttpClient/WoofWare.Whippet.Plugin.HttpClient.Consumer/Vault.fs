namespace ConsumePlugin

open System
open System.Collections.Generic
open System.Text.Json.Serialization
open System.Threading
open System.Threading.Tasks
open WoofWare.Whippet.Plugin.Json
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

[<HttpClient false>]
type IVaultClient =
    [<Get "v1/{mountPoint}/{path}">]
    abstract GetSecret :
        jwt : JwtVaultResponse *
        [<Path "path">] path : string *
        [<Path "mountPoint">] mountPoint : string *
        ?ct : CancellationToken ->
            Task<JwtSecretResponse>

    [<Get "v1/auth/jwt/login">]
    abstract GetJwt : role : string * jwt : string * ?ct : CancellationToken -> Task<JwtVaultResponse>

[<HttpClient false>]
type IVaultClientNonExtensionMethod =
    [<Get "v1/{mountPoint}/{path}">]
    abstract GetSecret :
        jwt : JwtVaultResponse *
        [<Path "path">] path : string *
        [<Path "mountPoint">] mountPoint : string *
        ?ct : CancellationToken ->
            Task<JwtSecretResponse>

    [<Get "v1/auth/jwt/login">]
    abstract GetJwt : role : string * jwt : string * ?ct : CancellationToken -> Task<JwtVaultResponse>

[<HttpClient(true)>]
type IVaultClientExtensionMethod =
    [<Get "v1/{mountPoint}/{path}">]
    abstract GetSecret :
        jwt : JwtVaultResponse *
        [<Path "path">] path : string *
        [<Path "mountPoint">] mountPoint : string *
        ?ct : CancellationToken ->
            Task<JwtSecretResponse>

    [<Get "v1/auth/jwt/login">]
    abstract GetJwt : role : string * jwt : string * ?ct : CancellationToken -> Task<JwtVaultResponse>

[<RequireQualifiedAccess>]
type VaultClientExtensionMethod =
    static member thisClashes = 99
