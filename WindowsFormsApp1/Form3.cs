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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            Text = "사용자 관리";

            dataGridView1.DataSource = DataManager.Users;
            dataGridView1.CurrentCellChanged += dataGridView1_CurrentCellChanged;

            button1.Click += (sender, e) =>
            {
                //추가
                try
                {
                    if (DataManager.Users.Exists(x => x.Id == textBox1.Text))
                    {
                        MessageBox.Show("사용자 ID가 중복됩니다.");
                    }
                    else
                    {
                        User user = new User()
                        {
                            Id = textBox1.Text,
                            Name = textBox2.Text
                        };
                        DataManager.Users.Add(user);

                        dataGridView1.DataSource = null;
                        dataGridView1.DataSource = DataManager.Users;
                        DataManager.Save();
                    }
                }
                catch (Exception ex)
                {

                }
            };

            button2.Click += (sender, e) =>
            {
                //수정
                try
                {
                    User user = DataManager.Users.Single(x => x.Id == textBox1.Text);
                    user.Name = textBox2.Text;

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = DataManager.Users;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("존재하지 않는 사용자 입니다.");
                }
            };

            button3.Click += (sender, e) =>
            {
                //삭제
                try
                {
                    User user = DataManager.Users.Single(x => x.Id == textBox1.Text);
                    DataManager.Users.Remove(user);

                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = DataManager.Users;
                    DataManager.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("존재하지 않는 사용자 입니다."); }
            };
        }

        private void dataGridView1_CurrentCellChanged(Object sender, EventArgs e)
        {
            try
            {
                //그리드의 셀이 선택되면 텍스트 박스에 글자 표시
                User user = dataGridView1.CurrentRow.DataBoundItem as User;
                textBox1.Text = user.Id.ToString();
                textBox2.Text = user.Name;
            }
            catch(Exception ex)
            {

            }
        }
    }
}
