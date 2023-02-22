using Installment.States;
using Installment.Domain;
Citizen citizen1 = new Citizen()
{
    PassportID = "AB6550011",
    FirstName = "Anvar",
    LastName = "Sanayev",
    MidName = " Inom o'g'li",
    BirthDate = new(2000, 02, 22),
    Region = "Samarkand"
};

Taxpayer taxpayer1 = new()
{
    PassportID = citizen1.PassportID,
    socialStatus = SocialStatus.employed,
    Income = 10000000,
    Debt = 0
};

FederalInfo federalInfo1 = new()
{
    PassportID = citizen1.PassportID,
    IsConvicted = false,
    IsDivorced = true
};

Extentions.WriteData(citizen1,taxpayer1,federalInfo1);

Console.Write(" Enter your password ID :");
string? passwordID = Console.ReadLine();

Task task=Extentions.TotalScore(passwordID);