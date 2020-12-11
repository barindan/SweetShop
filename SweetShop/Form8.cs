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
using System.Configuration;

namespace SweetShop
{
    public partial class Form8 : Form
    {
        public string stringConnection()
        {
            ConnectionStringSettings connectString;
            connectString = ConfigurationManager.ConnectionStrings["SweetShop.Properties.Settings.DB_SWEET_SHOPConnectionString"];

            return connectString.ConnectionString;
        }
        public Form8()
        {
            InitializeComponent();
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection()))
                {
                    connect.Open();
                    string query = "SELECT * FROM MenuForChef";
                    SqlCommand command = new SqlCommand(query, connect);
                    SqlDataReader reader = command.ExecuteReader();
                    List<string[]> data = new List<string[]>();
                    while (reader.Read())
                    {
                        data.Add(new string[8]);

                        data[data.Count - 1][0] = reader[0].ToString();
                        data[data.Count - 1][1] = reader[1].ToString();
                        data[data.Count - 1][2] = reader[2].ToString();
                        data[data.Count - 1][3] = reader[3].ToString();
                        data[data.Count - 1][4] = reader[4].ToString();
                        data[data.Count - 1][5] = reader[5].ToString();
                        data[data.Count - 1][6] = reader[6].ToString();
                        data[data.Count - 1][7] = reader[7].ToString();
                    }
                    reader.Close();

                    foreach (string[] s in data)
                        dataGridView1.Rows.Add(s);

                    string sqlRequest = "SELECT * FROM OrdersForChef";
                    SqlCommand getOrder = new SqlCommand(sqlRequest, connect);
                    SqlDataReader readerOrder = getOrder.ExecuteReader();
                    List<string[]> dataOrder = new List<string[]>();
                    while (readerOrder.Read())
                    {
                        dataOrder.Add(new string[5]);
                        dataOrder[dataOrder.Count - 1][0] = readerOrder[0].ToString();
                        dataOrder[dataOrder.Count - 1][1] = readerOrder[1].ToString();
                        dataOrder[dataOrder.Count - 1][2] = readerOrder[2].ToString();
                        dataOrder[dataOrder.Count - 1][3] = readerOrder[3].ToString();
                        dataOrder[dataOrder.Count - 1][4] = readerOrder[4].ToString();
                    }
                    readerOrder.Close();
                    foreach (string[] s in dataOrder)
                        dataGridView2.Rows.Add(s);

                    string sqlQuery = "SELECT * FROM AllOrders";
                    SqlCommand allOrderView = new SqlCommand(sqlQuery, connect);
                    SqlDataReader readerAllOrder = allOrderView.ExecuteReader();
                    List<string[]> listOrder = new List<string[]>();

                    while (readerAllOrder.Read())
                    {
                        listOrder.Add(new string[5]);

                        listOrder[listOrder.Count - 1][0] = readerAllOrder[0].ToString();
                        listOrder[listOrder.Count - 1][1] = readerAllOrder[1].ToString();
                        listOrder[listOrder.Count - 1][2] = readerAllOrder[2].ToString();
                        listOrder[listOrder.Count - 1][3] = readerAllOrder[3].ToString();
                        listOrder[listOrder.Count - 1][4] = readerAllOrder[4].ToString();

                    }
                    readerAllOrder.Close();

                    foreach (string[] s in listOrder)
                        dataGridView3.Rows.Add(s);

                    connect.Close();
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
            string id = textBox1.Text;
            if (id == "")
            {
                MessageBox.Show("Введите ID заказа");
            }
            else if (!id.All(char.IsDigit))
            {
                MessageBox.Show("ID должно быть числом");
            }
            else
            {
                string sqlExpression = "CheckOrderForChef";
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
                    {
                        connect.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connect);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter paramOne = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        command.Parameters.Add(paramOne);
                        var returnValue = command.Parameters.Add("@Return", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        command.ExecuteNonQuery();
                        var result = (returnValue.Value).ToString();

                        if (result == "0")
                        {
                            MessageBox.Show("Заказа с таким номером не существует.\nПроверьте правильность введеного значения");
                        }
                        else if (result == "1")
                        {
                            string query = $"EXEC UpdateOrderStatus'{id}'";
                            SqlCommand updateOrderStatus = new SqlCommand(query, connect);
                            updateOrderStatus.ExecuteNonQuery();

                            dataGridView2.Rows.Clear();
                            dataGridView3.Rows.Clear();

                            string sqlRequest = "SELECT * FROM OrdersForChef";
                            SqlCommand getOrder = new SqlCommand(sqlRequest, connect);
                            SqlDataReader readerOrder = getOrder.ExecuteReader();
                            List<string[]> dataOrder = new List<string[]>();
                            while (readerOrder.Read())
                            {
                                dataOrder.Add(new string[5]);
                                dataOrder[dataOrder.Count - 1][0] = readerOrder[0].ToString();
                                dataOrder[dataOrder.Count - 1][1] = readerOrder[1].ToString();
                                dataOrder[dataOrder.Count - 1][2] = readerOrder[2].ToString();
                                dataOrder[dataOrder.Count - 1][3] = readerOrder[3].ToString();
                                dataOrder[dataOrder.Count - 1][4] = readerOrder[4].ToString();
                            }
                            readerOrder.Close();
                            foreach (string[] s in dataOrder)
                                dataGridView2.Rows.Add(s);

                            string sqlQuery = "SELECT * FROM AllOrders";
                            SqlCommand allOrderView = new SqlCommand(sqlQuery, connect);
                            SqlDataReader readerAllOrder = allOrderView.ExecuteReader();
                            List<string[]> listOrder = new List<string[]>();

                            while (readerAllOrder.Read())
                            {
                                listOrder.Add(new string[5]);

                                listOrder[listOrder.Count - 1][0] = readerAllOrder[0].ToString();
                                listOrder[listOrder.Count - 1][1] = readerAllOrder[1].ToString();
                                listOrder[listOrder.Count - 1][2] = readerAllOrder[2].ToString();
                                listOrder[listOrder.Count - 1][3] = readerAllOrder[3].ToString();
                                listOrder[listOrder.Count - 1][4] = readerAllOrder[4].ToString();

                            }
                            readerAllOrder.Close();

                            foreach (string[] s in listOrder)
                                dataGridView3.Rows.Add(s);

                            textBox1.Clear();
                        }
                        else MessageBox.Show("Что-то пошло не так");

                        connect.Close();
                    }

                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
