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
        private SqlDataAdapter AdapterOtherTable;

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
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveData(comboBox1.SelectedIndex);
        }

        private void SaveData(int numberView)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
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
            catch(System.FormatException)
            {

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // закрываем подключение
                connection.Close();
            }
        }

        private void LoadData(int numberView)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {

                // Открываем подключение
                connection.Open();

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
            //catch (FormatException ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
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
            dataGridView2.Visible = false;
            buttonUndo.Visible = false;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].InheritedStyle.BackColor = Color.Red;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.Visible = true;
            buttonUndo.Visible = true;
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {

                // Открываем подключение
                connection.Open();
                string studentId = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                AdapterOtherTable = new SqlDataAdapter("SELECT * FROM Студент_предмет WHERE Студент=" + studentId, connection);
                // Создаем объект Dataset
                DataSet ds = new DataSet();
                // Заполняем Dataset
                AdapterOtherTable.Fill(ds); // прочитать 
                // Отображаем данные
                dataGridView2.DataSource = ds.Tables[0];

                //if (numberView != 2)
                //{
                //    AdapterSecondTable = new SqlDataAdapter("SELECT * FROM " + names[numberView, 1], connection);
                //    AdapterSecondTable.Fill(ds, names[numberView, 1]); // прочитать 
                //    ds.Relations.Add(new DataRelation("relation", ds.Tables[names[numberView, 1]].Columns["id"], ds.Tables[names[numberView, 0]].Columns[names[numberView, 2]]));
                //    //dataGridView1.DataSource = ds.Tables[names[numberView, 0]]; 

                //    dataGridView1.Columns[names[numberView, 2]].Visible = false; // скрыть колонку с идентификатором
                //    dataGridView1.Columns["id"].Visible = false; // скрыть колонку с идентификатором

                //    var ComboBoxColumn = new DataGridViewComboBoxColumn
                //    {
                //        Name = names[numberView, 2],
                //        DataSource = ds.Tables[names[numberView, 1]],
                //        DisplayMember = names[numberView, 3], // Отображать 
                //        ValueMember = "id",
                //        DataPropertyName = names[numberView, 2], // Для связи с 
                //        MaxDropDownItems = 10,
                //        FlatStyle = FlatStyle.Flat
                //    }; // добавить новую колонку
                //    dataGridView1.Columns.Add(ComboBoxColumn);
                //    dataGridView1.Columns[dataGridView1.ColumnCount - 1].Width = 200;
                //}
            }
            //catch (FormatException ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            catch (SqlException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // закрываем подключение
                connection.Close();
            }
        }

        private void buttonUndo_Click(object sender, EventArgs e)
        {

            dataGridView2.Visible = false;
            buttonUndo.Visible = false;
        }
    }
}
