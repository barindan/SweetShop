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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public string stringConnection()
        {
            ConnectionStringSettings connectString;
            connectString = ConfigurationManager.ConnectionStrings["SweetShop.Properties.Settings.DB_SWEET_SHOPConnectionString"];

            return connectString.ConnectionString;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1();
            f.Show();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            if (login == "" || password == "")
            {
                MessageBox.Show("Введите логин и пароль");
            }
            else if (login == "boss" && password == "boss")
            {
                Form5 f = new Form5();
                f.Show();
                this.Close();
            }
            else if (login.All(char.IsDigit))
            {
                try
                {
                    using (SqlConnection connect = new SqlConnection(stringConnection()))
                    {
                        connect.Open();
                        //Проверка id сотрудника
                        string sqlRequest = "CheckEmployee";
                        SqlCommand CheckEmp = new SqlCommand(sqlRequest, connect);
                        CheckEmp.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter paramOne = new SqlParameter
                        {
                            ParameterName = "@id",
                            Value = login
                        };
                        CheckEmp.Parameters.Add(paramOne);
                        var returnValue = CheckEmp.Parameters.Add("@Return", SqlDbType.Int);
                        returnValue.Direction = ParameterDirection.ReturnValue;
                        CheckEmp.ExecuteNonQuery();
                        var result = (returnValue.Value).ToString();
                        if (result == "0")
                        {
                            MessageBox.Show("Неправильно введен логин или пароль");
                        }
                        else if (result == "1")
                        {
                            //Проверка пароля сотрудника
                            string sqlExpression = "CheckEmployeePassword";
                            SqlCommand CheckPassword = new SqlCommand(sqlExpression, connect);
                            CheckPassword.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter paramPassword = new SqlParameter
                            {
                                ParameterName = "@password",
                                Value = password.ToString()
                            };
                            CheckPassword.Parameters.Add(paramPassword);
                            SqlParameter paramLogin = new SqlParameter
                            {
                                ParameterName = "@login",
                                Value = int.Parse(login)
                            };
                            CheckPassword.Parameters.Add(paramLogin);
                            var returnValueInfo = CheckPassword.Parameters.Add("@Return", SqlDbType.Int);
                            returnValueInfo.Direction = ParameterDirection.ReturnValue;
                            CheckPassword.ExecuteNonQuery();
                            var resultInfo = (returnValueInfo.Value).ToString();
                            if (resultInfo == "0")
                            {
                                MessageBox.Show("Неправильно введен логин или пароль");
                            }
                            else if (resultInfo == "1")
                            {
                                //Получение должности сотрудника
                                string query = "GetPost";
                                SqlCommand getPost = new SqlCommand(query, connect);
                                getPost.CommandType = System.Data.CommandType.StoredProcedure;
                                SqlParameter paramId = new SqlParameter
                                {
                                    ParameterName = "@id",
                                    Value = int.Parse(login)
                                };
                                getPost.Parameters.Add(paramId);
                                SqlParameter postParam = new SqlParameter
                                {
                                    ParameterName = "@post",
                                    SqlDbType = SqlDbType.Int,
                                    Direction = ParameterDirection.Output
                                };
                                getPost.Parameters.Add(postParam);
                                getPost.ExecuteNonQuery();

                                var post = getPost.Parameters["@post"].Value;

                                if (post.ToString() == "1")
                                {
                                    Form6 f = new Form6();
                                    f.Show();
                                    this.Close();
                                }
                                else if (post.ToString() == "2")
                                {
                                    Form7 f = new Form7();
                                    f.Show();
                                    this.Close();
                                }
                                else if (post.ToString() == "3")
                                {
                                    Form8 f = new Form8();
                                    f.Show();
                                    this.Close();
                                }
                                else MessageBox.Show("Что-то пошло не так");
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
            else MessageBox.Show("Неправильный логин или пароль");
        }
    }
}
