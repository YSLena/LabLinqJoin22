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

        /* TODO:
         * Исправить путь к файлу БД в строке соединения в контексте данных (Models.STUD_20Context.cs)
         * Если есть проблемы с БД - перезагрузить или пересоздать её, используя сценарий Stud_20.sql
         * Выполнять задания в этом файле в соответсвии с вариантом
         */


        /* Пример обращения к нескольким сущностям в одном запросе.
         * Запрос обращается к связаннім сущностям Tutors и Chairs,
         * но в инстукции from имеется только обращение к Tutors (2).
         * Данные Chairs получены при помощи навигационного свойства 
         * Также обратите внимание на использование навигационных свойств 
         * для вычисления агрегатных функций (3),
         * а также на использование подзапроса (4)
         * 
         * Задача для примера: извлечь преподавателей,
         * (1) для которых указан новмер кафедры
         * (2) и в группах, которые они курируют, количество занятий больше 
         *     среднего количества занятий во всех группах;
         *    для каждого преподавателя указать:
         * (3) ФИО преподавателя,
         * (4) номер кафедры, на которой он работает 
         * (5) количество групп, которые он курирует 
         * (6) группу, имеющую наибольшее количество занятий из групп той кафедры,
         *     на которой работает данный преподаватель;
         * (7) упорядочить по убыванию номера кафедры, а затем - по имени преподавателя   
         */

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

        public object Query2()
        {
            return
                null;
        }//Query2()

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


        /* TODO 5.1 Пример группировки
         * Запрос извлекает преподавателей, для которых задана ссылка на кафедру, 
         * группируя их по ФИО завкафедрой,
         * группа преподов, заведующим которых является Соколов, не извлекается,
         * полученные группы сортируются по ФИО завкафедрой в обратном порядке.
         * 
         * Обратите внимание, что связанные данные надо подгружать до группировки.
         * Для этого используются методы Include() и ThenInclude().
         * 
         * Обратите внимание на метод AsEnumerable(), заставляющий программу прочитать данные
         * EF Core выполняет группировку только после загрузки данных
         * 
         * Также обратите внимание, что инструкция where используется дважды: до группировки и после
         */

        public IOrderedEnumerable<IGrouping<string, Models.Tutor>> Task5Example()
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
        }//Task5Example()

        /* TODO 5.2 
         * Допишите код метода Task5() для получения данных в соответствии с вариантом
         * Обратите внимание, что ключ группировки имеет тип string
         * Если у вас по заданию ключ числовой - преобразуйте в строку
         * Это связано с настройкой вывода на форму, на самом деле допустим любой тип данных
         */

        public IOrderedEnumerable<IGrouping<string, Models.Student>> Task5()
        {
            return
                null;
        }//Task5()

        /* TODO 6.1 Пример группировки с подсчётом агрегатных функций
         * Функции Count(), Min(), Max(), Average(), Sum()
         * подсчитываются по каждой группе данных (считается длина фамилий)
        */

        public object Task6Example()
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
        }//Task6Example

        /* TODO 6.2
         * Напишите запрос с группировкой и подсчётом агрегатных функций
         * в соответсвии с вариантом задания
         */

        public object Task6()
        {
            return
                null;
        }//Task6

        /* TODO 7.1 Пример группировки с выполнением расчётов по связанным данным.
         * По каждому куратору подсчитывается количество групп и общая длина фамилий студентов без пробелов
         * 
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


        public object Task7Example()
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

        }//Task7Example()

        /* TODO 7.2
         * Напишите запрос с группировкой и подсчётом агрегатных функций
         * в соответсвии с вариантом задания
         */

        //По кожній групі визначити студента, який має найбільшу кількість незданих лабораторних в цій групі,
        //вказати прізвище студента, GROUP_ID, кількість незданих лабораторних, відсортувати за прізвищем студента
        //(примітка: таких студентів може бути кілька, якщо вони мають однакову кількість незданих лабораторних)
        //(273 rows; значення для перевірки: 2)

        public object Task7()
        {
            return
                null;
        }//Query7()


    }//class DataAccess}
}
