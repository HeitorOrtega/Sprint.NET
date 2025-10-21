using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Sprint.DTOs; 
using Sprint.Helpers; 
using Sprint.Models;
using Xunit;

namespace Sprint.Tests
{
    public class MotoTests : IClassFixture<WebApplicationFactory<Program>> 
    {
        private readonly HttpClient _client;

        public MotoTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            _client.DefaultRequestHeaders.Add("x-api-key", "12345-API-KEY-MOTOBLU");
        }

        [Fact(DisplayName = "GET /v1/moto deve retornar lista de motos")]
        public async Task GetMotos_DeveRetornarLista()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/moto");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<MotoHateoasDto>>>();
            
            Assert.NotNull(apiResponse);
            Assert.NotNull(apiResponse.Data); 
        }

        [Fact(DisplayName = "POST /v1/moto deve criar uma nova moto")]
        public async Task PostMoto_DeveCriarMoto()
        {
            // Arrange
            var novaMoto = new MotoCreateDto // Usar DTO de criação para o POST
            {
                Cor = "Vermelha",
                Placa = "XYZ1234",
                DataFabricacao = new DateTime(2020, 5, 10)
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/v1/moto", novaMoto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<MotoHateoasDto>>();
            Assert.NotNull(apiResponse);
            
            var motoCriada = apiResponse.Data;
            
            Assert.NotNull(motoCriada); 
            Assert.Equal("XYZ1234", motoCriada.Placa);
            Assert.Equal("Vermelha", motoCriada.Cor);
            
            Assert.True(motoCriada.Id > 0); 
        }

        [Fact(DisplayName = "DELETE /v1/moto/{id} deve remover a moto")]
        public async Task DeleteMoto_DeveRemoverMoto()
        {
            var motoParaDeletar = new MotoCreateDto
            {
                Cor = "Azul",
                Placa = "DEL1234",
                DataFabricacao = new DateTime(2022, 3, 15)
            };

            var createResponse = await _client.PostAsJsonAsync("/api/v1/moto", motoParaDeletar);
            createResponse.EnsureSuccessStatusCode();

            var apiResponse = await createResponse.Content.ReadFromJsonAsync<ApiResponse<MotoHateoasDto>>();
            var motoCriada = apiResponse.Data;
            Assert.NotNull(motoCriada);
            Assert.True(motoCriada.Id > 0); 

            var deleteResponse = await _client.DeleteAsync($"/api/v1/moto/{motoCriada.Id}");

        
            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getAfterDelete = await _client.GetAsync($"/api/v1/moto/{motoCriada.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getAfterDelete.StatusCode);
        }
    }
}
