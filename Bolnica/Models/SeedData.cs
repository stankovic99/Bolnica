using Bolnica.Areas.Identity.Data;
using Bolnica.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bolnica.Models
{
    public class SeedData
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<BolnicaUser>>();
            IdentityResult roleResult;

            //Add Admin Role
            var roleCheck = await RoleManager.RoleExistsAsync("Admin");
            if (!roleCheck) { roleResult = await RoleManager.CreateAsync(new IdentityRole("Admin")); }

            //Add Admin User
            BolnicaUser user = await UserManager.FindByEmailAsync("admin@bolnica.com");
            if (user == null)
            {
                var User = new BolnicaUser();
                User.Email = "admin@bolnica.com";
                User.UserName = "admin@bolnica.com";
                string userPWD = "Admin123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                //Add default User to Role Admin
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Admin"); }
            }

            //Add Teacher Role
            roleCheck = await RoleManager.RoleExistsAsync("Doktor");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Doktor"));
            }

            user = await UserManager.FindByEmailAsync("doktor@bolnica.com");
            if (user == null)
            {
                var User = new BolnicaUser();
                User.Email = "doktor@bolnica.com";
                User.UserName = "doktor@bolnica.com";

                User.DoktorId = 1;
                string userPWD = "Doktor123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Doktor"); }
            }

            //Add Student Role
            roleCheck = await RoleManager.RoleExistsAsync("Pacient");
            if (!roleCheck)
            {
                roleResult = await RoleManager.CreateAsync(new IdentityRole("Pacient"));
            }

            user = await UserManager.FindByEmailAsync("pacient@bolnica.com");
            if (user == null)
            {
                var User = new BolnicaUser();
                User.Email = "pacient@bolnica.com";
                User.UserName = "pacient@bolnica.com";

                User.PacientId = 2;
                string userPWD = "Pacient123";
                IdentityResult chkUser = await UserManager.CreateAsync(User, userPWD);
                if (chkUser.Succeeded) { var result1 = await UserManager.AddToRoleAsync(User, "Pacient"); }
            }
        }
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BolnicaContext(
            serviceProvider.GetRequiredService<
            DbContextOptions<BolnicaContext>>()))
            {
                CreateUserRoles(serviceProvider).Wait();
                // Look for any movies.
                if (context.Doktor.Any() || context.Pacient.Any())
                {
                    return; // DB has been seeded
                }
                context.Doktor.AddRange(
                new Doktor { Ime = "Христо", Prezime = "Пејков", Zvanje = "Кардиохирург", Vozrast = 56, DataVrabotuvanje = DateTime.Parse("1985-3-6") },
                new Doktor { Ime = "Жан", Prezime = "Митрев", Zvanje = "Кардиохирург", Vozrast = 60, DataVrabotuvanje = DateTime.Parse("1984-7-12") },
                new Doktor { Ime = "Петко", Prezime = "Петковски", Zvanje = "Болничар", Vozrast = 26, DataVrabotuvanje = DateTime.Parse("1993-5-20") },
                new Doktor { Ime = "Петра", Prezime = "Петровска", Zvanje = "Медицинска сестра", Vozrast = 30, DataVrabotuvanje = DateTime.Parse("1990-10-25") },
                new Doktor { Ime = "Марко", Prezime = "Марковски", Zvanje = "Дерматолог", Vozrast = 42, DataVrabotuvanje = DateTime.Parse("1995-11-10") },
                new Doktor { Ime = "Стефан", Prezime = "Станковиќ", Zvanje = "Болничар", Vozrast = 30, DataVrabotuvanje = DateTime.Parse("1999-12-1") },
                new Doktor { Ime = "Марта", Prezime = "Мартовска", Zvanje = "Медицинска сестра", Vozrast = 36, DataVrabotuvanje = DateTime.Parse("1988-3-23") }
                );
                context.SaveChanges();
                context.Pacient.AddRange(
                new Pacient { Ime = "Billy", Prezime = "Crystal", Vozrast = 12, PriemenDatum = DateTime.Parse("2018-3-14") },
                new Pacient { Ime = "Meg", Prezime = "Ryan", Vozrast = 18, PriemenDatum = DateTime.Parse("2013-11-9") },
                new Pacient { Ime = "Carrie", Prezime = "Fisher", Vozrast = 20, PriemenDatum = DateTime.Parse("2019-10-12") },
                new Pacient { Ime = "Bill", Prezime = "Murray", Vozrast = 56, PriemenDatum = DateTime.Parse("2021-9-21") },
                new Pacient { Ime = "Dan", Prezime = "Aykroyd", Vozrast = 33, PriemenDatum = DateTime.Parse("2000-7-1") },
                new Pacient { Ime = "Sigourney", Prezime = "Weaver", Vozrast = 28, PriemenDatum = DateTime.Parse("2017-11-9") },
                new Pacient { Ime = "John", Prezime = "Wayne", Vozrast = 15, PriemenDatum = DateTime.Parse("2012-5-26") },
                new Pacient { Ime = "Dean", Prezime = "Martin", Vozrast = 38, PriemenDatum = DateTime.Parse("2015-6-7") }
                );
                context.SaveChanges();

                context.LekuvanPacient.AddRange
                (
                new LekuvanPacient { PacientId = 1, DoktorId = 1, Lek = "Оспен" },
                new LekuvanPacient { PacientId = 2, DoktorId = 1, Lek = "Амоксиклав" },
                new LekuvanPacient { PacientId = 3, DoktorId = 2, Lek = "Линукс" },
                new LekuvanPacient { PacientId = 4, DoktorId = 2, Lek = "Оспен" },
                new LekuvanPacient { PacientId = 5, DoktorId = 2, Lek = "Вентор" },
                new LekuvanPacient { PacientId = 6, DoktorId = 3, Lek = "Амоксиклав" },
                new LekuvanPacient { PacientId = 4, DoktorId = 3, Lek = "Бруфен" },
                new LekuvanPacient { PacientId = 5, DoktorId = 7, Lek = "Диазепам" },
                new LekuvanPacient { PacientId = 6, DoktorId = 7, Lek = "Амоксиклав" },
                new LekuvanPacient { PacientId = 7, DoktorId = 4, Lek = "Аналгин" },
                new LekuvanPacient { PacientId = 8, DoktorId = 4, Lek = "Вентор" },
                new LekuvanPacient { PacientId = 1, DoktorId = 5, Lek = "Амоксиклав" },
                new LekuvanPacient { PacientId = 2, DoktorId = 5, Lek = "Вентор" },
                new LekuvanPacient { PacientId = 3, DoktorId = 6, Lek = "Аналгин" },
                new LekuvanPacient { PacientId = 4, DoktorId = 6, Lek = "Оспен" },
                new LekuvanPacient { PacientId = 5, DoktorId = 6, Lek = "Аналгин" }
                );
                context.SaveChanges();
            }
        }
    }
}
