using System.Runtime.Serialization;

namespace TEC.FullService.Shared.Common;

public enum CertificateStatus
{
    //[EnumMember(Value = "Aktiv")]
    Active,

    //[EnumMember(Value = "Udløber Snart")]
    ExpiringSoon,

    //[EnumMember(Value = "Udløbet")]
    Expired,

    //[EnumMember(Value = "Fornyet")]
    Renewed,

    //[EnumMember(Value = "Ugyldig")]
    Invalid
}

public enum FundApplicationStatus
{
    [EnumMember(Value = "Afventer Handling")]
    PendingHandling,
    [EnumMember(Value = "Søgt")]
    Sought,
    [EnumMember(Value = "Ikke Relevant")]
    NotRelevant,
    [EnumMember(Value = "Godkendt")]
    Approved,
    [EnumMember(Value = "Afvist")]
    Rejected
}

public enum EducationStatus
{
    [EnumMember(Value = "Faglært")]
    Skilled,
    [EnumMember(Value = "Ufaglært")]
    Unskilled,
    [EnumMember(Value = "Andet")]
    Other
}

public enum EnrollmentStatus
{
    [EnumMember(Value = "Afventer Godkendelse")]
    PendingApproval,
    [EnumMember(Value = "Godkendt")]
    Approved,
    [EnumMember(Value = "Annulleret")]
    Cancelled,
    [EnumMember(Value = "Gennemført")]
    Completed
}
