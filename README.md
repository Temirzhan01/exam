            {
    "field": "value"
} отправляю это
        
        public async Task<IActionResult> ProduceStatus([FromBody]JsonElement body) 
        {
            var message = JsonConvert.SerializeObject(body); тут он превращается в         {"ValueKind":1}
            if (message == null) 
            {
                return BadRequest("Null data");
            }
            await _service.ProduceStatus(message);
            return Ok();
        }
Почему так происходит?
