﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab4.Data;
using Lab4.Models;
using Lab4.Service;

namespace Lab4.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly CachedDataService _cachedDataService;

        public SubscribersController(CachedDataService cachedDataService)
        {
            _cachedDataService = cachedDataService;
        }

        // GET: Subscribers
        public async Task<IActionResult> Index()
        {
            return View(_cachedDataService.GetSubscribers());
        }
    }
}