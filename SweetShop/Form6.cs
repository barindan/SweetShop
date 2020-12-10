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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnect))
                {
                    connect.Open();
                    string query = "SELECT * FROM EmployeesView";
                    SqlCommand command = new SqlCommand(query, connect);
                    SqlDataReader reader = command.ExecuteReader();
                    List<string[]> data = new List<string[]>();
                    while (reader.Read())
                    {
                        data.Add(new string[6]);

                        data[data.Count - 1][0] = reader[0].ToString();
                        data[data.Count - 1][1] = reader[1].ToString();
                        data[data.Count - 1][2] = reader[2].ToString();
                        data[data.Count - 1][3] = reader[3].ToString();
                        data[data.Count - 1][4] = reader[4].ToString();
                        data[data.Count - 1][5] = reader[5].ToString();

                    }
                    reader.Close();

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

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
            string lastName = textBox1.Text;
            string firstName = textBox2.Text;
            string middleName = textBox3.Text;
            string age = textBox4.Text;
            string experience = textBox5.Text;
            string post = textBox6.Text;
            string passwordEmp = textBox7.Text;
            if (lastName == "" || firstName == "" || middleName == "" || age == "" || experience == "" || post == "" || passwordEmp == "")
            {
                MessageBox.Show("Введите все значения");
            }
            else if (passwordEmp.Length > 12)
            {
                MessageBox.Show("Пароль не может быть больше 12 символов");
            }
            else if (!age.All(char.IsDigit) || int.Parse(age) < 14)
            {
                MessageBox.Show("Возраст должен быть числом >= 14");
            }
            else if (!experience.All(char.IsDigit) || int.Parse(experience) < 0 || int.Parse(experience) > (int.Parse(age) - 14))
            {
                MessageBox.Show("Стаж должен быть положительным числом, меньшим чем Возраст - 14 ");
            }
            else if (post != "Manager" && post != "Chef" && post != "Cook")
            {
                MessageBox.Show("Неправильно указана должность. Варианты:\nManager\nChef\nCook");
            }
            else
            {
                string sqlExpression = $"EXEC dbo.AddEmployee'{lastName}','{firstName}','{middleName}','{age}','{experience}','{post}','{passwordEmp}'";
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnect))
                    {
                        connect.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connect);
                        command.ExecuteNonQuery();


                        dataGridView1.Rows.Clear();

                        string SqlExpr = "SELECT * FROM EmployeesView";
                        SqlCommand comm = new SqlCommand(SqlExpr, connect);
                        SqlDataReader readerEmp = comm.ExecuteReader();
                        List<string[]> dataEmp = new List<string[]>();
                        while (readerEmp.Read())
                        {
                            dataEmp.Add(new string[6]);
                            dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                            dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                            dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                            dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                            dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                            dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                        }

                        readerEmp.Close();
                        foreach (string[] s in dataEmp)
                            dataGridView1.Rows.Add(s);

                        connect.Close();
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                        textBox1.Text = "";
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
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";
            string idEmp = textBox8.Text;
            if (idEmp == "")
            {
                MessageBox.Show("Введите ID сотрудника,\nкоторого нужно уволить");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnect))
                    {
                        string sqlExpr = "CheckEmp";
                        connect.Open();
                        SqlCommand command = new SqlCommand(sqlExpr, connect);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter paramId = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = int.Parse(idEmp)
                        };
                        command.Parameters.Add(paramId);
                        SqlParameter paramOut = new SqlParameter
                        {
                            ParameterName = "@infoNum",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(paramOut);
                        command.ExecuteNonQuery();
                        string result = command.Parameters["@infoNum"].Value.ToString();
                        if (result == "0")
                        {
                            MessageBox.Show("Сотрудника с данным индксом не существует");
                        }
                        else if (result == "1")
                        {
                            string sqlRequest = "DelEmployee";
                            SqlCommand delEmp = new SqlCommand(sqlRequest, connect);
                            delEmp.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter paramEmpId = new SqlParameter
                            {
                                ParameterName = "@id",
                                Value = int.Parse(idEmp)
                            };
                            delEmp.Parameters.Add(paramEmpId);
                            delEmp.ExecuteNonQuery();

                            dataGridView1.Rows.Clear();

                            string SqlExpr = "SELECT * FROM EmployeesView";
                            SqlCommand comm = new SqlCommand(SqlExpr, connect);
                            SqlDataReader readerEmp = comm.ExecuteReader();
                            List<string[]> dataEmp = new List<string[]>();
                            while (readerEmp.Read())
                            {
                                dataEmp.Add(new string[6]);
                                dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                                dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                            }
                            readerEmp.Close();

                            foreach (string[] s in dataEmp)
                                dataGridView1.Rows.Add(s);
                            textBox8.Clear();

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
            if (textBox9.Text == "")
            {
                MessageBox.Show("Заполните поле");
            }
            else
            {
                string id = textBox9.Text;
                bool flag = false;
                string fio = "";
                string age = "";
                string exp = "";
                string post = "";
                string pass = "";
                for(var i=0; i < dataGridView1.Rows.Count; i++)
                {
                    if(dataGridView1[0,i].Value.ToString() == id)
                    {
                        flag = true;
                        fio = dataGridView1[1, i].Value.ToString();
                        age = dataGridView1[2, i].Value.ToString();
                        exp = dataGridView1[3, i].Value.ToString();
                        post = dataGridView1[4, i].Value.ToString();
                        pass = dataGridView1[5, i].Value.ToString();
                    }
                }
                if (flag)
                {
                    MessageBox.Show($"ФИО: {fio}\nВозраст: {age}\nСтаж: {exp}\nДолжность: {post}\nПароль: {pass}");
                    textBox9.Clear();
                }else MessageBox.Show("Сотрудник с таким ID найден");

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string lastName = textBox12.Text;
            string post = textBox13.Text;
            string pass = textBox14.Text;
            string id = textBox10.Text;
            string stringConnect = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_SWEET_SHOP;Integrated Security=True";

            if (lastName == "" && post == "" && pass == "")
            {
                MessageBox.Show("Введите данные, которые хотите изменить");
            }else if(id == "")
            {
                MessageBox.Show("Введите ID сотрудника");
            }
            else if(id != "")
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnect))
                    {
                        string sqlExpr = "CheckEmp";
                        connect.Open();
                        SqlCommand command = new SqlCommand(sqlExpr, connect);
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter paramId = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = int.Parse(id)
                        };
                        command.Parameters.Add(paramId);
                        SqlParameter paramOut = new SqlParameter
                        {
                            ParameterName = "@infoNum",
                            SqlDbType = SqlDbType.Int,
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(paramOut);
                        command.ExecuteNonQuery();
                        string result = command.Parameters["@infoNum"].Value.ToString();
                        if (result == "0")
                        {
                            MessageBox.Show("Сотрудника с данным индксом не существует");
                        }
                        else if (result == "1")
                        {
                            if (lastName != "" && post != "" && pass != "")
                            {
                                if (post != "Manager" && post != "Chef" && post != "Cook")
                                {
                                    MessageBox.Show("Неправильно указана должность. Варианты:\nManager\nChef\nCook");
                                }
                                else
                                {
                                    string sqlRequest = $"EXEC UpdateAllEmp'{id}','{lastName}','{post}','{pass}'";
                                    SqlCommand updAll = new SqlCommand(sqlRequest, connect);
                                    updAll.ExecuteNonQuery();

                                    dataGridView1.Rows.Clear();

                                    string SqlExpr = "SELECT * FROM EmployeesView";
                                    SqlCommand comm = new SqlCommand(SqlExpr, connect);
                                    SqlDataReader readerEmp = comm.ExecuteReader();
                                    List<string[]> dataEmp = new List<string[]>();
                                    while (readerEmp.Read())
                                    {
                                        dataEmp.Add(new string[6]);
                                        dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                        dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                        dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                        dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                        dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                                        dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                                    }
                                    readerEmp.Close();

                                    foreach (string[] s in dataEmp)
                                        dataGridView1.Rows.Add(s);
                                    textBox10.Clear();
                                    textBox14.Clear();
                                    textBox13.Clear();
                                    textBox12.Clear();
                                }
                            }
                            else if (lastName != "" && post == "" && pass == "")
                            {
                                string sqlRequest = $"EXEC UpdateLastName'{id}','{lastName}'";
                                SqlCommand updAll = new SqlCommand(sqlRequest, connect);
                                updAll.ExecuteNonQuery();

                                dataGridView1.Rows.Clear();

                                string SqlExpr = "SELECT * FROM EmployeesView";
                                SqlCommand comm = new SqlCommand(SqlExpr, connect);
                                SqlDataReader readerEmp = comm.ExecuteReader();
                                List<string[]> dataEmp = new List<string[]>();
                                while (readerEmp.Read())
                                {
                                    dataEmp.Add(new string[6]);
                                    dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                    dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                    dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                    dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                    dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                                    dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                                }
                                readerEmp.Close();

                                foreach (string[] s in dataEmp)
                                    dataGridView1.Rows.Add(s);
                                textBox10.Clear();
                                textBox14.Clear();
                                textBox13.Clear();
                                textBox12.Clear();
                            }
                            else if (lastName == "" && post != "" && pass == "")
                            {
                                if (post != "Manager" && post != "Chef" && post != "Cook")
                                {
                                    MessageBox.Show("Неправильно указана должность. Варианты:\nManager\nChef\nCook");
                                }
                                else
                                {
                                    string sqlRequest = $"EXEC UpdatePostEmp'{id}','{post}'";
                                    SqlCommand updAll = new SqlCommand(sqlRequest, connect);
                                    updAll.ExecuteNonQuery();

                                    dataGridView1.Rows.Clear();

                                    string SqlExpr = "SELECT * FROM EmployeesView";
                                    SqlCommand comm = new SqlCommand(SqlExpr, connect);
                                    SqlDataReader readerEmp = comm.ExecuteReader();
                                    List<string[]> dataEmp = new List<string[]>();
                                    while (readerEmp.Read())
                                    {
                                        dataEmp.Add(new string[6]);
                                        dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                        dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                        dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                        dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                        dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                                        dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                                    }
                                    readerEmp.Close();

                                    foreach (string[] s in dataEmp)
                                        dataGridView1.Rows.Add(s);
                                    textBox10.Clear();
                                    textBox14.Clear();
                                    textBox13.Clear();
                                    textBox12.Clear();
                                }
                            }
                            else if (lastName == "" && post == "" && pass != "")
                            {
                                string sqlRequest = $"EXEC UpdatePassEmp'{id}','{pass}'";
                                SqlCommand updAll = new SqlCommand(sqlRequest, connect);
                                updAll.ExecuteNonQuery();

                                dataGridView1.Rows.Clear();

                                string SqlExpr = "SELECT * FROM EmployeesView";
                                SqlCommand comm = new SqlCommand(SqlExpr, connect);
                                SqlDataReader readerEmp = comm.ExecuteReader();
                                List<string[]> dataEmp = new List<string[]>();
                                while (readerEmp.Read())
                                {
                                    dataEmp.Add(new string[6]);
                                    dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                    dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                    dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                    dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                    dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                                    dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                                }
                                readerEmp.Close();

                                foreach (string[] s in dataEmp)
                                    dataGridView1.Rows.Add(s);
                                textBox10.Clear();
                                textBox14.Clear();
                                textBox13.Clear();
                                textBox12.Clear();
                            }
                            else if (lastName != "" && post != "" && pass == "")
                            {
                                if (post != "Manager" && post != "Chef" && post != "Cook")
                                {
                                    MessageBox.Show("Неправильно указана должность. Варианты:\nManager\nChef\nCook");
                                }
                                else
                                {
                                    string sqlRequest = $"EXEC UpdateLastNameAndPostEmp'{id}','{lastName}','{post}'";
                                    SqlCommand updAll = new SqlCommand(sqlRequest, connect);
                                    updAll.ExecuteNonQuery();

                                    dataGridView1.Rows.Clear();

                                    string SqlExpr = "SELECT * FROM EmployeesView";
                                    SqlCommand comm = new SqlCommand(SqlExpr, connect);
                                    SqlDataReader readerEmp = comm.ExecuteReader();
                                    List<string[]> dataEmp = new List<string[]>();
                                    while (readerEmp.Read())
                                    {
                                        dataEmp.Add(new string[6]);
                                        dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                        dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                        dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                        dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                        dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                                        dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                                    }
                                    readerEmp.Close();

                                    foreach (string[] s in dataEmp)
                                        dataGridView1.Rows.Add(s);
                                    textBox10.Clear();
                                    textBox14.Clear();
                                    textBox13.Clear();
                                    textBox12.Clear();
                                }
                            }
                            else if (lastName != "" && post == "" && pass != "")
                            {
                                string sqlRequest = $"EXEC UpdateLastNameAndPasswordEmp'{id}','{lastName}','{pass}'";
                                SqlCommand updAll = new SqlCommand(sqlRequest, connect);
                                updAll.ExecuteNonQuery();

                                dataGridView1.Rows.Clear();

                                string SqlExpr = "SELECT * FROM EmployeesView";
                                SqlCommand comm = new SqlCommand(SqlExpr, connect);
                                SqlDataReader readerEmp = comm.ExecuteReader();
                                List<string[]> dataEmp = new List<string[]>();
                                while (readerEmp.Read())
                                {
                                    dataEmp.Add(new string[6]);
                                    dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                    dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                    dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                    dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                    dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                                    dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                                }
                                readerEmp.Close();

                                foreach (string[] s in dataEmp)
                                    dataGridView1.Rows.Add(s);
                                textBox10.Clear();
                                textBox14.Clear();
                                textBox13.Clear();
                                textBox12.Clear();
                            }
                            else if (lastName == "" && post != "" && pass != "")
                            {
                                if (post != "Manager" && post != "Chef" && post != "Cook")
                                {
                                    MessageBox.Show("Неправильно указана должность. Варианты:\nManager\nChef\nCook");
                                }
                                else
                                {
                                    string sqlRequest = $"EXEC UpdatePostAndPasswordEmp'{id}','{post}','{pass}'";
                                    SqlCommand updAll = new SqlCommand(sqlRequest, connect);
                                    updAll.ExecuteNonQuery();

                                    dataGridView1.Rows.Clear();

                                    string SqlExpr = "SELECT * FROM EmployeesView";
                                    SqlCommand comm = new SqlCommand(SqlExpr, connect);
                                    SqlDataReader readerEmp = comm.ExecuteReader();
                                    List<string[]> dataEmp = new List<string[]>();
                                    while (readerEmp.Read())
                                    {
                                        dataEmp.Add(new string[6]);
                                        dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                        dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                        dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                        dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                        dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                                        dataEmp[dataEmp.Count - 1][5] = readerEmp[5].ToString();
                                    }
                                    readerEmp.Close();

                                    foreach (string[] s in dataEmp)
                                        dataGridView1.Rows.Add(s);
                                    textBox10.Clear();
                                    textBox14.Clear();
                                    textBox13.Clear();
                                    textBox12.Clear();
                                }
                            }
                            else MessageBox.Show("Что-то пошло не так");
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
