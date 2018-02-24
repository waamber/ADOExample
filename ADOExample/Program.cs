using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace ADOExample
{
    class Program
    {
        static void Main(string[] args)


        {
            var firstLetter = Console.ReadLine();

            using (var connection = new SqlConnection("Server = (Local);Database=Chinook;Trusted_Connection=True;"))
            {
                connection.Open();

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select x.invoiceId, billingaddress
                                    from invoice i
                                        join InvoiceLine x 
                                            on x.InvoiceId = i.InvoiceId
                                    where exists(select TrackId from Track 
                                                 where Name like @FirstLetter +'%' and TrackId = x.TrackId)";

                var firstLetterParam = new SqlParameter("@FirstLetter", SqlDbType.NVarChar);
                firstLetterParam.Value = firstLetter;
                cmd.Parameters.Add(firstLetterParam);

                var reader = cmd.ExecuteReader();

                while (reader.Read()) //while reader.Read is true
                {
                    var invoiceId = reader.GetInt32(0);
                    var billingaddress = reader["BillingAddress"].ToString();

                    Console.WriteLine($"Invoice {invoiceId} is going to be {billingaddress}.");
                } 
            } //calls dispose() when the using statement is complete
          
            Console.ReadLine();
        }
    }
}
