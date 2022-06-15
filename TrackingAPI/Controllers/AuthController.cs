﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackingAPI.Controllers
{
    [Route("api/[controller]")]
     [ApiController]
     [Authorize]  // For authorization
    public class AuthController : ControllerBase
    {
    }
}
