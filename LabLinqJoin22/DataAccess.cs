using LabLinqJoin22.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * Установка пакетов и создание модели данных
 * Install-Package Microsoft.EntityFrameworkCore.SqlServer -version 5.0.17
 * Install-Package Microsoft.EntityFrameworkCore.Tools -version 5.0.17
 * Install-Package Microsoft.Extensions.Logging
 * Scaffold-DbContext "Data Source=magister-v;Initial Catalog=Faculty_UA_22;Integrated Security=True" Microsoft.EntityFrameworkCore.SqlServer -Context FacultyContext -OutputDir Models
 * 
 * Модель данных получена командой
 * Scaffold-DbContext "Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=E:\Users\Lena\NET\DB\STUD_20.mdf;Integrated Security=True;Connect Timeout=30" Microsoft.EntityFrameworkCore.SqlServer -Context STUD_20Context -OutputDir Models -Tables STUDENTS, GROUPS, SUBJECTS, TUTORS, CHAIRS, CURRICULUM
 * Команда не отработает, если есть ошибки компиляции
 * 
 * https://www.thecodebuzz.com/efcore-scaffold-dbcontext-commands-orm-net-core/#install-efcore-tools
 * 
 */



namespace LabLinqJoin22
{
    class DataAccess 
    {
        /* 
         * Контекст данных
         * Классы модели данных находятся в папке Models
         */
        Models.FacultyContext context = new Models.FacultyContext();

        public DataAccess()
        {

        }//DataAccess()

        /* TODO 0
          * Виправити шлях до файлу БД у рядку з'єднання в контексті даних (Models.LabLinqJoin22.cs)
          * Якщо є проблеми з БД - перезавантажити чи перестворити її
          * Виконувати завдання у цьому файлі у відповідності з варіантом         */


        /* TODO 1 - 4 
         * 
         * Для виконання з'єднань даних з кількох сутностей (таблиць) в LINQ може використовуватись 
         * як оператор join, так і навігаційні свойства.
         * Див. документацію:
         * https://learn.microsoft.com/ru-ru/dotnet/csharp/linq/perform-inner-joins
         * 
         * 
         * Приклад звернення до кількох сутностей в одному запиті.
        * Запит звертається до пов'язаних сутностей Tutors та Chairs,
        * але в інструкції від є тільки звернення до Tutors (2).
        * Дані Chairs отримані за допомогою навігаційного свойства
        * Також зверніть увагу на використання навігаційних властивостей
        * для обчислення агрегатних функцій (3),
        * а також на використання підзапиту (4)
        *
        * Завдання для прикладу: отримати викладачів,
        * (1) для яких вказано номер кафедри
        * (2) та в групах, які вони курирують, кількість занять більша
        * середньої кількості занять у всіх групах;
        * для кожного викладача вказати:
        * (3) ПІБ викладача,
        * (4) номер кафедри, на якій він працює
        * (5) кількість груп, які він курирує
        * (6) групу, що має найбільшу кількість занять із груп тієї кафедри,
        * на якій працює даний викладач;
        * (7) упорядкувати за спаданням номера кафедри, а потім - на ім'я викладача         */

        public object Query1Example()
        {
            return
            (
            from t in context.Tutors
            where
              //(1)
              (t.Chair.ChairNumber != "") &&
              //(2)
              (t.Groups.Average(gr => gr.StudyHours) > context.Groups.Average(gr => gr.StudyHours))
            //(7)
            orderby t.Chair.ChairNumber descending, t.NameFio
            select new
            {
                //(3)
                Name = t.NameFio,

                // (4) получаем информацию о кафедре через навигационное свойство
                Chair = t.Chair.ChairNumber,

                // (5) подсчитываем количество курируемых групп 
                CuratorGroupCount = t.Groups.Count(),

                // (6) подзапрос
                MaxStudyHoursGroup =
                (
                    from gr in t.Chair.Groups
                    where gr.StudyHours == t.Chair.Groups.Max(ggr => ggr.StudyHours)
                    select gr.GroupNumber
                ).FirstOrDefault()

            }
            ).ToList();  // ToList() нужен, т.к. EF Core не получит данные, пока мы не попытаемся их прочитать
        }//Query1Example()

        public object Query1()
        {
            return
                null;
        }//Query1()

        // Query2() пропускаем, чтобы сохранить нумерацию заданий

        //public object Query2()
        //{
        //    return
        //        null;
        //}//Query2()


        public object Query3()
        {
            return
                null;
        }//Query3()

        public object Query4()
        {
            return
                null;
        }//Query4()

        /* TODO 5.1 Пример группировки с подсчётом агрегатных функций
         * Функции Count(), Min(), Max(), Average(), Sum()
         * подсчитываются по каждой группе данных (считается длина фамилий)
        */

        public object Query5Example()
        {
            return
                (
                from tut in context.Tutors.AsEnumerable()
                group tut by tut.NameFio.Substring(0, 1) into gr
                orderby gr.Key descending
                select new
                {
                    Number = gr.Key,
                    Count = gr.Count(),
                    minLength = gr.Min(t => t.NameFio.Length),
                    maxLength = gr.Max(t => t.NameFio.Length),
                    avgLength = gr.Average(t => t.NameFio.Length),
                    sumLength = gr.Sum(t => t.NameFio.Length)
                }
                ).ToList();
        }//Query5Example()

        /* TODO 5.2
        * Напишіть запит із групуванням та підрахунком агрегатних функцій
        * у відповідності з варіантом завдання         */

        public object Query5()
        {
            return
                null;
        }//Query5

        /* TODO 6.1 Приклад угруповання з виконанням розрахунків за пов'язаними даними в підзапиті
         * 
         * https://learn.microsoft.com/ru-ru/dotnet/csharp/linq/perform-a-subquery-on-a-grouping-operation
         * https://learn.microsoft.com/ru-ru/dotnet/framework/data/adonet/ef/language-reference/query-expression-syntax-examples-navigating-relationships
         *
         * По кожному куратору підраховується кількість груп та загальна довжина прізвищ студентів без пробілів         * 
         * Аналогичный запрос на SQL:
         * 
            SELECT T.NAME_FIO, COUNT(DISTINCT G.GROUP_ID), SUM(LEN(S.SURNAME))
            FROM TUTORS T JOIN GROUPS G ON T.TUTOR_ID = G.CURATOR_ID
            JOIN STUDENTS S ON G.GROUP_ID = S.GROUP_ID
            GROUP BY T.NAME_FIO
         * 
         * В более ранних версиях можно было использовать подзапросы вида
         * SumSurnameLen = t.CuratorOfGroups.Sum(gg => gg.Students.Sum(stt => stt.Surname.Trim().Length))
         * Однако в EF Core 3.0 было запрещено использование подзапросов в подзапросах, т.к. 
         * их нельзя преобразовать в SQL, а довыполнение запросов на клиенте может вызывать непредсказуемое поведение
         * Подробности:
         * https://docs.microsoft.com/ru-ru/ef/core/what-is-new/ef-core-3.x/breaking-changes#linq-queries-are-no-longer-evaluated-on-the-client
         * Поэтому теперь надо сначала вытащить все связанные данные, а потом группировать их и считать агрегатные функции
         */


        public object Query6Example()
        {
            return
                (
                from st in context.Students.Include(st => st.Group).ThenInclude(g => g.Curator).AsEnumerable()
                group st by st.Group.Curator.NameFio into gr
                orderby gr.Key
                select new
                {
                    CuratorFIO = gr.Key,
                    GroupCount = (from stt in gr select stt.Group).Distinct().Count(),
                    SumSurnameLen = gr.Sum(sttt => sttt.Surname.Trim().Length)
                }
                ).ToList();

        }//Query6Example()

        /* TODO 6.2
         * Напішіть запит з підзапитом згідно до варіанту завданння
         */

        public object Query6()
        {
            return
                null;
        }//Query6()


        /* TODO 7.1 Приклад угруповання
         * 
        * Запит витягує викладачів, для яких задано посилання на кафедру,
        * групуючи їх за ПІБ завкафедрою,
        * група викладачів, завідувачем яких є Соколов, не витягується,
        * Отримані групи сортуються за ПІБ завкафедрою у зворотному порядку.
        *
        * Зверніть увагу, що пов'язані дані треба підвантажувати до угруповання.
        * Для цього використовуються методи Include() та ThenInclude().
        *
        * Зверніть увагу на метод AsEnumerable(), який змушує програму прочитати дані
        * EF Core виконує угруповання тільки після завантаження даних
        *
        * Також зверніть увагу, що інструкція where використовується двічі: до угруповання та після         */

        public IOrderedEnumerable<IGrouping<string, Models.Tutor>> Query7Example()
        {
            return

                from tut in context.Tutors.Include(t => t.Chair).ThenInclude(c => c.ChairHead).AsEnumerable() // подгружаем преподов, кафедры и заведующих
                where tut.Chair != null                             // проверяем наличие ссылки на кафедру, фильтруя исхожные данные
                group tut by tut.Chair.ChairHead.NameFio into gr    // собственно группировка
                where !gr.Key.Contains("Соколов")                   // фильтрация после группировки
                orderby gr.Key descending                           // сортировка групп
                select gr;


            /*
             * Это старый пример. Он тоже работает
             * 
                from tut in context.Tutors.AsEnumerable()  // AsEnumerable() нужно добавлять, т.к. EF Core не будет группировать данные до их чтения
                where tut.NameFio.Length <= 12   // фильтрация элементов данных
                group tut by tut.NameFio.Substring(0, 1) into gr
                where (gr.Key != "М") && (gr.Key != "Ж")        // фильтрация групп
                orderby gr.Key descending
                select gr;
                
            */
        }//Query7Example()

        /* TODO 7.2
         * Допишіть код методу Task7() для отримання даних відповідно до варіанта
         * Зверніть увагу, що ключ угруповання має тип string
         * Якщо у вас за завданням ключ числовий - перетворіть на рядок
         * Це пов'язано з налаштуванням виведення на форму, насправді припустимо будь-який тип даних
         */
        public IOrderedEnumerable<IGrouping<string, Models.Student>> Query7()
        {
            return
                null;
        }//Query7()

        /* TODO 8.1 Ліве зовнішне з'єднання
         * 
         * На відміну від SQL, виконання лівих зовнішніх з'єднаннь в LINQ
         * істотно відрізняється від внутришніх з'єднань.
         * Подробиці див. в документації:
         * https://learn.microsoft.com/ru-ru/dotnet/csharp/linq/perform-left-outer-joins
         * 
         * Приклад:
         * Вибрати всіх викладачів, вказавши прізвище, номер кафедри (якщо є) та назви предметів, 
         * які вони ведуть (якщо є), відсортувати за прізвищем викладача у зворотньому порядку 
         */

        public object Query8Example()
        {
            return
            (
            from t in context.Tutors
            join ch in context.Chairs on t.Chair equals ch into tutOnCh
            from tch in tutOnCh.DefaultIfEmpty()
            join cur in context.Curricula on t equals cur.Tutor into tutOnCur
            from tcur in tutOnCur.DefaultIfEmpty()
            join sub in context.Subjects on tcur.Subject equals sub into tutOnSub
            from tsub in tutOnSub.DefaultIfEmpty()
            orderby t.NameFio descending
            select new
            {
                FIO = t.NameFio,
                ChairNum = tch.ChairNumber ?? String.Empty,
                Subject = tsub.Name ?? String.Empty
            }
            ).ToList();
        }//Query8Example()


        /* TODO 8.2 
         * Напішіть запит зовнішнім з'єднанням згідно до варіанту завданння
         */

        public object Query8()
        {
            return
                null;
        }//Query8()




    }//class DataAccess}
}
