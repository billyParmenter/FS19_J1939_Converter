using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace FAST_UI
{
    class DALManager
    {
        //conection string taken from App.config
        static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        static SqlCommand command;
        static SqlDataAdapter adapter;

        public static DataTable GetSPNInfo(int spn)
        {
            DataTable result = new DataTable();
            
            //build command with this connection
            command = new SqlCommand("GetSPNInfo", conn);

            //what type of command is this?
            command.CommandType = CommandType.StoredProcedure;

            //what are the parameters and its values?
            command.Parameters.Add("@SPN", SqlDbType.Int).Value = spn;
            try
            {
                conn.Open();
            }
            catch(Exception e)
            {
                throw e;
            }

            //adapt results and put it into a dataTable
            adapter = new SqlDataAdapter(command);
            adapter.Fill(result);


            //close connection, ditch the adapter
            conn.Close();
            adapter.Dispose();
            return result;
        }

        internal static void GetSPN(ref SPN spn)
        {
            DataTable table = GetSPNInfo(spn.SpnNumber);
            string[] elements = System.Text.RegularExpressions.Regex.Split(table.Rows[0]["SPN Position"].ToString(), @"-|\.");
            spn.Position = int.Parse(elements[0]);
            spn.SpnLength = new SPNLength(table.Rows[0]["SPN Length"].ToString());
            spn.pgnResolution = new ResolutionRatio(table.Rows[0]["Resolution"].ToString());
        }
    }
}
