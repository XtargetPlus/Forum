﻿namespace Forum.API.Dtos.Requests;

public class SignInRequest
{
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}
