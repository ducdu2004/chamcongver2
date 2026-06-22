using chamcong.Application.DTOs;
using chamcong.Application.Common;

namespace chamcong.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<TokenResponseDto>> GenerateTokenAsync(LoginRequestDto request);
    }
}
