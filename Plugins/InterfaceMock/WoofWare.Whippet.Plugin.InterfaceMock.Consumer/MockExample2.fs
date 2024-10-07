namespace SomeNamespace

open System

type IPublicType =
    abstract Mem1 : string * int -> string list
    abstract Mem2 : string -> int
    abstract Mem3 : x : int * ?ct : System.Threading.CancellationToken -> string

type IPublicTypeInternalFalse =
    abstract Mem1 : string * int -> string list
    abstract Mem2 : string -> int
    abstract Mem3 : x : int * ?ct : System.Threading.CancellationToken -> string

type internal InternalType =
    abstract Mem1 : string * int -> unit
    abstract Mem2 : string -> int

type private PrivateType =
    abstract Mem1 : string * int -> unit
    abstract Mem2 : string -> int

type private PrivateTypeInternalFalse =
    abstract Mem1 : string * int -> unit
    abstract Mem2 : string -> int

type VeryPublicType<'a, 'b> =
    abstract Mem1 : 'a -> 'b

type Curried<'a> =
    abstract Mem1 : int -> 'a -> string
    abstract Mem2 : int * string -> 'a -> string
    abstract Mem3 : (int * string) -> 'a -> string
    abstract Mem4 : (int * string) -> ('a * int) -> string
    abstract Mem5 : x : int * string -> ('a * int) -> string
    abstract Mem6 : int * string -> y : 'a * int -> string

type TypeWithInterface =
    inherit IDisposable
    abstract Mem1 : string option -> string[] Async
    abstract Mem2 : unit -> string[] Async
