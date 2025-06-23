using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Repository.Repositories.Interface;
using Service.DTO.Admin.PromoCode;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class PromoCodeService : IPromoCodeService
    {
        private readonly IPromoCodeRepository _promoRepo;
        private readonly ISendEmail _sendMail;

        public PromoCodeService(IPromoCodeRepository promoRepo, ISendEmail sendMail)
        {
            _promoRepo = promoRepo;
            _sendMail = sendMail;
        }

        public async Task CreateAsync(PromoCodeCreateDto dto)
        {
            var promo = new PromoCode
            {
                Code = dto.Code,
                DiscountPercent = dto.DiscountPercent,
                UsageLimit = dto.UsageLimit,
                IsActive = true
            };

            await _promoRepo.AddAsync(promo);

            var members = await _promoRepo.GetAllMembersAsync();

            string from = "aitajjf2@gmail.com";
            string displayName = "JoiFurn";
            string subject = "Yeni Promokod Endirimi";
            string body = $"Salam! Sizə xüsusi <b>{dto.DiscountPercent}%</b> endirim imkanı verən promokod yaradıldı: <b>{dto.Code}</b>. Qaçırmayın!";

            foreach (var user in members)
            {
                await _sendMail.SendAsync(from, displayName, user.Email, body, subject);
            }
        }


        public async Task<PromoCodeResultDto?> GetByCodeAsync(string code)
        {
            var promo = await _promoRepo.GetByCodeAsync(code);
            if (promo == null) return null;

            return new PromoCodeResultDto
            {
                Code = promo.Code,
                DiscountPercent = promo.DiscountPercent,
                IsActive = promo.IsActive,
                UsageLimit = promo.UsageLimit,
                UsageCount = promo.UsageCount
            };
        }

        public async Task<PromoCodeCheckResultDto> CheckAndApplyAsync(string code)
        {
            var promo = await _promoRepo.GetByCodeAsync(code);

            if (promo == null || !promo.IsActive)
            {
                return new PromoCodeCheckResultDto
                {
                    IsValid = false,
                    DiscountPercent = 0,
                    Message = "Promokod tapılmadı və ya deaktivdir."
                };
            }

            if (promo.UsageLimit > 0 && promo.UsageCount >= promo.UsageLimit)
            {
                return new PromoCodeCheckResultDto
                {
                    IsValid = false,
                    DiscountPercent = 0,
                    Message = "Bu promokod artıq istifadə limiti qədər istifadə olunub."
                };
            }

            return new PromoCodeCheckResultDto
            {
                IsValid = true,
                DiscountPercent = promo.DiscountPercent,
                Message = $"{promo.DiscountPercent}% endirim tətbiq olundu!"
            };
        }

        public async Task<bool> IncrementUsageCountAsync(string code)
        {
            var promo = await _promoRepo.GetByCodeAsync(code);
            if (promo == null || !promo.IsActive)
                return false;

            if (promo.UsageLimit > 0 && promo.UsageCount >= promo.UsageLimit)
                return false;

            promo.UsageCount++;
            await _promoRepo.UpdateAsync(promo);
            return true;
        }
        public async Task<List<PromoCodeDto>> GetAllAsync()
        {
            var promos = await _promoRepo.GetAllAsync();

            return promos.Select(x => new PromoCodeDto
            {
                Id = x.Id,
                Code = x.Code,
                DiscountPercent = x.DiscountPercent,
                UsageLimit = x.UsageLimit,
                IsActive = x.IsActive
            }).ToList();
        }
    }
}
