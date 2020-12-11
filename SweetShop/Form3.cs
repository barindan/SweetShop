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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public string stringConnection()
        {
            ConnectionStringSettings connectString;
            connectString = ConfigurationManager.ConnectionStrings["SweetShop.Properties.Settings.DB_SWEET_SHOPConnectionString"];

            return connectString.ConnectionString;
        }
        public Form3(string orderNumber)
        {
            InitializeComponent();
            label10.Text = orderNumber;
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection()))
                {
                    connect.Open();
                    string SqlExpression = "EXEC DessertMenu";
                    SqlCommand command = new SqlCommand(SqlExpression, connect);
                    SqlDataReader reader = command.ExecuteReader();
                    List<string[]> data = new List<string[]>();
                    while (reader.Read())
                    {
                        data.Add(new string[7]);

                        data[data.Count - 1][0] = reader[0].ToString();
                        data[data.Count - 1][1] = reader[1].ToString();
                        data[data.Count - 1][2] = reader[2].ToString();
                        data[data.Count - 1][3] = reader[3].ToString();
                        data[data.Count - 1][4] = reader[4].ToString();
                        data[data.Count - 1][5] = reader[5].ToString();
                        data[data.Count - 1][6] = reader[6].ToString();
                    }
                    reader.Close();
                    foreach (string[] s in data)
                    {
                        dataGridView1.Rows.Add(s);
                    }

                    connect.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string clientid = label10.Text;
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection()))
                {
                    connect.Open();
                    string sqlRequest = $"EXEC DeleteClient'{clientid}'";
                    SqlCommand command = new SqlCommand(sqlRequest, connect);
                    command.ExecuteReader();
                    connect.Close();
                    Form1 f = new Form1();
                    f.Show();
                    this.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string clientid = label10.Text;
            string dessertid = textBox4.Text;

            if (dessertid == "")
            {
                MessageBox.Show("Вы не ввели индекс");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
                    {

                        connect.Open();
                        //Проверка id дессерта
                        string sqlRequest = "CheckDessert";
                        SqlCommand checkDessert = new SqlCommand(sqlRequest, connect);
                        checkDessert.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter paramOne = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = int.Parse(dessertid)
                        };
                        checkDessert.Parameters.Add(paramOne);
                        var returnValue = checkDessert.Parameters.Add("@Return", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        checkDessert.ExecuteNonQuery();
                        var result = (returnValue.Value).ToString();
                        if (result == "0")
                        {
                            MessageBox.Show("Неправильно введен индекс десерта");
                        }
                        else if (result == "1")
                        {
                            textBox4.Text = "";
                            dataGridView2.Rows.Clear();
                            //Добавление заказа
                            string sqlExpression = "AddOrder";
                            SqlCommand command = new SqlCommand(sqlExpression, connect);
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter dessertidParam = new SqlParameter
                            {
                                ParameterName = "@dessertId",
                                Value = int.Parse(dessertid)
                            };
                            command.Parameters.Add(dessertidParam);
                            SqlParameter clientidParam = new SqlParameter
                            {
                                ParameterName = "@clientid",
                                Value = int.Parse(clientid)
                            };
                            command.Parameters.Add(clientidParam);
                            command.ExecuteNonQuery();

                            //Выводим текущий заказ
                            string sqlExpr = $"GetOrderInfoNow'{int.Parse(clientid)}'";
                            SqlCommand getOrder = new SqlCommand(sqlExpr, connect);
                            SqlDataReader reader = getOrder.ExecuteReader();
                            List<string[]> data = new List<string[]>();
                            while (reader.Read())
                            {
                                data.Add(new string[3]);

                                data[data.Count - 1][0] = reader[0].ToString();
                                data[data.Count - 1][1] = reader[1].ToString();
                                data[data.Count - 1][2] = reader[2].ToString();
                            }
                            reader.Close();
                            foreach (string[] s in data)
                            {
                                dataGridView2.Rows.Add(s);
                            }
                            //Расчет стоимости заказа
                            string sqlExpressionSum = "GetAmountOrder";
                            SqlCommand getAmount = new SqlCommand(sqlExpressionSum, connect);
                            getAmount.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter paramId = new SqlParameter
                            {
                                ParameterName = "@id",
                                Value = int.Parse(clientid)
                            };
                            getAmount.Parameters.Add(paramId);
                            var returnValueSum = getAmount.Parameters.Add("@Return", SqlDbType.Int);
                            returnValueSum.Direction = ParameterDirection.ReturnValue;
                            getAmount.ExecuteNonQuery();
                            var resultSumAmount = (returnValueSum.Value).ToString();
                            if (resultSumAmount == "")
                            {
                                label6.Text = "0";
                            }
                            else label6.Text = resultSumAmount;

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

        private void button3_Click(object sender, EventArgs e)
        {
            string clientid = label10.Text;
            string dessertid = textBox5.Text;

            if (dessertid == "")
            {
                MessageBox.Show("Вы не ввели индекс");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
                    {

                        connect.Open();
                        //Проверка id дессерта
                        string sqlRequest = "CheckOrderDessert";
                        SqlCommand checkDessert = new SqlCommand(sqlRequest, connect);
                        checkDessert.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter paramOne = new SqlParameter
                        {
                            ParameterName = "@iddessert",
                            Value = int.Parse(dessertid)
                        };
                        checkDessert.Parameters.Add(paramOne);
                        SqlParameter paramTwo = new SqlParameter
                        { 
                            ParameterName = "@idclient",
                            Value = int.Parse(clientid)
                        };
                        checkDessert.Parameters.Add(paramTwo);
                        var returnValue = checkDessert.Parameters.Add("@Return", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        checkDessert.ExecuteNonQuery();
                        var result = (returnValue.Value).ToString();
                        if (result == "0")
                        {
                            MessageBox.Show("Неправильно введен индекс десерта");
                        }
                        else if (result == "1")
                        {
                            textBox5.Text = "";
                            dataGridView2.Rows.Clear();
                            //Удаление заказа
                            string sqlExpression = "DeleteOneOrder";
                            SqlCommand command = new SqlCommand(sqlExpression, connect);
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter dessertidParam = new SqlParameter
                            {
                                ParameterName = "@dessertId",
                                Value = int.Parse(dessertid)
                            };
                            command.Parameters.Add(dessertidParam);
                            SqlParameter clientidParam = new SqlParameter
                            {
                                ParameterName = "@clientid",
                                Value = int.Parse(clientid)
                            };
                            command.Parameters.Add(clientidParam);
                            command.ExecuteNonQuery();

                            //Выводим текущий заказ
                            string sqlExpr = $"GetOrderInfoNow'{int.Parse(clientid)}'";
                            SqlCommand getOrder = new SqlCommand(sqlExpr, connect);
                            SqlDataReader reader = getOrder.ExecuteReader();
                            List<string[]> data = new List<string[]>();
                            while (reader.Read())
                            {
                                data.Add(new string[3]);

                                data[data.Count - 1][0] = reader[0].ToString();
                                data[data.Count - 1][1] = reader[1].ToString();
                                data[data.Count - 1][2] = reader[2].ToString();
                            }
                            reader.Close();
                            foreach (string[] s in data)
                            {
                                dataGridView2.Rows.Add(s);
                            }
                            //Расчет стоимости заказа
                            string sqlExpressionSum = "GetAmountOrder";
                            SqlCommand getAmount = new SqlCommand(sqlExpressionSum, connect);
                            getAmount.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter paramId = new SqlParameter
                            {
                                ParameterName = "@id",
                                Value = int.Parse(clientid)
                            };
                            getAmount.Parameters.Add(paramId);
                            var returnValueSum = getAmount.Parameters.Add("@Return", SqlDbType.Int);
                            returnValueSum.Direction = ParameterDirection.ReturnValue;
                            getAmount.ExecuteNonQuery();
                            var resultSumAmount = (returnValueSum.Value).ToString();
                            
                            if (resultSumAmount == "")
                            {
                                label6.Text = "0";
                            }
                            else label6.Text = resultSumAmount;

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

        private void button1_Click(object sender, EventArgs e)
        {
            string clientid = label10.Text;

            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Введите пожалуйста ФИО");
            }
            else
            {
                string sqlExpression = "OrdersThereIs";
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
                            Value = int.Parse(clientid)
                        };
                        command.Parameters.Add(paramOne);
                        var returnValue = command.Parameters.Add("@Return", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        command.ExecuteNonQuery();
                        var result = (returnValue.Value).ToString();

                        if (result == "0")
                        {
                            MessageBox.Show("Ваш заказ пуст!\nДобавьте что-нибудь в заказ");
                        }
                        else if (result == "1")
                        {
                            string sqlRequest = $"EXEC UpdateClientInfo'{clientid}','{textBox1.Text}','{textBox2.Text}','{textBox3.Text}'";
                            SqlCommand updateClient = new SqlCommand(sqlRequest, connect);
                            updateClient.ExecuteReader();
                            Form1 f = new Form1();
                            f.Show();
                            this.Close();
                            MessageBox.Show($"Ваш заказ успешно создан\nНомер заказа: {clientid}");
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
