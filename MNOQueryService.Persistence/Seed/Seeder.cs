

using MNOQueryService.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using MNOQueryService.Domain.Entities;
using MNOQueryService.Domain.EqualityComparer;
using MNOQueryService.Domain.Interfaces;
using System.Text.Json;

namespace MNOQueryService.Persistence.Seed
{
    public static class Seeder
    {
        public static async Task SeedDataAsync(IMNODbContext context)
        {
            await SeedCountryOperatorsAsync(context);
        }

        private static async Task SeedCountryOperatorsAsync(IMNODbContext context)
        {
            var countriesInFile = await LoadFileFromJsonAsync<CountryDto>("countries");

            if (!countriesInFile.Any())
            {
                return;
            }

            var operatorsInFile = await LoadFileFromJsonAsync<OperatorDto>("operators");

            List<Country> countries = new List<Country>();

            foreach (var country in countriesInFile)
            {
                var countryModel = new Country(country.Id, country.Name, country.CountryCode, country.CountryIso);

                 var operators = operatorsInFile
                    .Where(x => x.CountryId == country.Id)
                    .Select(x => new NetworkOperator(x.Id, x.Operator, x.OperatorCode))
                    .AsQueryable();

                foreach(var @operator in operators)
                {
                    countryModel.Operators.Add(@operator);
                }

                countries.Add(countryModel);
            }

            var existingCountriesInDb = await context.Countries.AsNoTracking().ToListAsync();

            var newCountries = countries
                .Except(existingCountriesInDb, new CountryComparer()).ToList();

            if (newCountries.Count > 0)
            {
                await context.Countries.AddRangeAsync(newCountries);
                await context.SaveChangesAsync();
            }
        }


        private static async Task<IReadOnlyCollection<TResponse>> LoadFileFromJsonAsync<TResponse>(string fileName)
            where TResponse : class
        {
            var jsonResult = await File.ReadAllTextAsync(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Seed", "JsonFiles", $"{fileName}.json"));

            return JsonSerializer.Deserialize<IReadOnlyCollection<TResponse>>(jsonResult);
        }
    }
}
