using System;
using System.Data.SQLite;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Данные_студента_2
{
    public partial class Zaprosi : Form
    {
        private const string connectionString = "Data Source=styd.db;Version=3;";

        public Zaprosi()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form fsamples = new samples();
            fsamples.Show();
            fsamples.FormClosed += new FormClosedEventHandler(form_FormClosed);
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void RegistrationButton_Click(object sender, EventArgs e)
        {
            string query = "SELECT group_number, SUM(scholarship_amount) AS total_scholarship " +
               "FROM Groups " +
               "GROUP BY group_number;";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader reader = command.ExecuteReader();

                    StringBuilder results = new StringBuilder();
                    while (reader.Read())
                    {
                        string groupNumber = reader.GetString(0);
                        decimal totalScholarship = reader.GetDecimal(1);
                        string result = $"{groupNumber}: {totalScholarship}";
                        results.AppendLine(result);
                    }

                    label1.Text = results.ToString();
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения
                Console.WriteLine(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string query = "SELECT COUNT(*) AS student_count " +
               "FROM Students " +
               "GROUP BY course;";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader reader = command.ExecuteReader();

                    StringBuilder results = new StringBuilder();
                    while (reader.Read())
                    {
                        int studentCount = reader.GetInt32(0);
                        string result = $"Количество студентов: {studentCount} 🎓";
                        results.AppendLine(result);
                    }

                    label1.Text = results.ToString();
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения
                Console.WriteLine(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string query = "SELECT Groups.group_number, COUNT(*) AS student_count " +
                "FROM Groups " +
                "JOIN Faculties ON Groups.faculty_id = Faculties.faculty_id " +
                "GROUP BY Groups.group_number;";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(query, connection);
                    SQLiteDataReader reader = command.ExecuteReader();

                    StringBuilder results = new StringBuilder();
                    while (reader.Read())
                    {
                        string groupNumber = reader.GetString(0);
                        int studentCount = reader.GetInt32(1);
                        string result = $"{groupNumber}: {studentCount}";
                        results.AppendLine(result);
                    }

                    label1.Text = results.ToString();
                }
            }
            catch (Exception ex)
            {
                // Обработка исключения
                Console.WriteLine(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM Students WHERE date_of_birth > DATE('now', '-18 years');";
                var command = new SQLiteCommand(query, connection);
                var adapter = new SQLiteDataAdapter(command);
                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                {
                    string result = "";

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        result += $"Name: {row["full_name"]}, Date of Birth: {row["date_of_birth"]}\n";
                    }

                    // Выводим результат в Label
                    label1.Text = result;
                }
                else
                {
                    // Если нет данных о студентах младше 18 лет
                    label1.Text = "Нет студентов младше 18 лет.";
                }
            }
        }
    }
}