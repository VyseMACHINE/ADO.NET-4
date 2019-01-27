using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace CreateDbLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet libraryDataSet = new DataSet("Library");

            DataTable books = new DataTable("Books");
            DataTable readers = new DataTable("Readers");
            DataTable delivery = new DataTable("Delivery");

            DataColumn bookId = new DataColumn("bookId", typeof(int));
            bookId.Unique = true;
            bookId.AutoIncrement = true;
            bookId.AutoIncrementSeed = 1;
            bookId.AutoIncrementStep = 1;
            DataColumn name = new DataColumn("name", typeof(string));
            DataColumn author = new DataColumn("author", typeof(string));

            books.Columns.AddRange(new DataColumn[] { bookId, name, author });
            books.PrimaryKey = new DataColumn[] { books.Columns["bookId"] };

            DataColumn libCardNum = new DataColumn("libCardNum", typeof(int));
            libCardNum.Unique = true;
            DataColumn fio = new DataColumn("fio", typeof(string));            
            DataColumn dateOfBirth = new DataColumn("dateOfBirth", typeof(DateTime));            
            DataColumn address = new DataColumn("address", typeof(string));            
            DataColumn passNum = new DataColumn("passNum", typeof(string));            

            readers.Columns.AddRange(new DataColumn[] { libCardNum, fio, dateOfBirth, address, passNum});
            readers.PrimaryKey = new DataColumn[] { readers.Columns["libCardNum"] };

            DataColumn delId = new DataColumn("delId", typeof(int));
            delId.Unique = true;
            delId.AutoIncrement = true;
            delId.AutoIncrementSeed = 1;
            delId.AutoIncrementStep = 1;
            DataColumn idOfBook = new DataColumn("bookId", typeof(int));
            DataColumn numOfLibCard = new DataColumn("libCardNum", typeof(int));
            DataColumn dateOfDel = new DataColumn("dateOfDelivery", typeof(DateTime));
            DataColumn returned = new DataColumn("returned", typeof(bool));

            delivery.Columns.AddRange(new DataColumn[] { delId, idOfBook, numOfLibCard, dateOfDel, returned});
            delivery.PrimaryKey = new DataColumn[] { delivery.Columns["delId"] };

            libraryDataSet.Tables.AddRange(new DataTable[] { books, readers, delivery });

            libraryDataSet.Relations.Add("DeliveryBook", books.Columns["bookId"], delivery.Columns["bookId"]);
            libraryDataSet.Relations.Add("DeliveryReader", readers.Columns["libCardNum"], delivery.Columns["libCardNum"]);

            DataRow rowBook = books.NewRow();
            rowBook.ItemArray = new object[] {null,"Война и Мир", "Лев Толстой"};
            books.Rows.Add(rowBook);
            rowBook = books.NewRow();
            rowBook.ItemArray = new object[] {null,"Убить пересмешника", "Ли Харпер" };
            books.Rows.Add(rowBook);
            rowBook = books.NewRow();
            rowBook.ItemArray = new object[] {null,"Титан", "Теодор Драйзер" };
            books.Rows.Add(rowBook);

            DataRow rowReader = readers.NewRow();
            rowReader.ItemArray = new object[] { 123, "Сабитов Ильяс Маратович", new DateTime(1992, 2, 8), "Туркестан 30/1", "9999"};
            readers.Rows.Add(rowReader);
            rowReader = readers.NewRow();
            rowReader.ItemArray = new object[] { 321, "Наумов Петр Иванович", new DateTime(1995, 4, 12), "Сыганак 24", "88888" };
            readers.Rows.Add(rowReader);
            rowReader = readers.NewRow();
            rowReader.ItemArray = new object[] { 543, "Скатов Ерлан Асланович", new DateTime(1998, 8, 25), "Сайран 12", "555555" };
            readers.Rows.Add(rowReader);

            DataRow rowDelivery = delivery.NewRow();
            rowDelivery.ItemArray = new object[] {null,1, 123, new DateTime(2018,12,12), false};
            delivery.Rows.Add(rowDelivery);
            rowDelivery = delivery.NewRow();
            rowDelivery.ItemArray = new object[] {null,2, 321, new DateTime(2018, 10, 12), false };
            delivery.Rows.Add(rowDelivery);
            rowDelivery = delivery.NewRow();
            rowDelivery.ItemArray = new object[] {null,3, 543, new DateTime(2018, 11, 26), false };
            delivery.Rows.Add(rowDelivery);

            var query = from book in libraryDataSet.Tables["Books"].AsEnumerable()
                        from reader in libraryDataSet.Tables["Readers"].AsEnumerable()
                        from del in libraryDataSet.Tables["Delivery"].AsEnumerable()
                        where book.Field<int>("bookId") == del.Field<int>("bookId")
                        where reader.Field<int>("libCardNum") == del.Field<int>("libCardNum")
                        select new
                        {
                            id = del["delId"],
                            bookName = book["Name"],
                            readerFIO = reader["fio"],
                            date = del["dateOfDelivery"],
                            isReturned = del["returned"]
                        };

            
            foreach (var row in query)
            {
                
                Console.WriteLine("Id: {0}, Назв. книги: {1}, ФИО чит: {2}, Дата выд: {3}, Возращена: {4}", 
                    row.id,row.bookName, row.readerFIO, ((DateTime)row.date).ToShortDateString(), row.isReturned);
            }
        }
    }
}
