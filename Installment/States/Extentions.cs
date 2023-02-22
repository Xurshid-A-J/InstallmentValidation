using System;
using Installment.Domain;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Installment.States
{
	public static class Extentions
	{ 
		//private static List<Citizen> citizens = new();
        public static ReaderWriterLockSlim rwLockSlim = new();
		private static readonly string path = "../../../DataBase";

        private static bool isValidCitizen(string passportID)
		{
            rwLockSlim.EnterReadLock();
			try
			{
				if (File.Exists(path + "/CitizenData.json"))
				{
					List<Citizen> citizens = GetAllCitizens();
					return citizens.Any(x => x.PassportID == passportID);
				}
				else return false;
			}
            finally
            {
                rwLockSlim.ExitReadLock();
            }
        }

		private static List<Citizen> GetAllCitizens()
		{
			rwLockSlim.EnterReadLock();
			try
			{
				string citizenJSON = File.ReadAllText(path + "/CitizenData.json");
				return JsonConvert.DeserializeObject<List<Citizen>>(citizenJSON);
			}
			finally
			{
				rwLockSlim.ExitReadLock();
			}
		}

		private static Taxpayer GetTaxpayer(string passportID)
		{ 
			if (!isValidCitizen(passportID))  return null;
			List<Taxpayer>? taxpayers = JsonConvert.DeserializeObject<List<Taxpayer>>(File.ReadAllText(path + "/TaxpayerData.json"));
			
			return taxpayers.Find(x => x.PassportID == passportID);
			
        }

		private static FederalInfo GetFederalInfo(string passportID)
		{
            if (!isValidCitizen(passportID)) return null;
            List<FederalInfo>? federalInfos= JsonConvert.DeserializeObject<List<FederalInfo>>(File.ReadAllText(path + "/FederalInfoData.json"));

			return federalInfos.Find(x => x.PassportID == passportID);
        }


		private  static int TaxpayerScoring(string passportID)
		{
			var taxpayer = GetTaxpayer(passportID);
			int taxpayerScore = 0;
			
			if (taxpayer.Debt == 0) taxpayerScore += 35;
			else if (taxpayer.Debt < 2 * taxpayer.Income) taxpayerScore += 25;
			else if (taxpayer.Debt < 4 * taxpayer.Income) taxpayerScore += 15;
			if (taxpayer.socialStatus != SocialStatus.unemployed) taxpayerScore += 35;

                return taxpayerScore;
		}

		private  static int FederalInfoScoring(string passportID)
		{
			var federal = GetFederalInfo(passportID);
			int federalScore = 30;
			if (federal.IsConvicted) federalScore -= 10;
			if (federal.IsDivorced) federalScore -= 10;

			return federalScore;
		}


		public static void WriteData(Citizen citizen, Taxpayer taxpayer, FederalInfo federal)
		{
			rwLockSlim.EnterWriteLock();
			try
			{
				if ( GetAllCitizens().Any(x=> x.PassportID==citizen.PassportID))
				{
					throw new("No need to add again , Citizen data already exits ! ");
				}
				File.AppendAllText(
					path + "/CitizenData.json", JsonConvert.SerializeObject(citizen, Formatting.Indented));

				File.AppendAllText(
					path + "/FederalInfoData.json", JsonConvert.SerializeObject(federal, Formatting.Indented));

				File.AppendAllText(
					path + "/TaxpayerData.json", JsonConvert.SerializeObject(taxpayer, Formatting.Indented));
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
			finally
			{
				rwLockSlim.ExitWriteLock();
			}

		}

        public async static Task<Task> TotalScore(string passwordID)
		{
			int totalScore = 0;
			//Task task3= Task.Run (() =>  totalScore+= TaxpayerScoring(passwordID) );
            //Task task4= Task.Run (() => totalScore+= FederalInfoScoring(passwordID));
			//Task.WaitAll(task1, task2);

			Task task1= new(() =>  totalScore+= TaxpayerScoring(passwordID) );
            Task task2= new(() => totalScore+= FederalInfoScoring(passwordID));

			task1.Start();
			task2.Start();
			Task<Task>.WaitAll(task1, task2);

			Task result = new(
				() =>
				{
					decimal amount = GetTaxpayer(passwordID).Income * 10 * totalScore / 100;

					if (totalScore > 50) Console.WriteLine($" The user with ID {passwordID}" +
							$" can take any item with installment up to {amount} sums !!! ");

					else Console.WriteLine(" Sorry,You are not eligible to take any item for installment");
				}
				);
		return result;

		}
    }
}

