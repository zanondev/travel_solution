using System;
using System.Xml.Linq;

namespace SerraLinhasAereas.Domain.Entities
{
    public class Clients
    {
        public int Id { get; set; }
        public string Cpf { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string CompleteName => DoCompleteName();
        public Address Address { get; set; }

        public Clients(int id, string cpf, string name, string nickname, Address address)
        {
            Id = id;
            Cpf = cpf;
            Name = name;
            Nickname = nickname;
            Address = address;
        }

        public Clients()
        {

        }

        public string DoCompleteName()
        {
            return $"{Name} {Nickname}";
        }


        public string ReturnCompleteAddress()
        {
            return $"Rua {Address.Street}, Nº {Address.Number}, {Address.Complement}, Bairro: {Address.District}, CEP: {Address.Cep}";
        }




    }
}
