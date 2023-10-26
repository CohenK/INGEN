using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace InGen.Models
{
    [Serializable()]
    public class Company : ISerializable
    {
        private string name;
        private string address;
        private string city;
        private string state;
        private string zipCode;
        private double tax;

        public Company(string name = "not provided",
            string address = "not provided",
            string city = "not provided",
            string state = "",
            string zip = "",
            double tax = 0.0)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zip;
            Tax = tax;
        }

        public Company() : this("not provided", "not provided", "not provided", "", "", 0.0) { }


        public void UpdateCompany(string name, string address, string city, string state, string zipCode, double tax)
        {
            Name = name;
            Address = address;
            City = city;
            State = state;
            ZipCode = zipCode;
            Tax = tax;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", Name);
            info.AddValue("address", Address);
            info.AddValue("city", City);
            info.AddValue("state", State);
            info.AddValue("zipcode", ZipCode);
            info.AddValue("tax", Tax);
        }

        public Company(SerializationInfo info, StreamingContext context)
        {
            Name = (string)info.GetValue("name", typeof(string));
            Address = (string)info.GetValue("address", typeof(string));
            City = (string)info.GetValue("city", typeof(string));
            State = (string)info.GetValue("state", typeof(string));
            ZipCode = (string)info.GetValue("zipcode", typeof(string));
            Tax = (double)info.GetValue("tax", typeof(double));
        }

        #region variable properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }
        public double Tax
        {
            get { return tax; }
            set { tax = value; }
        }
        #endregion
    }
}
