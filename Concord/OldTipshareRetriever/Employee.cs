using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldTipshareRetriever
{
    public class Employee
    {
        public string name;
        public double amount;
        public string id;
        public string date;

        public Employee(string name, string id, string date, double amount)
        {
            this.name = name;
            this.id = id;
            this.amount = amount;
            this.date = date;
        }
    }
}
