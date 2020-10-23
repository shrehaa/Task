using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
u
using Microsoft.AspNetCore.Mvc;

namespace JWT.Controllers
{
    public class GenerateJWTTokenController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Authenticate(UserRequest user)
        {
            var userResponse = new UserResponse { };
            UserRequest userRequest = new UserRequest { };
            userRequest.Username = user.Username.ToLower();
            userRequest.Password = user.Password;

            IHttpActionResult response;
            HttpResponseMessage responseMsg = new HttpResponseMessage();
            bool isUsernamePasswordValid = false;

            if (user != null)
                isUsernamePasswordValid = userRequest.Password == "1234567" ? true : false;
            //if credentials are valid
            if (isUsernamePasswordValid)
            {
                string token = createToken(userRequest.Username);
                //return the token
                return Ok<string>(token);
            }
            else
            {
                // if credentials are not valid send unauthorized status code in response
                userResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                response = ResponseMessage(userResponse.responseMsg);
                return response;
            }
        }

        private string createToken(string username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.UtcNow;
            //set the time when it expires
            DateTime expires = DateTime.UtcNow.AddDays(7);

            
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(
                        issuer: "http://order-api.caradis.fr/api/User/Login",
                        audience: "http://order-api.caradis.fr/api/User/Login",
                        subject: claimsIdentity,
                        notBefore: issuedAt,
                        expires: expires,
                        signingCredentials: signingCredentials
                        );
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}