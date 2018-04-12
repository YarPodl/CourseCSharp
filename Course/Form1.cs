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
using System.Data.OleDb;

namespace Course
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataSet ds;
        SqlCommandBuilder commandBuilder;
        private SqlDataAdapter AdapterMainTable;
        private SqlDataAdapter AdapterSecondTable;
        private SqlDataAdapter adapter_Teachers;
        private SqlDataAdapter adapter_Subjects;

        //string connectionString = @"Data Source=DESKTOP-OT4EBDA;Integrated Security=True";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\PGU\C#\Курсовая\v2\Course\Course\Database1.mdf;Integrated Security=True";
        string command = @"Use DataBase2 SELECT Студенты.Имя,
			Студенты.Фамилия,
			Студенты.Отчество,
			Студенты.Возраст,
			Группы.Название AS Группа
            FROM Студенты
            INNER  JOIN Группы ON Группы.Id = Студенты.Группа";
        string command1 = @"Use DataBase2 SELECT Студенты.Имя,
			Студенты.Фамилия,
			Студенты.Отчество,
			Студенты.Возраст
            FROM Студенты";
        string[,] names = { { "Студент", "Группа", "Группа", "Название" }
            , { "Группа", "Преподаватель", "Куратор", "Фамилия" }
            , { "Преподаватель", "", "", "" }
            , { "Предмет", "Преподаватель", "Преподаватель", "Фамилия" } };
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadData(comboBox1.SelectedIndex);
            /*
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                
                // Открываем подключение
                connection.Open();
                //adapter_Students = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Студенты", connection);
                //adapter_Groups = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Группы", connection);
                //adapter_Teachers = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Преподаватели", connection);
                //adapter_Subjects = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Предметы", connection);

                adapter_Students = new SqlDataAdapter("SELECT * FROM Студент", connection);
                adapter_Groups = new SqlDataAdapter("SELECT * FROM Группа", connection);
                // Создаем объект Dataset
                ds = new DataSet();
                // Заполняем Dataset
                adapter_Students.Fill(ds, "Студенты"); // прочитать books
                adapter_Groups.Fill(ds, "Группы"); // прочитать series
                // Отображаем данные
                dataGridView1.DataSource = ds.Tables[0];
                ds.Relations.Add(new DataRelation("rlSeriesBooks", ds.Tables["Группы"].Columns["id"], ds.Tables["Студенты"].Columns["Группа"]));
                dataGridView1.DataSource = ds.Tables["Студенты"]; // Books - в DataGrid

                dataGridView1.Columns["Группа"].Visible = false; // скрыть колонку с идентификатором
                dataGridView1.Columns["id"].Visible = false; // скрыть колонку с идентификатором

                var ComboBoxColumn_Groups = new DataGridViewComboBoxColumn
                {
                    Name = "Группа",
                    DataSource = ds.Tables["Группы"],
                    DisplayMember = "Название", // Отображать из Series
                    ValueMember = "id",
                    DataPropertyName = "Группа", // Для связи с Books
                    MaxDropDownItems = 10,
                    FlatStyle = FlatStyle.Flat
                }; // добавить новую колонку
                dataGridView1.Columns.Insert(5, ComboBoxColumn_Groups);
                dataGridView1.Columns[5].Width = 200;
                
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // закрываем подключение
                connection.Close();
            }
            */
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveData(comboBox1.SelectedIndex);
            /*
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //connection.Open();
                //adapter = new SqlDataAdapter(command, connection);
                //commandBuilder = new SqlCommandBuilder(adapter);
                ////adapter.InsertCommand = new SqlCommand("sp_CreateUser", connection);
                ////adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                ////adapter.InsertCommand.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 50, "Name"));
                ////adapter.InsertCommand.Parameters.Add(new SqlParameter("@age", SqlDbType.Int, 0, "Age"));

                ////SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                ////parameter.Direction = ParameterDirection.Output;

                //adapter.Update(ds);


                connection.Open();
                adapter_Students = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Студенты", connection);
                adapter_Groups = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Группы", connection);
                commandBuilder = new SqlCommandBuilder(adapter_Students);
                commandBuilder = new SqlCommandBuilder(adapter_Groups);
                adapter_Students.Update(ds, "Студенты");
                adapter_Groups.Update(ds, "Группы");
            }
            */
        }
        private void SaveData(int numberView)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                AdapterMainTable = new SqlDataAdapter("SELECT * FROM " + names[numberView, 0], connection);
                commandBuilder = new SqlCommandBuilder(AdapterMainTable);
                AdapterMainTable.Update(ds, names[numberView, 0]);

                if (numberView != 2)
                {
                    AdapterSecondTable = new SqlDataAdapter("SELECT * FROM " + names[numberView, 1], connection);
                    commandBuilder = new SqlCommandBuilder(AdapterSecondTable);
                    AdapterSecondTable.Update(ds, names[numberView, 1]);
                }
            }
        }

        private void LoadData(int numberView)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {

                // Открываем подключение
                connection.Open();
                //adapter_Students = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Студенты", connection);
                //adapter_Groups = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Группы", connection);
                //adapter_Teachers = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Преподаватели", connection);
                //adapter_Subjects = new SqlDataAdapter("SELECT * FROM DataBase2.dbo.Предметы", connection);

                AdapterMainTable = new SqlDataAdapter("SELECT * FROM " + names[numberView, 0], connection);
                // Создаем объект Dataset
                ds = new DataSet();
                // Заполняем Dataset
                AdapterMainTable.Fill(ds, names[numberView,0]); // прочитать 
                // Отображаем данные
                dataGridView1.DataSource = ds.Tables[0];

                if (numberView != 2)
                {
                    AdapterSecondTable = new SqlDataAdapter("SELECT * FROM " + names[numberView, 1], connection);
                    AdapterSecondTable.Fill(ds, names[numberView, 1]); // прочитать 
                    ds.Relations.Add(new DataRelation("relation", ds.Tables[names[numberView, 1]].Columns["id"], ds.Tables[names[numberView, 0]].Columns[names[numberView, 2]]));
                    //dataGridView1.DataSource = ds.Tables[names[numberView, 0]]; 

                    dataGridView1.Columns[names[numberView, 2]].Visible = false; // скрыть колонку с идентификатором
                    dataGridView1.Columns["id"].Visible = false; // скрыть колонку с идентификатором

                    var ComboBoxColumn = new DataGridViewComboBoxColumn
                    {
                        Name = names[numberView, 2],
                        DataSource = ds.Tables[names[numberView, 1]],
                        DisplayMember = names[numberView, 3], // Отображать 
                        ValueMember = "id",
                        DataPropertyName = names[numberView, 2], // Для связи с 
                        MaxDropDownItems = 10,
                        FlatStyle = FlatStyle.Flat
                    }; // добавить новую колонку
                    dataGridView1.Columns.Insert(4, ComboBoxColumn);
                    dataGridView1.Columns[4].Width = 200;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // закрываем подключение
                connection.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(comboBox1.SelectedIndex);
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

        }
    }
}
