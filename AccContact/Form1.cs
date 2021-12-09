using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AccContact
{
    public partial class Form1 : Form
    {
        DataSet ds;
        SqlDataAdapter adapter;
        SqlCommandBuilder commandBuilder;
        string ConnectionString = "Server=subjectstudio.online;Database=Acc_contracts;User Id=SA;Password=Astro305;";
        string viewSql = "SELECT * FROM Contracts";
        int counterparty;
        public Form1()
        {
            InitializeComponent();
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.AllowUserToAddRows = false;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(viewSql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["id_con"].ReadOnly = true;
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables[0].NewRow();
            ds.Tables[0].Rows.Add(row);
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(viewSql, connection);
                ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns["id_con"].ReadOnly = true;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                adapter = new SqlDataAdapter(viewSql, connection);
                commandBuilder = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = new SqlCommand("sp_CreateContracts", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@name_con", SqlDbType.NVarChar, 100, "Name"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@date_con", SqlDbType.Date, 100, "Date"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@type_con", SqlDbType.Int, 20, "Type"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@currency", SqlDbType.NVarChar, 20, "Currency"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@counterparty", SqlDbType.Int, 100, "Counterparty"));
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@summ", SqlDbType.Decimal, 100, "Summa"));

                adapter.Update(ds);
            }

        }

        private void reportButton_Click(object sender, EventArgs e)
        {
            ReportForm reportForm = new ReportForm();
            reportForm.ShowDialog();
            if (reportForm.post != 0)
            {
                counterparty = reportForm.post;
                string sqlPost = $"SELECT * FROM Contracts WHERE counterparty = {counterparty}";
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    adapter = new SqlDataAdapter(sqlPost, connection);
                    ds = new DataSet();
                    adapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                    dataGridView1.Columns["id_con"].ReadOnly = true;
                }
            }
        }
    }
}
