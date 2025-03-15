global using AutoMapper;
global using FluentValidation;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;
global using Project.API.Errors;
global using Project.API.Extensions;
global using Project.API.Helpers;
global using Project.API.Helpers.Account;
global using Project.API.Middlewares;
global using Project.BLL.Dtos.Account;
global using Project.BLL.Dtos.Authentication;
global using Project.BLL.Dtos.Authorization;
global using Project.BLL.Dtos.Email;
global using Project.BLL.Interfaces;
global using Project.BLL.Repositories;
global using Project.BLL.Services;
global using Project.BLL.Specifications;
global using Project.DAL.Data;
global using Project.DAL.Entities;
global using Project.DAL.Entities.Identity;
global using Project.DAL.Helper;
global using System.ComponentModel.DataAnnotations;
global using System.Net;
global using System.Text;
global using System.Text.Json;
global using IAuthorizationService = Project.BLL.Services.IAuthorizationService;
global using Router = Project.DAL.AppMetaData.Router;



