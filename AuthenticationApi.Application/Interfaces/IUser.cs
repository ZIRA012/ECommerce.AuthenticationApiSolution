﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthenticationApi.Application.DTOs;
using ECommmerce.SharedLibrary.Responses;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IUser
    {
        Task<Response> Register(AppUserDTO appUserDTO);

        Task<Response> Login(LoginDTO loginDTO);

        Task<GetUserDTO> GetUser(int userId);
    }
}
