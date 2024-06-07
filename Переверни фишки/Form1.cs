using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Переверни_фишки
{

    public partial class Form1 : Form
    {
        int moves; //переменная, хранящая количество ходов
        int timeSecond = 0; //переменная, хранящая количество секунд 
        int timeMinute = 0; //переменная, хранящая количество минут
        public Form1()
        {
            InitializeComponent();
        }
        private void rulesButton_Click(object sender, EventArgs e) //метод вызова окна с правилами игры при нажатии на кнопку
        {
            MessageBox.Show("Игровое поле представляет собой таблицу размером 4х4 клеток. В клетках размещены фишки двух выбранных цветов. При щелчке мышкой по какой - нибудь фишки, её цвет и цвет соседних по горизонтали и соседних по вертикали фишек изменяется на противоположный. После щелчка в центре игрового поля цвет изменят пять фишек, на краю поля — четыре, а в углу — только три фишки. Цель игры состоит в том, чтобы сделать все фишки одного и того же цвета.", "Правила игры", MessageBoxButtons.OK);
        }
        private void start_game_button_Click(object sender, EventArgs e) // метод для старта игры при нажатии на кнопку
        {
            if (comboBox1.Text == "" || comboBox2.Text == "") { MessageBox.Show("Выберите цвета сторон фишек!", "Ошибка"); timer1.Enabled = false; }
            else 
            if (GetColor1() == GetColor2() && GetColor1() != Color.White) { MessageBox.Show("Цвета сторон фишек не должны совпадать!", "Ошибка"); timer1.Enabled = false; }
            else
            {
                generationGameTable();
                if (CheckWin() && moves == 0) // проверка на совпадение всех цветов при генерации игрового поля
                {
                    ClearDataGridView(); //очистка игрового поля
                    generationGameTable(); //повторная генерация поля
                }
                else
                {
                    timer1.Enabled = true; //начало работы таймера
                    dataGridView1.ClearSelection();
                    comboBox1.Enabled = false; //наложение ограничения на выбор первого цвета
                    comboBox2.Enabled = false; //наложение ограничения на выбор первого цвета
                    start_game_button.Enabled = false; //наложение ограничения на начало игры
                }
            }
        }
        private void reset_Button_Click(object sender, EventArgs e) //метод сброса состояния игры при нажатии на кнопку
        {
            DialogResult result = MessageBox.Show("Сбросить состояние игры?", "Сброс", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) resetAll();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) //метод для изменения цвета выбранной и соседним ей фишек и прибавления единицы к количеству ходов;
        {
            dataGridView1.ClearSelection();
            FlipCellColor(e.RowIndex, e.ColumnIndex); //переворачивание выбранной и соседних ей фишек
            moves++; //увеличение количества ходов на единицу
            textBox1.Text = moves.ToString();
            if (CheckWin()) //проверка на победу
            {
                timer1.Enabled = false;
                MessageBox.Show("Вы выиграли!\nКоличество ходов: " + moves + "\nВремя прохождения игры: " + label1.Text, "Победа!");
                resetAll();
            }
        }
        private void generationGameTable() //метод заполнения игрового поля фишками выбранных цветов
        {           
            dataGridView1.RowCount = 4; //количество строк
            dataGridView1.ColumnCount = 4; //количество столбцов
            int[,] a = new int[4, 4]; //инициализация матрицы
            int i, j;
            //заполнение игрового поля фишками и вывод
            Random rand = new Random();
            for (i = 0; i < dataGridView1.RowCount; i++)
                for (j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    dataGridView1.Columns[i].Width = 100; // ширина ячеек
                    dataGridView1.Rows[i].Height = 100; // высота ячеек
                    a[i, j] = rand.Next(0, 2); // присваивание ячейкам единицы либо ноль
                    if (a[i, j] == 0) // если у ячейки значение ноль, присваивается первый выбранный цвет, иначе присваивается второй цвет
                        dataGridView1.Rows[i].Cells[j].Style.BackColor = GetColor1();
                    else dataGridView1.Rows[i].Cells[j].Style.BackColor = GetColor2(); 
                    dataGridView1.Rows[i].Cells[j].Value = ""; //очищение значения ячейки после окрашивания
                }
        }
        private void FlipCellColor(int row, int col) //метод проверки положения выбранной фишки и изменения её цвета
        {
            if (dataGridView1.Rows[row].Cells[col].Style.BackColor == GetColor1()) //
                dataGridView1.Rows[row].Cells[col].Style.BackColor = GetColor2();
            else dataGridView1.Rows[row].Cells[col].Style.BackColor = GetColor1();
            // Проверка и переворачивание соседних фишек
            if (row > 0) // Проверка верхней ячейки
                if (dataGridView1.Rows[row - 1].Cells[col].Style.BackColor == GetColor1())
                    dataGridView1.Rows[row - 1].Cells[col].Style.BackColor = GetColor2();
                else dataGridView1.Rows[row - 1].Cells[col].Style.BackColor = GetColor1();
            if (row < 3) // Проверка нижней ячейки
                if (dataGridView1.Rows[row + 1].Cells[col].Style.BackColor == GetColor1())
                    dataGridView1.Rows[row + 1].Cells[col].Style.BackColor = GetColor2();
                else dataGridView1.Rows[row + 1].Cells[col].Style.BackColor = GetColor1();
            if (col > 0) // Проверка левой ячейки
                if (dataGridView1.Rows[row].Cells[col - 1].Style.BackColor == GetColor1())
                    dataGridView1.Rows[row].Cells[col - 1].Style.BackColor = GetColor2();
                else dataGridView1.Rows[row].Cells[col - 1].Style.BackColor = GetColor1();
            if (col < 3) // Проверка правой ячейки
                if (dataGridView1.Rows[row].Cells[col + 1].Style.BackColor == GetColor1())
                    dataGridView1.Rows[row].Cells[col + 1].Style.BackColor = GetColor2();
                else dataGridView1.Rows[row].Cells[col + 1].Style.BackColor = GetColor1();
        }

        private bool CheckWin() //метод проверки цветов фишек на игровом поле
        {
            bool allColor1 = true;
            bool allColor2 = true;
            for (int i = 0; i < dataGridView1.RowCount; i++)
                for (int j = 0; j < dataGridView1.ColumnCount; j++)
                {
                    if (dataGridView1.Rows[i].Cells[j].Style.BackColor != GetColor1()) //проверка на несовпадение первого выбранного цвета и цветов всех фишек
                        allColor1 = false;
                    if (dataGridView1.Rows[i].Cells[j].Style.BackColor != GetColor2()) //проверка на несовпадение первого выбранного цвета и цветов всех фишек
                        allColor2 = false;
                }
            if (allColor1 || allColor2)
                return true;
            else return false;
        }
        private void ClearDataGridView() //метод очистки игрового поля
        {
            dataGridView1.Rows.Clear(); // Очищаем строки
            dataGridView1.Columns.Clear(); // Очищаем столбцы
        }
        private void resetAll() //метод сброса состояния игры
        {
            comboBox1.Text = ""; //сброс первого цвета
            comboBox2.Text = ""; //сброс второго цвета
            comboBox1.Enabled = true; //снятие ограничения выбора первого цвета 
            comboBox2.Enabled = true; //снятие ограничения выбора второго цвета
            timer1.Enabled = false; //остановка работы таймера
            label1.Text = "0 : 0"; //установка значения длительности игры в начальное положение
            textBox1.Text = ""; //очистка поля, которое хранит количество ходов
            timeSecond = 0; //сброс количества секунд
            timeMinute = 0;//сброс количества минут
            ClearDataGridView(); //очистка игрового поля
            moves = 0; //обнуление счётчика ходов
            start_game_button.Enabled = true; //снятие ограничения начала игры
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1_CellClick(sender, e);
        }

        private void timer1_Tick(object sender, EventArgs e) //метод для обработки события «Tick» таймера
        {
            timeSecond++;
            if (timeSecond == 60) { timeSecond = 0; timeMinute++; }
            label1.Text = timeMinute.ToString() + " : " + timeSecond.ToString();
        }
        private Color GetColor1() //метод получения цвета обратной стороны фишки
        {
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Красный":
                    return Color.Red;
                case "Оранджевый":
                    return Color.Orange;
                case "Жёлтый":
                    return Color.Yellow;
                case "Зелёный":
                    return Color.Green;
                case "Синий":
                    return Color.Blue;
                case "Фиолетовый":
                    return Color.Purple;
                case "Чёрный":
                    return Color.Black;
                default:
                    return Color.White; 
            }
        }

        private Color GetColor2() //метод получения цвета обратной стороны фишки
        {
            switch (comboBox2.SelectedItem.ToString())
            {
                case "Красный":
                    return Color.Red;
                case "Оранджевый":
                    return Color.Orange;
                case "Жёлтый":
                    return Color.Yellow;
                case "Зелёный":
                    return Color.Green;
                case "Синий":
                    return Color.Blue;
                case "Фиолетовый":
                    return Color.Purple;
                case "Чёрный":
                    return Color.Black;
                default:
                    return Color.White; 
            }
        }
    }
}
