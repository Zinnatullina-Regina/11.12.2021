﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BankDeal
{
    class BankAmount
    {

        public int HashCode { get; set; }
        private Guid guid;
        private decimal balanceCurrent;
        private decimal balanceSave;
        private BankAccountType bankType;
        private Queue<BankTransaction> bankTransactions = new Queue<BankTransaction>();

        public enum BankAccountType
        {
            Save = 1,
            Current
        }

        public static bool operator ==(BankAmount bankAmount1, BankAmount bankAmount2)
        {
            return bankAmount1.guid == bankAmount2.guid && bankAmount2.GetHashCode() == bankAmount1.GetHashCode();
        }
        public static bool operator !=(BankAmount bankAmount1, BankAmount bankAmount2)
        {
            return bankAmount1.guid != bankAmount2.guid;
        }

        public void GetOnBalance(decimal money)
        {
            bool flag = this.bankType == BankAccountType.Save;
            if (flag)
            {
                bool flag2 = money > 0;
                if (flag2)
                {
                    bankTransactions.Enqueue(new BankTransaction(money, BankTransaction.TypeTransaction.Replenishment));
                    this.balanceSave += money;
                }
                else
                {
                    Console.WriteLine("Вы не можете положить отрицательное значение денег");
                }
                Console.WriteLine("Текущий баланс " + this.balanceSave.ToString());
            }
            else
            {
                bool flag3 = money > 0;
                if (flag3)
                {
                    this.balanceCurrent += money;
                    bankTransactions.Enqueue(new BankTransaction(money, BankTransaction.TypeTransaction.Replenishment));
                }
                else
                {
                    Console.WriteLine("Вы не можете положить отрицательное значение денег");
                }
                Console.WriteLine("Текущий баланс " + this.balanceCurrent.ToString());
            }
        }
        public void GetFromBalance(decimal money)
        {
            bool flag = this.bankType == BankAccountType.Save;
            if (flag)
            {
                bool flag2 = money <= this.balanceSave && money > 0;
                if (flag2)
                {
                    this.balanceSave -= money;
                    bankTransactions.Enqueue(new BankTransaction(money, BankTransaction.TypeTransaction.Withdrawal));
                }
                else
                {
                    Console.WriteLine("Вы не можете снять отрицательное значение денег/недостаточный баланс");
                }
                Console.WriteLine("Текущий баланс " + this.balanceSave.ToString());
            }
            else
            {
                bool flag3 = money <= this.balanceCurrent && money > 0;
                if (flag3)
                {
                    bankTransactions.Enqueue(new BankTransaction(money, BankTransaction.TypeTransaction.Withdrawal));
                    this.balanceCurrent -= money;
                }
                else
                {
                    Console.WriteLine("Вы не можете снять отрицательное значение денег/недостаточный баланс");
                }
                Console.WriteLine("Текущий баланс " + this.balanceCurrent.ToString());
            }
        }
        public void SwapBankTypes()
        {
            bool flag = this.bankType == BankAccountType.Save;
            if (flag)
            {
                this.bankType = BankAccountType.Current;
            }
            else
            {
                this.bankType = BankAccountType.Save;
            }
            Console.WriteLine("Текущий счет - " + this.bankType.ToString());
        }

        public void SetTypeBank(string type)
        {
            bool flag = type.ToLower().Equals("сберегательный");
            if (flag)
            {
                this.bankType = BankAccountType.Save;
            }
            else
            {
                this.bankType = BankAccountType.Current;
            }
        }


       
        public void Transaction(decimal summ)
        {
            if (this.bankType == BankAccountType.Current)
            {
                if (balanceCurrent - summ >= 0 && summ > 0)
                {
                    balanceCurrent -= summ;
                    balanceSave += summ;
                    bankTransactions.Enqueue(new BankTransaction(summ, BankTransaction.TypeTransaction.Transfer));
                }
                else
                {
                    Console.WriteLine("Операция невозможна");
                }

            }
            else
            {
                if (balanceSave - summ >= 0 && summ > 0)
                {
                    balanceCurrent += summ;
                    balanceSave -= summ;
                    bankTransactions.Enqueue(new BankTransaction(summ, BankTransaction.TypeTransaction.Transfer));
                }
                else
                {
                    Console.WriteLine("Операция невозможна");
                }
            }
        }

        public static void Dispose(BankAmount bankAmount)
        {
            for (int i = 0; i < bankAmount.bankTransactions.Count; i++)
            {
                string info = GetInfoAboutTransaction(bankAmount.bankTransactions.Dequeue());

                File.AppendAllText("transaction.txt", info + "\n");

            }
            GC.SuppressFinalize(bankAmount);
            Console.WriteLine("Информация о всех транзакциях записана в файл transaction.txt");
        }
        public void CheckType()
        {
            Console.WriteLine("Текущий тип банковского счета " + this.bankType.ToString());
        }
        public static string GetInfoAboutTransaction(BankTransaction bankTrans)
        {
            return $" Время выполнения операции {bankTrans.dateTime}; сумма перевода {bankTrans.summ} ; тип операции {bankTrans.typeTransaction}";
        }

        public override bool Equals(object obj)
        {
            return obj is BankAmount amount && id.Equals(amount.id) && HashCode.Equals(amount.HashCode) && guid.Equals(amount.guid);
        }

        internal BankAmount()
        {
            guid = Guid.NewGuid();
            balanceCurrent = 0;
            balanceSave = 0;
            HashCode = SHA256.Create().GetHashCode();
        }

        public override string ToString()
        {
            return $"Баланс текущего счета {balanceCurrent},Баланс текущего счета {balanceSave}";
        }

        public override int GetHashCode()
        {
            return HashCode;
        }

        public Guid id
        {
            get { return guid; }
        }

        [Conditional("DEBUG")]
        public void DumpToScreen()
        {
            bool flag = this.bankType == BankAccountType.Save;
            if (flag)
            {
                Console.WriteLine("Текущий баланс " + this.balanceSave.ToString() + "id" + id);
            }
            else
            {
                Console.WriteLine("Текущий баланс " + this.balanceCurrent.ToString() + "id" + id);
            }
        }




       
    }
}
