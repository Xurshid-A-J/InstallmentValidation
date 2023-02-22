using Installment.States;

Console.Write(" Enter your password ID :");
string? passwordID = Console.ReadLine();

Task Task = new(Extentions<>.TotalScore(passwordID));