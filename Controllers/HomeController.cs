using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LoginRegistration.Models;

namespace LoginRegistration.Controllers
{
    public class HomeController : Controller
    {
        private readonly DbConnector _dbConnector;
        public HomeController(DbConnector connect){
            _dbConnector = connect;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index(){
            return View("Index");
        }

        [HttpGet]
        [Route("/success")]
        public IActionResult Success(){
            return View("Success");
        }

        [HttpPost]
        [Route("/register")]
        public IActionResult Register(Users Reg){
            if(ModelState.IsValid){
                _dbConnector.Execute($"INSERT INTO users(first_name, last_name, email, password) VALUES('{Reg.FirstName}', '{Reg.LastName}', '{Reg.Email}', '{Reg.Password}');");
                TempData["register"] = "Registered";
                return RedirectToAction("Success");
            }
            return View("Index");
        }

        [HttpPost]
        [Route("/login")]
        public IActionResult Login(Users Login){
            List<Dictionary<string,object>> Allusers = _dbConnector.Query($"SELECT * FROM users WHERE email = '{Login.Email}';");
            Console.WriteLine(Allusers);
            foreach(var user in Allusers){
                if((string)user["password"] == Login.Password){
                    TempData["login"] = "Login";
                    return RedirectToAction("Success");
                }
            }
            return View("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
