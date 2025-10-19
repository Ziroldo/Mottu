using System;

namespace Mottu.Backend.Services
{
    public static class RentalPricingService
    {
        public static (decimal Total, int UsedDays, int RemainingDays, int ExtraDays, decimal Penalty, decimal ExtraCost)
            Compute(DateTime startDate, DateTime expectedEndDate, DateTime actualReturnDate, int planDays, decimal dailyPrice)
        {
            var usedDays = Math.Max(1, (actualReturnDate.Date - startDate.Date).Days + 1);
            var remaining = Math.Max(0, planDays - usedDays);

            if (actualReturnDate.Date <= expectedEndDate.Date)
            {
                var baseCost = usedDays * dailyPrice;
                decimal rate = planDays switch
                {
                    7  => 0.20m,
                    15 => 0.40m,
                    _  => 0.00m
                };
                var penalty = rate * (remaining * dailyPrice);
                return (Math.Round(baseCost + penalty, 2), usedDays, remaining, 0, Math.Round(penalty,2), 0m);
            }
            else
            {
                var extraDays = (actualReturnDate.Date - expectedEndDate.Date).Days;
                var planCost  = planDays * dailyPrice;
                var extraCost = extraDays * 50m;
                return (Math.Round(planCost + extraCost, 2), planDays, 0, extraDays, 0m, Math.Round(extraCost,2));
            }
        }

        public static decimal GetDailyPrice(int planDays) => planDays switch
        {
            7  => 30m,
            15 => 28m,
            30 => 22m,
            45 => 20m,
            50 => 18m,
            _  => throw new ArgumentOutOfRangeException(nameof(planDays), "Plano inválido")
        };
    }
}
