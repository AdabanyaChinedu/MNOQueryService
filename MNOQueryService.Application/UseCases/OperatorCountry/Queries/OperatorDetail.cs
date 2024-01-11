using MNOQueryService.Application.UseCases.MNOQuerys.ViewModels;
using MediatR;
using MNOQueryService.SharedLibrary.Model.ResponseModel;
using FluentValidation;
using MNOQueryService.Domain.Interfaces;
using MNOQueryService.SharedLibrary.Exceptions;
using MNOQueryService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MNOQueryService.SharedLibrary.Model.AppSettings;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace MNOQueryService.Application.UseCases.MNOQuerys.Queries
{

    public class OperatorDetail
    {
        public record Query(string phoneNumber) : IRequest<OperatorResponse>;

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.phoneNumber)
                     .NotEmpty()
                     .WithMessage("PhoneNumber is required.")
                     .Length(13)
                     .WithMessage("Invalid PhoneNumber.");
            }
        }

        public class QueryHandler : IRequestHandler<Query, OperatorResponse>
        {
            private readonly IMNODbContext mNODbContext;
            private readonly IConfiguration configuration;
            private readonly IRedisService redisService;
            private readonly AppOptimization appOptimization;
            private readonly IMapper mapper;

            public QueryHandler(IMNODbContext mNODbContext,
                IRedisService redisService,
                AppOptimization appOptimization,
                IMapper mapper)
            {
                this.mNODbContext = mNODbContext;
                this.redisService = redisService;
                this.appOptimization = appOptimization;
                this.mapper = mapper;
            }

            public async Task<OperatorResponse> Handle(Query request, CancellationToken cancellationToken)
            {
                bool optimizeQuery = this.appOptimization.RedisSettings.OptimizeOperatorQuery;
                int cacheTimeInMinute = this.appOptimization.RedisSettings.CacheTimeInMinute;

                string countryCode = request.phoneNumber.Substring(0, 3);

                try
                {
                    // Set OptimizeOperatorQuery to false in appsettings to disable caching
                    var cacheResult = optimizeQuery ? await this.redisService.GetAsync<CountryDto>(countryCode) : default;

                    if (cacheResult != default)
                    {
                        return BuildResponse(cacheResult, request.phoneNumber);
                    }


                    var dbResult = await this.mNODbContext.Countries
                               .Include(c => c.Operators)
                               .FirstOrDefaultAsync(c => c.CountryCode == countryCode, cancellationToken);

                    if (dbResult == null)
                    {
                        throw new EntityNotFoundException($"Operator details for {request.phoneNumber} does not exist");
                    }

                    var countryDto = this.mapper.Map<CountryDto>(dbResult);

                    if (optimizeQuery)
                    {
                        await this.redisService.SetAsync(countryCode, countryDto, cacheTimeInMinute);
                    }

                    return BuildResponse(countryDto, request.phoneNumber);
                }
                catch (EntityNotFoundException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {                    
                    throw new Exception("An error occured while trying to fetch operator details. Kindly contact Administrator");
                }
            }

            private OperatorResponse BuildResponse(CountryDto country, string phonenumber)
            {
               return new OperatorResponse
                {
                    Number = phonenumber,
                    Country = new OperatorCountryResponse
                    {
                        CountryCode = country.CountryCode,
                        Name = country.Name,
                        CountryIso = country.CountryIso,
                        CountryDetails = country.Operators.Select(o => new OperatorCountryDetailsResponse
                        {
                            Operator = o.Operator,
                            OperatorCode = o.OperatorCode
                        }).ToList()
                    }
                };

            }
        }
    }
}
