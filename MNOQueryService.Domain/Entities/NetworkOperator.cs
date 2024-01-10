using MNOQueryService.Domain.Interfaces;
using MNOQueryService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNOQueryService.Domain.Entities
{
    public class NetworkOperator : BaseEntity<int>
    {
        public NetworkOperator(string @operator, string operatorCode)
        {
            Operator = @operator;
            OperatorCode = operatorCode;
        }

        public NetworkOperator(int id, string @operator, string operatorCode)
            : this(@operator, operatorCode)
        {
            Id = id;
        }

        protected NetworkOperator()
        {
        }

        public int CountryId { get; protected set; }
        public string Operator { get; protected set; }
        public string OperatorCode { get; protected set; }
        public Country Country { get; protected set;}

    }
}
