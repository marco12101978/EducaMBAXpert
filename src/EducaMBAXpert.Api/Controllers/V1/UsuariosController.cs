using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.Api.Interfaces;
using EducaMBAXpert.Api.ViewModels.User;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Usuarios.Application.Services;
using EducaMBAXpert.Usuarios.Application.ViewModels;
using EducaMBAXpert.Usuarios.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace EducaMBAXpert.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/usuarios")]
    public class UsuariosController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly IMediator _mediator;


        public UsuariosController(SignInManager<IdentityUser> signInManager,
                                  UserManager<IdentityUser> userManager,
                                  RoleManager<IdentityRole> roleManager,
                                  IOptions<JwtSettings> jwtSettings,
                                  IAppIdentityUser appIdentityUser,
                                  NotificationContext _notificationContext ,
                                  IMediator mediator,
                                  IUsuarioAppService usuarioAppService) : base(mediator, _notificationContext, appIdentityUser)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _usuarioAppService = usuarioAppService;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        [SwaggerOperation(Summary = "Registra um novo usuário", Description = "Cria um novo usuário com os dados fornecidos e retorna um token JWT.")]
        [ProducesResponseType(typeof(RegisterUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (registerUser == null)
                NotificarErro("Falha ao registrar o usuário");

            if (!ModelState.IsValid) return ValidationProblem(ModelState);


            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);



            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("USER"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("USER"));
                }

                await _userManager.AddToRoleAsync(user, "USER");

                await _signInManager.SignInAsync(user, false);

                var usuario = new UsuarioViewModel(id: Guid.Parse(user.Id) ,nome: registerUser.Nome, email: user.Email , ativo:true);
                await _usuarioAppService.Adicionar(usuario);

                return CustomResponse(HttpStatusCode.OK,(await GerarJwt(user.Email)));
            }

            NotificarErro("Falha ao registrar o usuário");
            return CustomResponse(HttpStatusCode.InternalServerError);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Realiza o login do usuário", Description = "Autentica o usuário e retorna um token JWT.")]
        [ProducesResponseType(typeof(LoginUserViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (result.Succeeded)
            {
                //return Ok(await GerarJwt(loginUser.Email));
                return CustomResponse(HttpStatusCode.OK, await GerarJwt(loginUser.Email));
            }

            NotificarErro("Usuário ou senha incorretos");
            return CustomResponse(HttpStatusCode.NotFound);

        }


        [HttpPost("{id:guid}/enderecos")]
        [SwaggerOperation(Summary = "Adiciona um endereço ao usuário", Description = "Adiciona um novo endereço ao usuário informado.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AdicionarEndereco(Guid id, [FromBody] EnderecoViewModel endereco)
        {
            var usuario = await ObterUsuario(id);
            if (usuario == null)
            {
                NotificarErro("Usuário não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            endereco.UsuarioId = id;

            await _usuarioAppService.AdicionarEndereco(endereco);
            return CustomResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Obtém todos os usuários", Description = "Retorna uma lista com todos os usuários.")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioViewModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterTodos()
        {
            var usuarios = await _usuarioAppService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, usuarios);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter{id:guid}")]
        [SwaggerOperation(Summary = "Obtém um usuário por ID", Description = "Retorna os dados de um usuário específico.")]
        [ProducesResponseType(typeof(UsuarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterPorId(Guid id)
        {
            var usuario = await _usuarioAppService.ObterPorId(id);
            if (usuario == null)
            {
                NotificarErro("Usuário não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.OK, usuario);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}/inativar")]
        [SwaggerOperation(Summary = "Inativa um usuário", Description = "Inativa o usuário informado.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Inativar(Guid id)
        {
            var usuario = await ObterUsuario(id);
            if (usuario == null)
            {
                NotificarErro("Usuário não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var resultado = await _usuarioAppService.Inativar(id);
            if (!resultado)
            {
                NotificarErro("Não foi possível inativar o usuário.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}/ativar")]
        [SwaggerOperation(Summary = "Ativa um usuário", Description = "Ativa o usuário informado.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Ativar(Guid id)
        {
            var usuario = await ObterUsuario(id);
            if (usuario == null)
            {
                NotificarErro("Usuário não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var resultado = await _usuarioAppService.Ativar(id);
            if (!resultado)
            {
                NotificarErro("Não foi possível ativar o usuário.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            return CustomResponse(HttpStatusCode.NoContent);
        }


        private async Task<string> GerarJwt(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new (JwtRegisteredClaimNames.Sub, user.Id),
                new (ClaimTypes.Name, user.UserName ?? ""),
                new (ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            // Adicionar roles como claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }

        private async Task<UsuarioViewModel?> ObterUsuario(Guid id)
        {
            UsuarioViewModel usuario = await _usuarioAppService.ObterPorId(id);
            if (usuario == null)
            {
                return null;
            }

            return usuario;
        }
    }
}
