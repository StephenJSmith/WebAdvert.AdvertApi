﻿using AdvertApi.Models;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvertApi.Controllers
{
  [ApiController]
  [Route("adverts/v1")]
  public class Advert : ControllerBase
  {
    private readonly IAdvertStorageService _advertStorageService;

    public Advert(IAdvertStorageService advertStorageService)
    {
      _advertStorageService = advertStorageService;
    }

    [HttpPost]
    [Route("Create")]
    [ProducesResponseType(404)]
    [ProducesResponseType(200, Type = typeof(CreateAdvertResponse))]
    public async Task<IActionResult> Create(AdvertModel model)
    {
      string recordId;
      try
      {
        recordId = await _advertStorageService.Add(model);
      }
      catch (KeyNotFoundException)
      {
        return new NotFoundResult();
      }
      catch (System.Exception exception)
      {
        return StatusCode(500, exception.Message);
      }

      return StatusCode(201, new CreateAdvertResponse { Id = recordId });
    }

    [HttpPut]
    [Route("Confirm")]
    [ProducesResponseType(404)]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Confirm(ConfirmAdvertModel model)
    {
      try
      {
        await _advertStorageService.Confirm(model);
      }
      catch (KeyNotFoundException)
      {
        return new NotFoundResult();
      }
      catch (System.Exception exception)
      {
        return StatusCode(500, exception.Message);
      }

      return new OkResult();
    }
  }
}
