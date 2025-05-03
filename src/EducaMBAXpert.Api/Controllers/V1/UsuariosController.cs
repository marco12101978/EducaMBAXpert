using EducaMBAXpert.Api.Authentication;
using EducaMBAXpert.Api.Interfaces;
using EducaMBAXpert.Api.ViewModels.User;
using EducaMBAXpert.Core.Messages.CommonMessages.Notifications;
using EducaMBAXpert.Usuarios.Application.Interfaces;
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
        private readonly IUsuarioConsultaAppService _usuarioConsultaAppService;
        private readonly IUsuarioComandoAppService _usuarioComandoAppService;
        private readonly IMediator _mediator;


        public UsuariosController(SignInManager<IdentityUser> signInManager,
                                  UserManager<IdentityUser> userManager,
                                  RoleManager<IdentityRole> roleManager,
                                  IOptions<JwtSettings> jwtSettings,
                                  IAppIdentityUser appIdentityUser,
                                  NotificationContext _notificationContext ,
                                  IMediator mediator,
                                  IUsuarioConsultaAppService usuarioConsultaAppService,
                                  IUsuarioComandoAppService usuarioComandoAppService) : base(mediator, _notificationContext, appIdentityUser)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = jwtSettings.Value;
            _usuarioConsultaAppService = usuarioConsultaAppService;
            _usuarioComandoAppService = usuarioComandoAppService;   
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost("registrar")]
        [SwaggerOperation(Summary = "Registra um novo usuário", Description = "Cria um novo usuário e retorna um token JWT.")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar(RegisterUserViewModel registerUser)
        {
            if (registerUser == null)
                return CustomResponse(HttpStatusCode.BadRequest, "Falha ao registrar o usuário");

            if (!ModelState.IsValid) 
                return ValidationProblem(ModelState);


            var user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, registerUser.Password);

            if (!result.Succeeded)
            {
                NotificarErro("Falha ao registrar o usuário.");
                return CustomResponse(HttpStatusCode.InternalServerError);
            }


            if (!await _roleManager.RoleExistsAsync("USER"))
                await _roleManager.CreateAsync(new IdentityRole("USER"));

            await _userManager.AddToRoleAsync(user, "USER");
            await _signInManager.SignInAsync(user, false);

            var usuario = new UsuarioInputModel(id: Guid.Parse(user.Id) ,nome: registerUser.Nome, email: user.Email , ativo:true);
            await _usuarioComandoAppService.Adicionar(usuario);

            return CustomResponse(HttpStatusCode.OK,(await GerarJwt(user.Email)));

        }

        [AllowAnonymous]
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Autentica o usuário e retorna o token JWT")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login(LoginUserViewModel loginUser)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

            if (!result.Succeeded)
            {
                NotificarErro("Usuário ou senha incorretos.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var jwtResult = await GerarJwt(loginUser.Email);
            var user = await _userManager.FindByEmailAsync(loginUser.Email);

            var response = new
            {
                UserId = user?.Id,
                Token = jwtResult
            };
            return CustomResponse(HttpStatusCode.OK, response);

        }


        [HttpPost("{id:guid}/enderecos")]
        [SwaggerOperation(Summary = "Adiciona um endereço ao usuário")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AdicionarEndereco(Guid id, [FromBody] EnderecoInputModel endereco)
        {
            var usuario = await ObterUsuario(id);
            if (usuario == null)
                return NotFoundResponse("Usuário não encontrado.");

            endereco.UsuarioId = id;

            await _usuarioComandoAppService.AdicionarEndereco(endereco);
            return CustomResponse(HttpStatusCode.OK);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter_todos")]
        [SwaggerOperation(Summary = "Lista todos os usuários")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioViewModel>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ObterTodos()
        {
            var usuarios = await _usuarioConsultaAppService.ObterTodos();
            return CustomResponse(HttpStatusCode.OK, usuarios);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("obter/{id:guid}")]
        [SwaggerOperation(Summary = "Obtém um usuário por ID")]
        [ProducesResponseType(typeof(UsuarioViewModel), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorId(Guid id)
        {
            var usuario = await _usuarioConsultaAppService.ObterPorId(id);
            if (usuario == null)
                return NotFoundResponse("Usuário não encontrado.");

            return CustomResponse(HttpStatusCode.OK, usuario);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}/inativar")]
        [SwaggerOperation(Summary = "Inativa um usuário")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Inativar(Guid id)
        {
            var usuario = await ObterUsuario(id);
            if (usuario == null)
                return NotFoundResponse("Usuário não encontrado.");

            var resultado = await _usuarioComandoAppService.Inativar(id);
            if (!resultado)
            {
                NotificarErro("Não foi possível inativar o usuário.");
                return CustomResponse(HttpStatusCode.BadRequest);
            }

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:guid}/ativar")]
        [SwaggerOperation(Summary = "Ativa um usuário", Description = "Ativa o usuário informado.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Ativar(Guid id)
        {
            var usuario = await ObterUsuario(id);
            if (usuario == null)
            {
                NotificarErro("Usuário não encontrado.");
                return CustomResponse(HttpStatusCode.NotFound);
            }

            var resultado = await _usuarioComandoAppService.Ativar(id);
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
            UsuarioViewModel usuario = await _usuarioConsultaAppService.ObterPorId(id);
            if (usuario == null)
            {
                return null;
            }

            return usuario;
        }

        private IActionResult NotFoundResponse(string message)
        {
            NotificarErro(message);
            return CustomResponse(HttpStatusCode.NotFound);
        }
    }
}
