// Copyright (c) 2021 Muddy Boots Software Ltd.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using EnforcerDI.Entities;
using EnforcerDI.PIP;
using Rsk.Enforcer.PEP;
using Rsk.Enforcer.PolicyModels;

namespace EnforcerDI
{
    public class EnforcerPolicyExecutor
    {
        private readonly IPolicyEnforcementPoint _pep;
        private readonly LocationRepository _locationRepository;

        public EnforcerPolicyExecutor(IPolicyEnforcementPoint pep, LocationRepository locationRepository)
        {
            _pep = pep;
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetPermittedLocations(User user)
        {
            var locations = _locationRepository.GetAll();
            var permittedLocations = new List<Location>();

            foreach (var location in locations)
            {
                var result = await EvaluateAsync(user, location, Constants.Actions.Read);
                if (result.Result.Outcome == PolicyOutcome.Permit)
                {
                    permittedLocations.Add(result.Item);
                }
            }

            return permittedLocations;
        }

        private async Task<EvaluationResult> EvaluateAsync(User user, Location location,
            string action)
        {
            var context = EvaluationContextFactory.GetEvaluationContext(user, location, action);
            //var s = new Stopwatch();
            //s.Start();
            var result = await _pep.Evaluate(context);
            //s.Stop();
            //Console.WriteLine("Time: " + s.ElapsedMilliseconds + "ms");

            return new EvaluationResult(result, location);
        }
        private class EvaluationResult
        {
            public EvaluationResult(PolicyEvaluationOutcome result, Location item)
            {
                Result = result;
                Item = item;
            }

            public Location Item { get; }
            public PolicyEvaluationOutcome Result { get; }
        }
    }
}
