using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace BookListApp
{
        public partial class Form1 : Form
    {
        private List<Book> bookList = new List<Book>();
        public Form1()
        {
            InitializeComponent();
            textBoxFilter.TextChanged += textBoxFilter_TextChanged;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "제목";
            dataGridView1.Columns[1].Name = "저자";
            dataGridView1.Columns[2].Name = "출판년도";
            dataGridView1.Columns[0].FillWeight = 47;  // 제목 - 넓게
            dataGridView1.Columns[1].FillWeight = 30;  // 저자 - 중간
            dataGridView1.Columns[2].FillWeight = 23;  // 출판년도 - 좁게

            this.FormClosing += Form1_FormClosing;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string author = textBox2.Text.Trim();
            string title = textBox1.Text.Trim();

            int year;

            // 숫자 유효성 검사
            if (!int.TryParse(textBox3.Text, out year))
            {
                MessageBox.Show("출판년도는 숫자만 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //DateTime year = dateTimePicker1.Value;
            if (string.IsNullOrWhiteSpace(author) || string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("제목과 저자를 모두 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Book 객체 생성 및 리스트에 추가
            Book newBook = new Book(title, author, year);
            bookList.Add(newBook);

            // DataGridView에 행 추가
            dataGridView1.Rows.Add(newBook.title, newBook.author, newBook.year);
            //newBook.year.ToString("yyyy-MM-dd"));

            // 입력 초기화
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
 //           dateTimePicker1.Value = DateTime.Today;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // 선택된 행의 인덱스 가져오기
                int selectedIndex = dataGridView1.SelectedRows[0].Index;

                // 리스트에서 해당 Book 삭제
                if (selectedIndex >= 0 && selectedIndex < bookList.Count)
                {
                    bookList.RemoveAt(selectedIndex);
                }

                // DataGridView에서도 행 삭제
                dataGridView1.Rows.RemoveAt(selectedIndex);
            }
            else
            {
                MessageBox.Show("삭제할 책을 선택하세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string json = JsonConvert.SerializeObject(bookList, Formatting.Indented);
                File.WriteAllText("books.json", json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("책 목록 저장 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "books.json");

                if (File.Exists(path))
                {
                    string json = File.ReadAllText(path);
                    bookList = JsonConvert.DeserializeObject<List<Book>>(json);

                    // DataGridView에 책 정보 출력
                    foreach (Book book in bookList)
                    {
                        dataGridView1.Rows.Add(book.title, book.author, book.year);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("책 목록 불러오기 중 오류 발생: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            string filterText = textBoxFilter.Text.Trim().ToLower();

            // DataGridView 초기화
            dataGridView1.Rows.Clear();

            // 필터링된 책 리스트 만들기
            var filteredBooks = bookList.Where(book =>
                book.title.ToLower().Contains(filterText) ||
                book.author.ToLower().Contains(filterText)
            ).ToList();

            // 필터링된 결과 DataGridView에 다시 추가
            foreach (var book in filteredBooks)
            {
                dataGridView1.Rows.Add(book.title, book.author, book.year);
            }
        }
    }
}
