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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnect))
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
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
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
                    using (SqlConnection connect = new SqlConnection(stringConnect))
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
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
            string dessertName = textBox2.Text;
            string compositionId = textBox3.Text;
            string costPrice = textBox4.Text;
            string price = textBox5.Text;
            string productDate = dateTimePicker1.Value.ToString();
            string shelfDate = dateTimePicker2.Value.ToString();
            
            if (dessertName == "" || compositionId == "" || costPrice == "" || price=="")
            {
                MessageBox.Show("Заполните все поля");
            }
            else
            {
                
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnect)) 
                    {
                        
                        connect.Open();
                        string sqlRequest = $"EXEC AddDessert'{dessertName}','{compositionId}','{price}','{costPrice}','{productDate}','{shelfDate}'";
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
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
            string id = textBox1.Text;
            if (id == "")
            {
                MessageBox.Show("Введите индекс");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnect))
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
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
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
                    using (SqlConnection connect = new SqlConnection(stringConnect))
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

                        textBox13.Clear();
                        textBox8.Clear();
                        textBox9.Clear();

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
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
            if (id == "")
            {
                MessageBox.Show("Введите ID состава");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnect))
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

        private void button7_Click(object sender, EventArgs e)
        {
            string id = textBox12.Text;
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
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
                    using (SqlConnection connect = new SqlConnection(stringConnect))
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
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
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
                    using (SqlConnection connect = new SqlConnection(stringConnect))
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
    }
}
