    #r "System.Configuration"
    #r "System.Data"
    #r "System.Linq"


using System;
using System.Data;
using System.Linq;

using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

 public static async Task Run(TimerInfo myTimer, TraceWriter log)
     {
         var str = ConfigurationManager.ConnectionStrings["Connect_DEV"].ConnectionString;
            
            using (SqlConnection conn = new SqlConnection(str))
            try
                {
                
            
                    int rowCount=1;

                    // Create a String to hold the query.
                   var query = "SELECT * from sys.firewall_rules WHERE name NOT IN ('AllowAllWindowsAzureIps')";
                    //var query = "select * FROM sys.firewall_rules fr WHERE fr.name LIKE '%@%'";

                    // Create a SqlCommand object and pass the constructor the connection string and the query string.
                    using ( SqlCommand queryCommand = new SqlCommand(query, conn))
                        {
                        conn.Open();
                   

                        // Use the above SqlCommand object to create a SqlDataReader object.
                        SqlDataReader queryCommandReader = queryCommand.ExecuteReader();

                        // Create a DataTable object to hold all the data returned by the query.
                      //  DataTable dataTable = new DataTable();

                        // Use the DataTable.Load(SqlDataReader) function to put the results of the query into a DataTable.
                       // dataTable.Load(queryCommandReader);

                        while (queryCommandReader.Read())  
                        {  
                    
                    //    for (int i = 1; i < dataTable.Rows.Count; i++)
                       // {
                         //   String rowText = string.Empty;
                        //
                          //      rowText += dataTable.Rows[i][1] ;
                          rowCount++;
                         String rowText= queryCommandReader.GetString(1);
                         var Delete = "EXEC sp_delete_firewall_rule N'"+rowText+"'";
                        //var Delete=  "EXEC sp_delete_firewall_rule N'Kaushik.Thakkar@gep.com'";  
                        // var text22 = "EXEC sp_delete_firewall_rule fr WHERE fr.name LIKE 'Kaushik.Thakkar@gep.com'";
                        using (SqlCommand cmd = new SqlCommand(Delete, conn))
                        {
                            // Execute the command and log the # row deleted.
                            var rows = await cmd.ExecuteNonQueryAsync();
                            log.Info($"{rowText} row deleted");
                        }
                        }
                         log.Info($"{rowCount} rows were deleted");
                            queryCommandReader.Close();
                    }
                }

                catch (Exception ex)
                    {
                            throw ex;
                    }
                finally
                    {

                        if (conn != null && conn.State != ConnectionState.Closed)
                        {
                            conn.Close();
                        }
                        
                    }

           
       
           } 
        
     