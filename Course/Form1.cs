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
        DataSet ds2;
        SqlCommandBuilder commandBuilder;
        private SqlDataAdapter AdapterMainTable;
        private SqlDataAdapter AdapterSecondTable;
        private SqlDataAdapter AdapterOtherTable;
        string studentId;

        //string connectionString = @"Data Source=DESKTOP-OT4EBDA;Integrated Security=True";
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\PGU\C#\Курсовая\v2\Course\Course\Database1.mdf;Integrated Security=True";
        //     string command = @"Use DataBase2 SELECT Студенты.Имя,
        //Студенты.Фамилия,
        //Студенты.Отчество,
        //Студенты.Возраст,
        //Группы.Название AS Группа
        //         FROM Студенты
        //         INNER  JOIN Группы ON Группы.Id = Студенты.Группа";
        string command = @"SELECT Студент.Фамилия + ' ' + Студент.Имя AS Студент
                                        , Предмет.Название AS Предмет
                                        , Предмет.[Способ оценивания] 
                                        , Студент_предмет.[Семестр] 
                                        , Студент_предмет.[Статус изучения]
                                        , Студент_предмет.Оценка
                                    FROM Студент_предмет
                                    INNER JOIN Студент ON Студент.Id = Студент_предмет.Студент
                                    INNER JOIN Предмет ON Предмет.Id = Студент_предмет.Предмет";
   //     string command1 = @"Use DataBase2 SELECT Студенты.Имя,
			//Студенты.Фамилия,
			//Студенты.Отчество,
			//Студенты.Возраст
   //         FROM Студенты";
        
        // Содержит: Основную таблицу, зависимую таблицу, столбец основной, столбец зависимой
        string[,] names = { { "Студент", "Группа", "Группа", "Название" }
            , { "Группа", "Преподаватель", "Куратор", "Фамилия" }
            , { "Преподаватель", "", "", "" }
            , { "Предмет", "Преподаватель", "Преподаватель", "Фамилия" } };
        string[] marks = { "-Нет-", "Зачёт", "Неудовлетворительно", "Удовлетворительно", "Хорошо", "Отлично" };
        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
            LoadDataForMark();
            LoadData(comboBox1.SelectedIndex);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (dataGridView2.Visible)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {
                    connection.Open();
                    var AdapterStudent = new SqlDataAdapter("SELECT * FROM Студент", connection);
                    var AdapterPredmet = new SqlDataAdapter("SELECT * FROM Предмет", connection);
                    var AdapterStudentPredmet = new SqlDataAdapter("SELECT * FROM Студент_предмет WHERE id=" + studentId, connection);
                    

                    commandBuilder = new SqlCommandBuilder(AdapterStudent);
                    AdapterStudent.Update(ds2, "Студент");
                    commandBuilder = new SqlCommandBuilder(AdapterPredmet);
                    AdapterPredmet.Update(ds2, "Предмет");
                    commandBuilder = new SqlCommandBuilder(AdapterStudentPredmet);
                    AdapterStudentPredmet.Update(ds2, "Студент_предмет");
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

            else
            {
                SaveData(comboBox1.SelectedIndex);
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(comboBox1.SelectedIndex);
            //dataGridView2.Visible = false;
            //buttonUndo.Visible = false;
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
           
        }

        private void dataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].InheritedStyle.BackColor = Color.Red;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                dataGridView2.Visible = true;
                buttonUndo.Visible = true;
                SqlConnection connection = new SqlConnection(connectionString);
                try
                {

                    studentId = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    //// Открываем подключение
                    //connection.Open();
                    //
                    //AdapterOtherTable = new SqlDataAdapter(command + studentId, connection);
                    //// Создаем объект Dataset
                    //DataSet ds2 = new DataSet();
                    //// Заполняем Dataset
                    //AdapterOtherTable.Fill(ds2); // прочитать 
                    //                            // Отображаем данные
                    //dataGridView2.DataSource = ds2.Tables[0];
                    //dataGridView2.Columns["Студент"].ReadOnly = true;
                    //dataGridView2.Columns["Предмет"].ReadOnly = true;


                    
                    connection.Open();
                    var AdapterStudent = new SqlDataAdapter("SELECT * FROM Студент", connection);
                    var AdapterPredmet = new SqlDataAdapter("SELECT * FROM Предмет", connection);
                    var AdapterStudentPredmet = new SqlDataAdapter("SELECT * FROM Студент_предмет WHERE id=" + studentId, connection);
                    // Создаем объект Dataset
                    ds2 = new DataSet();
                    // Заполняем Dataset
                    AdapterStudentPredmet.Fill(ds2, "Студент_предмет"); // прочитать 
                    AdapterStudent.Fill(ds2, "Студент"); // прочитать 
                    AdapterPredmet.Fill(ds2, "Предмет"); // прочитать 
                                                     // Отображаем данные
                    dataGridView2.DataSource = ds2.Tables[0];

                    ds2.Relations.Add(new DataRelation("relation1", ds2.Tables["Предмет"].Columns["id"], ds2.Tables["Студент_предмет"].Columns["Предмет"]));
                    ds2.Relations.Add(new DataRelation("relation2", ds2.Tables["Студент"].Columns["id"], ds2.Tables["Студент_предмет"].Columns["Студент"]));
                    

                    dataGridView2.Columns["id"].Visible = false; // скрыть колонку с идентификатором
                    dataGridView2.Columns["Студент"].Visible = false; // скрыть колонку с идентификатором
                    dataGridView2.Columns["Предмет"].Visible = false; // скрыть колонку с идентификатором

                    var ComboBoxColumnPredmet = new DataGridViewComboBoxColumn
                    {
                        Name = "Предмет",
                        DataSource = ds2.Tables["Предмет"],
                        DisplayMember = "Название", // Отображать 
                        ValueMember = "id",
                        DataPropertyName = "Предмет", // Для связи с 
                        MaxDropDownItems = 10,
                        FlatStyle = FlatStyle.Flat
                    }; // добавить новую колонку
                    dataGridView2.Columns.Insert(2, ComboBoxColumnPredmet);
                    dataGridView2.Columns[4].Width = 200;

                    var ComboBoxColumnStudent = new DataGridViewComboBoxColumn
                    {
                        Name = "Студент",
                        DataSource = ds2.Tables["Студент"],
                        DisplayMember = "Фамилия", // Отображать 
                        ValueMember = "id",
                        DataPropertyName = "Студент", // Для связи с 
                        MaxDropDownItems = 10,
                        FlatStyle = FlatStyle.Flat
                    }; // добавить новую колонку
                    dataGridView2.Columns.Insert(1, ComboBoxColumnStudent);
                    dataGridView2.Columns[1].Width = 200;
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
        }

        private void buttonUndo_Click(object sender, EventArgs e)
        {

            dataGridView2.Visible = false;
            buttonUndo.Visible = false;
        }












        /// <summary>
        ////Сохраняет данные отображаемой таблицы
        /// </summary>
        /// <param name="numberView">Номер отображаемой таблицы</param>
        private void SaveData(int numberView)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            void SaveTable(string nameTable)
            {

                var adapter = new SqlDataAdapter("SELECT * FROM " + names[numberView, 0], connection);
                var commandBuilder = new SqlCommandBuilder(adapter);
                adapter.Update(ds, nameTable);
            }
            try
            {
                connection.Open();
                SaveTable(names[numberView, 0]);
                if (numberView != 2)
                {
                    SaveTable(names[numberView, 1]);
                }
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

        /// <summary>
        /// Загружает данные для таблицы оценок из базы данных
        /// </summary>
        private void LoadDataForMark()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {


                // Открываем подключение
                connection.Open();

                AdapterOtherTable = new SqlDataAdapter(command, connection);
                // Создаем объект Dataset
                DataSet ds2 = new DataSet();
                // Заполняем Dataset
                AdapterOtherTable.Fill(ds2); // прочитать 
                                             // Отображаем данные
                dataGridView2.DataSource = ds2.Tables[0];
                dataGridView2.Columns["Студент"].ReadOnly = true;
                dataGridView2.Columns["Предмет"].ReadOnly = true;
                dataGridView2.Columns["Способ оценивания"].ReadOnly = true;
                dataGridView2.Columns["Оценка"].Visible = false;

                var comboBoxColumnMark = new DataGridViewComboBoxColumn
                {
                    Name = "Оценка",
                    DataPropertyName = "Оценка",
                    FlatStyle = FlatStyle.Flat,
                }; // добавить новую колонку
                comboBoxColumnMark.Items.AddRange(marks);
                dataGridView2.Columns.Add(comboBoxColumnMark);
                //connection.Open();
                //var AdapterStudent = new SqlDataAdapter("SELECT * FROM Студент", connection);
                //var AdapterPredmet = new SqlDataAdapter("SELECT * FROM Предмет", connection);
                //var AdapterStudentPredmet = new SqlDataAdapter("SELECT * FROM Студент_предмет", connection);
                //// Создаем объект Dataset
                //ds2 = new DataSet();
                //// Заполняем Dataset
                //AdapterStudentPredmet.Fill(ds2, "Студент_предмет"); // прочитать 
                //AdapterStudent.Fill(ds2, "Студент"); // прочитать 
                //AdapterPredmet.Fill(ds2, "Предмет"); // прочитать 
                //                                     // Отображаем данные
                //dataGridView2.DataSource = ds2.Tables[0];

                //ds2.Relations.Add(new DataRelation("relation1", ds2.Tables["Предмет"].Columns["id"], ds2.Tables["Студент_предмет"].Columns["Предмет"]));
                //ds2.Relations.Add(new DataRelation("relation2", ds2.Tables["Студент"].Columns["id"], ds2.Tables["Студент_предмет"].Columns["Студент"]));


                //dataGridView2.Columns["id"].Visible = false; // скрыть колонку с идентификатором
                //dataGridView2.Columns["Студент"].Visible = false; // скрыть колонку с идентификатором
                //dataGridView2.Columns["Предмет"].Visible = false; // скрыть колонку с идентификатором


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


    /// <summary>
    /// Загружает данные для таблицы из базы данных
    /// </summary>
    /// <param name="numberView">Номер таблицы для загрузки</param>
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
                AdapterMainTable.Fill(ds, names[numberView, 0]); // прочитать 
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

        private void toolStripButtonMark_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }

        private void toolStripButtonEditor_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void toolStripButtonReport_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
        }
    }
}
