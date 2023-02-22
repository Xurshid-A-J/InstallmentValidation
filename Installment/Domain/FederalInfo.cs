using System;
using Installment.Interfaces;

namespace Installment.Domain
{
    public class FederalInfo : IPassportID
    {
        public required string PassportID { get; init; } = string.Empty;

        public bool IsConvicted { get; set; }

        public bool IsDivorced { get; set; }

    }
}

