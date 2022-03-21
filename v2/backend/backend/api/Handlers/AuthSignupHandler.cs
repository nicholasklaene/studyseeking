using System.Net;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using api.Commands;
using api.Data;
using api.Models;
using api.Response;
using MediatR;

namespace api.Handlers;

public class AuthSignupHandler : IRequestHandler<AuthSignupCommand, AuthSignupResponse>
{
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly AmazonCognitoIdentityProviderClient _identityClient;

    public AuthSignupHandler(
        ApplicationDbContext db,
        IConfiguration configuration,
        AmazonCognitoIdentityProviderClient identityClient
    )
    {
        _db = db;
        _configuration = configuration;
        _identityClient = identityClient;
    }
    
    public async Task<AuthSignupResponse> Handle(AuthSignupCommand request, CancellationToken cancellationToken)
    {
        var signupRequest = new SignUpRequest()
        {
            ClientId = _configuration["AWSCognito:AppClientId"],
            Username = request.Username,
            Password = request.Password 
        };

        var emailAttribute = new AttributeType()
        {
            Name = "email",
            Value = request.Email
        };
        
        signupRequest.UserAttributes.Add(emailAttribute);

        var response = new AuthSignupResponse();
        try
        {
            await _identityClient.SignUpAsync(signupRequest, cancellationToken);

            var confirmSignup = new AdminConfirmSignUpRequest()
            {
                Username = request.Username,
                UserPoolId = _configuration["AWSCognito:PoolId"]
            };

            var confirmResponse = await _identityClient.AdminConfirmSignUpAsync(confirmSignup, cancellationToken);

            if (confirmResponse.HttpStatusCode != HttpStatusCode.OK)
            {
                response.Errors.Add("Error confirming user");
            }
            
            _db.Users.Add(new User() {Username = request.Username, Email = request.Email});
            var numChanges = await _db.SaveChangesAsync(cancellationToken);

            if (numChanges == 0)
            {
                response.Errors.Add("Error creating user");
            }
        }
        catch (UsernameExistsException usernameExistsException)
        {
            response.Errors.Add("Username taken");
        }

        return response;
    }
}