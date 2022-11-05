﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotApiTest.ControllerTest
{
    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_post_parking_lot_successfullyAsync()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkingLotDto = new ParkingLotDto
            {
                Name = "parking lot 1",
                Capacity = 10,
                Location = "NYC",
            };

            // when
            var content = GenerateContent(parkingLotDto);
            await client.PostAsync("/parkingLot", content);

            // then
            var allParkingLots = await GetAllParkingLots(client);
            Assert.Single(allParkingLots);
        }

        private async Task<List<ParkingLotDto>> GetAllParkingLots(HttpClient client)
        {
            var response = await client.GetAsync("/parkingLot");
            var body = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);
        }

        private StringContent GenerateContent(ParkingLotDto parkingLotDto)
        {
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            return new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}