using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ZedCrestWalletApplication.Models
{
    [Table("CustomerAccountInformations")]
    public class CustomerAccountInformation
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AccountName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal CurrentAccountBalance { get; set; }
        public AccountType AccountType { get; set; }
        public string AccountNumberGenerated { get; set; }

        //we will also store the hash and salt of the Account Transaction pin
        [JsonIgnore]
        public byte[] PinHash { get; set; }
        [JsonIgnore]
        public byte[] PinSalt { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateLastUpdated { get; set; }

        // now to generate an accountnumber, let's do it in the constructor
        //first let's create a random obj
        Random rand = new Random();

        public CustomerAccountInformation()
        {
            AccountNumberGenerated = Convert.ToString((long) Math.Floor(rand.NextDouble() * 9_000_000_000L + 1_000_000_000L));
            AccountName = $"{FirstName} {LastName}";
        }
    }

    public enum AccountType
    {
        Savings,
        Current,
        Corporate,
        Government
    }
}
