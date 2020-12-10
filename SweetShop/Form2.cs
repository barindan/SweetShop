using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SweetShop
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(string orderNumber)
        {
            InitializeComponent();
            label2.Text = orderNumber;
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
            
            try {
                using (SqlConnection connect = new SqlConnection(stringConnect))
                {
                    connect.Open();
                    string query = $"EXEC GetOrderInfo'{orderNumber}'";
                    SqlCommand command = new SqlCommand(query, connect);
                    SqlDataReader reader = command.ExecuteReader();
                    List<string[]> data = new List<string[]>();
                    while (reader.Read())
                    {
                        data.Add(new string[2]);

                        data[data.Count - 1][0] = reader[0].ToString();
                        data[data.Count - 1][1] = reader[1].ToString();

                    }
                    reader.Close();

                    string sqlExpression = "GetAmountOrder";
                    SqlCommand getAmount = new SqlCommand(sqlExpression, connect);
                    getAmount.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter paramId = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = int.Parse(orderNumber)
                    };
                    getAmount.Parameters.Add(paramId);
                    var returnValue = getAmount.Parameters.Add("@Return", SqlDbType.Int);
                    returnValue.Direction = ParameterDirection.ReturnValue;
                    getAmount.ExecuteNonQuery();
                    var resultSumAmount = (returnValue.Value).ToString();
                    textBox1.Text = resultSumAmount;

                    string sqlCommand = "GetOrderStatus";
                    SqlCommand getOrderStatus = new SqlCommand(sqlCommand, connect);
                    getOrderStatus.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlParameter paramStatusId = new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = int.Parse(orderNumber)
                    };
                    getOrderStatus.Parameters.Add(paramStatusId);
                    var returnValueStatus = getOrderStatus.Parameters.Add("@Return", SqlDbType.Int);
                    returnValueStatus.Direction = ParameterDirection.ReturnValue;
                    getOrderStatus.ExecuteNonQuery();
                    var resultStatus = (returnValueStatus.Value).ToString();
                    if (resultStatus == "0")
                    {
                        textBox2.Text = "Готово";
                    }
                    else textBox2.Text = "Не готово";

                    connect.Close();

                    foreach (string[] s in data)
                        dataGridView1.Rows.Add(s);               
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string orderNumber = label2.Text;
            if (textBox2.Text == "Готово")
            {
                string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnect))
                    {
                        connect.Open();
                        string sqlRequest = $"EXEC DeleteClient'{orderNumber}'";
                        SqlCommand command = new SqlCommand(sqlRequest, connect);
                        command.ExecuteReader();
                        connect.Close();
                        Form1 f = new Form1();
                        f.Show();
                        this.Close();
                        MessageBox.Show("Вы забрали свой заказ!");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else MessageBox.Show("Ваш заказ ещё не готов!");
        }
    }
}
