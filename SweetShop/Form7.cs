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
    public partial class Form7 : Form
    {
        public string stringConnection()
        {
            ConnectionStringSettings connectString;
            connectString = ConfigurationManager.ConnectionStrings["SweetShop.Properties.Settings.DB_SWEET_SHOPConnectionString"];

            return connectString.ConnectionString;
        }
        public Form7()
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

                    string SqlExpr = "SELECT * FROM CompositionForChef";
                    SqlCommand comm = new SqlCommand(SqlExpr, connect);
                    SqlDataReader readerEmp = comm.ExecuteReader();
                    List<string[]> dataEmp = new List<string[]>();
                    while (readerEmp.Read())
                    {
                        dataEmp.Add(new string[3]);
                        dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                        dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                        dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                    }

                    readerEmp.Close();

                    foreach (string[] s in dataEmp)
                        dataGridView2.Rows.Add(s);

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
                        dataGridView3.Rows.Add(s);

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
                        dataGridView4.Rows.Add(s);

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


        private void button5_Click(object sender, EventArgs e)
        {
            //Изменение состава
            string id = textBox13.Text;
            string compos = textBox8.Text;
            string cal = textBox9.Text;
            if (id == "" || compos == "" || cal == "")
            {
                MessageBox.Show("Введите все поля");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
                    {
                        connect.Open();
                        //Проверка id состава
                        string sqlRequest = "CheckComposition";
                        SqlCommand checkComposition = new SqlCommand(sqlRequest, connect);
                        checkComposition.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter paramOne = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        checkComposition.Parameters.Add(paramOne);
                        var returnValue = checkComposition.Parameters.Add("@Return", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        checkComposition.ExecuteNonQuery();
                        var result = (returnValue.Value).ToString();
                        if (result == "0")
                        {
                            MessageBox.Show("Неправильно введен индекс состава");
                        }
                        else if (result == "1")
                        {

                            string sqlExpression = $"EXEC UpdateComposition'{id}','{compos}','{cal}'";
                            SqlCommand command = new SqlCommand(sqlExpression, connect);
                            command.ExecuteNonQuery();

                            dataGridView2.Rows.Clear();
                            string SqlExpr = "SELECT * FROM CompositionForChef";
                            SqlCommand comm = new SqlCommand(SqlExpr, connect);
                            SqlDataReader readerEmp = comm.ExecuteReader();
                            List<string[]> dataEmp = new List<string[]>();
                            while (readerEmp.Read())
                            {
                                dataEmp.Add(new string[3]);
                                dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                            }

                            textBox6.Clear();
                            textBox7.Clear();

                            readerEmp.Close();

                            foreach (string[] s in dataEmp)
                                dataGridView2.Rows.Add(s);


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
            //Добавление десерта
            string dessertName = textBox2.Text;
            string compositionId = textBox3.Text;
            string costPrice = textBox4.Text;
            string price = textBox5.Text;
            string[] dateProduct = dateTimePicker1.Value.ToString().Split(new Char[] { ' ' });
            string[] dateShelf = dateTimePicker2.Value.ToString().Split(new Char[] { ' ' });

            string[] productDate = dateProduct[0].Split(new Char[] {'.'});
            string[] shelfDate = dateShelf[0].Split(new Char[] { '.' });
            string d1 = productDate[1] + '.' + productDate[0] + '.' + productDate[2];
            string d2 = shelfDate[1] + '.' + shelfDate[0] + '.' + shelfDate[2];

            if (dessertName == "" || compositionId == "" || costPrice == "" || price=="")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection())) 
                    {
                        
                        connect.Open();
                        string sqlRequest = $"EXEC AddDessert'{dessertName}','{compositionId}','{price}','{costPrice}','{d1}','{d2}'";
                        SqlCommand updAll = new SqlCommand(sqlRequest, connect);
                        updAll.ExecuteNonQuery();

                        dataGridView1.Rows.Clear();
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

                        textBox2.Clear();
                        textBox3.Clear();
                        textBox4.Clear();
                        textBox5.Clear();

                        foreach (string[] s in data)
                            dataGridView1.Rows.Add(s);

                        connect.Close();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Удаление десерта
            string id = textBox1.Text;
            if (id == "")
            {
                MessageBox.Show("Введите индекс");
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
                            Value = id
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
                          
                            string sqlExpr = $"EXEC DeleteDessert'{id}'";
                            SqlCommand updAll = new SqlCommand(sqlExpr, connect);
                            updAll.ExecuteNonQuery();

                            dataGridView1.Rows.Clear();
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
                            textBox1.Clear();
                            foreach (string[] s in data)
                                dataGridView1.Rows.Add(s);

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

        private void button4_Click(object sender, EventArgs e)
        {
            //Добавление состава
            string composit = textBox6.Text;
            string calorie = textBox7.Text;
            if (composit == "" || calorie == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
                    {

                        connect.Open();
                        string sqlRequest = $"EXEC AddComposition'{composit}','{calorie}'";
                        SqlCommand command = new SqlCommand(sqlRequest, connect);
                        command.ExecuteNonQuery();

                        dataGridView2.Rows.Clear();

                        string SqlExpr = "SELECT * FROM CompositionForChef";
                        SqlCommand comm = new SqlCommand(SqlExpr, connect);
                        SqlDataReader readerEmp = comm.ExecuteReader();
                        List<string[]> dataEmp = new List<string[]>();
                        while (readerEmp.Read())
                        {
                            dataEmp.Add(new string[3]);
                            dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                            dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                            dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                        }

                        readerEmp.Close();

                        textBox6.Clear();
                        textBox7.Clear();

                        foreach (string[] s in dataEmp)
                            dataGridView2.Rows.Add(s);

                        connect.Close();
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Удаление состава
            string id = textBox10.Text;
            if (id == "")
            {
                MessageBox.Show("Введите ID состава");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
                    {
                        connect.Open();
                        //Проверка id состава
                        string sqlRequest = "CheckComposition";
                        SqlCommand checkComposition = new SqlCommand(sqlRequest, connect);
                        checkComposition.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter paramOne = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = id
                        };
                        checkComposition.Parameters.Add(paramOne);
                        var returnValue = checkComposition.Parameters.Add("@Return", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        checkComposition.ExecuteNonQuery();
                        var result = (returnValue.Value).ToString();
                        if (result == "0")
                        {
                            MessageBox.Show("Неправильно введен индекс состава");
                        }
                        else if (result == "1")
                        {

                            string sqlExpression = $"EXEC DeleteComposition'{id}'";
                            SqlCommand command = new SqlCommand(sqlExpression, connect);
                            command.ExecuteNonQuery();

                            dataGridView2.Rows.Clear();
                            string SqlExpr = "SELECT * FROM CompositionForChef";
                            SqlCommand comm = new SqlCommand(SqlExpr, connect);
                            SqlDataReader readerEmp = comm.ExecuteReader();
                            List<string[]> dataEmp = new List<string[]>();
                            while (readerEmp.Read())
                            {
                                dataEmp.Add(new string[3]);
                                dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                            }

                            textBox10.Clear();
                            readerEmp.Close();
                            foreach (string[] s in dataEmp)
                                dataGridView2.Rows.Add(s);


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

        private void UpdateOrderStatus_Click(object sender, EventArgs e)
        {
            string id = textBox12.Text;
            if (id == "")
            {
                MessageBox.Show("Введите ID заказа");
            }else if (!id.All(char.IsDigit))
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
                                dataGridView3.Rows.Add(s);


                            dataGridView4.Rows.Clear();
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
                                dataGridView4.Rows.Add(s);

                            textBox12.Clear();
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

        private void button8_Click(object sender, EventArgs e)
        {
            string id = textBox11.Text;
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
                string sqlExpression = "CheckOrderInCTE";
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
                            string query = $"EXEC DeleteOrders'{id}'";
                            SqlCommand updateOrderStatus = new SqlCommand(query, connect);
                            updateOrderStatus.ExecuteNonQuery();

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
                                dataGridView3.Rows.Add(s);

                            dataGridView4.Rows.Clear();
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
                                dataGridView4.Rows.Add(s);

                            textBox11.Clear();
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

        private void DeleteOrder_Click(object sender, EventArgs e)
        {
            string id = idOrderBox.Text;
            if(id == "")
            {
                MessageBox.Show("Введите ID заказа, который нужно удалить");
            }else if (!id.All(char.IsDigit))
            {
                MessageBox.Show("ID заказа должно быть целом числом");
            }
            else
            {
                try
                {
                    using(SqlConnection connect = new SqlConnection(stringConnection()))
                    {
                        string SqlExpression = "CheckOrderID";
                        connect.Open();
                        SqlCommand command = new SqlCommand(SqlExpression, connect);
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
                            string query = $"EXEC DeleteOrders'{id}'";
                            SqlCommand deleteOrder = new SqlCommand(query, connect);
                            deleteOrder.ExecuteNonQuery();

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
                                dataGridView3.Rows.Add(s);

                            dataGridView4.Rows.Clear();
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
                                dataGridView4.Rows.Add(s);

                            idOrderBox.Clear();
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
