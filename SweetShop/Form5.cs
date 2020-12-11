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
    public partial class Form5 : Form
    {
        public string stringConnection()
        {
            ConnectionStringSettings connectString;
            connectString = ConfigurationManager.ConnectionStrings["SweetShop.Properties.Settings.DB_SWEET_SHOPConnectionString"];

            return connectString.ConnectionString;
        }
        public Form5()
        {
            InitializeComponent();
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection()))
                {
                    connect.Open();
                    string query = "SELECT * FROM MenuForBoss";
                    SqlCommand command = new SqlCommand(query, connect);
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
                        dataGridView2.Rows.Add(s);

                    string SqlExpr = "SELECT * FROM EmployeesForBoss";
                    SqlCommand comm = new SqlCommand(SqlExpr, connect);
                    SqlDataReader readerEmp = comm.ExecuteReader();
                    List<string[]> dataEmp = new List<string[]>();
                    while (readerEmp.Read())
                    {
                        dataEmp.Add(new string[5]);
                        dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                        dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                        dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                        dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                        dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                    }

                    foreach (string[] s in dataEmp)
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
            string idEmp = textBox1.Text;
            if (idEmp == "")
            {
                MessageBox.Show("Введите ID сотрудника,\nкоторого нужно уволить");
            }
            else
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
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

                            string SqlExpr = "SELECT * FROM EmployeesForBoss";
                            SqlCommand comm = new SqlCommand(SqlExpr, connect);
                            SqlDataReader readerEmp = comm.ExecuteReader();
                            List<string[]> dataEmp = new List<string[]>();
                            while (readerEmp.Read())
                            {
                                dataEmp.Add(new string[5]);
                                dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                                dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                                dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                                dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                                dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
                            }
                            readerEmp.Close();

                            foreach (string[] s in dataEmp)
                                dataGridView1.Rows.Add(s);
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

        private void button3_Click(object sender, EventArgs e)
        {
            string lastName = textBox2.Text;
            string firstName = textBox3.Text;
            string middleName = textBox4.Text;
            string age = textBox5.Text;
            string experience = textBox6.Text;
            string post = textBox7.Text;
            string passwordEmp = textBox8.Text;
            if (lastName == "" || firstName == "" || middleName == "" || age == "" || experience == "" || post == "" || passwordEmp == "")
            {
                MessageBox.Show("Введите все значения");
            }
            else if (passwordEmp.Length > 12)
            {
                MessageBox.Show("Пароль не может быть больше 12 символов");
            } else if (!age.All(char.IsDigit) || int.Parse(age) < 14)
            {
                MessageBox.Show("Возраст должен быть числом >= 14");
            } else if (!experience.All(char.IsDigit) || int.Parse(experience) < 0 || int.Parse(experience) > (int.Parse(age) - 14))
            {
                MessageBox.Show("Стаж должен быть положительным числом, меньшим чем Возраст - 14 ");
            } 
            else if (post != "Manager" && post != "Chef" && post != "Cook")
            {
                MessageBox.Show("Неправильно указана должность. Варианты:\nManager\nChef\nCook");
            } else
            {
                string sqlExpression = $"EXEC dbo.AddEmployee'{lastName}','{firstName}','{middleName}','{age}','{experience}','{post}','{passwordEmp}'";
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
                    {
                        connect.Open();
                        SqlCommand command = new SqlCommand(sqlExpression, connect);
                        command.ExecuteNonQuery();
                     

                        dataGridView1.Rows.Clear();

                        string SqlExpr = "SELECT * FROM EmployeesForBoss";
                        SqlCommand comm = new SqlCommand(SqlExpr, connect);
                        SqlDataReader readerEmp = comm.ExecuteReader();
                        List<string[]> dataEmp = new List<string[]>();
                        while (readerEmp.Read())
                        {
                            dataEmp.Add(new string[5]);
                            dataEmp[dataEmp.Count - 1][0] = readerEmp[0].ToString();
                            dataEmp[dataEmp.Count - 1][1] = readerEmp[1].ToString();
                            dataEmp[dataEmp.Count - 1][2] = readerEmp[2].ToString();
                            dataEmp[dataEmp.Count - 1][3] = readerEmp[3].ToString();
                            dataEmp[dataEmp.Count - 1][4] = readerEmp[4].ToString();
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
                        textBox8.Text = "";
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
