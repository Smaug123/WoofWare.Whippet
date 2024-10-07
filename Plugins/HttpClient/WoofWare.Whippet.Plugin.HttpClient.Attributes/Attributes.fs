namespace WoofWare.Whippet.Plugin.HttpClient

/// Attribute indicating a record type to which the "create HTTP client" Myriad
/// generator should apply during build.
/// This generator is intended to replicate much of the functionality of RestEase,
/// i.e. to stamp out HTTP REST clients from interfaces defining the API.
///
/// If you supply isExtensionMethod = false, you will get a genuine module (which can
/// be consumed from C#) rather than extension methods.
type HttpClientAttribute (isExtensionMethod : bool) =
    inherit System.Attribute ()
    /// The default value of `isExtensionMethod`, the optional argument to the HttpClientAttribute constructor.
    static member DefaultIsExtensionMethod = true

    /// Shorthand for the "isExtensionMethod = false" constructor; see documentation there for details.
    new () = HttpClientAttribute HttpClientAttribute.DefaultIsExtensionMethod
