using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxi.Common.Enums;
using Taxi.Web.Data.Entities;
using Taxi.Web.Helpers;

namespace Taxi.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        public SeedDb(DataContext dataContext, IUserHelper userHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
        }
        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRolesAnsync();
            var admin = await CheckUserAsync("1010", "Abel", "Hernandez", "abelhr@yahoo.com.mx", "350 634 2747", "Calle Luna Calle Sol", UserType.Admin);
            var driver = await CheckUserAsync("2020", "Abel", "Hernandez", "heracomp@outlook.com", "350 634 2747", "Calle Luna Calle Sol", UserType.Driver);
            var user1 = await CheckUserAsync("3030", "Abel", "Hernandez", "abelhr@gmail.com", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            var user2 = await CheckUserAsync("3030", "Eliezer", "Wolkoff", "Wolkoff_e@yahoo.com.mx", "350 634 2747", "Calle Luna Calle Sol", UserType.User);
            await CheckTaxiAsync(driver,user1,user2);
        }

        private async Task<UserEntity> CheckUserAsync(
            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new UserEntity
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    UserType = userType
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task CheckRolesAnsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.Driver.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task CheckTaxiAsync(UserEntity driver,UserEntity user1,UserEntity user2)
        {
            if (_dataContext.Taxis.Any())
            {
                return;
            }
            _dataContext.Taxis.Add(new TaxiEntity
            {
                User=driver,
                Plaque = "TPQ123",
                Trips = new List<TripEntity>
                {
                    new TripEntity
                    {
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMinutes(30),
                        Qualification = 4.5f,
                        Source = "ITM Fraternidad",
                        Target = "ITM Robledo",
                        Remarks = "Muy buen servicio",
                        User=user1
                    },
                    new TripEntity
                    {
                        StartDate = DateTime.UtcNow,
                        EndDate = DateTime.UtcNow.AddMinutes(30),
                        Qualification = 4.8f,
                        Source = "ITM Robledo",
                        Target = "ITM Fraternidad",
                        Remarks = "Conductor muy amable",
                        User=user1

                    }
                }
            });

            _dataContext.Taxis.Add(new TaxiEntity
            {
                User = driver,
                Plaque = "THW321",
                Trips = new List<TripEntity>
                    {
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.5f,
                            Source = "ITM Fraternidad",
                            Target = "ITM Robledo",
                            Remarks = "Muy buen servicio",
                            User=user2
                        },
                        new TripEntity
                        {
                            StartDate = DateTime.UtcNow,
                            EndDate = DateTime.UtcNow.AddMinutes(30),
                            Qualification = 4.8f,
                            Source = "ITM Robledo",
                            Target = "ITM Fraternidad",
                            Remarks = "Conductor muy amable",
                            User=user2
                        }
                    }
            }); ;

            await _dataContext.SaveChangesAsync();
        }
    }
}
