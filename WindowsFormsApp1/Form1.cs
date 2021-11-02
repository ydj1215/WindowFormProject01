using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "도서관 관리";

            //라벨에 숫자 나오게
            label1.Text = DataManager.Books.Count.ToString(); //전체 도서 수
            label9.Text = DataManager.Users.Count.ToString(); //사용자 수
            label11.Text = DataManager.Books.Where((x)=>x.IsBorrowed).Count().ToString(); //대출 중인 도서의 수
            label10.Text = DataManager.Books.Where((x)=> {
                return x.IsBorrowed && x.BorrowedAt.AddDays(7) < DateTime.Now;
            }).Count().ToString(); //연체중인 도서의 수

            //데이터 그리드에 정보 나오게
            dataGridView1.DataSource = DataManager.Books;
            dataGridView2.DataSource = DataManager.Users;
            dataGridView1.CurrentCellChanged += dataGridView1_CurrentCellChanged;
            dataGridView2.CurrentCellChanged += dataGridView2_CurrentCellChanged;

            //버튼 이벤트 설정
            button1.Click += button1_Click;
            button2.Click += button2_Click;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            //대여
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Isbn을 입력해주세요.");
            }
            else if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("사용자 Id를 입력해주세요.");
            }
            else
            {
                try
                {
                    //where조건에 맞는 하나만을 선택할 때 쓰는 Single 메서드
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                    if (book.IsBorrowed)
                    {
                        //이미 대여중 이라면
                        MessageBox.Show("이미 대여 중인 도서입니다!");
                    }
                    else
                    {
                        //아직 대여중이 아니라면
                        User user = DataManager.Users.Single(x => x.Id.ToString() == textBox3.Text);
                        book.UserId = user.Id;
                        book.UserName = user.Name;
                        book.IsBorrowed = true;
                        book.BorrowedAt = DateTime.Now;

                        //변경되었음을 지정해주기 위한 리프레쉬 작업
                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Books;
                        //파일에도 바뀐 내용을 저장
                        DataManager.Save();

                        MessageBox.Show("\"" + book.Name + "\"이/가\""+user.Name+"\"님께 대여되었습니다.");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("존재하지 않는 도서 또는 사용자입니다.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //반납
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("Isbn을 입력해주세요.");
            }
            else
            {
                try
                {
                    Book book = DataManager.Books.Single(x => x.Isbn == textBox1.Text);
                    if (book.IsBorrowed)
                    {
                        User user = DataManager.Users.Single(x => x.Id.ToString() == textBox3.Text);
                        book.UserId = "";
                        book.UserName = "";
                        book.IsBorrowed = false;
                        book.BorrowedAt = new DateTime();

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Books;

                        DataManager.Save();

                        //연체처리
                        if (book.BorrowedAt.AddDays(7) > DateTime.Now)
                        {
                            MessageBox.Show("\"" + book.Name + "\"이/가 연체 상태로 반납되었습니다.");
                        }
                        else
                        {
                            MessageBox.Show("\"" + book.Name + "\"이/가 반납되었습니다.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("대여 상태가 아닙니다.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("존재하지 않는 도서 또는 사용자입니다.");
                }
            }
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                //선택된 아이템을 데이터(Book)로 형변환을 해줘야한다
                Book book = dataGridView1.CurrentRow.DataBoundItem as Book;
                textBox1.Text = book.Isbn;
                textBox2.Text = book.Name;
            }
            catch (Exception ex)
            {
                //오류에 대해서 오류 처리를 하지 않겠다는 의미의 빈칸
            }
        }

        private void dataGridView2_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                User user = dataGridView2.CurrentRow.DataBoundItem as User;
                textBox3.Text = user.Id.ToString(); //Id가 int기 때문에 형변환 해줘야 한다.
            }
            catch (Exception ex)
            {

            }
        }

        private void 도서관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        private void 사용자관리ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form3().ShowDialog();
        }
    }
}
