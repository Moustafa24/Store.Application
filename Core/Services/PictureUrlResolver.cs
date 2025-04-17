﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Execution;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Shared;

namespace Services
{
    public class PictureUrlResolver(IConfiguration configuration) : IValueResolver<Product, ProductResultDto, string>
    {
        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl)) return string.Empty;
            
            return $"{configuration["basUrl"]}/{source.PictureUrl}"; 
        }
    }
}
