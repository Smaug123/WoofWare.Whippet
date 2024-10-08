namespace PureGym

open System
open System.Text.Json.Serialization
open WoofWare.Whippet.Plugin.Json

/// Module containing JSON serializing extension members for the Member type
[<AutoOpen>]
module MemberJsonSerializeExtension =
    /// Extension methods for JSON parsing
    type Member with

        /// Serialize to a JSON node
        static member toJsonNode (input : Member) : System.Text.Json.Nodes.JsonNode =
            let node = System.Text.Json.Nodes.JsonObject ()

            do
                node.Add ("id", (input.Id |> System.Text.Json.Nodes.JsonValue.Create<int>))

                node.Add (
                    "compoundMemberId",
                    (input.CompoundMemberId |> System.Text.Json.Nodes.JsonValue.Create<string>)
                )

                node.Add ("firstName", (input.FirstName |> System.Text.Json.Nodes.JsonValue.Create<string>))
                node.Add ("lastName", (input.LastName |> System.Text.Json.Nodes.JsonValue.Create<string>))
                node.Add ("homeGymId", (input.HomeGymId |> System.Text.Json.Nodes.JsonValue.Create<int>))
                node.Add ("homeGymName", (input.HomeGymName |> System.Text.Json.Nodes.JsonValue.Create<string>))
                node.Add ("emailAddress", (input.EmailAddress |> System.Text.Json.Nodes.JsonValue.Create<string>))
                node.Add ("gymAccessPin", (input.GymAccessPin |> System.Text.Json.Nodes.JsonValue.Create<string>))
                node.Add ("dateofBirth", (input.DateOfBirth |> System.Text.Json.Nodes.JsonValue.Create<DateOnly>))
                node.Add ("mobileNumber", (input.MobileNumber |> System.Text.Json.Nodes.JsonValue.Create<string>))
                node.Add ("postCode", (input.Postcode |> System.Text.Json.Nodes.JsonValue.Create<string>))
                node.Add ("membershipName", (input.MembershipName |> System.Text.Json.Nodes.JsonValue.Create<string>))
                node.Add ("membershipLevel", (input.MembershipLevel |> System.Text.Json.Nodes.JsonValue.Create<int>))
                node.Add ("suspendedReason", (input.SuspendedReason |> System.Text.Json.Nodes.JsonValue.Create<int>))
                node.Add ("memberStatus", (input.MemberStatus |> System.Text.Json.Nodes.JsonValue.Create<int>))

            node :> _

namespace PureGym

/// Module containing JSON parsing extension members for the GymOpeningHours type
[<AutoOpen>]
module GymOpeningHoursJsonParseExtension =
    /// Extension methods for JSON parsing
    type GymOpeningHours with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : GymOpeningHours =
            let arg_1 =
                (match node.["openingHours"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("openingHours")
                         )
                     )
                 | v -> v)
                    .AsArray ()
                |> Seq.map (fun elt -> elt.AsValue().GetValue<System.String> ())
                |> List.ofSeq

            let arg_0 =
                (match node.["isAlwaysOpen"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("isAlwaysOpen")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            {
                IsAlwaysOpen = arg_0
                OpeningHours = arg_1
            }
namespace PureGym

/// Module containing JSON parsing extension members for the GymAccessOptions type
[<AutoOpen>]
module GymAccessOptionsJsonParseExtension =
    /// Extension methods for JSON parsing
    type GymAccessOptions with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : GymAccessOptions =
            let arg_1 =
                (match node.["qrCodeAccess"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("qrCodeAccess")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            let arg_0 =
                (match node.["pinAccess"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("pinAccess")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            {
                PinAccess = arg_0
                QrCodeAccess = arg_1
            }
namespace PureGym

/// Module containing JSON parsing extension members for the GymLocation type
[<AutoOpen>]
module GymLocationJsonParseExtension =
    /// Extension methods for JSON parsing
    type GymLocation with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : GymLocation =
            let arg_1 =
                try
                    (match node.["latitude"] with
                     | null ->
                         raise (
                             System.Collections.Generic.KeyNotFoundException (
                                 sprintf "Required key '%s' not found on JSON object" ("latitude")
                             )
                         )
                     | v -> v)
                        .AsValue()
                        .GetValue<System.Double> ()
                with :? System.InvalidOperationException as exc ->
                    if exc.Message.Contains "cannot be converted to" then
                        if
                            System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
                        then
                            (match node.["latitude"] with
                             | null ->
                                 raise (
                                     System.Collections.Generic.KeyNotFoundException (
                                         sprintf "Required key '%s' not found on JSON object" ("latitude")
                                     )
                                 )
                             | v -> v)
                                .AsValue()
                                .GetValue<string> ()
                            |> System.Double.Parse
                        else
                            reraise ()
                    else
                        reraise ()
                |> LanguagePrimitives.FloatWithMeasure

            let arg_0 =
                try
                    (match node.["longitude"] with
                     | null ->
                         raise (
                             System.Collections.Generic.KeyNotFoundException (
                                 sprintf "Required key '%s' not found on JSON object" ("longitude")
                             )
                         )
                     | v -> v)
                        .AsValue()
                        .GetValue<System.Double> ()
                with :? System.InvalidOperationException as exc ->
                    if exc.Message.Contains "cannot be converted to" then
                        if
                            System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
                        then
                            (match node.["longitude"] with
                             | null ->
                                 raise (
                                     System.Collections.Generic.KeyNotFoundException (
                                         sprintf "Required key '%s' not found on JSON object" ("longitude")
                                     )
                                 )
                             | v -> v)
                                .AsValue()
                                .GetValue<string> ()
                            |> System.Double.Parse
                        else
                            reraise ()
                    else
                        reraise ()

            {
                Longitude = arg_0
                Latitude = arg_1
            }
namespace PureGym

/// Module containing JSON parsing extension members for the GymAddress type
[<AutoOpen>]
module GymAddressJsonParseExtension =
    /// Extension methods for JSON parsing
    type GymAddress with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : GymAddress =
            let arg_5 =
                (match node.["postcode"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("postcode")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_4 =
                match node.["county"] with
                | null -> None
                | v -> v.AsValue().GetValue<System.String> () |> Some

            let arg_3 =
                (match node.["town"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("town")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_2 =
                match node.["addressLine3"] with
                | null -> None
                | v -> v.AsValue().GetValue<System.String> () |> Some

            let arg_1 =
                match node.["addressLine2"] with
                | null -> None
                | v -> v.AsValue().GetValue<System.String> () |> Some

            let arg_0 =
                (match node.["addressLine1"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("addressLine1")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                AddressLine1 = arg_0
                AddressLine2 = arg_1
                AddressLine3 = arg_2
                Town = arg_3
                County = arg_4
                Postcode = arg_5
            }
namespace PureGym

/// Module containing JSON parsing extension members for the Gym type
[<AutoOpen>]
module GymJsonParseExtension =
    /// Extension methods for JSON parsing
    type Gym with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : Gym =
            let arg_10 =
                (match node.["reopenDate"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("reopenDate")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_9 =
                (match node.["timeZone"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("timeZone")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_8 =
                GymLocation.jsonParse (
                    match node.["location"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("location")
                            )
                        )
                    | v -> v
                )

            let arg_7 =
                GymAccessOptions.jsonParse (
                    match node.["accessOptions"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("accessOptions")
                            )
                        )
                    | v -> v
                )

            let arg_6 =
                GymOpeningHours.jsonParse (
                    match node.["gymOpeningHours"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("gymOpeningHours")
                            )
                        )
                    | v -> v
                )

            let arg_5 =
                (match node.["emailAddress"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("emailAddress")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_4 =
                (match node.["phoneNumber"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("phoneNumber")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_3 =
                GymAddress.jsonParse (
                    match node.["address"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("address")
                            )
                        )
                    | v -> v
                )

            let arg_2 =
                (match node.["status"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("status")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_1 =
                (match node.["id"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("id")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_0 =
                (match node.["name"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("name")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                Name = arg_0
                Id = arg_1
                Status = arg_2
                Address = arg_3
                PhoneNumber = arg_4
                EmailAddress = arg_5
                GymOpeningHours = arg_6
                AccessOptions = arg_7
                Location = arg_8
                TimeZone = arg_9
                ReopenDate = arg_10
            }
namespace PureGym

/// Module containing JSON parsing extension members for the Member type
[<AutoOpen>]
module MemberJsonParseExtension =
    /// Extension methods for JSON parsing
    type Member with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : Member =
            let arg_14 =
                (match node.["memberStatus"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("memberStatus")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_13 =
                (match node.["suspendedReason"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("suspendedReason")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_12 =
                (match node.["membershipLevel"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("membershipLevel")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_11 =
                (match node.["membershipName"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("membershipName")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_10 =
                (match node.["postCode"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("postCode")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_9 =
                (match node.["mobileNumber"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("mobileNumber")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_8 =
                (match node.["dateofBirth"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("dateofBirth")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<string> ()
                |> System.DateOnly.Parse

            let arg_7 =
                (match node.["gymAccessPin"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("gymAccessPin")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_6 =
                (match node.["emailAddress"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("emailAddress")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_5 =
                (match node.["homeGymName"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("homeGymName")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_4 =
                (match node.["homeGymId"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("homeGymId")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_3 =
                (match node.["lastName"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lastName")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_2 =
                (match node.["firstName"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("firstName")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_1 =
                (match node.["compoundMemberId"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("compoundMemberId")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_0 =
                (match node.["id"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("id")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            {
                Id = arg_0
                CompoundMemberId = arg_1
                FirstName = arg_2
                LastName = arg_3
                HomeGymId = arg_4
                HomeGymName = arg_5
                EmailAddress = arg_6
                GymAccessPin = arg_7
                DateOfBirth = arg_8
                MobileNumber = arg_9
                Postcode = arg_10
                MembershipName = arg_11
                MembershipLevel = arg_12
                SuspendedReason = arg_13
                MemberStatus = arg_14
            }
namespace PureGym

/// Module containing JSON parsing extension members for the GymAttendance type
[<AutoOpen>]
module GymAttendanceJsonParseExtension =
    /// Extension methods for JSON parsing
    type GymAttendance with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : GymAttendance =
            let arg_8 =
                (match node.["maximumCapacity"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("maximumCapacity")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_7 =
                (match node.["lastRefreshedPeopleInClasses"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lastRefreshedPeopleInClasses")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<string> ()
                |> System.DateTime.Parse

            let arg_6 =
                (match node.["lastRefreshed"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lastRefreshed")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<string> ()
                |> System.DateTime.Parse

            let arg_5 =
                (match node.["attendanceTime"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("attendanceTime")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<string> ()
                |> System.DateTime.Parse

            let arg_4 =
                (match node.["isApproximate"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("isApproximate")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            let arg_3 =
                match node.["totalPeopleSuffix"] with
                | null -> None
                | v -> v.AsValue().GetValue<System.String> () |> Some

            let arg_2 =
                (match node.["totalPeopleInClasses"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("totalPeopleInClasses")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_1 =
                (match node.["totalPeopleInGym"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("totalPeopleInGym")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_0 =
                (match node.["description"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("description")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                Description = arg_0
                TotalPeopleInGym = arg_1
                TotalPeopleInClasses = arg_2
                TotalPeopleSuffix = arg_3
                IsApproximate = arg_4
                AttendanceTime = arg_5
                LastRefreshed = arg_6
                LastRefreshedPeopleInClasses = arg_7
                MaximumCapacity = arg_8
            }
namespace PureGym

/// Module containing JSON parsing extension members for the MemberActivityDto type
[<AutoOpen>]
module MemberActivityDtoJsonParseExtension =
    /// Extension methods for JSON parsing
    type MemberActivityDto with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : MemberActivityDto =
            let arg_5 =
                (match node.["lastRefreshed"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lastRefreshed")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<string> ()
                |> System.DateTime.Parse

            let arg_4 =
                (match node.["isEstimated"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("isEstimated")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            let arg_3 =
                (match node.["totalClasses"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("totalClasses")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_2 =
                (match node.["totalVisits"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("totalVisits")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_1 =
                (match node.["averageDuration"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("averageDuration")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_0 =
                (match node.["totalDuration"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("totalDuration")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            {
                TotalDuration = arg_0
                AverageDuration = arg_1
                TotalVisits = arg_2
                TotalClasses = arg_3
                IsEstimated = arg_4
                LastRefreshed = arg_5
            }
namespace PureGym

/// Module containing JSON parsing extension members for the SessionsAggregate type
[<AutoOpen>]
module SessionsAggregateJsonParseExtension =
    /// Extension methods for JSON parsing
    type SessionsAggregate with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : SessionsAggregate =
            let arg_2 =
                (match node.["Duration"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("Duration")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_1 =
                (match node.["Visits"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("Visits")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_0 =
                (match node.["Activities"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("Activities")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            {
                Activities = arg_0
                Visits = arg_1
                Duration = arg_2
            }
namespace PureGym

/// Module containing JSON parsing extension members for the VisitGym type
[<AutoOpen>]
module VisitGymJsonParseExtension =
    /// Extension methods for JSON parsing
    type VisitGym with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : VisitGym =
            let arg_2 =
                (match node.["Status"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("Status")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_1 =
                (match node.["Name"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("Name")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_0 =
                (match node.["Id"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("Id")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            {
                Id = arg_0
                Name = arg_1
                Status = arg_2
            }
namespace PureGym

/// Module containing JSON parsing extension members for the Visit type
[<AutoOpen>]
module VisitJsonParseExtension =
    /// Extension methods for JSON parsing
    type Visit with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : Visit =
            let arg_3 =
                VisitGym.jsonParse (
                    match node.["Gym"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("Gym")
                            )
                        )
                    | v -> v
                )

            let arg_2 =
                (match node.["Duration"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("Duration")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_1 =
                (match node.["StartTime"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("StartTime")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<string> ()
                |> System.DateTime.Parse

            let arg_0 =
                (match node.["IsDurationEstimated"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("IsDurationEstimated")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            {
                IsDurationEstimated = arg_0
                StartTime = arg_1
                Duration = arg_2
                Gym = arg_3
            }
namespace PureGym

/// Module containing JSON parsing extension members for the SessionsSummary type
[<AutoOpen>]
module SessionsSummaryJsonParseExtension =
    /// Extension methods for JSON parsing
    type SessionsSummary with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : SessionsSummary =
            let arg_1 =
                SessionsAggregate.jsonParse (
                    match node.["ThisWeek"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("ThisWeek")
                            )
                        )
                    | v -> v
                )

            let arg_0 =
                SessionsAggregate.jsonParse (
                    match node.["Total"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("Total")
                            )
                        )
                    | v -> v
                )

            {
                Total = arg_0
                ThisWeek = arg_1
            }
namespace PureGym

/// Module containing JSON parsing extension members for the Sessions type
[<AutoOpen>]
module SessionsJsonParseExtension =
    /// Extension methods for JSON parsing
    type Sessions with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : Sessions =
            let arg_1 =
                (match node.["Visits"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("Visits")
                         )
                     )
                 | v -> v)
                    .AsArray ()
                |> Seq.map (fun elt -> Visit.jsonParse elt)
                |> List.ofSeq

            let arg_0 =
                SessionsSummary.jsonParse (
                    match node.["Summary"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("Summary")
                            )
                        )
                    | v -> v
                )

            {
                Summary = arg_0
                Visits = arg_1
            }
namespace PureGym

/// Module containing JSON parsing extension members for the UriThing type
[<AutoOpen>]
module UriThingJsonParseExtension =
    /// Extension methods for JSON parsing
    type UriThing with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : UriThing =
            let arg_0 =
                (match node.["someUri"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("someUri")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<string> ()
                |> System.Uri

            {
                SomeUri = arg_0
            }
