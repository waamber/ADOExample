using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOExample.DataAccess
{
    class InvoiceQuery
    {
        public List<Invoice> GetInvoiceByTrackFirstLetter(string firstCharacter)
        {
            using (var connection = new SqlConnection("Server = (Local);Database=Chinook;Trusted_Connection=True;"))
            {
                connection.Open();

                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select 
                                    from invoice i.*
                                        join InvoiceLine x 
                                            on x.InvoiceId = i.InvoiceId
                                    where exists(select TrackId from Track 
                                                 where Name like @FirstLetter +'%' and TrackId = x.TrackId)";

                var firstLetterParam = new SqlParameter("@FirstLetter", SqlDbType.NVarChar);
                firstLetterParam.Value = firstCharacter;
                cmd.Parameters.Add(firstLetterParam);

                var reader = cmd.ExecuteReader();

                var invoices = new List<Invoice>();

                while (reader.Read()) //while reader.Read is true
                {
                    var invoice = new Invoice
                    {
                        InvoiceId = int.Parse(reader["InvoiceId"].ToString()),
                        CustomerId = int.Parse(reader["CustomerId"].ToString()),
                        BillingAddress = reader["BillingAddress"].ToString(),
                        InvoiceDate = DateTime.Parse(reader["InvoiceDate"].ToString()),
                        BillingCity = reader["BillingCity"].ToString(),
                        BillingCountry = reader["BillingCountry"].ToString(),
                        BillingPostalCode = reader["BillingPostalCode"].ToString(),
                        BillingState = reader["BillingState"].ToString(),
                        Total = double.Parse(reader["Total"].ToString())
                    };

                    invoices.Add(invoice);
                }

                return invoices;
            } //calls dispose() when the using statement is complete
        }
    }
}
