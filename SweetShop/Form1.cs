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

    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public string stringConnection()
        {
            ConnectionStringSettings connectString;
            connectString = ConfigurationManager.ConnectionStrings["SweetShop.Properties.Settings.DB_SWEET_SHOPConnectionString"];
            
            return connectString.ConnectionString;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string stringConnect = stringConnection();

            string orderNumberString = textBox1.Text;
            if (orderNumberString == "")
            {
                MessageBox.Show("Введите номер заказа");
            }
            else
            {
                string sqlExpression = "CheckOrder";
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
                            Value = int.Parse(textBox1.Text)
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
                            Form2 orderInfo = new Form2(textBox1.Text);
                            orderInfo.Show();
                            this.Hide();
                        }
                        else MessageBox.Show("Что-то пошло не так");
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
            try
            {
                using (SqlConnection connect = new SqlConnection(stringConnection()))
                {
                    connect.Open();
                    string sqlExpression = "AddClient";
                    SqlCommand command = new SqlCommand(sqlExpression, connect);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var result = command.ExecuteScalar();

                    Form3 order = new Form3(result.ToString());
                    order.Show();
                    this.Hide();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form4 autorization = new Form4();
            autorization.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
