// Ishan Pranav's REBUS: Controller.cs
// Copyright (c) Ishan Pranav. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Rebus.Exceptions;

namespace Rebus
{
    public class Controller
    {
        private readonly IStringLocalizer _localizer;
        private readonly IRepository _repository;
        private readonly IPathfinderFactory<HexPoint> _pathfinderFactory;

        public Controller(IStringLocalizer localizer, IRepository repository, IPathfinderFactory<HexPoint> pathfinderFactory)
        {
            _localizer = localizer;
            _repository = repository;
            _pathfinderFactory = pathfinderFactory;
        }

        public void Report(ExpressionWriter writer, ISpacecraft spacecraft)
        {
            writer.Write(_localizer["TransmissionHeader", spacecraft, spacecraft.Region]);
        }

        private async Task<bool> AddWealthAsync(ExpressionWriter writer, int playerId, int value)
        {
            int wealth = await _repository.GetWealthAsync(playerId);

            if (value > 0)
            {
                writer.Write(_localizer["WealthIncrease", value]);

                if (wealth < 0)
                {
                    const double interestRate = 0.1;
                    int penalty = (int)Math.Round(value * interestRate);

                    wealth -= penalty;

                    writer.Write(_localizer["WealthPenalty", interestRate, penalty]);
                }
            }
            else
            {
                writer.Write(_localizer["WealthDecrease", -value]);
            }

            wealth += value;

            await _repository.SetWealthAsync(playerId, wealth);

            writer.Write(_localizer["WealthBalance", wealth]);

            return wealth > 0;
        }

        public async Task ViewAsync(ExpressionWriter writer, HexPoint region)
        {
            List<IFeature> stars = new List<IFeature>();
            List<HexPoint> neighbors = new List<HexPoint>();

            foreach (HexPoint neighbor in region.Neighbors())
            {
                IFeature? star = await _repository.GetStarAsync(neighbor);

                if (star is null)
                {
                    neighbors.Add(neighbor);
                }
                else
                {
                    stars.Add(star);
                }
            }

            object? planet = await _repository.GetPlanetAsync(region);

            if (planet is null)
            {
                writer.Write(_localizer["RegionDescription", neighbors]);
            }
            else
            {
                writer.Write(_localizer["RegionDescription", planet, neighbors]);
            }

            foreach (IFeature star in stars)
            {
                writer.Write(_localizer["StarDescription", star, star.Region]);
            }
        }

        public async Task NavigateAsync(ExpressionWriter writer, ISpacecraft spacecraft, HexPoint destination)
        {
            switch (HexPoint.Distance(spacecraft.Region, destination))
            {
                case 0:
                    throw new RebusException(_localizer["NavigationFailure"]);

                case 1:
                    await enterAsync(destination);
                    await _repository.AddNavigationAsync(spacecraft.PlayerId, destination);
                    break;

                default:
                    Stack<HexPoint> steps = _pathfinderFactory
                        .CreatePathfinder(spacecraft.PlayerId)
                        .GetSteps(spacecraft.Region, destination);

                    while (steps.TryPop(out HexPoint step))
                    {
                        await enterAsync(step);
                    }
                    break;

                    async Task<bool> enterAsync(HexPoint region)
                    {
                        await ViewAsync(writer, region);
                        return !await AddWealthAsync(writer, spacecraft.PlayerId, value: -1);
                    }
            }
        }
    }
}
