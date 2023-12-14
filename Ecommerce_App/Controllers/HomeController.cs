using System;
using System.Net.Http;
using System.Threading.Tasks;
using Ecommerce_App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace ECommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly Uri _baseAddress;
        private readonly HttpClient _httpClient;

        public HomeController()
        {
            _baseAddress = new Uri("https://localhost:7286/api/Ecommerce/");
            _httpClient = new HttpClient { BaseAddress = _baseAddress };
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("Login", model);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        // Deserialize the response content to get the login response
                        var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                        if (loginResponse.Success)
                        {
                            // Redirect based on the user's role
                            switch (loginResponse.Role?.ToLower())
                            {
                                case "admin":
                                    return RedirectToAction("AdminIndex");
                                case "inventory":
                                    return RedirectToAction("InventoryIndex");
                                case "user":
                                    return RedirectToAction("UserIndex");
                                default:
                                    // Log the unexpected role value
                                    Console.WriteLine($"Unexpected role value: {loginResponse.Role}");

                                    // Handle other roles or unexpected values
                                    return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or print the exception details
                        Console.WriteLine($"Error deserializing response content: {ex.Message}");
                        ModelState.AddModelError(string.Empty, "Error during login attempt.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error during login attempt.");
                }
            }

            return View(model);
        }

        public IActionResult AdminIndex()
        {
            // Action for admin role
            return View();
        }

        public IActionResult InventoryIndex()
        {
            // Action for inventory role
            return View();
        }

        public IActionResult UserIndex()
        {
            // Action for user role
            return View();
        }
    }
}
