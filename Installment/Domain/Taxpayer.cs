using System;
using Installment.Interfaces;
using Installment.States;

namespace Installment.Domain
{
	public class Taxpayer:IPassportID
	{
        public required string PassportID { get; init; } = string.Empty;

        public required SocialStatus socialStatus { get; set; } = 0;

        public decimal Income { get; set; }

        public decimal Debt { get; set; }


    }
}

