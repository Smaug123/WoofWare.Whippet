// Copied from https://gitea.patrickstevens.co.uk/patrick/puregym-unofficial-dotnet/src/commit/2741c5e36cf0bdb203b12b78a8062e25af9d89c7/PureGym/Api.fs

namespace PureGym

open System
open System.Text.Json.Serialization
open WoofWare.Whippet.Plugin.Json

[<JsonParse>]
type GymOpeningHours =
    {
        IsAlwaysOpen : bool
        OpeningHours : string list
    }

[<JsonParse>]
type GymAccessOptions =
    {
        PinAccess : bool
        QrCodeAccess : bool
    }

[<Measure>]
type measure

[<JsonParse>]
type GymLocation =
    {
        [<JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)>]
        Longitude : float
        [<JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)>]
        Latitude : float<measure>
    }

[<JsonParse>]
type GymAddress =
    {
        [<JsonRequired>]
        AddressLine1 : string
        AddressLine2 : string option
        AddressLine3 : string option
        [<JsonRequired>]
        Town : string
        County : string option
        [<JsonRequired>]
        Postcode : string
    }

[<JsonParse>]
type Gym =
    {
        [<JsonRequired>]
        Name : string
        [<JsonRequired>]
        Id : int
        [<JsonRequired>]
        Status : int
        [<JsonRequired>]
        Address : GymAddress
        [<JsonRequired>]
        PhoneNumber : string
        [<JsonRequired>]
        EmailAddress : string
        [<JsonRequired>]
        GymOpeningHours : GymOpeningHours
        [<JsonRequired>]
        AccessOptions : GymAccessOptions
        [<JsonRequired>]
        Location : GymLocation
        [<JsonRequired>]
        TimeZone : string
        ReopenDate : string
    }

[<JsonParse true>]
[<JsonSerialize true>]
type Member =
    {
        Id : int
        CompoundMemberId : string
        FirstName : string
        LastName : string
        HomeGymId : int
        HomeGymName : string
        EmailAddress : string
        GymAccessPin : string
        [<JsonPropertyName "dateofBirth">]
        DateOfBirth : DateOnly
        MobileNumber : string
        [<JsonPropertyName "postCode">]
        Postcode : string
        MembershipName : string
        MembershipLevel : int
        SuspendedReason : int
        MemberStatus : int
    }

[<JsonParse>]
type GymAttendance =
    {
        [<JsonRequired>]
        Description : string
        [<JsonRequired>]
        TotalPeopleInGym : int
        [<JsonRequired>]
        TotalPeopleInClasses : int
        TotalPeopleSuffix : string option
        [<JsonRequired>]
        IsApproximate : bool
        AttendanceTime : DateTime
        LastRefreshed : DateTime
        LastRefreshedPeopleInClasses : DateTime
        MaximumCapacity : int
    }

[<JsonParse>]
type MemberActivityDto =
    {
        [<JsonRequired>]
        TotalDuration : int
        [<JsonRequired>]
        AverageDuration : int
        [<JsonRequired>]
        TotalVisits : int
        [<JsonRequired>]
        TotalClasses : int
        [<JsonRequired>]
        IsEstimated : bool
        [<JsonRequired>]
        LastRefreshed : DateTime
    }

[<JsonParse>]
type SessionsAggregate =
    {
        [<JsonPropertyName "Activities">]
        Activities : int
        [<JsonPropertyName "Visits">]
        Visits : int
        [<JsonPropertyName "Duration">]
        Duration : int
    }

[<JsonParse>]
type VisitGym =
    {
        [<JsonPropertyName "Id">]
        Id : int
        [<JsonPropertyName "Name">]
        Name : string
        [<JsonPropertyName "Status">]
        Status : string
    }

[<JsonParse>]
type Visit =
    {
        [<JsonPropertyName "IsDurationEstimated">]
        IsDurationEstimated : bool
        [<JsonPropertyName "StartTime">]
        StartTime : DateTime
        [<JsonPropertyName "Duration">]
        Duration : int
        [<JsonPropertyName "Gym">]
        Gym : VisitGym
    }

[<JsonParse>]
type SessionsSummary =
    {
        [<JsonPropertyName "Total">]
        Total : SessionsAggregate
        [<JsonPropertyName "ThisWeek">]
        ThisWeek : SessionsAggregate
    }

[<JsonParse>]
type Sessions =
    {
        [<JsonPropertyName "Summary">]
        Summary : SessionsSummary
        [<JsonPropertyName "Visits">]
        Visits : Visit list
    }

[<JsonParse>]
type UriThing =
    {
        SomeUri : Uri
    }
