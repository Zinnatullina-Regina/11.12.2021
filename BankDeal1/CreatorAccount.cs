using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BankDeal
{
    class CreatorAccount
    {
        public static BankAmount CreateAccount(Dictionary<int, BankAmount> hashList)
        {
            BankAmount bankAmount = new BankAmount();
            hashList.Add(bankAmount.HashCode, bankAmount);
            return bankAmount;
        }

        public static void DeleteAccount(Dictionary<SHA256, BankAmount> hashList, Guid id)
        {
            foreach (var bankAmount in hashList)
            {
                if (bankAmount.Value.id == id)
                {
                    hashList.Remove(bankAmount.Key);
                    Console.WriteLine("Успешное удаление!");
                }
            }
        }

    }
}
