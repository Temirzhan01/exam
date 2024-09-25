  Message: 
System.InvalidCastException : Unable to cast object of type 'Microsoft.AspNetCore.Mvc.NoContentResult' to type 'Microsoft.AspNetCore.Mvc.BadRequestResult'.

  Stack Trace: 
TestDatabaseCheckController.CheckTerrorAsync_ShouldReturn400BadRequestStatus() line 63
--- End of stack trace from previous location ---

        [Fact]
        public async Task CheckTerrorAsync_ShouldReturn400BadRequestStatus()
        {
            /// Arrange
            _checkServiceMock.Setup(service => service.CheckTerrorAsync(null));
            var _controller = new DatabaseCheckController(_checkServiceMock.Object, _loggerMock.Object);

            /// Act
            var result = (BadRequestResult)await _controller.OnlineKK(null);

            /// Assert
            result.StatusCode.Should().Be(400);
        }

         [HttpPost]
        [Route("OnlineKK")]
        public async Task<IActionResult> OnlineKK(CheckTerror model)
        {
            _logger.LogInfoCustom("OnlineKK", "Input params", string.Empty, string.Empty, model);
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            var result = await _checkService.CheckTerrorAsync(model);
            if (result == null) 
            {
                return NoContent();
            }
            return Ok(result);
        }
