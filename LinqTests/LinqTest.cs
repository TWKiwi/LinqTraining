using System;
using ExpectedObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using LinqSample.WithoutLinq;

namespace LinqTests
{
    [TestClass]
    public class LinqTest
    {
        [TestMethod]
        public void find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts().ToList();
            var actual = WithoutLinq.Find(products);
            var expected = new List<Product>()
            {
                new Product {Id = 2, Cost = 21, Price = 210, Supplier = "Yahoo"},
                new Product {Id = 3, Cost = 31, Price = 310, Supplier = "Odd-e"},
                new Product {Id = 4, Cost = 41, Price = 410, Supplier = "Odd-e"},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_products_that_price_between_200_and_500_and_cost_more_than_30()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = WithoutLinq.Find(products, p => p.Price > 200 && p.Price < 500 && p.Cost > 30);
            var expected = new List<Product>()
            {
                //new Product {Id = 2, Cost = 21, Price = 210, Supplier = "Yahoo"},
                new Product {Id = 3, Cost = 31, Price = 310, Supplier = "Odd-e"},
                new Product {Id = 4, Cost = 41, Price = 410, Supplier = "Odd-e"},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_employee_that_age_more_30_item_index_more_2()
        {
            var employees = RepositoryFactory.GetEmployees();
            //var actual = WithoutLinq.Find<Employee>(employees, (e, index) => e.Age > 30 && index >= 2);
            var actual = employees.Find<Employee>((e, index) => e.Age > 30 && index >= 2);
            var expected = new List<Employee>()
            {
                //new Employee {Name = "Joe", Role = RoleType.Engineer, MonthSalary = 100, Age = 44, WorkingYear = 2.6},
                //new Employee {Name = "Tom", Role = RoleType.Engineer, MonthSalary = 140, Age = 33, WorkingYear = 2.6},
                new Employee {Name = "Kevin", Role = RoleType.Manager, MonthSalary = 380, Age = 55, WorkingYear = 2.6},
                //new Employee {Name = "Andy", Role = RoleType.OP, MonthSalary = 80, Age = 22, WorkingYear = 2.6},
                new Employee {Name = "Bas", Role = RoleType.Engineer, MonthSalary = 280, Age = 36, WorkingYear = 2.6},
                //new Employee {Name = "Mary", Role = RoleType.OP, MonthSalary = 180, Age = 26, WorkingYear = 2.6},
                //new Employee {Name = "Frank", Role = RoleType.Engineer, MonthSalary = 120, Age = 16, WorkingYear = 2.6},
                new Employee {Name = "Joey", Role = RoleType.Engineer, MonthSalary = 250, Age = 40, WorkingYear = 2.6},
            };

            //foreach (var item in actual)
            //{
            //    Console.WriteLine(item.Price);
            //}

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void YourWhere_YourSelect()
        {
            var employees = RepositoryFactory.GetEmployees();

            var actual = employees
                .YourWhere(e => e.Age < 25)
                .YourSelect(e => $"{e.Role}:{e.Name}");

            foreach (var titleName in actual)
            {
                Console.WriteLine(titleName);
            }

            var expected = new List<string>()
            {
                "OP:Andy",
                "Engineer:Frank",
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void Take()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = WithoutLinq.Take(employees, 2);
            var expected = new List<Employee>
            {
                new Employee {Name = "Joe", Role = RoleType.Engineer, MonthSalary = 100, Age = 44, WorkingYear = 2.6},
                new Employee {Name = "Tom", Role = RoleType.Engineer, MonthSalary = 140, Age = 33, WorkingYear = 2.6},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void Skip()
        {
            var employees = RepositoryFactory.GetEmployees();
            var act = WithoutLinq.YourSkip(employees, 6);
            var expected = new List<Employee>
            {
                new Employee {Name = "Frank", Role = RoleType.Engineer, MonthSalary = 120, Age = 16, WorkingYear = 2.6},
                new Employee {Name = "Joey", Role = RoleType.Engineer, MonthSalary = 250, Age = 40, WorkingYear = 2.6},
            };

            expected.ToExpectedObject().ShouldEqual(act.ToList());
        }

        [TestMethod]
        public void TakeWhile()
        {
            var employees = RepositoryFactory.GetEmployees();
            var act = WithoutLinq.YourTakeWhile(employees, 2, e => e.MonthSalary > 150);
            var expected = new List<Employee>
            {
                new Employee {Name = "Kevin", Role = RoleType.Manager, MonthSalary = 380, Age = 55, WorkingYear = 2.6},
                new Employee {Name = "Bas", Role = RoleType.Engineer, MonthSalary = 280, Age = 36, WorkingYear = 2.6},
            };
            expected.ToExpectedObject().ShouldEqual(act.ToList());
        }

        [TestMethod]
        public void SkipWhile()
        {
            var employees = RepositoryFactory.GetEmployees();
            var act = WithoutLinq.YourSkipWhile(employees, 3, e => e.MonthSalary < 150);
            var expected = new List<Employee>
            {
                new Employee {Name = "Kevin", Role = RoleType.Manager, MonthSalary = 380, Age = 55, WorkingYear = 2.6},
                new Employee {Name = "Bas", Role = RoleType.Engineer, MonthSalary = 280, Age = 36, WorkingYear = 2.6},
                new Employee {Name = "Mary", Role = RoleType.OP, MonthSalary = 180, Age = 26, WorkingYear = 2.6},
                new Employee {Name = "Frank", Role = RoleType.Engineer, MonthSalary = 120, Age = 16, WorkingYear = 2.6},
                new Employee {Name = "Joey", Role = RoleType.Engineer, MonthSalary = 250, Age = 40, WorkingYear = 2.6},
            };
            expected.ToExpectedObject().ShouldEqual(act.ToList());
        }

        // 需求是希望將employees每三筆資料分成一個group並將group裡的salary sum起來。
        // 最後若剩不到3筆資料就將剩下資料分成一組。
        [TestMethod]
        public void groupSalary()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = WithoutLinq.YourGroup(employees, 3, e => e.MonthSalary);

            var expected = new List<int>()
            {
                620,
                540,
                370
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void First()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = WithoutLinq.YourFirst(employees, e => e.Age > 30).ToList().First();

            var expected = new Employee
            {
                Name = "Joe",
                Role = RoleType.Engineer,
                MonthSalary = 100,
                Age = 44,
                WorkingYear = 2.6
            };


            expected.ToExpectedObject().ShouldEqual(actual);
        }

        [TestMethod]
        public void Last()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = WithoutLinq.YourLast(employees, e => e.Age > 30).ToList().First();

            var expected = new Employee
            {
                Name = "Joey",
                Role = RoleType.Engineer,
                MonthSalary = 250,
                Age = 40,
                WorkingYear = 2.6
            };


            expected.ToExpectedObject().ShouldEqual(actual);
        }
    }
}