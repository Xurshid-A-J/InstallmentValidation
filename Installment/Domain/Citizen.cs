using System;
using Installment.Interfaces;

namespace Installment.Domain
{
	public class Citizen: IPassportID
    {
		public required string PassportID { get; init; } = string.Empty;

		public required string FirstName { get; init; } = string.Empty;

		public required string LastName { get; init; } = string.Empty;

		public required string MidName { get; init; } = string.Empty;

		public required DateOnly BirthDate { get; init; } = default;

		public required string Region { get; init; } = string.Empty;


	}

}

