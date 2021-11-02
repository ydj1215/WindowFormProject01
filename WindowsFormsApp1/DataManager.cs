using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    class DataManager
    {
        public static List <Book> Books = new List<Book> ();
        public static List <User> Users = new List<User> ();
        //추가적으로 객체 생성을 하지않고 한곳에서 전부 할 것이기 때문에 static

        static DataManager()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                string booksOutput = File.ReadAllText(@"./Books.xml");
                //현 디렉토리밑의 Books.xml부터 다 읽어와라
                XElement booksXElement = XElement.Parse(booksOutput);
                //xml 태그 가지고 잘라내는 작업

                Books = (from item in booksXElement.Descendants("book")
                             //나는 booksXElement 안에 태그들을 item으로 book단위로서 가져오겠다
                         select new Book() //하나의 books를 가지고 book이라는 객체를 만들어 books에 저장하겠다
                         {
                             Isbn = item.Element("isbn").Value, //" "안은 xml 텍스트에 있는 isbn이 소문자이므로 소문자
                             Name = item.Element("name").Value,
                             Publisher = item.Element("publisher").Value,
                             Page = int.Parse(item.Element("page").Value),
                             BorrowedAt = DateTime.Parse(item.Element("borrowedAt").Value),
                             IsBorrowed = item.Element("isBorrowed").Value != "0" ? true : false,
                             UserId = item.Element("userId").Value,
                             UserName = item.Element("userName").Value
                         }).ToList<Book>();

                string usersOutput = File.ReadAllText(@"./Users.xml");
                XElement usersXElement = XElement.Parse(usersOutput);
                Users = (from item in usersXElement.Descendants("user")
                         select new User()
                         {
                             Id = item.Element("id").Value,
                             Password = item.Element("password").Value,
                             Name = item.Element("name").Value
                         }).ToList<User>();
                //오타확인(지우자확인하고 주석)
            }
            catch (FileNotFoundException)
            {
                Save();
            }
        }          

        public static void Save()
        {
            //load와는 반대로 다시 스트링(xml코드)으로 바꿔 파일에 저장해야한다.
            string booksOutput = "";
            booksOutput += "<books>\n";
            foreach (var item in Books)
            {
                booksOutput += "<book>\n";

                booksOutput += "<isbn>" + item.Isbn + "</isbn>\n";
                booksOutput += "<name>" + item.Name + "</name>\n";
                booksOutput += "<publisher>" + item.Publisher + "</publisher>\n";
                booksOutput += "<page>" + item.Page + "</page>\n";
                booksOutput += "<userId>" + item.UserId + "</userId>\n";
                booksOutput += "<userName>" + item.UserName + "</userName>\n";
                booksOutput += "<isBorrowed>" + (item.IsBorrowed ? 1 : 0) + "</isBorrowed>\n";
                booksOutput += "<borrowedAt>" + item.BorrowedAt.ToLongDateString() + "</borrowedAt>\n";

                booksOutput += "</book>\n";
            }

            booksOutput += "</books>";
            string usersOutput = "";
            usersOutput += "<users>\n";
            foreach (var item in Users)
            {
                usersOutput += "<user>\n";
                usersOutput += "<id>\n" + item.Id + "</id>\n";
                usersOutput += "<password>\n" + item.Password + "</password>\n";
                usersOutput += "<name>\n" + item.Name + "</name>\n";
                usersOutput += "</user>\n";
            }

            usersOutput += "</users>";
            File.WriteAllText(@"./Books.xml", booksOutput);
            File.WriteAllText(@"./Users.xml", usersOutput);
        }

    }
}
